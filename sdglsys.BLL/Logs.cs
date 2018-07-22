using System.Collections.Generic;
namespace sdglsys.DbHelper
{
    public class Logs : DbContext
    {
        public List<Entity.TLog> getAll()
        {
            return Db.Queryable<Entity.TLog>().ToList();
        }

        /// <summary>
        /// 通过ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entity.TLog findById(int id)
        {
            return LogDb.GetById(id);
        }

        /// <summary>
        /// 通过ID删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            return LogDb.DeleteById(id);
        }

        /// <summary>
        /// 更新日志信息
        /// </summary>
        /// <param name="Log"></param>
        /// <returns></returns>
        public bool Update(Entity.TLog Log)
        {
            return LogDb.Update(Log);
        }

        /// <summary>
        /// 添加日志信息
        /// </summary>
        /// <param name="Log"></param>
        /// <returns></returns>
        public bool Add(Entity.TLog Log)
        {
            return LogDb.Insert(Log);
        }

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="ip">IP</param>
        /// <param name="login_name">登录IP</param>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool Add(string ip, string login_name, string info)
        {
            return Add(new Entity.TLog()
            {
                Ip = ip,
                Info = info,
                Login_name = login_name,
            });
        }

        /// <summary>
        /// 查找日志
        /// </summary>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="totalCount">当前页结果数</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public List<Entity.TLog> getByPages(int pageIndex, int pageSize, ref int totalCount, string where = null)
        {
            return (where == null) ? Db.Queryable<Entity.TLog>().OrderBy(a => a.Log_date, SqlSugar.OrderByType.Desc).ToPageList(pageIndex, pageSize, ref totalCount) :
                Db.Queryable<Entity.TLog>().Where(a => a.Info.Contains(where) || a.Ip.Contains(where) || a.Login_name.Contains(where)).OrderBy(a => a.Log_date, SqlSugar.OrderByType.Desc).ToPageList(pageIndex, pageSize, ref totalCount);
        }
    }
}
