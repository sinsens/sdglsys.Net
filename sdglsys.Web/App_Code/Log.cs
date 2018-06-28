using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sdglsys.Web.App_Code
{
    public class Log
    {
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="login_name"></param>
        /// <param name="info"></param>
        public static void LogThis(string ip, string login_name, string info) {
            var Log = new BLL.Logs();
            Log.Add(ip, login_name, info);
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="info"></param>
        public static void LogThis(HttpContext httpContext, string info) {
            var request = httpContext.Request;
            var session = HttpContext.Current.Session;
            LogThis(request.UserHostAddress, (string)session["login_name"], info);
        }
    }
}