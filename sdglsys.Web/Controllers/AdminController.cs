using Newtonsoft.Json;
using sdglsys.DbHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sdglsys.Web.Controllers
{
    public class AdminController : Controller
    {

        #region 登录处理
        /// <summary>
        /// 登录处理
        /// </summary>
        /// <returns></returns>
        public void Login()
        {
            /// #trial
            if (!XUtils.IsTrial)
            {
                Response.Write("非常抱歉地提示您，您可能未经授权就使用了我的程序，或者该程序已到期，已经无法使用，现在是：" + DateTime.Now + "<br/>如有任何疑问，请联系QQ：1278386874");
                Response.End();
            }
            var msg = new Msg();
            string ip = "";
            string login_name = "";
            try
            {
                ip = Request.UserHostAddress;
                login_name = Request["login_name"];
                var pwd = Request.Form["password"];
                var user = XUtils.Login(login_name, pwd);
                if (user != null)
                {
                    Session["id"] = user.Id;
                    Session["login_name"] = user.Login_name;
                    Session["nickname"] = user.Nickname;
                    Session["role"] = user.Role;
                    Session["pid"] = user.Pid;
                    msg.msg = "登录成功！";
                    msg.content = "/admin/index";
                    XUtils.Log(new Entity.TLog
                    {
                        Info = "Login in",
                        Ip = ip,
                        Login_name = login_name,
                    });
                    /// #Dv0.1请求处理
                    var Session_ID = Guid.NewGuid().ToString("N"); // cookie端的Session_ID
                    Response.SetCookie(new HttpCookie("Session_ID", Session_ID)); // 设置cookie
                    var Login_Info = new DbHelper.LoginInfo();
                    var login_info = Login_Info.GetByUserId(user.Id);
                    if (login_info != null)
                    {
                        // 更新登录信息
                        login_info.Ip = ip;
                        login_info.Uid = user.Id;
                        login_info.Login_name = login_name;
                        login_info.Session_id = Session_ID;
                        login_info.Login_date = DateTime.Now;
                        login_info.Expired_Date = DateTime.Now.AddHours(2);
                        Login_Info.Update(login_info);
                    }
                    else {
                        // 添加登录信息
                        Login_Info.Add(new Entity.TLogin_Info
                        {
                            Ip = ip,
                            Login_name = login_name,
                            Session_id = Session_ID,
                            Uid = user.Id
                        });
                    }
                    
                    /// #end of Dv0.1请求处理
                    // HttpContext.Application["OnLineUserCount"] = Convert.ToInt32(HttpContext.Application["OnLineUserCount"]) + 1; // 登录成功，在线用户+1
                }
                else
                {
                    msg.code = 400;
                    msg.msg = "用户名或密码错误！";
                    XUtils.Log(new Entity.TLog
                    {
                        Info = "Login falied",
                        Ip = ip,
                        Login_name = login_name,
                    });
                }

            }
            catch (Exception ex)
            {
                msg.code = 500;
                msg.msg += ex.Message;
            }
            Response.Write(msg.ToJson());
            Response.End();
        }
        #endregion

        #region 注销处理
        /// <summary>
        /// 注销处理
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {

            try
            {
                new DbHelper.LoginInfo().DeleteBySessionId(Request.Cookies.Get("Session_ID").Value);
                new DbHelper.Logs().Add(new Entity.TLog
                {
                    Info = "Log off",
                    Ip = Request.UserHostAddress,
                    Login_name = Session["login_name"].ToString()
                }); // 写入日志
            }
            catch (Exception ex)
            {
                XUtils.Log("system", "", ex.Message);
            }
            Session.Clear();
            return Redirect("/");
        }
        #endregion

        // GET: Admin
        [NeedLogin]
        public ActionResult Index()
        {
            /// #trial
            if (!XUtils.IsTrial)
            {
                Response.Write("非常抱歉地提示您，您可能未经授权就使用了我的程序，或者该程序已到期，已经无法使用，现在是：" + DateTime.Now + "<br/>如有任何疑问，请联系QQ：1278386874");
                Response.End();
            }
            HttpContext.Application["OnLineUserCount"] = XUtils.CountOnLineUser(); // 更新在线人数统计
            return View();
        }

        // GET: Admin/Info:查看个人信息
        [NeedLogin]
        public ActionResult Info()
        {
            return View(new Users().FindById((int) Session["id"]));
        }

        // GET: Admin/Info:修改个人信息
        [NeedLogin]
        [HttpPost]
        public void UpdateInfo(FormCollection collection)
        {
            var msg = new Msg();
            var User = new Users();
            var user = User.FindById((int) Session["id"]);
            try
            {
                user.Nickname = collection["nickname"];
                user.Note = collection["note"];
                user.Phone = collection["phone"];
                if (User.Update(user))
                {
                    msg.msg = "保存成功！";
                }
                else
                {
                    msg.code = 500;
                    msg.msg = "发生未知错误！";
                }
            }
            catch (Exception ex)
            {
                msg.code = 500;
                msg.msg = ex.Message;
            }
            Response.Write(msg.ToJson());
            Response.End();
        }

        // GET: Admin/Info:修改个人密码
        [NeedLogin]
        [HttpPost]
        public void UpdatePassword(FormCollection collection)
        {
            var msg = new Msg();
            var User = new Users();
            var user = User.FindById((int) Session["id"]);
            try
            {
                var pwd_old = collection["pwd_old"];
                var pwd_new = collection["pwd_new"];
                // 验证原密码
                if (XUtils.checkpw(pwd_old, user.Pwd))
                {
                    user.Pwd = XUtils.hashpwd(pwd_new);
                    if (User.Update(user))
                    {
                        msg.msg = "修改密码成功！";
                    }
                    else
                    {
                        msg.code = 500;
                        msg.msg = "发生未知错误！";
                    }
                }
                else
                {
                    msg.code = 403;
                    msg.msg = "原密码输入有误";
                }
            }
            catch (Exception ex)
            {
                msg.code = 500;
                msg.msg = ex.Message;
            }
            Response.Write(msg.ToJson());
            Response.End();
        }

        // GET: Admin/System:费率及基础配额设置
        [IsAdmin]
        public ActionResult System()
        {
            var Rate = new Rates();
            var Quota = new Quotas();
            var rate = Rate.getLast();
            if (rate == null)
            {
                rate = new Entity.TRate()
                {
                    Note = "暂未设置费率",
                };
            }
            var quota = Quota.getLast();
            if (quota == null)
            {
                quota = new Entity.TQuota()
                {
                    Note = "暂未设置基础配额"
                };
            }
            ViewBag.rate = rate;
            ViewBag.quota = quota;
            return View();
        }

        /// <summary>
        /// 设置基础配额
        /// </summary>
        /// <param name="collection"></param>
        [HttpPost]
        [IsAdmin]
        public void Quota(FormCollection collection)
        {
            var msg = new Msg();
            var Quota = new Quotas();
            var quota = new Entity.TQuota();
            try
            {
                quota.Cold_water_value = Convert.ToSingle(collection["cold_water_value"]);
                quota.Hot_water_value = Convert.ToSingle(collection["hot_water_value"]);
                quota.Electric_value = Convert.ToSingle(collection["electric_value"]);
                quota.Is_active = Convert.ToBoolean(collection["is_active"]);
                quota.Note = collection["note"];
                if (Quota.Update(quota))
                {
                    msg.msg = "保存成功！";
                }
                else
                {
                    msg.msg = "发生未知错误，保存失败！";
                }
            }
            catch (Exception ex)
            {
                msg.code = 500;
                msg.msg = ex.Message;
            }
            finally
            {
                Response.Write(msg.ToJson());
                Response.End();
            }
        }

        /// <summary>
        /// 设置费率
        /// </summary>
        /// <param name="collection"></param>
        [HttpPost]
        [IsAdmin]
        public void Rate(FormCollection collection)
        {
            var msg = new Msg();
            var Rate = new Rates();
            var rate = new Entity.TRate();
            try
            {
                rate.Cold_water_value = Convert.ToSingle(collection["cold_water_value"]);
                rate.Hot_water_value = Convert.ToSingle(collection["hot_water_value"]);
                rate.Electric_value = Convert.ToSingle(collection["electric_value"]);
                //rate.Is_active = Convert.ToBoolean(collection["is_active"]);
                rate.Note = collection["note"];
                if (Rate.Update(rate))
                {
                    msg.msg = "保存成功！";
                }
                else
                {
                    msg.msg = "发生未知错误，保存失败！";
                }
            }
            catch (Exception ex)
            {
                msg.code = 500;
                msg.msg = ex.Message;
            }
            Response.Write(msg.ToJson());
            Response.End();
        }


        [NeedLogin]
        /// <summary>
        /// 欢迎页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Welcome()
        {
            return View();
        }

        [NotLowUser]
        /// <summary>
        /// 图表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Charts()
        {
            return View(new DbHelper.Useds().GetUsedDatas());
        }

        /// <summary>
        /// 获取图表所需数据
        /// </summary>
        [NotLowUser]
        [HttpPost]
        public void Charts(FormCollection collection)
        {
            var Used_data = new DbHelper.Useds();
            var data = new Entity.Used_datas();
            var _type = 0;
            var _id = 0;
            var _start = DateTime.Now;
            var _end = DateTime.Now;
            try
            {
                _type = Convert.ToInt32(collection["type"]);
                _id = _type == 0 ? 0 : Convert.ToInt32(collection["id"]);
                _start = _type == 0 ? default(DateTime) : DateTime.Parse(collection["start_date"]);
                _end = _type == 0 ? default(DateTime) : DateTime.Parse(collection["end_date"]);

            }
            catch
            {
                data.info = "请求参数有误";
            }
            data = Used_data.GetUsedDatas(_type, _id, _start, _end);
            Response.Write(XUtils.ToJson(data));
            Response.End();
        }


        /// <summary>
        /// 测试用的上传文件界面
        /// </summary>
        /// <returns></returns>
        [NotLowUser]
        public ActionResult Upload()
        {
            return View();
        }


        #region 上传文件
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="collection"></param>
        [HttpPost]
        [NotLowUser]
        public void Upload(FormCollection collection)
        {
            if (Request.Files.Count > 0 && Request.Files["file"] != null && Request.Files["file"].ContentLength > 128)
            {
                var file_ext = Request.Files["file"].FileName.Split('.').Reverse().First();
                /// 文件后缀名过滤
                var flage = false;
                foreach (var item in XUtils.GetAppSetting("AllowFiles", typeof(string)).ToString().Split(','))
                {
                    if (item.Equals(file_ext))
                        flage = true;
                }
                if (!flage)
                {
                    var msg = new
                    {
                        code = 0,
                        msg = "禁止上传该类型的文件！当前上传的文件类型为：" + file_ext,
                        data = new
                        {
                            src = "",
                            title = "",
                        }
                    };
                    Response.Write(XUtils.ToJson(msg));
                    Response.End();
                }
                var upload_dir = Server.MapPath("~/Uploads/" + DateTime.Now.ToString("yyyy_MM") + "/");
                var now = DateTime.Now.ToString("_yyyy_MM_dd_HHmmss");
                var filename = Session["login_name"].ToString() + now + '.' + file_ext;
                try
                {
                    var FileInfo = new System.IO.DirectoryInfo(upload_dir);
                    if (!FileInfo.Exists)
                    {
                        FileInfo.Create();
                    }
                    Request.Files["file"].SaveAs(upload_dir + filename);
                    var msg = new
                    {
                        code = 0,
                        msg = "文件上传成功！",
                        data = new
                        {
                            src = "/Uploads/" + DateTime.Now.ToString("yyyy_MM") + "/" + filename,
                            title = Request.Files["file"].FileName,
                            files = XUtils.ToJson(Request.Files.AllKeys),
                        }
                    };
                    Response.Write(XUtils.ToJson(msg));
                }
                catch (Exception ex)
                {
                    var msg = new
                    {
                        code = 500,
                        msg = ex.Message,
                        data = new
                        {
                            src = "",
                            title = "",
                            files = XUtils.ToJson(Request.Files.AllKeys),
                        }
                    };
                    Response.Write(XUtils.ToJson(msg));
                }
            }
            else
            {
                var error = new
                {
                    code = 0,
                    msg = "请求参数不正确",
                    data = new
                    {
                        src = "",
                        title = "",
                    }
                };
                Response.Write(XUtils.ToJson(error));
            }
            Response.End();
        }
        #endregion

        /// <summary>
        /// 查看系统日志
        /// </summary>
        /// <returns></returns>
        [IsAdmin]
        public ViewResult Log()
        {
            return View();
        }


        /// <summary>
        /// 查看系统日志
        /// 测试Layui Table模板引擎
        /// </summary>
        /// <returns></returns>
        [IsAdmin]
        [OutputCache(Duration = 10)]
        public void GetLogList()
        {
            var msg = new ResponseData();
            var Log = new DbHelper.Logs();
            string keyword = "";
            int page = 1;
            int limit = 10;
            int count = 0;
            try
            {
                keyword = Request["keyword"]; // 搜索关键词
                page = Convert.ToInt32(Request["page"]); if (page < 1) page = 1;
                limit = Convert.ToInt32(Request["limit"]); if (limit > 99 || limit < 1) limit = 10;
            }
            catch
            {
                msg.code = 500;
            }
            msg.data = Log.getByPages(page, limit, ref count, keyword); // 获取列表
            msg.count = count;
            Response.Write(msg.ToJson());
            Response.End();
        }
    }
}