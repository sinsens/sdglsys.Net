using SqlSugar;

namespace sdglsys.Entity
{
    [SugarTable("t_notice")]
    /// <summary>
    /// 公告通知
    /// </summary>
    public class TNotice
    {
        private System.Int32 _Id;
        /// <summary>
        /// ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public System.Int32 Id { get { return this._Id; } set { this._Id = value; } }

        private System.String _Login_name;
        /// <summary>
        /// 用户名
        /// </summary>
        public System.String Login_name { get { return this._Login_name; } set { this._Login_name = value.Trim(); } }

        private System.String _Title;
        /// <summary>
        /// 标题
        /// </summary>
        [SugarColumn(Length = 40, ColumnDataType = "varchar")]
        public System.String Title { get { return this._Title; } set { this._Title = value.Trim(); } }

        private System.String _Content;
        /// <summary>
        /// 内容
        /// </summary>
        public System.String Content { get { return this._Content; } set { this._Content = value.Trim(); } }

        private System.DateTime _Post_date = System.DateTime.Now;
        /// <summary>
        /// 发布时间
        /// </summary>
        public System.DateTime Post_date { get { return this._Post_date; } set { this._Post_date = value; } }

        private System.DateTime _Mod_date = System.DateTime.Now;
        /// <summary>
        /// 更新时间
        /// </summary>
        public System.DateTime Mod_date { get { return this._Mod_date; } set { this._Mod_date = value; } }

        private System.Boolean _Is_active = true;
        /// <summary>
        /// 状态：0已注销，1正常
        /// </summary>
        public System.Boolean Is_active { get { return this._Is_active; } set { this._Is_active = value; } }
    }
}