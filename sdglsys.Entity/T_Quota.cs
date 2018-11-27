using SqlSugar;

namespace sdglsys.Entity
{
    /// <summary>
    /// 基础配额信息表
    /// </summary>
    public class T_Quota
    {
        private System.Int32 _Quota_id;

        /// <summary>
        /// 基础配额ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public System.Int32 Quota_id { get { return this._Quota_id; } set { this._Quota_id = value; } }

        private System.DateTime _Quota_post_date = System.DateTime.Now;

        /// <summary>
        /// 更新时间
        /// </summary>
        public System.DateTime Quota_post_date { get { return this._Quota_post_date; } set { this._Quota_post_date = value; } }

        private System.Single _Quota_cold_water_value;

        /// <summary>
        /// 冷水配额
        /// </summary>
        public System.Single Quota_cold_water_value { get { return this._Quota_cold_water_value; } set { this._Quota_cold_water_value = value; } }

        private System.Single _Quota_hot_water_value;

        /// <summary>
        /// 热水配额
        /// </summary>
        public System.Single Quota_hot_water_value { get { return this._Quota_hot_water_value; } set { this._Quota_hot_water_value = value; } }

        private System.Single _Quota_electric_value;

        /// <summary>
        /// 电力配额
        /// </summary>
        public System.Single Quota_electric_value { get { return this._Quota_electric_value; } set { this._Quota_electric_value = value; } }

        private System.String _Quota_note;

        /// <summary>
        /// 备注
        /// </summary>
        public System.String Quota_note { get { return this._Quota_note; } set { this._Quota_note = value?.Trim(); } }

        private System.Boolean _Quota_is_active = true;

        /// <summary>
        /// 状态：1激活，0注销
        /// </summary>
        public System.Boolean Quota_is_active { get { return this._Quota_is_active; } set { this._Quota_is_active = value; } }

        private System.Boolean _Quota_model_state = true;

        /// <summary>
        /// 对象状态：1正常，0已删除
        /// </summary>
        public System.Boolean Quota_model_state { get { return this._Quota_model_state; } set { this._Quota_model_state = value; } }
    }
}