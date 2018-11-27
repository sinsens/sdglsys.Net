using SqlSugar;

namespace DesktopTools
{
    public static class DbContext
    {
        public static SqlSugarClient Client = null;

        /// <summary>
        /// 自定义连接
        /// </summary>
        /// <param name="ConnectionString">数据库连接字符串</param>
        /// <param name="DBType">mysql,sqlserver,postgresql,oracle,sqlite</param>
        public static void Init(string connectionString, string DBType)
        {
            try
            {
                var dbType = new SqlSugar.DbType();
                // 判断数据库类型
                switch (DBType.ToLower())
                {
                    case "mysql":
                        dbType = DbType.MySql;
                        connectionString += "Allow User Variables=True;AllowZeroDateTime=True;ConvertZeroDateTime=True;SslMode=none;";
                        break;

                    case "sqlserver": dbType = DbType.SqlServer; break;
                    case "postgresql": dbType = DbType.PostgreSQL; break;
                    case "sqlite": dbType = DbType.Sqlite; break;
                    case "oracle": dbType = DbType.Oracle; break;
                }
                Client = new SqlSugarClient(new ConnectionConfig()
                {
                    ConnectionString = connectionString,
                    DbType = dbType,
                    IsAutoCloseConnection = true,
                    InitKeyType = InitKeyType.Attribute
                });
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}