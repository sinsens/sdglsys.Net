using SqlSugar;

namespace sdglsys.Entity
{
    /// <summary>
    /// 系统角色表
    /// </summary>
    public class VUser
    {
        private System.Int32 _Id;
        /// <summary>
        /// ID
        /// </summary>
        public System.Int32 User_Id { get { return this._Id; } set { this._Id = value; } }

        private System.String _Login_name;
        /// <summary>
        /// 登录名
        /// </summary>
        public System.String User_Login_name { get { return this._Login_name; } set { this._Login_name = value.Trim(); } }

        private System.String _Nickname;
        /// <summary>
        /// 姓名
        /// </summary>
        public System.String User_Nickname { get { return this._Nickname; } set { this._Nickname = value.Trim(); } }

        private System.String _Pwd;
        /// <summary>
        /// 密码
        /// </summary>
        public System.String User_Pwd { get { return this._Pwd; } set { this._Pwd = value.Trim(); } }

        private System.String _Note;
        /// <summary>
        /// 备注
        /// </summary>
        public System.String User_Note { get { return this._Note; } set { this._Note = value.Trim(); } }

        private System.String _Phone;
        /// <summary>
        /// 联系方式
        /// </summary>
        public System.String User_Phone { get { return this._Phone; } set { this._Phone = value.Trim(); } }

        private System.Int32 _Pid;
        /// <summary>
        /// 所属园区ID
        /// </summary>
        public System.Int32 User_Dorm_id { get { return this._Pid; } set { this._Pid = value; } }
        /// <summary>
        /// 园区名称
        /// </summary>
        public System.String User_Dorm_Nickname { get; set; }

        private System.Int32 _Role;
        /// <summary>
        /// 权限：1辅助登记员，2宿舍（园区）管理员，3系统管理员
        /// </summary>
        public System.Int32 User_Role { get { return this._Role; } set { this._Role = value; } }
        /// <summary>
        /// 权限：1辅助登记员，2宿舍（园区）管理员，3系统管理员
        /// </summary>
        public System.String User_RoleName { get {
                return User_Role == 3 ? "系统管理员" : User_Role == 2 ? "园区管理员" : "辅助登记员";
            } set {

            } }

        private System.DateTime _Reg_date= System.DateTime.Now;
        /// <summary>
        /// 添加时间
        /// </summary>
        public System.DateTime User_Reg_date { get { return this._Reg_date; } set { this._Reg_date = value; } }

        private System.Boolean _Is_active = true;
        /// <summary>
        /// 状态
        /// </summary>
        public System.Boolean User_Is_active { get { return this._Is_active; } set { this._Is_active = value; } }
    }
}