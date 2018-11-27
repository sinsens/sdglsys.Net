using sdglsys.Entity;
using SqlSugar;
using System.Collections.Generic;

namespace sdglsys.DbHelper
{
    public class Buildings : DbContext
    {
        public List<Entity.VBuilding> GetAll()
        {
            return Db.Queryable<sdglsys.Entity.T_Building, Entity.T_Dorm>((b, d) => new object[] { JoinType.Left, b.Building_dorm_id == d.Dorm_id }).Where(b => b.Building_model_state).Select((b, d) => new VBuilding
            {
                Building_Dorm_id = d.Dorm_id,
                Building_Dorm_Nickname = d.Dorm_nickname,
                Building_Id = b.Building_id,
                Building_Is_active = b.Building_is_active,
                Building_Nickname = b.Building_nickname,
                Building_Note = b.Building_note,
                Building_Vid = b.Building_vid
            }).ToList();
        }

        /// <summary>
        /// 获取已启用的宿舍楼
        /// </summary>
        /// <returns></returns>
        public List<Entity.VBuilding> GetAllActive()
        {
            return Db.Queryable<sdglsys.Entity.T_Building, Entity.T_Dorm>((b, d) => new object[] { JoinType.Left, b.Building_dorm_id == d.Dorm_id }).Where((b, d) => b.Building_is_active && b.Building_model_state).Select((b, d) => new VBuilding
            {
                Building_Dorm_id = d.Dorm_id,
                Building_Dorm_Nickname = d.Dorm_nickname,
                Building_Id = b.Building_id,
                Building_Is_active = b.Building_is_active,
                Building_Nickname = b.Building_nickname,
                Building_Note = b.Building_note,
                Building_Vid = b.Building_vid
            }).ToList();
        }

        /// <summary>
        /// 获取已启用的宿舍楼
        /// </summary>
        /// <param name="pid">园区ID</param>
        /// <returns></returns>
        public List<Entity.VBuilding> GetAllActiveById(int pid)
        {
            return Db.Queryable<sdglsys.Entity.T_Building, Entity.T_Dorm>((b, d) => new object[] { JoinType.Left, b.Building_dorm_id == d.Dorm_id }).Where((b, d) => b.Building_is_active && b.Building_model_state && b.Building_dorm_id == pid).Select((b, d) => new VBuilding
            {
                Building_Dorm_id = d.Dorm_id,
                Building_Dorm_Nickname = d.Dorm_nickname,
                Building_Id = b.Building_id,
                Building_Is_active = b.Building_is_active,
                Building_Nickname = b.Building_nickname,
                Building_Note = b.Building_note,
                Building_Vid = b.Building_vid
            }).ToList();
        }

        /// <summary>
        /// 通过ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entity.T_Building FindById(int id)
        {
            return BuildingDb.GetById(id);
        }

        /// <summary>
        /// 通过ID删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            return BuildingDb.DeleteById(id);
        }

        /// <summary>
        /// 更新宿舍楼信息
        /// </summary>
        /// <param name="Building"></param>
        /// <returns></returns>
        public bool Update(Entity.T_Building Building)
        {
            return BuildingDb.Update(Building);
        }

        /// <summary>
        /// 添加宿舍楼信息
        /// </summary>
        /// <param name="Building"></param>
        /// <returns></returns>
        public bool Add(Entity.T_Building Building)
        {
            return BuildingDb.Insert(Building);
        }

        /// <summary>
        /// 查找宿舍楼
        /// </summary>
        /// <param name="page">当前页数</param>
        /// <param name="limit">每页数量</param>
        /// <param name="totalCount">当前页结果数</param>
        /// <param name="where">关键词</param>
        /// <param name="pid">园区ID</param>
        /// <returns></returns>
        public List<VBuilding> GetByPages(int page, int limit, ref int totalCount, string where = null, int pid = 0)
        {
            var sql = Db.Queryable<sdglsys.Entity.T_Building, Entity.T_Dorm>((b, d) => new object[] { JoinType.Left, b.Building_dorm_id == d.Dorm_id }).Where(b => b.Building_model_state);
            if (pid != 0)
            {
                sql = sql.Where(b => b.Building_dorm_id == pid);
            }
            if (where != null)
            {
                sql = sql.Where((b, d) => b.Building_vid.Contains(where) || b.Building_nickname.Contains(where) || b.Building_note.Contains(where));
            }
            return sql.OrderBy((b, d) => b.Building_id, OrderByType.Desc).Select((b, d) => new VBuilding
            {
                Building_Dorm_id = d.Dorm_id,
                Building_Dorm_Nickname = d.Dorm_nickname,
                Building_Id = b.Building_id,
                Building_Is_active = b.Building_is_active,
                Building_Nickname = b.Building_nickname,
                Building_Note = b.Building_note,
                Building_Vid = b.Building_vid
            }).ToPageList(page, limit, ref totalCount);
        }
    }
}