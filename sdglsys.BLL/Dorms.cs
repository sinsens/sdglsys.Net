using System.Collections.Generic;

namespace sdglsys.DbHelper
{
    public class Dorms : DbContext
    {
        public List<Entity.T_Dorm> GetAll()
        {
            return Db.Queryable<Entity.T_Dorm>().Where(x=>x.Dorm_model_state).ToList();
        }

        /// <summary>
        /// 获取已启用的园区
        /// </summary>
        /// <returns></returns>
        public List<Entity.T_Dorm> GetAllActive()
        {
            return Db.Queryable<Entity.T_Dorm>().Where(a=>a.Dorm_is_active&&a.Dorm_model_state).ToList();
        }

        /// <summary>
        /// 通过ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entity.T_Dorm FindById(int id)
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
            var dorm = FindById(id);
            if (dorm != null) {
                dorm.Dorm_model_state = false;
                return DormDb.Update(dorm);
            }
            return false;
        }

        /// <summary>
        /// 更新园区信息
        /// </summary>
        /// <param name="dorm"></param>
        /// <returns></returns>
        public bool Update(Entity.T_Dorm dorm)
        {
            return DormDb.Update(dorm);
        }

        /// <summary>
        /// 添加园区信息
        /// </summary>
        /// <param name="dorm"></param>
        /// <returns></returns>
        public bool Add(Entity.T_Dorm dorm)
        {
            return DormDb.Insert(dorm);
        }

        /// <summary>
        /// 查找园区
        /// </summary>
        /// <param name="page">当前页数</param>
        /// <param name="limit">每页数量</param>
        /// <param name="totalCount">当前页结果数</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public List<Entity.T_Dorm> GetByPages(int page, int limit, ref int totalCount, string where=null)
        {
            if(where==null)
                return Db.Queryable<Entity.T_Dorm>().Where(x=>x.Dorm_model_state).OrderBy(d=>d.Dorm_id, SqlSugar.OrderByType.Desc).ToPageList(page, limit, ref totalCount);
            return Db.Queryable<Entity.T_Dorm>().Where(a=>a.Dorm_model_state&&a.Dorm_nickname.Contains(where)||a.Dorm_note.Contains(where)).OrderBy(d=>d.Dorm_id, SqlSugar.OrderByType.Desc).ToPageList(page, limit, ref totalCount);
        }
    }
}
