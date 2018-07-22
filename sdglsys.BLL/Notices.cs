using System.Collections.Generic;

namespace sdglsys.DbHelper
{
    public class Notices : DbContext
    {
        public List<Entity.TNotice> getAll()
        {
            return Db.Queryable<Entity.TNotice>().ToList();
        }

        /// <summary>
        /// 获取已启用的公告
        /// </summary>
        /// <returns></returns>
        public List<Entity.TNotice> getAllActive()
        {
            return Db.Queryable<Entity.TNotice>().Where(a=>a.Is_active==true).ToList();
        }

        /// <summary>
        /// 通过ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entity.TNotice findById(int id)
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
            return NoticeDb.DeleteById(id);
        }

        /// <summary>
        /// 更新公告信息
        /// </summary>
        /// <param name="Notice"></param>
        /// <returns></returns>
        public bool Update(Entity.TNotice Notice)
        {
            return NoticeDb.Update(Notice);
        }

        /// <summary>
        /// 添加公告信息
        /// </summary>
        /// <param name="Notice"></param>
        /// <returns></returns>
        public bool Add(Entity.TNotice Notice)
        {
            return NoticeDb.Insert(Notice);
        }

        /// <summary>
        /// 查找公告
        /// </summary>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="totalCount">当前页结果数</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public List<Entity.TNotice> getByPages(int pageIndex, int pageSize, ref int totalCount, string where=null)
        {
            if(where==null)
                return Db.Queryable<Entity.TNotice>().OrderBy(a=>a.Post_date,SqlSugar.OrderByType.Desc).ToPageList(pageIndex, pageSize, ref totalCount);
            return Db.Queryable<Entity.TNotice>().OrderBy(a => a.Post_date, SqlSugar.OrderByType.Desc).Where(a=>a.Title.Contains(where)||a.Login_name.Contains(where)).ToPageList(pageIndex, pageSize, ref totalCount);
        }

        /// <summary>
        /// 查找公告
        /// </summary>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="totalCount">当前页结果数</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public List<Entity.TNotice> getListByPages(int pageIndex, int pageSize, ref int totalCount)
        {
            return Db.Queryable<Entity.TNotice>().OrderBy(a => a.Mod_date, SqlSugar.OrderByType.Desc).Select(n=>new Entity.TNotice() {
                 Id = n.Id, Login_name = n.Login_name, Post_date = n.Post_date, Title = n.Title
            }).ToPageList(pageIndex, pageSize, ref totalCount);
        }
    }
}
