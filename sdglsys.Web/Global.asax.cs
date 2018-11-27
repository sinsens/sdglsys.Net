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
            try
            {
                /// 测试是否能正常连接数据库及进行写入日志操作
                var db = new DbHelper.DbContext().Db;
                db.Insertable(new Entity.T_Log
                {
                    Log_info = "Startup",
                    Log_login_name = "system",
                    Log_ip = "127.0.0.1"
                }).ExecuteCommand();
                /// 清除过期Token
                new DbHelper.Token().DistroyExpiredToken();
                db.Insertable(new Entity.T_Log
                {
                    Log_info = "Clear Token",
                    Log_login_name = "system",
                    Log_ip = "127.0.0.1"
                }).ExecuteCommand();
                /// 是否进入调试模式
                Application["debug"] = new WebUtils().GetAppSetting("Debug", typeof(bool));
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
            ///  记录日志
            new WebUtils().Log(new Entity.T_Log
            {
                Log_info = "Shutdown",
                Log_login_name = "system",
                Log_ip = "127.0.0.1"
            });
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            /// 系统启动后每次接受请求的事件
            Response.Headers.Add("Content-Type", "charset=utf-8"); // 强制使用UTF-8编码
            //Response.ContentEncoding = System.Text.Encoding.GetEncoding("gbk");

            /// #trial
            if (!WebUtils.IsTrial())
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
        }

        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            try
            {
                /// 调试模式自动登录
                if ((bool)Application["debug"] && Session["login_name"] == null)
                {
                    var user = new sdglsys.DbHelper.Users().GetAdminUser();
                    if (user != null)
                    {
                        Session["id"] = user.User_id;
                        Session["login_name"] = user.User_login_name;
                        Session["nickname"] = "as_debug_admin";
                        Session["role"] = 3;
                        Session["pid"] = 0;
                        var Token = new DbHelper.Token();
                        var token = Token.GetByUserId(user.User_id);
                        if (token == null)
                        {
                            token = new Entity.T_Token();
                            token.Token_expired_date = DateTime.Now.AddHours(2);
                            token.Token_id = Guid.NewGuid().ToString("N");
                            token.Token_user_id = user.User_id;
                            Token.Add(token);
                        }
                        else
                        {
                            token.Token_expired_date = DateTime.Now.AddHours(2);
                            token.Token_id = Guid.NewGuid().ToString("N");
                            token.Token_user_id = user.User_id;
                            Token.Update(token);
                        }

                        Session["token"] = token.Token_id;

                        new WebUtils().Log(new Entity.T_Log
                        {
                            Log_info = "Login as debug admin",
                            Log_ip = Request.UserHostAddress,
                            Log_login_name = user.User_login_name,
                        });
                    }
                    Response.Write(new Msg { Message = "请先添加一个系统管理员角色，否则无法继续进行调试。", Code = -1 });
                    Response.End();
                }
            }
            catch (Exception)
            {
                throw;
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

            if (ex is HttpException && (bool)Application["debug"] == false)
            {
                var msg = new Web.Msg
                {
                    Code = -1
                };
                switch (((HttpException)ex).GetHttpCode())
                {
                    case 404:
                        msg.Message = "页面不存在";
                        break;

                    case 500:
                        msg.Message = "系统内部错误";
                        break;

                    default:
                        msg.Message = ex.Message;
                        break;
                }

                Response.Write(msg.ToJson());
                Response.End();
            }
            else if ((bool)Application["debug"] == false)
            {
                /*
                 非调试模式输出序列化错误信息
                 */
                //Response.Redirect("/Error/" + ((HttpException)ex).GetHttpCode());
                /// 非HTTP 异常，写入日志
                new WebUtils().Log(new Entity.T_Log
                {
                    Log_info = ex.Message,
                    Log_login_name = "system error",
                    Log_ip = "127.0.0.1"
                });

                Response.Write(new Web.Msg
                {
                    Code = -1,
                    Message = "发生系统错误",
                    Content = "请查看系统日志以获取详细信息"
                }.ToJson());
                Response.End();
            }
            else
            {
                /*
                直接抛出
                */
                throw ex;
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
            /*
            /// 用户关闭所有相关页面时的事件
            /// 先判断该用户是否已登录
            if (Session["login_name"] != null)
            {
                /*
                /// 删减在线统计人数
                Application.Lock();
                Application["OnLineUserCount"] = Convert.ToInt32(Application["OnLineUserCount"]) - 1; // 在线人数-1
                Application.UnLock();
                **
                /// 删除数据库中关联的已登录信息
                try
                {
                    new DbHelper.LoginInfo().DeleteBySessionId(Session.SessionID);
                    new DbHelper.Logs().Add(new Entity.T_Log
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
            }*/
        }

        #endregion 统计在线人数
    }
}