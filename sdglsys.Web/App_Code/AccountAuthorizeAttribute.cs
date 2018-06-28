using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sdglsys.BLL;


namespace sdglsys.Web
{
    /// <summary>
    /// 登录验证
    /// https://www.cnblogs.com/xinbaba/p/8194142.html
    /// </summary>
    public class AccountAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext authorizationContext)
        {

            var httpContext = authorizationContext.HttpContext;
            var request = httpContext.Request;

            ActionResult actionResult = null;
            string message = string.Empty;
            var session = HttpContext.Current.Session;
            /// 如果开启调试模式，直接赋值登录用户给Session
            if ((bool)Utils.getSetting("is_debug", typeof(bool)))
            {
                session["login_name"] = "admin";
                session["role"] = 3;
                session["nickname"] = "admin";
            }
            if (session["login_name"] == null)
            {
                String url = request.RawUrl;
                UrlHelper urlHelper = new UrlHelper(request.RequestContext);
                //利用Action 指定的操作名称、控制器名称和路由值生成操作方法的完全限定 URL。
                string returnUrl = urlHelper.Action("Index", "Home", new { returnUrl = "", message = message });
                actionResult = new RedirectResult(returnUrl);
            }

            authorizationContext.Result = actionResult;
        }
    }

    /// <summary>
    /// 登录验证，验证是否为系统管理员
    /// https://www.cnblogs.com/xinbaba/p/8194142.html
    /// </summary>
    public class IsAdmin : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext authorizationContext)
        {

            var httpContext = authorizationContext.HttpContext;
            var request = httpContext.Request;

            ActionResult actionResult = null;
            string message = string.Empty;
            var session = HttpContext.Current.Session;
            /// 如果开启调试模式，直接赋值登录用户给Session
            if ((bool)Utils.getSetting("is_debug", typeof(bool)))
            {
                session["login_name"] = "admin";
                session["role"] = 3;
                session["nickname"] = "admin";
            }
            if (session["role"] == null || (int)session["role"] < 3)
            {
                String url = request.RawUrl;
                UrlHelper urlHelper = new UrlHelper(request.RequestContext);
                //利用Action 指定的操作名称、控制器名称和路由值生成操作方法的完全限定 URL。
                string returnUrl = urlHelper.Action("Index", "Home", new { returnUrl = "", message = message });
                actionResult = new RedirectResult(returnUrl);
            }

            authorizationContext.Result = actionResult;
        }
    }

    /// <summary>
    /// 记录操作日志
    /// https://www.cnblogs.com/xinbaba/p/8194142.html
    /// </summary>
    public class LogThis : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext authorizationContext)
        {
            var httpContext = authorizationContext.HttpContext;
            var request = httpContext.Request;
            string message = string.Empty;
            var session = HttpContext.Current.Session;
            var Log = new BLL.Logs();
            Log.Add(new Entity.Log()
            {
                Info = request.Path,
                Ip = request.UserHostAddress,
                Login_name = (string)session["login_name"]
            });

        }
    }
}