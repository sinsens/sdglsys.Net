using SqlSugar;

namespace sdglsys.Entity
{
    /// <summary>
    /// 园区信息表
    /// </summary>
    public class T_Dorm
    {
        private System.Int32 _Dorm_id;
        /// <summary>
        /// 园区ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public System.Int32 Dorm_id { get { return this._Dorm_id; } set { this._Dorm_id = value; } }

        private System.String _Dorm_nickname;
        /// <summary>
        /// 园区名称
        /// </summary>
        public System.String Dorm_nickname { get { return this._Dorm_nickname; } set { this._Dorm_nickname = value?.Trim(); } }

        private System.Boolean _Dorm_type=true;
        /// <summary>
        /// 园区类型：0女，1男，默认1
        /// </summary>
        public System.Boolean Dorm_type { get { return this._Dorm_type; } set { this._Dorm_type = value; } }

        private System.String _Dorm_note;
        /// <summary>
        /// 备注
        /// </summary>
        public System.String Dorm_note { get { return this._Dorm_note; } set { this._Dorm_note = value?.Trim(); } }

        private System.Boolean _Dorm_is_active=true;
        /// <summary>
        /// 状态：1激活，0注销
        /// </summary>
        public System.Boolean Dorm_is_active { get { return this._Dorm_is_active; } set { this._Dorm_is_active = value; } }

        private System.Boolean _Dorm_model_state=true;
        /// <summary>
        /// 对象状态：1正常，0已删除
        /// </summary>
        public System.Boolean Dorm_model_state { get { return this._Dorm_model_state; } set { this._Dorm_model_state = value; } }
    }
}