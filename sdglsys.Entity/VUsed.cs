using SqlSugar;
using System;

namespace sdglsys.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public class VUsed
    {
        private System.Int32 _Id;
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public System.Int32 Used_Id { get { return this._Id; } set { this._Id = value; } }

        private System.Int32 _Pid;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 Used_Room_id { get { return this._Pid; } set { this._Pid = value; } }

        private System.Int32 _Dorm_id;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 Used_Dorm_id { get { return this._Dorm_id; } set { this._Dorm_id = value; } }

        private System.Int32 _Building_id;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 Used_Building_id { get { return this._Building_id; } set { this._Building_id = value; } }

        private System.DateTime _Post_date=DateTime.Now;
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime Used_Post_date { get { return this._Post_date; } set { this._Post_date = value; } }

        private System.Int32 _Post_uid;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 Used_Post_user_id { get { return this._Post_uid; } set { this._Post_uid = value; } }

        private System.Single _Cold_water_value;
        /// <summary>
        /// 
        /// </summary>
        public System.Single Used_Cold_water_value { get { return this._Cold_water_value; } set { this._Cold_water_value = value; } }

        private System.Single _Hot_water_value;
        /// <summary>
        /// 
        /// </summary>
        public System.Single Used_Hot_water_value { get { return this._Hot_water_value; } set { this._Hot_water_value = value; } }

        private System.Single _Electric_value;
        /// <summary>
        /// 
        /// </summary>
        public System.Single Used_Electric_value { get { return this._Electric_value; } set { this._Electric_value = value; } }

        private System.String _Note;
        /// <summary>
        /// 
        /// </summary>
        public System.String Used_Note { get { return this._Note; } set { this._Note = value.Trim(); } }

        private System.Boolean _Is_active = true;
        /// <summary>
        /// 
        /// </summary>
        public System.Boolean Used_Is_active { get { return this._Is_active; } set { this._Is_active = value; } }

        /// <summary>
        /// 登记者姓名
        /// </summary>
        public string Used_Post_User_Nickname { set; get; }

        /// <summary>
        /// 宿舍名称
        /// </summary>
        public string Used_Room_Nickname { set; get; }
        /// <summary>
        /// 宿舍楼名称
        /// </summary>
        public string Used_Building_Nickname { set; get; }
        /// <summary>
        /// 园区名称
        /// </summary>
        public string Used_Dorm_Nickname { set; get; }
    }
}