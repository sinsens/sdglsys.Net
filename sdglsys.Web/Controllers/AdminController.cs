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
            if (!DbHelper.Utils.IsTrial)
            {
                Response.Write("非常抱歉地提示您，您可能未经授权就使用了我的程序，或者该程序已到期，已经无法使用，现在是：" + DateTime.Now + "<br/>如有任何疑问，请联系QQ：1278386874");
                Response.End();
            }
            var msg = new Msg();
            string ip = "";
            var login_name = "";
            try
            {
                var Log = new Logs();
                ip = Request.UserHostAddress;
                login_name = Request["login_name"];

                var pwd = Request.Form["password"];
                var user = Utils.Login(login_name, pwd);
                msg.content += login_name + ":" + pwd;
                if (user != null)
                {
                    Session["id"] = user.Id;
                    Session["login_name"] = user.Login_name;
                    Session["nickname"] = user.Nickname;
                    Session["role"] = user.Role;
                    Session["pid"] = user.Pid;
                    msg.msg = "登录成功！";
                    msg.content = "/admin/index";
                    Log.Add(new Entity.TLog
                    {
                        Info = "登录成功",
                        Ip = ip,
                        Login_name = login_name,
                    });
                }
                else
                {
                    msg.code = 400;
                    msg.msg = "用户名或密码错误！";
                    Log.Add(new Entity.TLog
                    {
                        Info = "登录失败",
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
            Session.Clear();
            return Redirect("/");
        }
        #endregion

        // GET: Admin
        [NeedLogin]
        public ActionResult Index()
        {
            /// #trial
            if (!DbHelper.Utils.IsTrial)
            {
                Response.Write("非常抱歉地提示您，您可能未经授权就使用了我的程序，或者该程序已到期，已经无法使用，现在是：" + DateTime.Now + "<br/>如有任何疑问，请联系QQ：1278386874");
                Response.End();
            }
            return View();
        }

        // GET: Admin/Info:查看个人信息
        [NeedLogin]
        public ActionResult Info()
        {
            var User = new Users();
            return View(User.findById((int)Session["id"]));
        }

        // GET: Admin/Info:修改个人信息
        [NeedLogin]
        [HttpPost]
        public void UpdateInfo(FormCollection collection)
        {
            var msg = new Msg();
            var User = new Users();
            var user = User.findById((int)Session["id"]);
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
            finally
            {
                Response.Write(msg.ToJson());
                Response.End();
            }
        }

        // GET: Admin/Info:修改个人密码
        [NeedLogin]
        [HttpPost]
        public void UpdatePassword(FormCollection collection)
        {
            var msg = new Msg();
            var User = new Users();
            var user = User.findById((int)Session["id"]);
            try
            {
                var pwd_old = collection["pwd_old"];
                var pwd_new = collection["pwd_new"];
                // 验证原密码
                if (Utils.checkpw(pwd_old, user.Pwd))
                {
                    user.Pwd = Utils.hashpwd(pwd_new);
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
            finally
            {
                Response.Write(msg.ToJson());
                Response.End();
            }
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
            finally
            {
                Response.Write(msg.ToJson());
                Response.End();
            }
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
            var used_data = new DbHelper.Useds();
            var data = used_data.GetUsedDatas();
            return View(data);
        }

        /// <summary>
        /// 获取图表所需数据
        /// </summary>
        [NotLowUser]
        [HttpPost]
        public void Charts(FormCollection collection)
        {
            var Used_data = new DbHelper.Useds();
            var data = new Used_datas();
            try
            {
                var _type = Convert.ToInt32(collection["type"]);
                var _id = _type == 0 ? 0 : Convert.ToInt32(collection["id"]);
                var _start = _type == 0 ? default(DateTime) : DateTime.Parse(collection["start_date"]);
                var _end = _type == 0 ? default(DateTime) : DateTime.Parse(collection["end_date"]);
                data = Used_data.GetUsedDatas(_type, _id, _start, _end);
            }
            finally
            {
                Response.Write(DbHelper.Utils.ToJson(data));
                Response.End();
            }

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
                foreach (var item in Utils.GetAppSetting("AllowFiles", typeof(string)).ToString().Split(','))
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
                    Response.Write(DbHelper.Utils.ToJson(msg));
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
                            files = DbHelper.Utils.ToJson(Request.Files.AllKeys),
                        }
                    };
                    Response.Write(DbHelper.Utils.ToJson(msg));
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
                            files = DbHelper.Utils.ToJson(Request.Files.AllKeys),
                        }
                    };
                    Response.Write(DbHelper.Utils.ToJson(msg));
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
                Response.Write(DbHelper.Utils.ToJson(error));
            }
            Response.End();
        }


        /// <summary>
        /// 查看系统日志
        /// </summary>
        /// <returns></returns>
        [IsAdmin]
        public ViewResult Log()
        {
            string keyword="";
            var Log = new DbHelper.Logs();
            int pageIndex = 1;
            int pageSize = 10;

            try
            {
                keyword = Request["keyword"]; // 搜索关键词
                pageIndex = Convert.ToInt32(Request["pageIndex"]); if (pageIndex < 1) pageIndex = 1;
                pageSize = Convert.ToInt32(Request["pageSize"]); if (pageSize > 99 || pageSize < 1) pageSize = 10;
            }
            catch
            {}

            int count = 0;
            ViewBag.logs = Log.getByPages(pageIndex, pageSize, ref count, keyword); // 获取列表


            ViewBag.keyword = keyword;
            ViewBag.count = count;  // 获取当前页数量
            ViewBag.pageIndex = pageIndex;  // 获取当前页
            return View();
        }
    }
}