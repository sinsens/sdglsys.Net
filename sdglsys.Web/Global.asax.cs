using System;
using System.Web;
using System.Web.Mvc;
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
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
            Application["OnLineUserCount"] = 0;//统计在线人数
            //var Application = System.Web.HttpContext.Current.Application;
        }

        protected void Application_End(object sender, EventArgs e)
        {
            ///  在应用程序关闭时运行的代码
            new DbHelper.DbContext().Db.Insertable(new Entity.TLog
            {
                Info = "Shutdown",
                Login_name = "system",
                Ip = "127.0.0.1"
            }).ExecuteCommand();
        }


        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            /// 系统启动后第一次接受请求的事件
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("gbk");
            /// #trial
            if (!XUtils.IsTrial)
            {
                Response.Write("非常抱歉地提示您，您可能未经授权就使用了我的程序，或者该程序已到期，已经无法使用，现在是：" + DateTime.Now + "<br/>如有任何疑问，请联系QQ：1278386874");
                Response.End();
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
            if (ex is HttpException && (bool) XUtils.GetAppSetting("Debug", typeof(bool)) == false)
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
            else {
                new DbHelper.DbContext().Db.Insertable(new Entity.TLog
                {
                    Info = ex.Message,
                    Login_name = "system",
                    Ip = "127.0.0.1"
                }).ExecuteCommand();
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
                Application.Lock();
                Application["OnLineUserCount"] = Convert.ToInt32(Application["OnLineUserCount"]) - 1; // 在线人数-1
                Application.UnLock();
            }
        }
        #endregion
    }
}
