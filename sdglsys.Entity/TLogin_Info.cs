using SqlSugar;
using System;

namespace sdglsys.Entity
{
    [Serializable]
    [SugarTable("t_login_info")]
    /// <summary>
    /// 日志
    /// </summary>
    public class TLogin_Info
    {
        private System.String _Sid;
        /// <summary>
        /// Session ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "Session ID", ColumnName = "session_id")]
        public System.String Session_id { get { return this._Sid; } set { this._Sid = value; } }

        private System.Int32 _Uid;
        /// <summary>
        /// 用户ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "用户ID", ColumnName = "uid")]
        public System.Int32 Uid { get { return this._Uid; } set { this._Uid = value; } }

        private System.String _Login_name;
        /// <summary>
        /// 用户名
        /// </summary>
        [SugarColumn(Length = 15, ColumnDescription = "用户名", ColumnDataType = "varchar")]
        public System.String Login_name { get { return this._Login_name; } set { this._Login_name = value.Trim(); } }

        private System.String _Ip;
        /// <summary>
        /// 操作IP
        /// </summary>
        [SugarColumn(Length = 46, ColumnDescription = "操作IP")]
        public System.String Ip { get { return this._Ip; } set { this._Ip = value.Trim(); } }


        private System.DateTime _Log_date = DateTime.Now;
        /// <summary>
        /// 登录时间
        /// </summary>
        [SugarColumn(ColumnDescription = "登录时间")]
        public System.DateTime Login_date { get { return this._Log_date; } set { this._Log_date = value; } }

        private System.DateTime _Expired_Date = DateTime.Now.AddHours(2); // 默认2小时过期
        /// <summary>
        /// 身份认证过期时间（默认2小时过期）
        /// </summary>
        [SugarColumn(ColumnDescription = "身份认证过期时间")]
        public System.DateTime Expired_Date { get { return this._Expired_Date; } set { this._Expired_Date = value; } }
    }
}