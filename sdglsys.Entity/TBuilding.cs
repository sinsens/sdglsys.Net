using SqlSugar;

namespace sdglsys.Entity
{
    [SugarTable("t_building")]
    /// <summary>
    /// 宿舍楼
    /// </summary>
    public class TBuilding
    {
        private System.Int32 _Id;
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public System.Int32 Id { get { return this._Id; } set { this._Id = value; } }

        private System.Int32 _Pid;
        /// <summary>
        /// 园区ID
        /// </summary>
        public System.Int32 Pid { get { return this._Pid; } set { this._Pid = value; } }

        private System.String _Vid;
        /// <summary>
        /// 自定义编号
        /// </summary>
        public System.String Vid { get { return this._Vid; } set { this._Vid = value.Trim(); } }

        private System.String _Nickname;
        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(Length = 20)]
        public System.String Nickname { get { return this._Nickname; } set { this._Nickname = value.Trim(); } }

        private System.String _Note;
        /// <summary>
        /// 备注
        /// </summary>
        public System.String Note { get { return this._Note; } set { this._Note = value.Trim(); } }

        private System.Boolean _Is_active = true;
        /// <summary>
        /// 状态,1正常,0已注销
        /// </summary>
        public System.Boolean Is_active { get { return this._Is_active; } set { this._Is_active = value; } }
    }
}