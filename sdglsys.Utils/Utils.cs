using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace sdglsys.Utils
{
    public class Utils
    {
        /// <summary>
        /// 生成md5
        /// https://coderwall.com/p/4puszg/c-convert-string-to-md5-hash
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string HashMD5(string input)
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
        /// 不可逆加密密码或字符串
        /// </summary>
        /// <param name="pwd">密码或字符串</param>
        /// <returns>加密后的密码或字符串hash</returns>
        public string HashPassword(string pwd)
        {
            return BCrypt.Net.BCrypt.HashPassword(HashMD5(pwd), 4);
        }


        /// <summary>
        /// 使用Bcrypt加密
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public string HashBcrypt(string pwd) {
            return BCrypt.Net.BCrypt.HashPassword(pwd, 4);
        }

        /// <summary>
        /// Json化对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string ToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }

        /// <summary>
        /// 密码验证
        /// </summary>
        /// <param name="pwd">密码明文</param>
        /// <param name="hashpwd">密码hash</param>
        /// <returns></returns>
        public bool CheckPasswd(string pwd, string hashpwd)
        {
            return BCrypt.Net.BCrypt.Verify(pwd, hashpwd);
        }

        /// <summary>
        /// 获取App配置信息
        /// </summary>
        /// <param name="key">App配置节点名称(Key)</param>
        /// <param name="type">类型：typeof(string,bool,int)</param>
        /// <returns></returns>
        public object GetAppSetting(string key, Type type)
        {
            var setting = new AppSettingsReader();
            return setting.GetValue(key, type);
        }

    }
}
