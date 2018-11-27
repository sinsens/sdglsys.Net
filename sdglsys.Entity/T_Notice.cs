using SqlSugar;

namespace sdglsys.Entity
{
    /// <summary>
    /// 公告信息表
    /// </summary>
    public class T_Notice
    {
        private System.Int32 _Notice_id;

        /// <summary>
        /// 公告ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public System.Int32 Notice_id { get { return this._Notice_id; } set { this._Notice_id = value; } }

        private System.String _Notice_login_name;

        /// <summary>
        /// 发布者用户名
        /// </summary>
        public System.String Notice_login_name { get { return this._Notice_login_name; } set { this._Notice_login_name = value?.Trim(); } }

        private System.String _Notice_title;

        /// <summary>
        /// 标题
        /// </summary>
        public System.String Notice_title { get { return this._Notice_title; } set { this._Notice_title = value?.Trim(); } }

        private System.String _Notice_content;

        /// <summary>
        /// 内容（经过ZIP压缩的HTML文档）
        /// </summary>
        public System.String Notice_content { get { return this._Notice_content; } set { this._Notice_content = value?.Trim(); } }

        private System.DateTime _Notice_post_date = System.DateTime.Now;

        /// <summary>
        /// 发布时间
        /// </summary>
        public System.DateTime Notice_post_date { get { return this._Notice_post_date; } set { this._Notice_post_date = value; } }

        private System.DateTime _Notice_mod_date = System.DateTime.Now;

        /// <summary>
        /// 修改时间
        /// </summary>
        public System.DateTime Notice_mod_date { get { return this._Notice_mod_date; } set { this._Notice_mod_date = value; } }

        private System.Boolean _Notice_is_active = true;

        /// <summary>
        /// 状态：1激活，0注销
        /// </summary>
        public System.Boolean Notice_is_active { get { return this._Notice_is_active; } set { this._Notice_is_active = value; } }

        private System.Boolean _Notice_model_state = true;

        /// <summary>
        /// 对象状态：1正常，0已删除
        /// </summary>
        public System.Boolean Notice_model_state { get { return this._Notice_model_state; } set { this._Notice_model_state = value; } }
    }
}