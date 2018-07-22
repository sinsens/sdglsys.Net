using SqlSugar;
using System;

namespace sdglsys.Entity
{
    [SugarTable("t_used")]
    /// <summary>
    /// 用量
    /// </summary>
    public class TUsed
    {
        private System.Int32 _Id;
        /// <summary>
        /// ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public System.Int32 Id { get { return this._Id; } set { this._Id = value; } }

        private System.Int32 _Pid;
        /// <summary>
        /// 宿舍ID
        /// </summary>
        public System.Int32 Pid { get { return this._Pid; } set { this._Pid = value; } }

        private System.Int32 _Dorm_id;
        /// <summary>
        /// 园区ID
        /// </summary>
        public System.Int32 Dorm_id { get { return this._Dorm_id; } set { this._Dorm_id = value; } }

        private System.Int32 _Building_id;
        /// <summary>
        /// 宿舍楼ID
        /// </summary>
        public System.Int32 Building_id { get { return this._Building_id; } set { this._Building_id = value; } }

        private System.DateTime _Post_date=DateTime.Now;
        /// <summary>
        /// 登记时间
        /// </summary>
        public System.DateTime Post_date { get { return this._Post_date; } set { this._Post_date = value; } }

        private System.Int32 _Post_uid;
        /// <summary>
        /// 登记人用户ID
        /// </summary>
        public System.Int32 Post_uid { get { return this._Post_uid; } set { this._Post_uid = value; } }

        private System.Single _Cold_water_value;
        /// <summary>
        /// 冷水用量
        /// </summary>
        public System.Single Cold_water_value { get { return this._Cold_water_value; } set { this._Cold_water_value = value; } }

        private System.Single _Hot_water_value;
        /// <summary>
        /// 热水用量
        /// </summary>
        public System.Single Hot_water_value { get { return this._Hot_water_value; } set { this._Hot_water_value = value; } }

        private System.Single _Electric_value;
        /// <summary>
        /// 用电量
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