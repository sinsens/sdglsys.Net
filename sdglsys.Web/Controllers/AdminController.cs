using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sdglsys.BLL;

namespace sdglsys.Web.Controllers
{
    public class AdminController : Controller
    {
        /// <summary>
        /// 登录处理
        /// </summary>
        /// <returns></returns>
        public void Login()
        {
            var msg = new BLL.Msg();
            var login_name = Request["login_name"];
            var pwd = Request["password"];
            var user = Utils.Login(login_name, pwd);
            if (user != null)
            {
                Session["id"] = user.Id;
                Session["login_name"] = user.Login_name;
                Session["nick_name"] = user.Nickname;
                Session["role"] = user.Role;
                Session["pid"] = user.Pid;

                msg.msg = "登录成功！";
                msg.content = "/admin/index";
            }
            else {
                msg.code = 400;
                msg.msg = "用户名或密码错误！";
            }
            Response.Write(msg.toJson());
            Response.End();
        }

        /// <summary>
        /// 注销处理
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            Session.Clear();
            return Redirect("/");
        }

        // GET: Admin
        [AccountAuthorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}