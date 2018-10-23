using SqlSugar;

namespace sdglsys.Entity
{
    /// <summary>
    /// 用量登记表
    /// </summary>
    public class T_Used
    {
        private System.Int32 _Used_id;
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public System.Int32 Used_id { get { return this._Used_id; } set { this._Used_id = value; } }

        private System.Int32 _Used_room_id;
        /// <summary>
        /// 宿舍ID
        /// </summary>
        public System.Int32 Used_room_id { get { return this._Used_room_id; } set { this._Used_room_id = value; } }

        private System.Int32 _Used_dorm_id;
        /// <summary>
        /// 园区ID
        /// </summary>
        public System.Int32 Used_dorm_id { get { return this._Used_dorm_id; } set { this._Used_dorm_id = value; } }

        private System.Int32 _Used_building_id;
        /// <summary>
        /// 宿舍楼ID
        /// </summary>
        public System.Int32 Used_building_id { get { return this._Used_building_id; } set { this._Used_building_id = value; } }

        private System.DateTime _Used_post_date = System.DateTime.Now;
        /// <summary>
        /// 登记时间
        /// </summary>
        public System.DateTime Used_post_date { get { return this._Used_post_date; } set { this._Used_post_date = value; } }

        private System.Int32 _Used_post_user_id;
        /// <summary>
        /// 登记者用户ID
        /// </summary>
        public System.Int32 Used_post_user_id { get { return this._Used_post_user_id; } set { this._Used_post_user_id = value; } }

        private System.Single _Used_cold_water_value;
        /// <summary>
        /// 冷水用量
        /// </summary>
        public System.Single Used_cold_water_value { get { return this._Used_cold_water_value; } set { this._Used_cold_water_value = value; } }

        private System.Single _Used_hot_water_value;
        /// <summary>
        /// 热水用量
        /// </summary>
        public System.Single Used_hot_water_value { get { return this._Used_hot_water_value; } set { this._Used_hot_water_value = value; } }

        private System.Single _Used_electric_value;
        /// <summary>
        /// 用电量
        /// </summary>
        public System.Single Used_electric_value { get { return this._Used_electric_value; } set { this._Used_electric_value = value; } }

        private System.String _Used_note;
        /// <summary>
        /// 备注
        /// </summary>
        public System.String Used_note { get { return this._Used_note; } set { this._Used_note = value?.Trim(); } }

        private System.Boolean _Used_is_active=true;
        /// <summary>
        /// 状态：1激活，0注销
        /// </summary>
        public System.Boolean Used_is_active { get { return this._Used_is_active; } set { this._Used_is_active = value; } }

        private System.Boolean _Used_model_state=true;
        /// <summary>
        /// 对象状态：1正常，0已删除
        /// </summary>
        public System.Boolean Used_model_state { get { return this._Used_model_state; } set { this._Used_model_state = value; } }
    }
}