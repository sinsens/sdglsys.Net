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
        public System.Int32 Id { get { return this._Id; } set { this._Id = value; } }

        private System.String _Login_name;
        /// <summary>
        /// 登录名
        /// </summary>
        public System.String Login_name { get { return this._Login_name; } set { this._Login_name = value.Trim(); } }

        private System.String _Nickname;
        /// <summary>
        /// 姓名
        /// </summary>
        public System.String Nickname { get { return this._Nickname; } set { this._Nickname = value.Trim(); } }

        private System.String _Pwd;
        /// <summary>
        /// 密码
        /// </summary>
        public System.String Pwd { get { return this._Pwd; } set { this._Pwd = value.Trim(); } }

        private System.String _Note;
        /// <summary>
        /// 备注
        /// </summary>
        public System.String Note { get { return this._Note; } set { this._Note = value.Trim(); } }

        private System.String _Phone;
        /// <summary>
        /// 联系方式
        /// </summary>
        public System.String Phone { get { return this._Phone; } set { this._Phone = value.Trim(); } }

        private System.Int32 _Pid;
        /// <summary>
        /// 所属园区ID
        /// </summary>
        public System.Int32 Pid { get { return this._Pid; } set { this._Pid = value; } }
        /// <summary>
        /// 园区名称
        /// </summary>
        public System.String DormName { get; set; }

        private System.Int32 _Role;
        /// <summary>
        /// 权限：1辅助登记员，2宿舍（园区）管理员，3系统管理员
        /// </summary>
        public System.Int32 Role { get { return this._Role; } set { this._Role = value; } }
        /// <summary>
        /// 权限：1辅助登记员，2宿舍（园区）管理员，3系统管理员
        /// </summary>
        public System.String RoleName { get; set; }

        private System.DateTime _Reg_date= System.DateTime.Now;
        /// <summary>
        /// 添加时间
        /// </summary>
        public System.DateTime Reg_date { get { return this._Reg_date; } set { this._Reg_date = value; } }

        private System.Boolean _Is_active = true;
        /// <summary>
        /// 状态
        /// </summary>
        public System.Boolean Is_active { get { return this._Is_active; } set { this._Is_active = value; } }
    }
}