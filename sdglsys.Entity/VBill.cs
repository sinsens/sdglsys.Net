using SqlSugar;
using System;

namespace sdglsys.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public class VBill
    {
        private System.Int32 _Id;
        /// <summary>
        /// 账单ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public System.Int32 Id { get { return this._Id; } set { this._Id = value; } }

        private System.Int32 _Pid;
        /// <summary>
        /// 用量登记ID
        /// </summary>
        public System.Int32 Pid { get { return this._Pid; } set { this._Pid = value; } }

        private System.Int32 _Quota_id;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 Quota_id { get { return this._Quota_id; } set { this._Quota_id = value; } }

        private System.Int32 _Rates_id;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 Rates_id { get { return this._Rates_id; } set { this._Rates_id = value; } }

        private System.Int32 _Dorm_id;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 Dorm_id { get { return this._Dorm_id; } set { this._Dorm_id = value; } }

        private System.Int32 _Building_id;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 Building_id { get { return this._Building_id; } set { this._Building_id = value; } }

        private System.Int32 _Room_id;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 Room_id { get { return this._Room_id; } set { this._Room_id = value; } }

        private System.Decimal _Cold_water_cost;
        /// <summary>
        /// 
        /// </summary>
        public System.Decimal Cold_water_cost { get { return this._Cold_water_cost; } set { this._Cold_water_cost = value; } }

        private System.Decimal _Hot_water_cost;
        /// <summary>
        /// 
        /// </summary>
        public System.Decimal Hot_water_cost { get { return this._Hot_water_cost; } set { this._Hot_water_cost = value; } }

        private System.Decimal _Electric_cost;
        /// <summary>
        /// 
        /// </summary>
        public System.Decimal Electric_cost { get { return this._Electric_cost; } set { this._Electric_cost = value; } }

        private System.String _Note;
        /// <summary>
        /// 
        /// </summary>
        public System.String Note { get { return this._Note; } set { this._Note = value.Trim(); } }

        private System.DateTime _Post_date=DateTime.Now;
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime Post_date { get { return this._Post_date; } set { this._Post_date = value; } }

        private System.DateTime _Mod_date;
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime Mod_date { get { return this._Mod_date; } set { this._Mod_date = value; } }

        private System.Int16 _Is_active=1;
        /// <summary>
        /// 
        /// </summary>
        public System.Int16 Is_active { get { return this._Is_active; } set { this._Is_active = value; } }

        private System.Single _Cold_water_value;
        public System.Single Cold_water_value { get { return this._Cold_water_value; } set { this._Cold_water_value = value; } }

        private System.Single _Hot_water_value;
        /// <summary>
        /// 
        /// </summary>
        public System.Single Hot_water_value { get { return this._Hot_water_value; } set { this._Hot_water_value = value; } }

        private System.Single _Electric_value;
        /// <summary>
        /// 
        /// </summary>
        public System.Single Electric_value { get { return this._Electric_value; } set { this._Electric_value = value; } }

        /// <summary>
        /// 宿舍名称
        /// </summary>
        public string PNickname { set; get; }
        /// <summary>
        /// 宿舍楼名称
        /// </summary>
        public string Building_Nickname { set; get; }
        /// <summary>
        /// 园区名称
        /// </summary>
        public string Dorm_Nickname { set; get; }
    }
}