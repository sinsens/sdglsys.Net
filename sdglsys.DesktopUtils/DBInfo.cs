namespace sdglsys.DesktopUtils
{
    /// <summary>
    /// 数据库连接信息
    /// </summary>
    public class DBInfo
    {
        public static DbHelper.DbContext DB { get; set; }

        /// <summary>
        /// 备份数据库
        /// </summary>
        /// <param name="databaseName">数据库名称</param>
        /// <param name="fullFileName">绝对文件路径</param>
        /// <returns></returns>
        public static bool BackupDataBase(string fullFileName,string databaseName = null) {
            if (DB == null)
                return false;
            var flage = false;
            var db = DB.Db;
            if (databaseName == null) {
                foreach (var item in db.CurrentConnectionConfig.ConnectionString.Split(';'))
                {
                    if (item.ToLower().Contains("database=")) {
                        databaseName = item.Split('=')[1];
                    }
                }
            }
            if (databaseName == null) return false;
             
            try
            {
                db.Ado.BeginTran();
                flage = DB.Db.DbMaintenance.BackupDataBase(databaseName, fullFileName);
            }
            catch (System.Exception)
            {
                flage = false;
                throw;
            }
            finally {
                db.Ado.CommitTran();
            }
            return flage;
        }
    }
}
