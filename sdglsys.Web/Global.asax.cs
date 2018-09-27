using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace sdglsys.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            /// 系统启动时的事件
            /// 测试是否能正常连接数据库及进行写入日志操作
            try
            {
                new DbHelper.DbContext().Db.Insertable(new Entity.TLog
                {
                    Info = "Startup",
                    Login_name = "system",
                    Ip = "127.0.0.1"
                }).ExecuteCommand();
            }
            catch (Exception ex)
            {
                throw new Exception("无法连接到数据库服务器，请先配置好数据库服务器！错误提示：" + ex.Message);
            }
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Application["OnLineUserCount"] = 0;//统计在线人数
            //var Application = System.Web.HttpContext.Current.Application;
        }

        protected void Application_End(object sender, EventArgs e)
        {
            ///  在应用程序关闭时运行的代码
            ///  删除所有过期的登录信息
            new DbHelper.LoginInfo().LoginInfoDb.Delete(u => u.Expired_Date <= DateTime.Now);
            ///  记录日志
            XUtils.Log(new Entity.TLog
            {
                Info = "Shutdown",
                Login_name = "system",
                Ip = "127.0.0.1"
            });
        }


        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            /// 系统启动后每次接受请求的事件
            Response.Headers.Add("Content-Type", "text/html; charset=utf-8");
            //Response.ContentEncoding = System.Text.Encoding.GetEncoding("gbk");

            /// #trial
            if (!XUtils.IsTrial)
            {
                Response.Write("非常抱歉地提示您，您可能未经授权就使用了我的程序，或者该程序已到期，已经无法使用，现在是：" + DateTime.Now + "<br/>如有任何疑问，请联系QQ：1278386874");
                Response.End();
            }

        }

        /// <summary>
        /// #Dv0.1请求处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            /// 系统获取会话信息（如Session）完成后的事件
            if (Session["login_name"] == null && Request.Cookies.Get("Session_ID") != null) // Session没有login_name（未通过登录界面登录）且不是新Session才进行判断
            {
                var login_info = new DbHelper.LoginInfo().GetBySessionId(Request.Cookies.Get("Session_ID").Value);
                if (login_info != null && login_info.Expired_Date > DateTime.Now) // 登录信息不是null且未过期
                {
                    var user = new DbHelper.Users().FindById(login_info.Uid);
                    if (user != null)
                    {
                        Session["id"] = user.Id;
                        Session["login_name"] = user.Login_name;
                        Session["nickname"] = user.Nickname;
                        Session["role"] = user.Role;
                        Session["pid"] = user.Pid;
                    }
                }
            }
            else if (Session["login_name"] != null && Request.Cookies.Get("Session_ID") != null)
            { // 预防正在操作时身份验证突然过期
                var login_info = new DbHelper.LoginInfo().GetBySessionId(Request.Cookies.Get("Session_ID").Value);
                if (login_info != null && login_info.Expired_Date.Subtract(DateTime.Now.AddMinutes(10)).TotalMinutes < 10) // Session还剩20分钟过期
                {
                    login_info.Expired_Date.AddHours(1); // 过期时间增加1个小时
                    new DbHelper.LoginInfo().Update(login_info);
                }
            }
        }


        /// <summary>
        /// 自定义错误页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error(object sender, EventArgs e)
        {
            /// 系统发生错误时的事件
            Exception ex = Server.GetLastError();
            if (ex is HttpException)
            {
                XUtils.Log(new Entity.TLog
                {
                    Info = ex.Message,
                    Login_name = "system",
                    Ip = "127.0.0.1"
                });
                if ((bool) XUtils.GetAppSetting("Debug", typeof(bool)) == false)
                {
                    //Response.Redirect("/Error/" + ((HttpException)ex).GetHttpCode());
                    var msg = new Msg();
                    msg.code = ((HttpException) ex).GetHttpCode();
                    switch (msg.code)
                    {
                        case 404:
                            msg.msg = "找不到该页面，可能是输入的参数有误";
                            break;
                        case 500:
                            msg.msg = "发生了系统内部错误<br/>" + ex.Message;
                            break;
                        default:
                            msg.msg = ex.Message;
                            break;
                    }
                    Response.Write(msg.ToJson());
                    Response.End();
                }
            }
        }


        #region 统计在线人数
        /// <summary>
        /// fork from https://www.cnblogs.com/forthelichking/p/4255070.html
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Session_Start(object sender, EventArgs e)
        {
            /// 用户第一次加载页面时的事件
            //Application.Lock();
            //Application["OnLineUserCount"] = Convert.ToInt32(Application["OnLineUserCount"]) + 1;
            //Application.UnLock();
        }

        protected void Session_End(object sender, EventArgs e)
        {
            /// 用户关闭所有相关页面时的事件
            /// 先判断该用户是否已登录
            if (Session["login_name"] != null)
            {
                /*
                /// 删减在线统计人数
                Application.Lock();
                Application["OnLineUserCount"] = Convert.ToInt32(Application["OnLineUserCount"]) - 1; // 在线人数-1
                Application.UnLock();
                */
                /// 删除数据库中关联的已登录信息
                try
                {
                    new DbHelper.LoginInfo().DeleteBySessionId(Session.SessionID);
                    new DbHelper.Logs().Add(new Entity.TLog
                    {
                        Info = "Auth Expired",
                        Ip = Request.UserHostAddress,
                        Login_name = Session["login_name"].ToString()
                    }); // 写入日志
                }
                catch (Exception ex)
                {
                    XUtils.Log("system", "", ex.Message);
                }
            }
            Session.Clear();
        }
        #endregion
    }
}
