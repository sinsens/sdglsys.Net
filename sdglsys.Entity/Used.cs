using SqlSugar;

namespace sdglsys.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public class Used
    {
        private System.Int32 _Id;
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public System.Int32 Id { get { return this._Id; } set { this._Id = value; } }

        private System.Int32? _Pid;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? Pid { get { return this._Pid; } set { this._Pid = value; } }

        private System.Int32? _Dorm_id;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? Dorm_id { get { return this._Dorm_id; } set { this._Dorm_id = value; } }

        private System.Int32? _Building_id;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? Building_id { get { return this._Building_id; } set { this._Building_id = value; } }

        private System.DateTime? _Post_date;
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? Post_date { get { return this._Post_date; } set { this._Post_date = value; } }

        private System.Int32? _Post_uid;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? Post_uid { get { return this._Post_uid; } set { this._Post_uid = value; } }

        private System.Single? _Cold_water_value;
        /// <summary>
        /// 
        /// </summary>
        public System.Single? Cold_water_value { get { return this._Cold_water_value; } set { this._Cold_water_value = value; } }

        private System.Single? _Hot_water_value;
        /// <summary>
        /// 
        /// </summary>
        public System.Single? Hot_water_value { get { return this._Hot_water_value; } set { this._Hot_water_value = value; } }

        private System.Single? _Electric_value;
        /// <summary>
        /// 
        /// </summary>
        public System.Single? Electric_value { get { return this._Electric_value; } set { this._Electric_value = value; } }

        private System.String _Note;
        /// <summary>
        /// 
        /// </summary>
        public System.String Note { get { return this._Note; } set { this._Note = value?.Trim(); } }

        private System.Boolean? _Is_active;
        /// <summary>
        /// 
        /// </summary>
        public System.Boolean? Is_active { get { return this._Is_active; } set { this._Is_active = value; } }
    }
}