using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sdglsys.Web.Controllers
{
    public class HomeController : Controller
    {
        public void Index()
        {
            /// #trial
            if (!WebUtils.IsTrial())
            {
                Response.Write("非常抱歉地提示您，您可能未经授权就使用了我的程序，或者该程序已到期，已经无法使用，现在是：" + DateTime.Now + "<br/>如有任何疑问，请联系QQ：1278386874");
                Response.End();
            }
            new AutoLogin().LoginMe(Request); // 自动登录
            if (Session["login_name"] != null)
            {
                /// 自动登录
                //Redirect("/admin/index");
                //Server.Transfer("~/admin/index");
                Server.TransferRequest("~/admin/index");
            }
            else {
                Server.TransferRequest("~/Login.html"); // 跳转到登录页面
            }
            
        }
    }
}