using SqlSugar;

namespace sdglsys.Entity
{
    /// <summary>
    /// 账单信息表
    /// </summary>
    public class T_Bill
    {
        private System.Int32 _Bill_id;
        /// <summary>
        /// 账单ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public System.Int32 Bill_id { get { return this._Bill_id; } set { this._Bill_id = value; } }

        private System.Int32 _Bill_used_id;
        /// <summary>
        /// 用量登记ID
        /// </summary>
        public System.Int32 Bill_used_id { get { return this._Bill_used_id; } set { this._Bill_used_id = value; } }

        private System.Int32 _Bill_quota_id;
        /// <summary>
        /// 基础配额ID
        /// </summary>
        public System.Int32 Bill_quota_id { get { return this._Bill_quota_id; } set { this._Bill_quota_id = value; } }

        private System.Int32 _Bill_rates_id;
        /// <summary>
        /// 费率ID
        /// </summary>
        public System.Int32 Bill_rates_id { get { return this._Bill_rates_id; } set { this._Bill_rates_id = value; } }

        private System.Int32 _Bill_dorm_id;
        /// <summary>
        /// 园区ID
        /// </summary>
        public System.Int32 Bill_dorm_id { get { return this._Bill_dorm_id; } set { this._Bill_dorm_id = value; } }

        private System.Int32 _Bill_building_id;
        /// <summary>
        /// 宿舍楼ID
        /// </summary>
        public System.Int32 Bill_building_id { get { return this._Bill_building_id; } set { this._Bill_building_id = value; } }

        private System.Int32 _Bill_room_id;
        /// <summary>
        /// 宿舍ID
        /// </summary>
        public System.Int32 Bill_room_id { get { return this._Bill_room_id; } set { this._Bill_room_id = value; } }

        private System.Decimal _Bill_cold_water_cost;
        /// <summary>
        /// 冷水费用
        /// </summary>
        public System.Decimal Bill_cold_water_cost { get { return this._Bill_cold_water_cost; } set { this._Bill_cold_water_cost = value; } }

        private System.Decimal _Bill_hot_water_cost;
        /// <summary>
        /// 热水费用
        /// </summary>
        public System.Decimal Bill_hot_water_cost { get { return this._Bill_hot_water_cost; } set { this._Bill_hot_water_cost = value; } }

        private System.Decimal _Bill_electric_cost;
        /// <summary>
        /// 电费
        /// </summary>
        public System.Decimal Bill_electric_cost { get { return this._Bill_electric_cost; } set { this._Bill_electric_cost = value; } }

        private System.String _Bill_note;
        /// <summary>
        /// 备注
        /// </summary>
        public System.String Bill_note { get { return this._Bill_note; } set { this._Bill_note = value?.Trim(); } }

        private System.DateTime _Bill_post_date=System.DateTime.Now;
        /// <summary>
        /// 生成时间
        /// </summary>
        public System.DateTime Bill_post_date { get { return this._Bill_post_date; } set { this._Bill_post_date = value; } }

        private System.DateTime _Bill_mod_date=System.DateTime.Now;
        /// <summary>
        /// 修改时间
        /// </summary>
        public System.DateTime Bill_mod_date { get { return this._Bill_mod_date; } set { this._Bill_mod_date = value; } }

        private System.SByte _Bill_is_active=1;
        /// <summary>
        /// 状态：0已注销，1已登记，2已结算
        /// </summary>
        public System.SByte Bill_is_active { get { return this._Bill_is_active; } set { this._Bill_is_active = value; } }

        private System.Boolean _Bill_model_state=true;
        /// <summary>
        /// 对象状态：1正常，0已删除
        /// </summary>
        public System.Boolean Bill_model_state { get { return this._Bill_model_state; } set { this._Bill_model_state = value; } }
    }
}