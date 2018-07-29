using sdglsys.Entity;
using SqlSugar;
using System.Collections.Generic;

namespace sdglsys.DbHelper
{
    public class Useds_total : DbContext
    {
        public List<Entity.TUsed_total> getAll()
        {
            return Db.Queryable<Entity.TUsed_total>().ToList();
        }

        /// <summary>
        /// 通过宿舍ID查询最后登记记录
        /// </summary>
        /// <param name="id">宿舍ID</param>
        /// <returns></returns>
        public Entity.TUsed_total Last(int id)
        {
            return Db.Queryable<Entity.TUsed_total>().Where(a => a.Pid == id).OrderBy(a => a.Id, OrderByType.Desc).First();
        }

        /// <summary>
        /// 通过ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entity.TUsed_total findById(int id)
        {
            return Used_totalDb.GetById(id);
        }

        /// <summary>
        /// 通过宿舍ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entity.TUsed_total findByPid(int id)
        {
            return Db.Queryable<TUsed_total>().Where(u => u.Pid == id).First();
        }

        /// <summary>
        /// 通过ID删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            return Used_totalDb.DeleteById(id);
        }

        /// <summary>
        /// 更新登记信息
        /// </summary>
        /// <param name="Room"></param>
        /// <returns></returns>
        public bool Update(Entity.TUsed_total used)
        {
            return Used_totalDb.Update(used);
        }

        /// <summary>
        /// 添加登记信息
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public bool Add(Entity.TUsed_total used)
        {
            return Used_totalDb.Insert(used);
        }

        /// <summary>
        /// 查找读表数值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public VUsed_total FindById(int id) {
            return Db.Queryable<TUsed_total, TRoom, TBuilding, TDorm>((u, r, b, d) => new object[] { JoinType.Left, u.Pid == r.Id, JoinType.Left, r.Pid == b.Id, JoinType.Left, b.Pid == d.Id }).
                Where((u, r, b, d)=>u.Id==id).
                  Select((u, r, b, d) => new VUsed_total
                  {
                      Id = u.Id,
                      Pid = u.Pid,
                      Building_id = b.Id,
                      Dorm_id = d.Id,
                      Note = u.Note,
                      PNickname = r.Nickname,
                      Building_Nickname = b.Nickname,
                      Dorm_Nickname = d.Nickname,
                      Cold_water_value = u.Cold_water_value,
                      Electric_value = u.Electric_value,
                      Hot_water_value = u.Hot_water_value,
                      Post_date = u.Post_date,
                  }).First();
        }

        /// <summary>
        /// 查找读表数值
        /// </summary>
        /// <param name="id">宿舍ID</param>
        /// <returns></returns>
        public VUsed_total FindByPid(int id)
        {
            return Db.Queryable<TUsed_total, TRoom, TBuilding, TDorm>((u, r, b, d) => new object[] { JoinType.Left, u.Pid == r.Id, JoinType.Left, r.Pid == b.Id, JoinType.Left, b.Pid == d.Id }).
                Where((u, r, b, d) => u.Pid == id).
                  Select((u, r, b, d) => new VUsed_total
                  {
                      Id = u.Id,
                      Pid = u.Pid,
                      Building_id = b.Id,
                      Dorm_id = d.Id,
                      Note = r.Note,
                      PNickname = r.Nickname,
                      Building_Nickname = b.Nickname,
                      Dorm_Nickname = d.Nickname,
                      Cold_water_value = u.Cold_water_value,
                      Electric_value = u.Electric_value,
                      Hot_water_value = u.Hot_water_value,
                      Post_date = u.Post_date,
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
        public List<VUsed_total> getByPages(int page, int limit, ref int totalCount, string where = null)
        {
            if (where == null)
                return Db.Queryable<TUsed_total, TRoom, TBuilding, TDorm>((u, r, b, d) => new object[] { JoinType.Left, u.Pid == r.Id, JoinType.Left, r.Pid == b.Id, JoinType.Left, b.Pid == d.Id }).
                    OrderBy((u, r, b, d) => u.Post_date, OrderByType.Desc).
                  Select((u, r, b, d) => new VUsed_total
                  {
                      Id = u.Id,
                      Pid = u.Pid,
                      Building_id = b.Id,
                      Dorm_id = d.Id,
                      Note = r.Note,
                      PNickname = r.Nickname,
                      Building_Nickname = b.Nickname,
                      Dorm_Nickname = d.Nickname,
                      Cold_water_value = u.Cold_water_value,
                      Electric_value = u.Electric_value,
                      Hot_water_value = u.Hot_water_value,
                      Post_date = u.Post_date,
                  }).ToPageList(page, limit, ref totalCount);
            return Db.Queryable<TUsed_total, TRoom, TBuilding, TDorm>((u, r, b, d) => new object[] { JoinType.Left, u.Pid == r.Id, JoinType.Left, r.Pid == b.Id, JoinType.Left, b.Pid == d.Id }).
                Where((u, r, b, d) => r.Nickname.Contains(where) || r.Note.Contains(where) || r.Vid.Contains(where)||b.Nickname.Contains(where)||d.Nickname.Contains(where)).
                OrderBy((u, r, b, d) => u.Post_date, OrderByType.Desc).
                  Select((u, r, b, d) => new VUsed_total
                  {
                      Id = u.Id,
                      Pid = u.Pid,
                      Building_id = b.Id,
                      Dorm_id = d.Id,
                      Note = r.Note,
                      PNickname = r.Nickname,
                      Building_Nickname = b.Nickname,
                      Dorm_Nickname = d.Nickname,
                      Cold_water_value = u.Cold_water_value,
                      Electric_value = u.Electric_value,
                      Hot_water_value = u.Hot_water_value,
                      Post_date = u.Post_date,
                  }).ToPageList(page, limit, ref totalCount);
            //return Db.Queryable<Entity.Building>().Where((b) => b.Nickname.Contains(where) || b.Note.Contains(where)).ToPageList(page, limit, ref totalCount);
        }

        /// <summary>
        /// 查找登记
        /// </summary>
        /// <param name="page">当前页数</param>
        /// <param name="limit">每页数量</param>
        /// <param name="id">园区ID</param>
        /// <param name="totalCount">当前页结果数</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public List<VUsed_total> getByPagesByDormId(int page, int limit, int id, ref int totalCount, string where = null)
        {
            if (where == null)
                return Db.Queryable<TUsed_total, TRoom, TBuilding, TDorm>((u, r, b, d) => new object[] { JoinType.Left, u.Pid == r.Id, JoinType.Left, r.Pid == b.Id, JoinType.Left, b.Pid == d.Id }).
                    Where((u, r, b, d) => u.Dorm_id == id).
                    OrderBy((u, r, b, d)=>u.Post_date, OrderByType.Desc).
                  Select((u, r, b, d) => new VUsed_total
                  {
                      Cold_water_value = u.Cold_water_value,
                      Electric_value = u.Electric_value,
                      Hot_water_value = u.Hot_water_value,
                      Post_date = u.Post_date,
                      Id = u.Id,
                      Pid = u.Pid,
                      Building_id = b.Id,
                      Dorm_id = d.Id,
                      Note = r.Note,
                      PNickname = r.Nickname,
                      Building_Nickname = b.Nickname,
                      Dorm_Nickname = d.Nickname,
                  }).ToPageList(page, limit, ref totalCount);
            return Db.Queryable<TUsed_total, TRoom, TBuilding, TDorm>((u, r, b, d) => new object[] { JoinType.Left, u.Pid == r.Id, JoinType.Left, r.Pid == b.Id, JoinType.Left, b.Pid == d.Id }).
                Where((u, r, b, d) => r.Nickname.Contains(where) || r.Note.Contains(where) || r.Vid.Contains(where) || b.Nickname.Contains(where) || d.Nickname.Contains(where)).
                Where((u, r, b, d) => u.Dorm_id == id).
                OrderBy((u, r, b, d) => u.Post_date, OrderByType.Desc).
                  Select((u, r, b, d) => new VUsed_total
                  {
                      Id = u.Id,
                      Pid = u.Pid,
                      Building_id = b.Id,
                      Dorm_id = d.Id,
                      Note = r.Note,
                      PNickname = r.Nickname,
                      Building_Nickname = b.Nickname,
                      Dorm_Nickname = d.Nickname,
                      Cold_water_value = u.Cold_water_value,
                      Electric_value = u.Electric_value,
                      Hot_water_value = u.Hot_water_value,
                      Post_date = u.Post_date,
                  }).ToPageList(page, limit, ref totalCount);
            //return Db.Queryable<Entity.Building>().Where((b) => b.Nickname.Contains(where) || b.Note.Contains(where)).ToPageList(page, limit, ref totalCount);
        }
    }
}
