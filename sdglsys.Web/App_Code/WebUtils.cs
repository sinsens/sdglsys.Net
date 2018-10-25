using System;
using System.Web;

namespace sdglsys.Web
{
    /// <summary>
    /// 常用工具集
    /// </summary>
    public class WebUtils:Utils.Utils
    {
        public static bool IsTrial()
        {
            DateTime trial_end_date = new DateTime();
            DateTime.TryParse("2019-12-30 23:59", out trial_end_date);
            return DateTime.Now < trial_end_date;
        }


        /// <summary>
        /// 检查是否为敏感操作
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns>false</returns>
        public bool NeedAudit(string url)
        {
            url = url.ToLower();
            // 敏感操作列表
            string[] permit_list = {
                "create","edit","delete","pay","createusedinfo","ediT_Usedinfo","deleteusedinfo","reset","update","quota","rate","charts","upload"
            };
            foreach (var item in permit_list)
            {
                if (url.Contains(item))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="log"></param>
        public void Log(Entity.T_Log log) {
            new DbHelper.Logs().Add(log);
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="login_name">用户名</param>
        /// <param name="ip">操作IP</param>
        /// <param name="info">日志信息</param>
        public void Log(string login_name,string ip, string info) {
            Log(new Entity.T_Log {
                 Log_info = info, Log_ip = ip, Log_login_name = login_name
            });
        }

        /// <summary>
        /// 按需求记录日志
        /// </summary>
        /// <param name="httpContext">页面对象</param>
        public void Log(HttpContextBase httpContext)
        {
            var request = httpContext.Request;
            var session = HttpContext.Current.Session;
            if (NeedAudit(request.Url.PathAndQuery) && request.HttpMethod.Equals("POST")) // 检查是否为敏感操作（涉及数据的增删改操作）
            {
                /// 加入日志
                Log(new Entity.T_Log()
                {
                    Log_info = request.Url.PathAndQuery,
                    Log_ip = request.UserHostAddress,
                    Log_login_name = (string)session["login_name"]
                });
            }
        }

    }
}
