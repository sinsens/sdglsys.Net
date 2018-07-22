using SqlSugar;
using System;

namespace sdglsys.Entity
{
    [SugarTable("t_log")]
    /// <summary>
    /// 日志
    /// </summary>
    public class TLog
    {
        private System.Int32 _Id;
        /// <summary>
        /// ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnDescription = "日志ID")]
        public System.Int32 Id { get { return this._Id; } set { this._Id = value; } }
        
        private System.String _Login_name;
        /// <summary>
        /// 用户名
        /// </summary>
        [SugarColumn(Length = 15,ColumnDescription = "用户名", ColumnDataType = "varchar")]
        public System.String Login_name { get { return this._Login_name; } set { this._Login_name = value.Trim(); } }
        
        private System.String _Ip;
        /// <summary>
        /// 操作IP
        /// </summary>
        [SugarColumn(Length = 20, ColumnDescription = "操作IP")]
        public System.String Ip { get { return this._Ip; } set { this._Ip = value.Trim(); } }

        private System.String _Info;
        /// <summary>
        /// 日志信息
        /// </summary>
        [SugarColumn(ColumnDataType = "text", ColumnDescription = "日志信息")]
        public System.String Info { get { return this._Info; } set { this._Info = value.Trim(); } }

        private System.DateTime _Log_date = DateTime.Now;
        /// <summary>
        /// 发生时间
        /// </summary>
        [SugarColumn(ColumnDescription ="发生时间")]
        public System.DateTime Log_date { get { return this._Log_date; } set { this._Log_date = value; } }
    }
}