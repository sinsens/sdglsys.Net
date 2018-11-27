using sdglsys.Entity;
using SqlSugar;
using System.Collections.Generic;

namespace sdglsys.DbHelper
{
    public class Useds_total : DbContext
    {
        /// <summary>
        /// 通过宿舍ID查询最后登记记录
        /// </summary>
        /// <param name="id">宿舍ID</param>
        /// <returns></returns>
        public Entity.T_Used_total Last(int id)
        {
            return Db.Queryable<Entity.T_Used_total>().Where(a => a.Ut_model_state && a.Ut_room_id == id).OrderBy(a => a.Ut_id, OrderByType.Desc).First();
        }

        /// <summary>
        /// 通过ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entity.T_Used_total FindById(int id)
        {
            return Used_totalDb.GetById(id);
        }

        /// <summary>
        /// 通过宿舍ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entity.T_Used_total FindByPid(int id)
        {
            return Db.Queryable<T_Used_total>().Where(u => u.Ut_model_state && u.Ut_room_id == id).First();
        }

        /// <summary>
        /// 通过ID删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            var ut = FindById(id);
            if (ut != null)
            {
                ut.Ut_model_state = false;
                return Update(ut);
            }
            return false;
        }

        /// <summary>
        /// 更新登记信息
        /// </summary>
        /// <param name="Room"></param>
        /// <returns></returns>
        public bool Update(Entity.T_Used_total used)
        {
            return Used_totalDb.Update(used);
        }

        /// <summary>
        /// 添加登记信息
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public bool Add(Entity.T_Used_total used)
        {
            return Used_totalDb.Insert(used);
        }

        /// <summary>
        /// 查找读表数值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public VUsed_total FindVUsedById(int id)
        {
            return Db.Queryable<T_Used_total, T_Room, T_Building, T_Dorm>((u, r, b, d) => new object[] { JoinType.Left, u.Ut_room_id == r.Room_id, JoinType.Left, r.Room_building_id == b.Building_id, JoinType.Left, b.Building_dorm_id == d.Dorm_id }).
                Where((u, r, b, d) => u.Ut_model_state && u.Ut_id == id).
                 Select((u, r, b, d) => new VUsed_total
                 {
                     UT_Room_id = r.Room_id,
                     Id = u.Ut_id,
                     UT_Dorm_Nickname = d.Dorm_nickname,
                     UT_Building_Nickname = b.Building_nickname,
                     UT_Room_Nickname = r.Room_nickname,
                     UT_Note = u.Ut_note,
                     UT_Post_date = u.Ut_post_date,
                     UT_Cold_water_value = u.Ut_cold_water_value,
                     UT_Electric_value = u.Ut_electric_value,
                     UT_Hot_water_value = u.Ut_hot_water_value
                 }).First();
        }

        /// <summary>
        /// 查找登记
        /// </summary>
        /// <param name="page">当前页数</param>
        /// <param name="limit">每页数量</param>
        /// <param name="totalCount">当前页结果数</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public List<VUsed_total> GetByPages(int page, int limit, ref int totalCount, string where = null, int dorm_id = 0)
        {
            var sql = Db.Queryable<T_Used_total, T_Room, T_Building, T_Dorm>((u, r, b, d) => new object[] { JoinType.Left, u.Ut_room_id == r.Room_id, JoinType.Left, r.Room_building_id == b.Building_id, JoinType.Left, b.Building_dorm_id == d.Dorm_id }).OrderBy((u, r, b, d) => u.Ut_post_date, OrderByType.Desc).Where(u => u.Ut_model_state);

            if (dorm_id != 0)
            {
                sql = sql.Where(u => u.Ut_dorm_id == dorm_id);
            }
            if (!string.IsNullOrWhiteSpace(where))
            {
                sql = sql.Where((u, r, b, d) => r.Room_nickname.Contains(where) || r.Room_note.Contains(where) || r.Room_vid.Contains(where));
            }
            return sql.Select((u, r, b, d) => new VUsed_total
            {
                Id = u.Ut_id,
                UT_Dorm_Nickname = d.Dorm_nickname,
                UT_Building_Nickname = b.Building_nickname,
                UT_Room_Nickname = r.Room_nickname,
                UT_Note = u.Ut_note,
                UT_Post_date = u.Ut_post_date,
                UT_Cold_water_value = u.Ut_cold_water_value,
                UT_Electric_value = u.Ut_electric_value,
                UT_Hot_water_value = u.Ut_hot_water_value
            }).ToPageList(page, limit, ref totalCount);
        }
    }
}