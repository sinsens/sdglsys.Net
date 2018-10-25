namespace sdglsys.Entity
{
    [System.Serializable]
    /// <summary>
    /// 自定义Json消息
    /// </summary>
    public class Msg
    {
        public Msg()
        {
            Code = 0;
        }
        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 响应代码：正常0，错误-1
        /// </summary>
        public sbyte Code { get; set; }
        /// <summary>
        /// 响应提示文本
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 响应内容
        /// </summary>
        public object Content { get; set; }
        private System.DateTime t = System.DateTime.Now;
        /// <summary>
        /// 执行用时，单位：ms
        /// </summary>
        public ushort Exetime
        {
            get
            {
                return System.Convert.ToUInt16((System.DateTime.Now.Ticks - t.Ticks)/ 10000);
            }
        }

    }
}
