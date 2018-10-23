using SqlSugar;

namespace sdglsys.Entity
{
    /// <summary>
    /// 宿舍信息表
    /// </summary>
    public class T_Room
    {
        private System.Int32 _Room_id;
        /// <summary>
        /// 宿舍ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public System.Int32 Room_id { get { return this._Room_id; } set { this._Room_id = value; } }

        private System.Int32 _Room_building_id;
        /// <summary>
        /// 宿舍楼ID
        /// </summary>
        public System.Int32 Room_building_id { get { return this._Room_building_id; } set { this._Room_building_id = value; } }

        private System.Int32 _Room_dorm_id;
        /// <summary>
        /// 园区ID
        /// </summary>
        public System.Int32 Room_dorm_id { get { return this._Room_dorm_id; } set { this._Room_dorm_id = value; } }

        private System.String _Room_vid;
        /// <summary>
        /// 宿舍编号
        /// </summary>
        public System.String Room_vid { get { return this._Room_vid; } set { this._Room_vid = value?.Trim(); } }

        private System.String _Room_nickname;
        /// <summary>
        /// 宿舍名称
        /// </summary>
        public System.String Room_nickname { get { return this._Room_nickname; } set { this._Room_nickname = value?.Trim(); } }

        private System.String _Room_note;
        /// <summary>
        /// 备注
        /// </summary>
        public System.String Room_note { get { return this._Room_note; } set { this._Room_note = value?.Trim(); } }

        private System.Boolean _Room_is_active=true;
        /// <summary>
        /// 状态：1激活，0注销
        /// </summary>
        public System.Boolean Room_is_active { get { return this._Room_is_active; } set { this._Room_is_active = value; } }

        private System.Boolean _Room_model_state=true;
        /// <summary>
        /// 对象状态：1正常，0已删除
        /// </summary>
        public System.Boolean Room_model_state { get { return this._Room_model_state; } set { this._Room_model_state = value; } }

        private System.SByte _Number=0;
        /// <summary>
        /// 宿舍人数
        /// </summary>
        public System.SByte Number { get { return this._Number; } set { this._Number = value; } }
    }
}