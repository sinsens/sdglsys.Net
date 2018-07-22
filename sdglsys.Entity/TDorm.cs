using SqlSugar;

namespace sdglsys.Entity
{
    [SugarTable("t_dorm")]
    /// <summary>
    /// 园区
    /// </summary>
    public class TDorm
    {
        private System.Int32 _Id;
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public System.Int32 Id { get { return this._Id; } set { this._Id = value; } }

        private System.String _Nickname;
        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(Length = 20)]
        public System.String Nickname { get { return this._Nickname; } set { this._Nickname = value.Trim(); } }

        private System.Boolean _Type=true;
        /// <summary>
        /// 类型：0女,1男，默认1
        /// </summary>
        public System.Boolean Type { get { return this._Type; } set { this._Type = value; } }

        private System.String _Note;
        /// <summary>
        /// 备注
        /// </summary>
        public System.String Note { get { return this._Note; } set { this._Note = value.Trim(); } }

        private System.Boolean _Is_active = true;
        /// <summary>
        /// 状态：0已注销，1正常，默认1
        /// </summary>
        public System.Boolean Is_active { get { return this._Is_active; } set { this._Is_active = value; } }
    }
}