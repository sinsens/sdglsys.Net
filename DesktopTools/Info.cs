namespace DesktopTools
{
    [System.Serializable]
    /// <summary>
    /// 用来保存程序信息，比如连接信息的
    /// </summary>
    public class Info
    {
        public DbInfo DbInfo { get; set; }
    }

    [System.Serializable]
    /// <summary>
    /// 数据库连接信息
    /// </summary>
    public class DbInfo
    {
        public DbInfo()
        {
        }

        /// <summary>
        /// 数据库类型：SQLServer,MySQL
        /// </summary>
        /// <param name="dbType">SQLServer,MySQL</param>
        /// <param name="server">数据库域名或IP</param>
        /// <param name="dbName">数据库名称</param>
        /// <param name="username">数据库用户名</param>
        /// <param name="pwd">数据库密码</param>
        public DbInfo(string dbType, string server, string dbName, string username, string pwd)
        {
            DbType = dbType;
            Server = server;
            DbName = dbName;
            Username = username;
            Pwd = pwd;
        }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public string DbType { get; set; }

        /// <summary>
        /// 数据库域名或IP
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DbName { get; set; }

        /// <summary>
        /// 数据库用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 数据库密码
        /// </summary>
        public string Pwd { get; set; }
    }
}