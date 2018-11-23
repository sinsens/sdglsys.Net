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
            if (session["login_name"] == null)
            {
                UrlHelper urlHelper = new UrlHelper(request.RequestContext);
                //利用Action 指定的操作名称、控制器名称和路由值生成操作方法的完全限定 URL。
                string returnUrl = urlHelper.Action("Index", "Home", new { returnUrl = "", message = message });
                actionResult = new RedirectResult(returnUrl);
            }
            new WebUtils().Log(httpContext);
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
            if (session["role"] == null)
            {


                UrlHelper urlHelper = new UrlHelper(request.RequestContext);
                //利用Action 指定的操作名称、控制器名称和路由值生成操作方法的完全限定 URL。
                string returnUrl = urlHelper.Action("Index", "Home", new { returnUrl = "", message });
                actionResult = new RedirectResult(returnUrl);
            }
            else if ((int)session["role"] < 3)
            {
                /// 已登录但不是系统管理员
                var response = httpContext.Response;
                response.Write(new Entity.Msg
                {
                    Code = -1,
                    Message = "权限不足"
                });
                response.End();
                return;
            }
            new WebUtils().Log(httpContext);

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
            if (session["role"] == null)
            {
                UrlHelper urlHelper = new UrlHelper(request.RequestContext);
                //利用Action 指定的操作名称、控制器名称和路由值生成操作方法的完全限定 URL。
                string returnUrl = urlHelper.Action("Index", "Home", new { returnUrl = "", message });
                actionResult = new RedirectResult(returnUrl);
            }
            else if ((int)session["role"] < 2)
            {
                /// 已登录但不是系统管理员
                var response = httpContext.Response;
                response.Write(new Entity.Msg
                {
                    Code = -1,
                    Message = "权限不足"
                });
                response.End();
                return;
            }
            new WebUtils().Log(httpContext);
            authorizationContext.Result = actionResult;
        }
    }

    /// <summary>
    /// 自动登录
    /// </summary>
    public class AutoLogin:AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext authorizationContext)
        {
            var httpContext = authorizationContext.HttpContext;
            var request = httpContext.Request;
            var session = HttpContext.Current.Session;
            if (session["login_name"] == null && request["token"] != null)
            {
                /*
                Cookies中有Token
                */
                try
                {
                    var Token = new DbHelper.Token();
                    var token = Token.GetToken(request["token"]);
                    if (token != null && token.Token_expired_date > DateTime.Now) // 登录信息不是null且未过期
                    {
                        var user = Token.GetUserById(token.Token_id);
                        if (user != null)
                        {
                            session["id"] = user.User_id;
                            session["login_name"] = user.User_login_name;
                            session["nickname"] = user.User_nickname;
                            session["role"] = user.User_role;
                            session["pid"] = user.User_dorm_id;
                            /// 预防身份过期
                            token.Token_expired_date = DateTime.Now.AddDays(30); // 设置token为30天后过期
                            token.Token_login_date = DateTime.Now;
                            Token.Update(token);
                            /// 记录日志
                            new WebUtils().Log(new Entity.T_Log
                            {
                                Log_info = "Login by token",
                                Log_ip = request.UserHostAddress,
                                Log_login_name = user.User_login_name
                            });
                            session["token"] = token.Token_id;
                            httpContext.Server.TransferRequest(httpContext.Request.Url.ToString()); // 自动跳转到需要的页面，而不是以前的首页
                            //httpContext.Response.End();
                        }
                    }
                }
                catch
                {

                }
            }
        }
    }
}