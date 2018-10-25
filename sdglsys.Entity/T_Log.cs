using SqlSugar;

namespace sdglsys.Entity
{
    /// <summary>
    /// 日志信息表
    /// </summary>
    public class T_Log
    {
        private System.Int32 _Log_id;
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public System.Int32 Log_id { get { return this._Log_id; } set { this._Log_id = value; } }

        private System.String _Log_login_name;
        /// <summary>
        /// 用户名
        /// </summary>
        public System.String Log_login_name { get { return this._Log_login_name; } set { this._Log_login_name = value?.Trim(); } }

        private System.String _Log_ip;
        /// <summary>
        /// 操作IP
        /// </summary>
        public System.String Log_ip { get { return this._Log_ip; } set { this._Log_ip = value?.Trim(); } }

        private System.String _Log_info;
        /// <summary>
        /// 日志信息
        /// </summary>
        public System.String Log_info { get { return this._Log_info; } set { this._Log_info = value?.Trim(); } }

        private System.DateTime _Log_post_date=System.DateTime.Now;
        /// <summary>
        /// 发生时间，默认取当前系统时间
        /// </summary>
        public System.DateTime Log_post_date { get { return this._Log_post_date; } set { this._Log_post_date = value; } }
    }
}