using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sdglsys.Web.Controllers
{
    public class HomeController : Controller
    {
        [AutoLogin]
        public void Index()
        {
            /// #trial
            if (!WebUtils.IsTrial())
            {
                Response.Write("非常抱歉地提示您，您可能未经授权就使用了我的程序，或者该程序已到期，已经无法使用，现在是：" + DateTime.Now + "<br/>如有任何疑问，请联系QQ：1278386874");
                Response.End();
            }

            /// 自动跳转页面
            Server.TransferRequest((Session["login_name"] != null) ? "~/admin/index" : "~/Login.html");
            /*
             * //bool is_login = (Session["login_name"] != null); // 设置登录状态
        if (is_login)
        {
            Server.TransferRequest("~/admin/index"); // 自动登录成功，跳转到后台管理界面
        }
        else {
            Server.TransferRequest("~/Login.html"); // 自动登录失败，跳转到登录页面
        }
        */
        }
    }
}