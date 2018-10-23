using SqlSugar;

namespace sdglsys.Entity
{
    /// <summary>
    /// 费率信息表
    /// </summary>
    public class T_Rate
    {
        private System.Int32 _Rate_id;
        /// <summary>
        /// 费率ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public System.Int32 Rate_id { get { return this._Rate_id; } set { this._Rate_id = value; } }

        private System.DateTime _Rate_post_date=System.DateTime.Now;
        /// <summary>
        /// 更新时间
        /// </summary>
        public System.DateTime Rate_post_date { get { return this._Rate_post_date; } set { this._Rate_post_date = value; } }

        private System.Single _Rate_cold_water_value;
        /// <summary>
        /// 冷水费率
        /// </summary>
        public System.Single Rate_cold_water_value { get { return this._Rate_cold_water_value; } set { this._Rate_cold_water_value = value; } }

        private System.Single _Rate_hot_water_value;
        /// <summary>
        /// 热水费率
        /// </summary>
        public System.Single Rate_hot_water_value { get { return this._Rate_hot_water_value; } set { this._Rate_hot_water_value = value; } }

        private System.Single _Rate_electric_value;
        /// <summary>
        /// 电力费率
        /// </summary>
        public System.Single Rate_electric_value { get { return this._Rate_electric_value; } set { this._Rate_electric_value = value; } }

        private System.String _Rate_note;
        /// <summary>
        /// 备注
        /// </summary>
        public System.String Rate_note { get { return this._Rate_note; } set { this._Rate_note = value?.Trim(); } }

        private System.Boolean _Rate_is_active=true;
        /// <summary>
        /// 状态：1激活，0注销
        /// </summary>
        public System.Boolean Rate_is_active { get { return this._Rate_is_active; } set { this._Rate_is_active = value; } }

        private System.Boolean _Rate_model_state=true;
        /// <summary>
        /// 对象状态：1正常，0已删除
        /// </summary>
        public System.Boolean Rate_model_state { get { return this._Rate_model_state; } set { this._Rate_model_state = value; } }
    }
}