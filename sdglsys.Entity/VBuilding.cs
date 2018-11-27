using SqlSugar;

namespace sdglsys.Entity
{
    /// <summary>
    /// ËÞÉáÂ¥
    /// </summary>
    public class VBuilding
    {
        private System.Int32 _Id;

        /// <summary>
        ///
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public System.Int32 Building_Id { get { return this._Id; } set { this._Id = value; } }

        private System.Int32 _Pid;

        /// <summary>
        ///
        /// </summary>
        public System.Int32 Building_Dorm_id { get { return this._Pid; } set { this._Pid = value; } }

        private System.String _Vid;

        /// <summary>
        ///
        /// </summary>
        public System.String Building_Vid { get { return this._Vid; } set { this._Vid = value.Trim(); } }

        private System.String _Nickname;

        /// <summary>
        ///
        /// </summary>
        public System.String Building_Nickname { get { return this._Nickname; } set { this._Nickname = value.Trim(); } }

        private System.String _Note;

        /// <summary>
        ///
        /// </summary>
        public System.String Building_Note { get { return this._Note; } set { this._Note = value.Trim(); } }

        private System.Boolean _Is_active = true;

        /// <summary>
        ///
        /// </summary>
        public System.Boolean Building_Is_active { get { return this._Is_active; } set { this._Is_active = value; } }

        /// <summary>
        /// Ô°ÇøÃû³Æ
        /// </summary>
        public System.String Building_Dorm_Nickname { get; set; }
    }
}