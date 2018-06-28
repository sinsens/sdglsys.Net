using System.Data;
using sdglsys.Entity;
using SqlSugar;

namespace sdglsys.BLL
{
    public class DbContext
    {
        public SqlSugarClient Db;
        public DbContext()
        {

            Db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = (string)Utils.getSetting("MySQLDb", typeof(string)),
                DbType = SqlSugar.DbType.MySql,
                IsAutoCloseConnection = true
            });
        }

        public SimpleClient<Entity.Users> UserDb { get { return new SimpleClient<Entity.Users>(Db); } }
        public SimpleClient<Entity.Dorm> DormDb { get { return new SimpleClient<Entity.Dorm>(Db); } }
        public SimpleClient<Entity.Log> LogDb { get { return new SimpleClient<Entity.Log>(Db); } }
    }
}