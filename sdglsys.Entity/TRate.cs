using SqlSugar;
using System;

namespace sdglsys.Entity
{
    [SugarTable("t_rate")]
    /// <summary>
    /// 费率
    /// </summary>
    public class TRate
    {
        private System.Int32 _Id;
        /// <summary>
        /// ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public System.Int32 Id { get { return this._Id; } set { this._Id = value; } }

        private System.DateTime _Post_date=DateTime.Now;
        /// <summary>
        /// 更新时间
        /// </summary>
        public System.DateTime Post_date { get { return this._Post_date; } set { this._Post_date = value; } }

        private System.Single _Hot_water_value;
        /// <summary>
        /// 热水费率
        /// </summary>
        public System.Single Hot_water_value { get { return this._Hot_water_value; } set { this._Hot_water_value = value; } }

        private System.Single _Cold_water_value;
        /// <summary>
        /// 冷水费率
        /// </summary>
        public System.Single Cold_water_value { get { return this._Cold_water_value; } set { this._Cold_water_value = value; } }

        private System.Single _Electric_value;
        /// <summary>
        /// 电量费率
        /// </summary>
        public System.Single Electric_value { get { return this._Electric_value; } set { this._Electric_value = value; } }

        private System.String _Note;
        /// <summary>
        /// 备注
        /// </summary>
        public System.String Note { get { return this._Note; } set { this._Note = value.Trim(); } }

        private System.Boolean _Is_active = true;
        /// <summary>
        /// 状态：0已注销，1启用，默认1
        /// </summary>
        public System.Boolean Is_active { get { return this._Is_active; } set { this._Is_active = value; } }
    }
}