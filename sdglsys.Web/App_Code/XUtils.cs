using System;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace sdglsys.Web
{
    /// <summary>
    /// 常用工具集
    /// </summary>
    public class XUtils
    {
        /// <summary>
        /// 生成md5
        /// https://coderwall.com/p/4puszg/c-convert-string-to-md5-hash
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMD5(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }

        /// <summary>
        /// 密码验证
        /// </summary>
        /// <param name="pwd">密码明文</param>
        /// <param name="hashpwd">密码hash</param>
        /// <returns></returns>
        public static bool checkpw(string pwd, string hashpwd)
        {
            return BCrypt.Net.BCrypt.Verify(pwd, hashpwd);
        }


        /// <summary>
        /// 不可逆加密密码或字符串
        /// </summary>
        /// <param name="pwd">密码或字符串</param>
        /// <returns>加密后的密码或字符串hash</returns>
        public static string hashpwd(string pwd)
        {
            return BCrypt.Net.BCrypt.HashPassword(pwd, 4);
        }

        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="login_name">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public static Entity.TUser Login(string login_name, string pwd)
        {
            DbHelper.Users u = new DbHelper.Users();
            Entity.TUser user = u.findByLoginName(login_name);
            return (user == null || user.Is_active == false || checkpw(pwd, user.Pwd) == false) ? null : user;
        }

        /// <summary>
        /// 获取App配置信息
        /// </summary>
        /// <param name="key">App配置节点名称(Key)</param>
        /// <param name="type">类型：typeof(string,bool,int)</param>
        /// <returns></returns>
        public static object GetAppSetting(string key, Type type)
        {
            var setting = new AppSettingsReader();
            return setting.GetValue(key, type);
        }

        /// <summary>
        /// Json化对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }

        public static bool OutTrial()
        {
            DateTime trial_end_date;
            DateTime.TryParse("2019-12-30 20:51", out trial_end_date);
            return DateTime.Now >= trial_end_date;
        }

        public static bool IsTrial {
            get {
                return OutTrial()?false:true;
            }
        }

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

        /// <summary>
        /// 按需求记录日志
        /// </summary>
        /// <param name="httpContext">页面对象</param>
        public static void Log(HttpContextBase httpContext) {
            var request = httpContext.Request;
            var session = HttpContext.Current.Session;
            if (NeedAudit(request.Url.AbsolutePath)) // 检查是否为敏感操作（涉及数据的增删改操作）
            {
                /// 加入日志
                Log(new Entity.TLog()
                {
                    Info = request.Url.PathAndQuery,
                    Ip = request.UserHostAddress,
                    Login_name = (string) session["login_name"]
                });
            }
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="log"></param>
        public static void Log(Entity.TLog log) {
            new DbHelper.Logs().Add(log);
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="login_name">用户名</param>
        /// <param name="ip">操作IP</param>
        /// <param name="info">日志信息</param>
        public static void Log(string login_name,string ip, string info) {
            Log(new Entity.TLog {
                 Info = info, Ip = ip, Login_name = login_name
            });
        }

        /// <summary>
        /// 获取一个系统管理员权限的角色
        /// </summary>
        /// <returns></returns>
        public static Entity.TUser GetAdminUser() {
            return new DbHelper.Users().Db.Queryable<Entity.TUser>().Where(u => u.Is_active == true && u.Role == 3).First();
        }
    }
}
