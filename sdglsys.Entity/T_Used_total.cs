using SqlSugar;

namespace sdglsys.Entity
{
    /// <summary>
    /// 宿舍水电表读数表
    /// </summary>
    public class T_Used_total
    {
        private System.Int32 _Ut_id;
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public System.Int32 Ut_id { get { return this._Ut_id; } set { this._Ut_id = value; } }

        private System.Int32 _Ut_room_id;
        /// <summary>
        /// 宿舍ID
        /// </summary>
        public System.Int32 Ut_room_id { get { return this._Ut_room_id; } set { this._Ut_room_id = value; } }

        private System.Int32 _Ut_dorm_id;
        /// <summary>
        /// 园区ID
        /// </summary>
        public System.Int32 Ut_dorm_id { get { return this._Ut_dorm_id; } set { this._Ut_dorm_id = value; } }

        private System.Int32 _Ut_building_id;
        /// <summary>
        /// 宿舍楼ID
        /// </summary>
        public System.Int32 Ut_building_id { get { return this._Ut_building_id; } set { this._Ut_building_id = value; } }

        private System.DateTime _Ut_post_date=System.DateTime.Now;
        /// <summary>
        /// 更新时间
        /// </summary>
        public System.DateTime Ut_post_date { get { return this._Ut_post_date; } set { this._Ut_post_date = value; } }

        private System.Single _Ut_cold_water_value;
        /// <summary>
        /// 冷水表读数
        /// </summary>
        public System.Single Ut_cold_water_value { get { return this._Ut_cold_water_value; } set { this._Ut_cold_water_value = value; } }

        private System.Single _Ut_hot_water_value;
        /// <summary>
        /// 热水表读数
        /// </summary>
        public System.Single Ut_hot_water_value { get { return this._Ut_hot_water_value; } set { this._Ut_hot_water_value = value; } }

        private System.Single _Ut_electric_value;
        /// <summary>
        /// 电表读数
        /// </summary>
        public System.Single Ut_electric_value { get { return this._Ut_electric_value; } set { this._Ut_electric_value = value; } }

        private System.String _Ut_note;
        /// <summary>
        /// 备注
        /// </summary>
        public System.String Ut_note { get { return this._Ut_note; } set { this._Ut_note = value?.Trim(); } }

        private System.Boolean _Ut_model_state=true;
        /// <summary>
        /// 对象状态：1正常，0已删除
        /// </summary>
        public System.Boolean Ut_model_state { get { return this._Ut_model_state; } set { this._Ut_model_state = value; } }
    }
}