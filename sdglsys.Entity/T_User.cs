using SqlSugar;

namespace sdglsys.Entity
{
    /// <summary>
    /// 系统角色信息表
    /// </summary>
    public class T_User
    {
        private System.Int32 _User_id;
        /// <summary>
        /// 用户ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public System.Int32 User_id { get { return this._User_id; } set { this._User_id = value; } }

        private System.String _User_login_name;
        /// <summary>
        /// 用户名
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        public System.String User_login_name { get { return this._User_login_name; } set { this._User_login_name = value?.Trim(); } }

        private System.String _User_nickname;
        /// <summary>
        /// 姓名
        /// </summary>
        public System.String User_nickname { get { return this._User_nickname; } set { this._User_nickname = value?.Trim(); } }

        private System.String _User_pwd;
        /// <summary>
        /// 密码
        /// </summary>
        public System.String User_pwd { get { return this._User_pwd; } set { this._User_pwd = value?.Trim(); } }

        private System.String _User_note;
        /// <summary>
        /// 备注
        /// </summary>
        public System.String User_note { get { return this._User_note; } set { this._User_note = value?.Trim(); } }

        private System.String _User_phone;
        /// <summary>
        /// 联系电话
        /// </summary>
        public System.String User_phone { get { return this._User_phone; } set { this._User_phone = value?.Trim(); } }

        private System.Int32 _User_dorm_id;
        /// <summary>
        /// 所属园区ID
        /// </summary>
        public System.Int32 User_dorm_id { get { return this._User_dorm_id; } set { this._User_dorm_id = value; } }

        private System.Int32 _User_role=1;
        /// <summary>
        /// 权限：1辅助登记员，2宿舍管理员，3系统管理员
        /// </summary>
        public System.Int32 User_role { get { return this._User_role; } set { this._User_role = value; } }

        private System.DateTime _User_reg_date=System.DateTime.Now;
        /// <summary>
        /// 创建时间
        /// </summary>
        public System.DateTime User_reg_date { get { return this._User_reg_date; } set { this._User_reg_date = value; } }

        private System.Boolean _User_is_active=true;
        /// <summary>
        /// 状态：1激活，0注销
        /// </summary>
        public System.Boolean User_is_active { get { return this._User_is_active; } set { this._User_is_active = value; } }

        private System.Boolean _User_model_state=true;
        /// <summary>
        /// 对象状态：1正常，0已删除
        /// </summary>
        public System.Boolean User_model_state { get { return this._User_model_state; } set { this._User_model_state = value; } }
    }
}