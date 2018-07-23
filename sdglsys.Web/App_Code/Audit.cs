using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sdglsys.Web.App_Code
{
    public static class Audit
    {
        /// <summary>
        /// 检查是否为敏感操作
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns>false</returns>
        public static bool NeedAudit(string url)
        {
            url.ToLower();
            // 敏感操作列表
            string[] permit_list = {
                "create","edit","delete","pay","createusedinfo","editusedinfo","deleteusedinfo","reset","update","quota","rate","charts","upload"
            };
            foreach (var item in permit_list)
            {
                if (url.Contains(item))
                    return true;
            }
            return false;
        }
    }
}