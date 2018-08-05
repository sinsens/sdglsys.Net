using System;
using System.Web;
using System.Web.Mvc;

namespace sdglsys.Web
{

    /// <summary>
    /// 登录验证
    /// https://www.cnblogs.com/xinbaba/p/8194142.html
    /// </summary>
    public class NeedLogin : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext authorizationContext)
        {
            var httpContext = authorizationContext.HttpContext;
            var request = httpContext.Request;
            var session = HttpContext.Current.Session;

            ActionResult actionResult = null;
            string message = string.Empty;

            /// 如果开启调试模式，直接赋值登录用户给Session
            if ((bool) XUtils.GetAppSetting("Debug", typeof(bool)) && session[ "login_name" ] == null)
            {
                var user = XUtils.GetAdminUser();
                if (user != null)
                {
                    session[ "id" ] = user.Id;
                    session[ "login_name" ] = user.Login_name;
                    session[ "role" ] = 3;
                    session[ "nickname" ] = user.Nickname;
                }
                else
                {
                    UrlHelper urlHelper = new UrlHelper(request.RequestContext);
                    //利用Action 指定的操作名称、控制器名称和路由值生成操作方法的完全限定 URL。
                    string returnUrl = urlHelper.Action("Index", "Home", new { returnUrl = "", message = message });
                    actionResult = new RedirectResult(returnUrl);
                }

            }
            else if (session[ "login_name" ] == null)
            {
                UrlHelper urlHelper = new UrlHelper(request.RequestContext);
                //利用Action 指定的操作名称、控制器名称和路由值生成操作方法的完全限定 URL。
                string returnUrl = urlHelper.Action("Index", "Home", new { returnUrl = "", message = message });
                actionResult = new RedirectResult(returnUrl);
            }
            XUtils.Log(httpContext);
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
            var session = HttpContext.Current.Session;

            ActionResult actionResult = null;
            string message = string.Empty;
            /// 如果开启调试模式，直接赋值登录用户给Session
            if ((bool) XUtils.GetAppSetting("Debug", typeof(bool)) && session[ "role" ] == null)
            {
                var user = XUtils.GetAdminUser();
                if (user != null)
                {
                    session[ "id" ] = user.Id;
                    session[ "login_name" ] = user.Login_name;
                    session[ "role" ] = 3;
                    session[ "nickname" ] = user.Nickname;
                }
                else
                {
                    UrlHelper urlHelper = new UrlHelper(request.RequestContext);
                    //利用Action 指定的操作名称、控制器名称和路由值生成操作方法的完全限定 URL。
                    string returnUrl = urlHelper.Action("Index", "Home", new { returnUrl = "", message = message });
                    actionResult = new RedirectResult(returnUrl);
                }
            }
            else if (session[ "role" ] == null || (int) session[ "role" ] < 3)
            {

                UrlHelper urlHelper = new UrlHelper(request.RequestContext);
                //利用Action 指定的操作名称、控制器名称和路由值生成操作方法的完全限定 URL。
                string returnUrl = urlHelper.Action("Index", "Home", new { returnUrl = "", message = message });
                actionResult = new RedirectResult(returnUrl);
            }
            XUtils.Log(httpContext);

            authorizationContext.Result = actionResult;
        }
    }

    /// <summary>
    /// 登录验证，验证权限是否大于辅助登记员
    /// https://www.cnblogs.com/xinbaba/p/8194142.html
    /// </summary>
    public class NotLowUser : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext authorizationContext)
        {
            var httpContext = authorizationContext.HttpContext;
            var request = httpContext.Request;
            var session = HttpContext.Current.Session;

            ActionResult actionResult = null;
            string message = string.Empty;
            

            /// 如果开启调试模式，直接赋值登录用户给Session
            if ((bool) XUtils.GetAppSetting("Debug", typeof(bool)) && session[ "role" ] == null)
            {
                var user = XUtils.GetAdminUser();

                if (user != null)
                {
                    session[ "id" ] = user.Id;
                    session[ "login_name" ] = user.Login_name;
                    session[ "role" ] = 3;
                    session[ "nickname" ] = user.Nickname;
                }
                else
                {
                    UrlHelper urlHelper = new UrlHelper(request.RequestContext);
                    //利用Action 指定的操作名称、控制器名称和路由值生成操作方法的完全限定 URL。
                    string returnUrl = urlHelper.Action("Index", "Home", new { returnUrl = "", message = message });
                    actionResult = new RedirectResult(returnUrl);
                }
            }
            else if (session[ "role" ] == null || (int) session[ "role" ] < 2)
            {
                UrlHelper urlHelper = new UrlHelper(request.RequestContext);
                //利用Action 指定的操作名称、控制器名称和路由值生成操作方法的完全限定 URL。
                string returnUrl = urlHelper.Action("Index", "Home", new { returnUrl = "", message = message });
                actionResult = new RedirectResult(returnUrl);
            }
            XUtils.Log(httpContext);
            authorizationContext.Result = actionResult;
        }
    }
}