using sdglsys.DbHelper;
using sdglsys.Entity;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sdglsys.Web.Controllers
{
    [AutoLogin]
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
            if (!WebUtils.IsTrial())
            {
                Response.Write("非常抱歉地提示您，您可能未经授权就使用了我的程序，或者该程序已到期，已经无法使用，现在是：" + DateTime.Now + "<br/>如有任何疑问，请联系QQ：1278386874");
                Response.End();
            }
            var msg = new Msg();
            string ip = "";
            string login_name = "";
            var Utils = new WebUtils();
            try
            {
                ip = Request.UserHostAddress;
                login_name = Request["login_name"];
                var pwd = Request.Form["password"];
                var user = new Users().Login(login_name, pwd);
                if (user != null)
                {
                    Session["id"] = user.User_id;
                    Session["login_name"] = user.User_login_name;
                    Session["nickname"] = user.User_nickname;
                    Session["role"] = user.User_role;
                    Session["pid"] = user.User_dorm_id;

                    msg.Message = "登录成功！";
                    msg.Content = "/admin/index";
                    /// 记录登录日志
                    Utils.Log(new Entity.T_Log
                    {
                        Log_info = "Login in",
                        Log_ip = ip,
                        Log_login_name = login_name,
                    });
                    /// 创建Token
                    var token_id = Guid.NewGuid().ToString("N"); // Guid
                    var Token = new DbHelper.Token();
                    var token = Token.GetByUserId(user.User_id);
                    if (token != null)
                    {
                        // 更新登录信息
                        token.Token_id = token_id;
                        token.Token_login_date = DateTime.Now;
                        token.Token_expired_date = DateTime.Now.AddMonths(1);
                        Token.Update(token);
                    }
                    else
                    {
                        token = new T_Token();
                        // 添加登录信息
                        Token.Add(new Entity.T_Token
                        {
                            Token_id = token_id,
                            Token_user_id = user.User_id
                        });
                    }
                    Session["token"] = token_id;
                    msg.Token = token_id;
                    /// 设置cookie
                    var cookie = new HttpCookie("token", token_id);
                    cookie.Expires = token.Token_expired_date;
                    cookie.HttpOnly = false;
                    Response.Cookies.Add(cookie);
                }
                else
                {
                    msg.Message = "用户名或密码错误！";
                    msg.Code = -1;
                    Utils.Log(new Entity.T_Log
                    {
                        Log_info = "Login falied",
                        Log_ip = ip,
                        Log_login_name = login_name,
                    });
                }
            }
            catch (Exception ex)
            {
                msg.Code = -1;
                msg.Message = ex.Message;
            }
            Response.Write(msg.ToJson());
            Response.End();
        }

        #endregion 登录处理

        #region 注销处理

        /// <summary>
        /// 注销处理
        /// </summary>
        /// <returns></returns>
        [NeedLogin]
        public void Logout()
        {
            try
            {
                var Logs = new DbHelper.Logs();
                Logs.Add(new Entity.T_Log
                {
                    Log_info = "Log off",
                    Log_ip = Request.UserHostAddress,
                    Log_login_name = Session["login_name"].ToString()
                }); // 写入日志
                // 清除Token
                new Token().Delete((int)Session["id"]);
            }
            catch (Exception ex)
            {
                new WebUtils().Log("system", "", ex.Message);
            }
            Session.Clear();
            Request.Cookies.Clear();
            Response.Cookies.Clear();
            Response.Redirect("/");
            //return Redirect("/");
        }

        #endregion 注销处理

        // GET: Admin
        [NeedLogin]
        public ActionResult Index()
        {
            /// #trial
            if (!WebUtils.IsTrial())
            {
                Response.Write("非常抱歉地提示您，您可能未经授权就使用了我的程序，或者该程序已到期，已经无法使用，现在是：" + DateTime.Now + "<br/>如有任何疑问，请联系QQ：1278386874");
                Response.End();
            }
            //HttpContext.Application["OnLineUserCount"] = XUtils.CountOnLineUser(); // 更新在线人数统计
            ViewBag.room_count = new DbHelper.Rooms().CountWithPeople((int)Session["pid"]);
            ViewBag.room_count2 = new DbHelper.Rooms().CountNoRecord((int)Session["pid"]);
            //ViewBag.balance = new DbHelper.Bills().SumTotal((int)Session["pid"]);
            //ViewBag.balance2 = new DbHelper.Bills().SumNoPay((int)Session["pid"]);
            return View();
        }

        // GET: Admin/Info:查看个人信息
        [NeedLogin]
        public ActionResult Info()
        {
            /*
            if ((bool)HttpContext.Application["debug"]) {
                Response.Write("当前为调试模式");
                Response.End();
            }*/
            return View(new Users().FindById((int)Session["id"]));
        }

        // GET: Admin/Info:修改个人信息
        [NeedLogin]
        [HttpPost]
        public void UpdateInfo(FormCollection collection)
        {
            var msg = new Msg();
            try
            {
                var User = new Users();
                var user = User.FindById((int)Session["id"]);
                if (user == null)
                {
                    throw new Exception("非法访问！");
                }
                user.User_nickname = collection["nickname"];
                user.User_note = collection["note"];
                user.User_phone = collection["phone"];
                if (User.Update(user))
                {
                    msg.Message = "保存成功！";
                }
                else
                {
                    throw new Exception("发生未知错误！");
                }
            }
            catch (Exception ex)
            {
                msg.Code = -1;
                msg.Message = ex.Message;
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
            var user = User.FindById((int)Session["id"]);
            try
            {
                var pwd_old = collection["pwd_old"];
                var pwd_new = collection["pwd_new"];
                var Utils = new Utils.Utils();
                // 验证原密码
                if (Utils.CheckPasswd(pwd_old, user.User_pwd))
                {
                    user.User_pwd = Utils.HashBcrypt(pwd_new);
                    if (User.Update(user))
                    {
                        msg.Message = "修改密码成功，下次登录时请使用新密码登录";
                    }
                    else
                    {
                        throw new Exception("发生未知错误");
                    }
                }
                else
                {
                    throw new Exception("原密码输入有误");
                }
            }
            catch (Exception ex)
            {
                msg.Code = -1;
                msg.Message = ex.Message;
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
            var rate = Rate.GetLast();
            if (rate == null)
            {
                rate = new Entity.T_Rate()
                {
                    Rate_note = "暂未设置费率",
                };
            }
            var quota = Quota.GetLast();
            if (quota == null)
            {
                quota = new Entity.T_Quota()
                {
                    Quota_note = "暂未设置基础配额"
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
            var quota = new Entity.T_Quota();
            try
            {
                quota.Quota_cold_water_value = Convert.ToSingle(collection["cold_water_value"]);
                quota.Quota_hot_water_value = Convert.ToSingle(collection["hot_water_value"]);
                quota.Quota_electric_value = Convert.ToSingle(collection["electric_value"]);
                quota.Quota_is_active = Convert.ToBoolean(collection["is_active"]);
                quota.Quota_note = collection["note"];
                /// 检查输入
                if (quota.Quota_cold_water_value < 0 || quota.Quota_cold_water_value > 99999)
                {
                    throw new Exception("冷水配额应在0~99999之间");
                }
                if (quota.Quota_hot_water_value < 0 || quota.Quota_hot_water_value > 99999)
                {
                    throw new Exception("热水配额应在0~99999之间");
                }
                if (quota.Quota_electric_value < 0 || quota.Quota_electric_value > 99999)
                {
                    throw new Exception("电力配额应在0~99999之间");
                }

                /// 保存数据
                if (Quota.Update(quota))
                {
                    msg.Message = "保存成功";
                }
                else
                {
                    throw new Exception("发生未知错误，保存失败");
                }
            }
            catch (Exception ex)
            {
                msg.Code = -1;
                msg.Message = ex.Message;
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
            var rate = new Entity.T_Rate();
            try
            {
                rate.Rate_cold_water_value = Convert.ToSingle(collection["cold_water_value"]);
                rate.Rate_hot_water_value = Convert.ToSingle(collection["hot_water_value"]);
                rate.Rate_electric_value = Convert.ToSingle(collection["electric_value"]);
                //rate.Is_active = Convert.ToBoolean(collection["is_active"]);
                /// 检查输入
                if (rate.Rate_cold_water_value < 0 || rate.Rate_cold_water_value > 99999.99)
                {
                    throw new Exception("冷水费率应在0.0~99999.99之间");
                }
                if (rate.Rate_hot_water_value < 0 || rate.Rate_hot_water_value > 99999.99)
                {
                    throw new Exception("热水费率应在0.0~99999.99之间");
                }
                if (rate.Rate_electric_value < 0 || rate.Rate_electric_value > 99999.99)
                {
                    throw new Exception("电力费率应在0.0~99999.99之间");
                }

                /// 保存数据
                rate.Rate_note = collection["note"];
                if (Rate.Update(rate))
                {
                    msg.Message = "保存成功";
                }
                else
                {
                    throw new Exception("发生未知错误，保存失败");
                }
            }
            catch (Exception ex)
            {
                msg.Code = -1;
                msg.Message = ex.Message;
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
            Response.Write(new Utils.Utils().ToJson(data));
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
            var Utils = new Utils.Utils();
            try
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
                        throw new Exception("禁止上传该类型的文件！当前上传的文件类型为：" + file_ext);
                    }
                    var upload_dir = Server.MapPath("~/Uploads/" + DateTime.Now.ToString("yyyy_MM") + "/");
                    var now = DateTime.Now.ToString("_yyyy_MM_dd_HHmmss");
                    var filename = Session["login_name"].ToString() + now + '.' + file_ext;
                    var FileInfo = new System.IO.DirectoryInfo(upload_dir);
                    if (!FileInfo.Exists)
                    {
                        FileInfo.Create();
                    }
                    Request.Files["file"].SaveAs(upload_dir + filename);
                    var msg = new
                    {
                        code = 0,
                        msg = "文件上传成功",
                        data = new
                        {
                            src = "/Uploads/" + DateTime.Now.ToString("yyyy_MM") + "/" + filename,
                            title = Request.Files["file"].FileName,
                            files = Utils.ToJson(Request.Files.AllKeys),
                        }
                    };
                    Response.Write(Utils.ToJson(msg));
                }
                else
                {
                    throw new Exception("请求参数不正确");
                }
            }
            catch (Exception ex)
            {
                var error = new
                {
                    code = -1,
                    msg = "发生错误：" + ex.Message,
                    data = new
                    {
                        src = "",
                        title = "",
                    }
                };
                Response.Write(Utils.ToJson(error));
            }

            Response.End();
        }

        #endregion 上传文件

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
        public void GetLogList()
        {
            var msg = new ResponseData();

            string keyword = null;
            int page = 1;
            int limit = 10;
            int count = 0;
            try
            {
                if (!string.IsNullOrWhiteSpace(Request["keyword"]))
                {
                    keyword = Request["keyword"]; // 搜索关键词
                }
                // 当前页码
                if (!string.IsNullOrWhiteSpace(Request["page"]))
                {
                    int.TryParse(Request["page"], out page);
                    page = page > 0 ? page : 1;
                }
                // 每页数量
                if (!string.IsNullOrWhiteSpace(Request["limit"]))
                {
                    int.TryParse(Request["limit"], out limit);
                    limit = limit > 0 ? limit : 10;
                }

                msg.data = new DbHelper.Logs().GetByPages(page, limit, ref count, keyword); // 获取列表
                msg.count = count;
                Response.Write(msg.ToJson());
                Response.End();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}