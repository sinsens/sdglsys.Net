using SqlSugar;

namespace sdglsys.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public class Dorm
    {
        private System.Int32 _Id;
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public System.Int32 Id { get { return this._Id; } set { this._Id = value; } }

        private System.String _Nickname;
        /// <summary>
        /// 
        /// </summary>
        public System.String Nickname { get { return this._Nickname; } set { this._Nickname = value?.Trim(); } }

        private System.Boolean? _Type;
        /// <summary>
        /// 
        /// </summary>
        public System.Boolean? Type { get { return this._Type; } set { this._Type = value; } }

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