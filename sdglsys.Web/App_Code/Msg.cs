using Newtonsoft.Json;

namespace sdglsys.Web
{
    public class Msg:Entity.Msg
    {
        /// <summary>
        /// 返回Json
        /// </summary>
        /// <returns></returns>
        public string ToJson() {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}