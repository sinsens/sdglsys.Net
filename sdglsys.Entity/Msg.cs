namespace sdglsys.Entity
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
        private System.DateTime t = System.DateTime.Now;
        public double exetime
        {
            get
            {
                return System.Math.Round((double) (System.DateTime.Now.Ticks - t.Ticks) / 10000000, 5);
            }
        }

    }
}
