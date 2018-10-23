using System.Collections.Generic;
namespace sdglsys.DbHelper
{
    public class Logs : DbContext
    {
        public List<Entity.T_Log> getAll()
        {
            return Db.Queryable<Entity.T_Log>().ToList();
        }

        /// <summary>
        /// 通过ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entity.T_Log FindById(int id)
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
        public bool Update(Entity.T_Log Log)
        {
            return LogDb.Update(Log);
        }

        /// <summary>
        /// 添加日志信息
        /// </summary>
        /// <param name="Log"></param>
        /// <returns></returns>
        public bool Add(Entity.T_Log Log)
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
            return Add(new Entity.T_Log()
            {
                Log_ip = ip,
                Log_info = info,
                Log_login_name = login_name,
            });
        }

        /// <summary>
        /// 查找日志
        /// </summary>
        /// <param name="page">当前页数</param>
        /// <param name="limit">每页数量</param>
        /// <param name="totalCount">当前页结果数</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public List<Entity.T_Log> getByPages(int page, int limit, ref int totalCount, string where = null)
        {
            return (where == null) ? Db.Queryable<Entity.T_Log>().OrderBy(a => a.Log_post_date, SqlSugar.OrderByType.Desc).ToPageList(page, limit, ref totalCount) :
                Db.Queryable<Entity.T_Log>().Where(a => a.Log_info.Contains(where) || a.Log_ip.Contains(where) || a.Log_login_name.Contains(where)).OrderBy(a => a.Log_post_date, SqlSugar.OrderByType.Desc).ToPageList(page, limit, ref totalCount);
        }
    }
}
