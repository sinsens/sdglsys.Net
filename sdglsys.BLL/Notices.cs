using System.Collections.Generic;

namespace sdglsys.DbHelper
{
    public class Notices : DbContext
    {

        /// <summary>
        /// 通过ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entity.T_Notice FindById(int id)
        {
            return NoticeDb.GetById(id);
        }

        /// <summary>
        /// 通过ID删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            var notice = FindById(id);
            if (notice != null)
            {
                notice.Notice_model_state = false;
                return NoticeDb.Update(notice);
            }
            return false;
        }

        /// <summary>
        /// 更新公告信息
        /// </summary>
        /// <param name="Notice"></param>
        /// <returns></returns>
        public bool Update(Entity.T_Notice Notice)
        {
            return NoticeDb.Update(Notice);
        }

        /// <summary>
        /// 添加公告信息
        /// </summary>
        /// <param name="Notice"></param>
        /// <returns></returns>
        public bool Add(Entity.T_Notice Notice)
        {
            return NoticeDb.Insert(Notice);
        }

        /// <summary>
        /// 查找公告
        /// </summary>
        /// <param name="page">当前页数</param>
        /// <param name="limit">每页数量</param>
        /// <param name="totalCount">当前页结果数</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public List<Entity.T_Notice> GetByPages(int page, int limit, ref int totalCount, string where = null)
        {
            if (where == null)
                return Db.Queryable<Entity.T_Notice>().Where(a => a.Notice_model_state).OrderBy(a => a.Notice_post_date, SqlSugar.OrderByType.Desc).ToPageList(page, limit, ref totalCount);
            return Db.Queryable<Entity.T_Notice>().Where(a => a.Notice_model_state && a.Notice_title.Contains(where) || a.Notice_login_name.Contains(where)).OrderBy(a => a.Notice_post_date, SqlSugar.OrderByType.Desc).ToPageList(page, limit, ref totalCount);
        }

        /// <summary>
        /// 查找公告
        /// </summary>
        /// <param name="page">当前页数</param>
        /// <param name="limit">每页数量</param>
        /// <param name="totalCount">当前页结果数</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public List<Entity.T_Notice> GetListByPages(int page, int limit, ref int totalCount)
        {
            return Db.Queryable<Entity.T_Notice>().Where(a=>a.Notice_model_state).OrderBy(a => a.Notice_post_date, SqlSugar.OrderByType.Desc).Select(n => new Entity.T_Notice()
            {
                Notice_id = n.Notice_id,
                Notice_login_name = n.Notice_login_name,
                Notice_post_date = n.Notice_post_date,
                Notice_title = n.Notice_title
            }).ToPageList(page, limit, ref totalCount);
        }
    }
}
