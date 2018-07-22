using SqlSugar;
using System;

namespace sdglsys.Entity
{
    [SugarTable("t_bill")]
    /// <summary>
    /// 账单
    /// </summary>
    public class TBill
    {
        private System.Int32 _Id;
        /// <summary>
        /// 账单ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnDescription = "账单ID")]
        public System.Int32 Id { get { return this._Id; } set { this._Id = value; } }

        private System.Int32 _Pid;
        /// <summary>
        /// 用量登记ID
        /// </summary>
        [SugarColumn( IsNullable = false, ColumnDescription = "用量登记ID")]
        public System.Int32 Pid { get { return this._Pid; } set { this._Pid = value; } }

        private System.Int32 _Quota_id;
        /// <summary>
        /// 基础配额ID
        /// </summary>
        [SugarColumn(IsNullable = false, ColumnDescription = "基础配额ID")]
        public System.Int32 Quota_id { get { return this._Quota_id; } set { this._Quota_id = value; } }

        private System.Int32 _Rates_id;
        /// <summary>
        /// 费率ID
        /// </summary>
        [SugarColumn(IsNullable = false, ColumnDescription = "费率ID")]
        public System.Int32 Rates_id { get { return this._Rates_id; } set { this._Rates_id = value; } }

        private System.Int32 _Dorm_id;
        /// <summary>
        /// 园区ID
        /// </summary>
        [SugarColumn(IsNullable = false, ColumnDescription = "园区ID")]
        public System.Int32 Dorm_id { get { return this._Dorm_id; } set { this._Dorm_id = value; } }

        private System.Int32 _Building_id;
        /// <summary>
        /// 宿舍楼ID
        /// </summary>
        [SugarColumn(IsNullable = false, ColumnDescription = "宿舍楼ID")]
        public System.Int32 Building_id { get { return this._Building_id; } set { this._Building_id = value; } }

        private System.Int32 _Room_id;
        /// <summary>
        /// 宿舍ID
        /// </summary>
        public System.Int32 Room_id { get { return this._Room_id; } set { this._Room_id = value; } }

        private System.Decimal _Cold_water_cost;
        /// <summary>
        /// 冷水费
        /// </summary>
        public System.Decimal Cold_water_cost { get { return this._Cold_water_cost; } set { this._Cold_water_cost = value; } }

        private System.Decimal _Hot_water_cost;
        /// <summary>
        /// 热水费
        /// </summary>
        public System.Decimal Hot_water_cost { get { return this._Hot_water_cost; } set { this._Hot_water_cost = value; } }

        private System.Decimal _Electric_cost;
        /// <summary>
        /// 电费
        /// </summary>
        public System.Decimal Electric_cost { get { return this._Electric_cost; } set { this._Electric_cost = value; } }

        private System.String _Note;
        /// <summary>
        /// 备注
        /// </summary>
        public System.String Note { get { return this._Note; } set { this._Note = value.Trim(); } }

        private System.DateTime _Post_date=DateTime.Now;
        /// <summary>
        /// 生成日期
        /// </summary>
        public System.DateTime Post_date { get { return this._Post_date; } set { this._Post_date = value; } }

        private System.DateTime _Mod_date = DateTime.Now;
        /// <summary>
        /// 更新日期
        /// </summary>
        public System.DateTime Mod_date { get { return this._Mod_date; } set { this._Mod_date = value; } }

        private System.Int16 _Is_active=1;
        /// <summary>
        /// 账单状态，0已注销,1已登记,2已缴费
        /// </summary>
        public System.Int16 Is_active { get { return this._Is_active; } set { this._Is_active = value; } }
    }
}