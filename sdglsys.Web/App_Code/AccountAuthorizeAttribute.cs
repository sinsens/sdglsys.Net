using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sdglsys.DbHelper;
using sdglsys.Web;


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

            ActionResult actionResult = null;
            string message = string.Empty;
            var session = HttpContext.Current.Session;

            /// 如果开启调试模式，直接赋值登录用户给Session
            if ((bool) Utils.GetAppSetting("Debug", typeof(bool)) && session[ "login_name" ] == null)
            {
                var db = new DbHelper.Users().Db;
                var user = db.Queryable<Entity.TUser>().Where(u => u.Is_active == true && u.Role == 3).First();
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
            if (DbHelper.Utils.NeedAudit(request.Url.AbsolutePath)) // 检查是否为敏感操作（涉及数据的增删改操作）
            {
                /// 加入日志
                var Log = new DbHelper.Logs();
                Log.Add(new Entity.TLog()
                {
                    Info = request.Url.PathAndQuery,
                    Ip = request.UserHostAddress,
                    Login_name = (string) session[ "login_name" ]
                });
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
            if ((bool) Utils.GetAppSetting("Debug", typeof(bool)) && session[ "role" ] == null)
            {
                var db = new DbHelper.Users().Db;
                var user = db.Queryable<Entity.TUser>().Where(u => u.Is_active == true && u.Role == 3).First();
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
            if (DbHelper.Utils.NeedAudit(request.Url.AbsolutePath)) // 检查是否为敏感操作（涉及数据的增删改操作）
            {
                /// 加入日志
                var Log = new DbHelper.Logs();
                Log.Add(new Entity.TLog()
                {
                    Info = request.Url.PathAndQuery,
                    Ip = request.UserHostAddress,
                    Login_name = (string) session[ "login_name" ]
                });
            }

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

            ActionResult actionResult = null;
            string message = string.Empty;
            var session = HttpContext.Current.Session;

            /// 如果开启调试模式，直接赋值登录用户给Session
            if ((bool) Utils.GetAppSetting("Debug", typeof(bool)) && session[ "role" ] == null)
            {
                var db = new DbHelper.Users().Db;
                var user = db.Queryable<Entity.TUser>().Where(u => u.Is_active == true && u.Role == 3).First();

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
            if (DbHelper.Utils.NeedAudit(request.Url.AbsolutePath)) // 检查是否为敏感操作（涉及数据的增删改操作）
            {
                /// 加入日志
                var Log = new DbHelper.Logs();
                Log.Add(new Entity.TLog()
                {
                    Info = request.Url.PathAndQuery,
                    Ip = request.UserHostAddress,
                    Login_name = (string) session[ "login_name" ]
                });
            }
            authorizationContext.Result = actionResult;
        }
    }
}