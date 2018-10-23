using SqlSugar;

namespace sdglsys.Entity
{
    /// <summary>
    /// Token验证表
    /// </summary>
    public class T_Token
    {
        private System.String _Token_id;
        /// <summary>
        /// Token ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        public System.String Token_id { get { return this._Token_id; } set { this._Token_id = value?.Trim(); } }

        private System.Int32 _Token_user_id;
        /// <summary>
        /// 用户ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        public System.Int32 Token_user_id { get { return this._Token_user_id; } set { this._Token_user_id = value; } }

        private System.DateTime _Token_login_date=System.DateTime.Now;
        /// <summary>
        /// 登录时间
        /// </summary>
        public System.DateTime Token_login_date { get { return this._Token_login_date; } set { this._Token_login_date = value; } }

        private System.DateTime _Token_expired_date=System.DateTime.Now.AddDays(30);
        /// <summary>
        /// Token过期时间，默认1个月
        /// </summary>
        public System.DateTime Token_expired_date { get { return this._Token_expired_date; } set { this._Token_expired_date = value; } }
    }
}