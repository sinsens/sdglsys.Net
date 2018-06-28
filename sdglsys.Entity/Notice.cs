using SqlSugar;

namespace sdglsys.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public class Notice
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

        private System.String _Title;
        /// <summary>
        /// 
        /// </summary>
        public System.String Title { get { return this._Title; } set { this._Title = value?.Trim(); } }

        private System.String _Content;
        /// <summary>
        /// 
        /// </summary>
        public System.String Content { get { return this._Content; } set { this._Content = value?.Trim(); } }

        private System.DateTime? _Post_date;
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? Post_date { get { return this._Post_date; } set { this._Post_date = value; } }

        private System.DateTime? _Mod_date;
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? Mod_date { get { return this._Mod_date; } set { this._Mod_date = value; } }

        private System.Boolean? _Is_active;
        /// <summary>
        /// 
        /// </summary>
        public System.Boolean? Is_active { get { return this._Is_active; } set { this._Is_active = value; } }
    }
}