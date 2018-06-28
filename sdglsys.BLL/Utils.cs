using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sdglsys.BLL
{
    public class Utils
    {
        /// <summary>
        /// 密码验证
        /// </summary>
        /// <param name="pwd"></param>
        /// <param name="hashpwd"></param>
        /// <returns></returns>
        public static bool checkpw(string pwd, string hashpwd) {
            return BCrypt.Net.BCrypt.Verify(pwd, hashpwd);
        }

        public static string hashpwd(string pwd) {
            return BCrypt.Net.BCrypt.HashPassword(pwd, 4);
        }

        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="login_name">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public static Entity.Users Login(string login_name, string pwd) {
            Users u = new Users();
            Entity.Users user = u.findByLoginName(login_name);
            if (user == null)
                return null;
            if (user.Is_active == false || checkpw(pwd, user.Pwd) == false)
                return null;
            return user;
        }

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object getSetting(string key, Type type) {
            var setting = new AppSettingsReader();
            return setting.GetValue(key, type);
        }

    }
}
