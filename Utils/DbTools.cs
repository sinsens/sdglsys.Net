using SqlSugar;
using System;
using sdglsys.Entity;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sdglsys.XUtils
{
    /// <summary>
    /// 数据库工具集
    /// </summary>
    public class DbTools
    {
        private SqlSugarClient db;

        public static object GetAppSetting(string key, Type type)
        {
            var setting = new AppSettingsReader();
            return setting.GetValue(key, type);
        }

        public DbTools()
        {
            var _dbtype = (string)GetAppSetting("DBType", typeof(string));
            var dbtype = new DbType();
            switch (_dbtype.ToLower())
            {
                case "mysql": dbtype = DbType.MySql; break;
                case "sqlserver": dbtype = DbType.SqlServer; break;
                case "postgresql": dbtype = DbType.PostgreSQL; break;
                case "sqlite": dbtype = DbType.Sqlite; break;
                case "oracle": dbtype = DbType.Oracle; break;
            }

            db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = (string)DbHelper.Utils.GetAppSetting("DBConnectionString", typeof(string)),
                DbType = dbtype,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });

        }


        /// <summary>
        /// 初始化数据库（创建数据库表）
        /// </summary>
        /// <returns></returns>
        public bool InitDb()
        {
           // return false;
            
            try
            {
                //db.Ado.BeginTran();
                ///1.备份&新建数据库表
                ///1.1检测&备份数据库表
                var now = DateTime.Now.Year +'_'+ DateTime.Now.Month +'_'+ DateTime.Now.Day +'_'+ DateTime.Now.Hour + DateTime.Now.Minute +'_'+ DateTime.Now.Millisecond;
                //if(db.DbMaintenance.IsAnyTable("t_dorm"))
                //    db.DbMaintenance.BackupTable("t_dorm_bak_"+ now,"t_dorm");

                //if (db.DbMaintenance.IsAnyTable("t_building"))
                //    db.DbMaintenance.BackupTable("t_building", "t_building_bak_"+ now);

                //if (db.DbMaintenance.IsAnyTable("t_room"))
                //    db.DbMaintenance.BackupTable("t_room", "t_room_bak_"+ now);

                //if (db.DbMaintenance.IsAnyTable("t_quota"))
                //    db.DbMaintenance.BackupTable("t_quota", "t_quota_bak_"+ now);

                //if (db.DbMaintenance.IsAnyTable("t_rate"))
                //    db.DbMaintenance.BackupTable("t_rate", "t_rate_bak_"+ now);

                //if (db.DbMaintenance.IsAnyTable("t_user"))
                //    db.DbMaintenance.BackupTable("t_user", "t_user_bak_"+ now);

                //if (db.DbMaintenance.IsAnyTable("t_used"))
                //    db.DbMaintenance.BackupTable("t_used", "t_used_bak_"+ now);

                //if (db.DbMaintenance.IsAnyTable("t_used_total"))
                //    db.DbMaintenance.BackupTable("t_used_total", "t_used_total_bak_"+ now);

                //if (db.DbMaintenance.IsAnyTable("t_bill"))
                //    db.DbMaintenance.BackupTable("t_bill", "t_bill_bak_"+ now);

                //if (db.DbMaintenance.IsAnyTable("t_log"))
                //    db.DbMaintenance.BackupTable("t_log", "t_log_bak_"+ now);

                //if (db.DbMaintenance.IsAnyTable("t_notice"))
                //    db.DbMaintenance.BackupTable("t_notice", "t_notice_bak_"+ now);
                //1.2创建数据库表
                db.CodeFirst.InitTables(typeof(TDorm), typeof(TDorm));
                db.CodeFirst.InitTables(typeof(TBuilding), typeof(TBuilding));
                db.CodeFirst.InitTables(typeof(TRoom), typeof(TRoom));
                db.CodeFirst.InitTables(typeof(TQuota), typeof(TQuota));
                db.CodeFirst.InitTables(typeof(TRate), typeof(TRate));
                db.CodeFirst.InitTables(typeof(TUser), typeof(TUser));
                db.CodeFirst.InitTables(typeof(TUsed), typeof(TUsed));
                db.CodeFirst.InitTables(typeof(TUsed_total), typeof(TUsed_total));
                db.CodeFirst.InitTables(typeof(TBill), typeof(TBill));
                db.CodeFirst.InitTables(typeof(TLog), typeof(TLog));
                db.CodeFirst.InitTables(typeof(TNotice), typeof(TNotice));
                db.Ado.CommitTran();
                ///2.设置User表的Login_name为主键
                db.DbMaintenance.AddPrimaryKey("t_user", "login_name");
                ///3.初始化基础配额数据
                db.Insertable(new TQuota()
                {
                    Cold_water_value = 0,
                    Electric_value = 0,
                    Hot_water_value = 0,
                    Is_active = true,
                    Note = "系统初始值"
                }).ExecuteCommand();
                ///4.初始化费率数据
                db.Insertable(new TRate()
                {
                    Note = "系统初始值",
                    Is_active = true,
                    Hot_water_value = 0,
                    Electric_value = 0,
                    Cold_water_value = 0
                }).ExecuteCommand();

                db.Ado.CommitTran();
                return true;
            }
            catch (Exception)
            {
                db.Ado.RollbackTran();
                throw;
            }
        }

        /// <summary>
        /// 创建系统管理员
        /// </summary>
        /// <param name="login_name">登录名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public bool CreateAdmin(string login_name, string pwd)
        {
            return db.Insertable<Entity.TUser>(new Entity.TUser()
            {
                Login_name = login_name,
                Nickname = login_name,
                Pwd = DbHelper.Utils.hashpwd(pwd),
                Role = 3,
                Note = "通过Utils工具添加的一个系统管理员角色"
            }).ExecuteCommand() > 0 ? true : false;

        }
    }
}
