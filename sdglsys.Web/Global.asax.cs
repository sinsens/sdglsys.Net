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
            /// 测试数据库连接是否正常
            try
            {
                var db = new DbHelper.DbContext().Db;
                db.Open();
                db.Close();
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
                                               /// 测试数据库配置
            //var Application = System.Web.HttpContext.Current.Application;
        }

        protected void Application_End(object sender, EventArgs e)
        {
            //  在应用程序关闭时运行的代码
        }


        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("gbk");
            /// #trial
            if (!DbHelper.Utils.IsTrial)
            {
                Response.Write("非常抱歉地提示您，您可能未经授权就使用了我的程序，或者该程序已到期，已经无法使用，现在是：" + DateTime.Now +"<br/>如有任何疑问，请联系QQ：1278386874");
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
            Exception ex = Server.GetLastError();
            if (ex is HttpException && (bool)DbHelper.Utils.GetAppSetting("Debug", typeof(bool)) == false)
            {
                //Response.Redirect("/Error/" + ((HttpException)ex).GetHttpCode());
                var msg = new DbHelper.Msg();
                msg.code = ((HttpException)ex).GetHttpCode();
                switch (msg.code)
                {
                    case 404:
                        msg.msg = "找不到该页面，可能是输入的参数有误";
                        break;
                    case 500:
                        msg.msg = "发生了系统内部错误";
                        break;
                    default:
                        msg.msg = ex.Message;
                        break;
                }
                Response.Write(msg.ToJson());
                Response.End();
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
            Application.Lock();
            Application["OnLineUserCount"] = Convert.ToInt32(Application["OnLineUserCount"]) + 1;
            Application.UnLock();
        }

        protected void Session_End(object sender, EventArgs e)
        {
            Application.Lock();
            Application["OnLineUserCount"] = Convert.ToInt32(Application["OnLineUserCount"]) - 1;
            Application.UnLock();
        }
        #endregion
    }
}
