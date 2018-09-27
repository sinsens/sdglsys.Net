using SqlSugar;

namespace sdglsys.Entity
{
    /// <summary>
    /// 宿舍
    /// </summary>
    public class VRoom
    {
        private System.Int32 _Id;
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public System.Int32 Id { get { return this._Id; } set { this._Id = value; } }

        private System.Int32 _Pid;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 Pid { get { return this._Pid; } set { this._Pid = value; } }

        private System.Int32 _Dorm_id;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 Dorm_id { get { return this._Dorm_id; } set { this._Dorm_id = value; } }

        private System.String _Vid;
        /// <summary>
        /// 
        /// </summary>
        public System.String Vid { get { return this._Vid; } set { this._Vid = value.Trim(); } }

        private System.String _Nickname;
        /// <summary>
        /// 
        /// </summary>
        public System.String Nickname { get { return this._Nickname; } set { this._Nickname = value.Trim(); } }

        private System.String _Note;
        /// <summary>
        /// 
        /// </summary>
        public System.String Note { get { return this._Note; } set { this._Note = value.Trim(); } }

        private System.Boolean _Is_active = true;
        /// <summary>
        /// 
        /// </summary>
        public System.Boolean Is_active { get { return this._Is_active; } set { this._Is_active = value; } }

        /// <summary>
        /// 宿舍楼名称
        /// </summary>
        public string PNickname
        {
            get; set;
        }

        /// <summary>
        /// 园区名称
        /// </summary>
        public string Dorm_Nickname
        {
            get; set;
        }
        /// <summary>
        /// 人数
        /// </summary>
        public short Number { get; set; }
    }
}