namespace sdglsys.Web
{
    /// <summary>
    /// API请求返回的数据对象
    /// 针对Layui的Table模块的返回数据
    /// 2018年7月2日 21点38分 sinsen
    /// </summary>
    public class ResponseData:sdglsys.Web.Msg
    {
        public int code { get; set; }
        /// <summary>
        /// 响应记录数
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 响应的数据
        /// </summary>
        public object data { get; set; }
    }
}
