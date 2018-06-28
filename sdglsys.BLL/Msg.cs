using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sdglsys.BLL
{
    /// <summary>
    /// 自定义Json消息
    /// </summary>
    public class Msg
    {
        public Msg()
        {
            code = 200;
        }
        public int code { get; set; }
        public string msg { get; set; }
        public object content { get; set; }

        /// <summary>
        /// Json化对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string toJson(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }

        /// <summary>
        /// 返回Json
        /// </summary>
        /// <returns></returns>
        public string toJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
