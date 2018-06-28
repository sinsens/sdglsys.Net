using SqlSugar;

namespace sdglsys.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public class Log
    {
        private System.Int32 _Id;
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public System.Int32 Id { get { return this._Id; } set { this._Id = value; } }

        private System.String _Login_name;
        /// <summary>
        /// 
        /// </summary>
        public System.String Login_name { get { return this._Login_name; } set { this._Login_name = value?.Trim(); } }

        private System.String _Ip;
        /// <summary>
        /// 
        /// </summary>
        public System.String Ip { get { return this._Ip; } set { this._Ip = value?.Trim(); } }

        private System.String _Info;
        /// <summary>
        /// 
        /// </summary>
        public System.String Info { get { return this._Info; } set { this._Info = value?.Trim(); } }

        private System.DateTime? _Log_date;
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? Log_date { get { return this._Log_date; } set { this._Log_date = value; } }
    }
}