using System.Collections.Generic;

namespace sdglsys.BLL
{
    public class Dorms : DbContext
    {
        public List<Entity.Dorm> getAll()
        {
            return Db.Queryable<Entity.Dorm>().ToList();
        }

        /// <summary>
        /// 获取已启用的园区
        /// </summary>
        /// <returns></returns>
        public List<Entity.Dorm> getAllActive()
        {
            return Db.Queryable<Entity.Dorm>().Where(a=>a.Is_active==true).ToList();
        }

        /// <summary>
        /// 通过ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entity.Dorm findById(int id)
        {
            return DormDb.GetById(id);
        }

        /// <summary>
        /// 通过ID删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            return DormDb.DeleteById(id);
        }

        /// <summary>
        /// 更新园区信息
        /// </summary>
        /// <param name="dorm"></param>
        /// <returns></returns>
        public bool Update(Entity.Dorm dorm)
        {
            return DormDb.Update(dorm);
        }

        /// <summary>
        /// 添加园区信息
        /// </summary>
        /// <param name="dorm"></param>
        /// <returns></returns>
        public bool Add(Entity.Dorm dorm)
        {
            return DormDb.Insert(dorm);
        }

        /// <summary>
        /// 查找园区
        /// </summary>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="totalCount">当前页结果数</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public List<Entity.Dorm> getByPages(int pageIndex, int pageSize, ref int totalCount, string where=null)
        {
            if(where==null)
                return Db.Queryable<Entity.Dorm>().ToPageList(pageIndex, pageSize, ref totalCount);
            return Db.Queryable<Entity.Dorm>().Where(a=>a.Nickname.Contains(where)||a.Note.Contains(where)).ToPageList(pageIndex, pageSize, ref totalCount);
        }
    }
}
