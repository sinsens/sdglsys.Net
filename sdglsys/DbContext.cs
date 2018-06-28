using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;
using sdglsys.Entity;
using SqlSugar;


namespace sdglsys.Repositories
{
    public class DbContext
    {
        public SqlSugarClient Db;
        public DbContext()
        {
            
            Db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = (string)Utils.getSetting("MySQLDb", typeof(string)),
                DbType = DbType.MySql,
                IsAutoCloseConnection = true
            });
        }

        public SimpleClient<Entity.Users> UserDb { get { return new SimpleClient<Entity.Users>(Db); } }
        public SimpleClient<Entity.Dorm> DormDb { get { return new SimpleClient<Entity.Dorm>(Db); } }
    }
}
