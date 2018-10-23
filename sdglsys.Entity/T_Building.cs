using SqlSugar;

namespace sdglsys.Entity
{
    /// <summary>
    /// 宿舍楼信息表
    /// </summary>
    public class T_Building
    {
        private System.Int32 _Building_id;
        /// <summary>
        /// 宿舍楼ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public System.Int32 Building_id { get { return this._Building_id; } set { this._Building_id = value; } }

        private System.Int32 _Building_dorm_id;
        /// <summary>
        /// 园区ID
        /// </summary>
        public System.Int32 Building_dorm_id { get { return this._Building_dorm_id; } set { this._Building_dorm_id = value; } }

        private System.String _Building_vid;
        /// <summary>
        /// 宿舍楼编号
        /// </summary>
        public System.String Building_vid { get { return this._Building_vid; } set { this._Building_vid = value?.Trim(); } }

        private System.String _Building_nickname;
        /// <summary>
        /// 宿舍楼名称
        /// </summary>
        public System.String Building_nickname { get { return this._Building_nickname; } set { this._Building_nickname = value?.Trim(); } }

        private System.String _Building_note;
        /// <summary>
        /// 备注
        /// </summary>
        public System.String Building_note { get { return this._Building_note; } set { this._Building_note = value?.Trim(); } }

        private System.Boolean _Building_is_active=true;
        /// <summary>
        /// 状态：1激活，0注销
        /// </summary>
        public System.Boolean Building_is_active { get { return this._Building_is_active; } set { this._Building_is_active = value; } }

        private System.Boolean _Building_model_state=true;
        /// <summary>
        /// 对象状态：1正常，0已删除
        /// </summary>
        public System.Boolean Building_model_state { get { return this._Building_model_state; } set { this._Building_model_state = value; } }
    }
}