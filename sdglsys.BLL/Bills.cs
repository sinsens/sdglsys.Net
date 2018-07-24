using sdglsys.Entity;
using SqlSugar;
using System.Collections.Generic;

namespace sdglsys.DbHelper
{
    public class Bills : DbContext
    {
        public List<Entity.TBill> getAll()
        {
            return Db.Queryable<Entity.TBill>().ToList();
        }


        /// <summary>
        /// 通过ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entity.TBill findById(int id)
        {
            return BillDb.GetById(id);
        }

        /// <summary>
        /// 通过ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entity.VBill FindById(int id)
        {
            return Db.Queryable<TBill, TUsed, TRoom, TBuilding, TDorm>((e, u, r, b, d) => new object[] { JoinType.Left, e.Pid == u.Id, JoinType.Left, u.Pid == r.Id, JoinType.Left, r.Pid == b.Id, JoinType.Left, b.Pid == d.Id }).
                Where(e => e.Id == id).
                  Select((e, u, r, b, d) => new VBill
                  {
                      Id = e.Id,
                      Pid = e.Pid,
                      Building_id = b.Id,
                      Dorm_id = d.Id,
                      Note = e.Note,
                      Is_active = e.Is_active,
                      PNickname = r.Nickname,
                      Building_Nickname = b.Nickname,
                      Dorm_Nickname = d.Nickname,
                      Cold_water_cost = e.Cold_water_cost,
                      Electric_cost = e.Electric_cost,
                      Hot_water_cost = e.Hot_water_cost,
                      Post_date = e.Post_date,
                      Mod_date = e.Mod_date,
                      Cold_water_value = u.Cold_water_value,
                      Electric_value = u.Electric_value,
                      Hot_water_value = u.Hot_water_value
                  }).First();
        }

        /// <summary>
        /// 通过ID删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            return BillDb.DeleteById(id);
        }

        /// <summary>
        /// 更新账单信息
        /// </summary>
        /// <param name="Room"></param>
        /// <returns></returns>
        public bool Update(Entity.TBill bill)
        {
            return BillDb.Update(bill);
        }

        /// <summary>
        /// 添加登记信息
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public bool Add(Entity.TBill bill)
        {
            return BillDb.Insert(bill);
        }

        /// <summary>
        /// 查找登记
        /// </summary>
        /// <param name="page">当前页数</param>
        /// <param name="limit">每页数量</param>
        /// <param name="totalCount">当前页结果数</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public List<VBill> getByPages(int page, int limit, ref int totalCount, string where = null, short stat = -1)
        {
            if (where == null && stat == -1)
                return Db.Queryable<TBill, TUsed, TRoom, TBuilding, TDorm>((e, u, r, b, d) => new object[] { JoinType.Left, e.Pid == u.Id, JoinType.Left, u.Pid == r.Id, JoinType.Left, r.Pid == b.Id, JoinType.Left, b.Pid == d.Id }).
                    OrderBy((e, u, r, b, d) => e.Id, OrderByType.Desc).
                  Select((e, u, r, b, d) => new VBill
                  {
                      Id = e.Id,
                      Pid = e.Pid,
                      Building_id = b.Id,
                      Dorm_id = d.Id,
                      Note = e.Note,
                      Is_active = e.Is_active,
                      PNickname = r.Nickname,
                      Building_Nickname = b.Nickname,
                      Dorm_Nickname = d.Nickname,
                      Cold_water_cost = e.Cold_water_cost,
                      Electric_cost = e.Electric_cost,
                      Hot_water_cost = e.Hot_water_cost,
                      Post_date = e.Post_date,
                      Mod_date = e.Mod_date,
                      Cold_water_value = u.Cold_water_value,
                      Electric_value = u.Electric_value,
                      Hot_water_value = u.Hot_water_value
                  }).ToPageList(page, limit, ref totalCount);
            if (where != null && stat == -1)
                return Db.Queryable<TBill, TUsed, TRoom, TBuilding, TDorm>((e, u, r, b, d) => new object[] { JoinType.Left, e.Pid == u.Id, JoinType.Left, u.Pid == r.Id, JoinType.Left, r.Pid == b.Id, JoinType.Left, b.Pid == d.Id }).
                    Where((e, u, r, b, d) => r.Vid.Contains(where) || r.Nickname.Contains(where) || b.Vid.Contains(where) || b.Nickname.Contains(where) || d.Nickname.Contains(where)).
                    OrderBy((e, u, r, b, d) => e.Id, OrderByType.Desc).
                  Select((e, u, r, b, d) => new VBill
                  {
                      Id = e.Id,
                      Pid = e.Pid,
                      Building_id = b.Id,
                      Dorm_id = d.Id,
                      Note = e.Note,
                      Is_active = e.Is_active,
                      PNickname = r.Nickname,
                      Building_Nickname = b.Nickname,
                      Dorm_Nickname = d.Nickname,
                      Cold_water_cost = e.Cold_water_cost,
                      Electric_cost = e.Electric_cost,
                      Hot_water_cost = e.Hot_water_cost,
                      Post_date = e.Post_date,
                      Mod_date = e.Mod_date,
                      Cold_water_value = u.Cold_water_value,
                      Electric_value = u.Electric_value,
                      Hot_water_value = u.Hot_water_value
                  }).ToPageList(page, limit, ref totalCount);
            if (where != null && stat != -1)
                return Db.Queryable<TBill, TUsed, TRoom, TBuilding, TDorm>((e, u, r, b, d) => new object[] { JoinType.Left, e.Pid == u.Id, JoinType.Left, u.Pid == r.Id, JoinType.Left, r.Pid == b.Id, JoinType.Left, b.Pid == d.Id }).
                    Where((e, u, r, b, d) => e.Is_active == stat).
                    Where((e, u, r, b, d) => r.Vid.Contains(where) || r.Nickname.Contains(where) || b.Vid.Contains(where) || b.Nickname.Contains(where) || d.Nickname.Contains(where)).
                    OrderBy((e, u, r, b, d) => e.Id, OrderByType.Desc).
                  Select((e, u, r, b, d) => new VBill
                  {
                      Id = e.Id,
                      Pid = e.Pid,
                      Building_id = b.Id,
                      Dorm_id = d.Id,
                      Note = e.Note,
                      Is_active = e.Is_active,
                      PNickname = r.Nickname,
                      Building_Nickname = b.Nickname,
                      Dorm_Nickname = d.Nickname,
                      Cold_water_cost = e.Cold_water_cost,
                      Electric_cost = e.Electric_cost,
                      Hot_water_cost = e.Hot_water_cost,
                      Post_date = e.Post_date,
                      Mod_date = e.Mod_date,
                      Cold_water_value = u.Cold_water_value,
                      Electric_value = u.Electric_value,
                      Hot_water_value = u.Hot_water_value,
                  }).ToPageList(page, limit, ref totalCount);
            return Db.Queryable<TBill, TUsed, TRoom, TBuilding, TDorm>((e, u, r, b, d) => new object[] { JoinType.Left, e.Pid == u.Id, JoinType.Left, u.Pid == r.Id, JoinType.Left, r.Pid == b.Id, JoinType.Left, b.Pid == d.Id }).
                Where((e, u, r, b, d) => e.Is_active == stat).
                OrderBy((e, u, r, b, d) => e.Id, OrderByType.Desc).
                  Select((e, u, r, b, d) => new VBill
                  {
                      Id = e.Id,
                      Pid = e.Pid,
                      Building_id = b.Id,
                      Dorm_id = d.Id,
                      Note = e.Note,
                      Is_active = e.Is_active,
                      PNickname = r.Nickname,
                      Building_Nickname = b.Nickname,
                      Dorm_Nickname = d.Nickname,
                      Cold_water_cost = e.Cold_water_cost,
                      Electric_cost = e.Electric_cost,
                      Hot_water_cost = e.Hot_water_cost,
                      Post_date = e.Post_date,
                      Mod_date = e.Mod_date,
                      Cold_water_value = u.Cold_water_value,
                      Electric_value = u.Electric_value,
                      Hot_water_value = u.Hot_water_value
                  }).ToPageList(page, limit, ref totalCount);
            //return Db.Queryable<Entity.Building>().Where((b) => b.Nickname.Contains(where) || b.Note.Contains(where)).ToPageList(page, limit, ref totalCount);
        }

        /// <summary>
        /// 查找登记
        /// </summary>
        /// <param name="page">当前页数</param>
        /// <param name="limit">每页数量</param>
        /// <param name="totalCount">当前页结果数</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public List<VBill> getByPagesByDormId(int page, int limit, int id, ref int totalCount, string where = null, short stat = -1)
        {
            if (where == null && stat == -1)
                return Db.Queryable<TBill, TUsed, TRoom, TBuilding, TDorm>((e, u, r, b, d) => new object[] { JoinType.Left, e.Pid == u.Id, JoinType.Left, u.Pid == r.Id, JoinType.Left, r.Pid == b.Id, JoinType.Left, b.Pid == d.Id }).
                    Where((e, u, r, b, d) => e.Dorm_id == id).
                    OrderBy((e, u, r, b, d) => e.Id, OrderByType.Desc).
                  Select((e, u, r, b, d) => new VBill
                  {
                      Id = e.Id,
                      Pid = e.Pid,
                      Building_id = b.Id,
                      Dorm_id = d.Id,
                      Note = e.Note,
                      Is_active = e.Is_active,
                      PNickname = r.Nickname,
                      Building_Nickname = b.Nickname,
                      Dorm_Nickname = d.Nickname,
                      Cold_water_cost = e.Cold_water_cost,
                      Electric_cost = e.Electric_cost,
                      Hot_water_cost = e.Hot_water_cost,
                      Post_date = e.Post_date,
                      Mod_date = e.Mod_date,
                      Cold_water_value = u.Cold_water_value,
                      Electric_value = u.Electric_value,
                      Hot_water_value = u.Hot_water_value
                  }).ToPageList(page, limit, ref totalCount);
            if (where != null && stat == -1)
                return Db.Queryable<TBill, TUsed, TRoom, TBuilding, TDorm>((e, u, r, b, d) => new object[] { JoinType.Left, e.Pid == u.Id, JoinType.Left, u.Pid == r.Id, JoinType.Left, r.Pid == b.Id, JoinType.Left, b.Pid == d.Id }).
                    Where((e, u, r, b, d) => e.Dorm_id == id).
                    Where((e, u, r, b, d) => r.Vid.Contains(where) || r.Nickname.Contains(where) || b.Vid.Contains(where) || b.Nickname.Contains(where) || d.Nickname.Contains(where)).
                    OrderBy((e, u, r, b, d) => e.Id, OrderByType.Desc).
                  Select((e, u, r, b, d) => new VBill
                  {
                      Id = e.Id,
                      Pid = e.Pid,
                      Building_id = b.Id,
                      Dorm_id = d.Id,
                      Note = e.Note,
                      Is_active = e.Is_active,
                      PNickname = r.Nickname,
                      Building_Nickname = b.Nickname,
                      Dorm_Nickname = d.Nickname,
                      Cold_water_cost = e.Cold_water_cost,
                      Electric_cost = e.Electric_cost,
                      Hot_water_cost = e.Hot_water_cost,
                      Post_date = e.Post_date,
                      Mod_date = e.Mod_date,
                      Cold_water_value = u.Cold_water_value,
                      Electric_value = u.Electric_value,
                      Hot_water_value = u.Hot_water_value
                  }).ToPageList(page, limit, ref totalCount);
            if (where != null && stat != -1)
                return Db.Queryable<TBill, TUsed, TRoom, TBuilding, TDorm>((e, u, r, b, d) => new object[] { JoinType.Left, e.Pid == u.Id, JoinType.Left, u.Pid == r.Id, JoinType.Left, r.Pid == b.Id, JoinType.Left, b.Pid == d.Id }).
                    Where((e, u, r, b, d) => e.Dorm_id == id).
                    Where((e, u, r, b, d) => e.Is_active == stat).
                    Where((e, u, r, b, d) => r.Vid.Contains(where) || r.Nickname.Contains(where) || b.Vid.Contains(where) || b.Nickname.Contains(where) || d.Nickname.Contains(where)).
                    OrderBy((e, u, r, b, d) => e.Id, OrderByType.Desc).
                  Select((e, u, r, b, d) => new VBill
                  {
                      Id = e.Id,
                      Pid = e.Pid,
                      Building_id = b.Id,
                      Dorm_id = d.Id,
                      Note = e.Note,
                      Is_active = e.Is_active,
                      PNickname = r.Nickname,
                      Building_Nickname = b.Nickname,
                      Dorm_Nickname = d.Nickname,
                      Cold_water_cost = e.Cold_water_cost,
                      Electric_cost = e.Electric_cost,
                      Hot_water_cost = e.Hot_water_cost,
                      Post_date = e.Post_date,
                      Mod_date = e.Mod_date,
                      Cold_water_value = u.Cold_water_value,
                      Electric_value = u.Electric_value,
                      Hot_water_value = u.Hot_water_value
                  }).ToPageList(page, limit, ref totalCount);
            return Db.Queryable<TBill, TUsed, TRoom, TBuilding, TDorm>((e, u, r, b, d) => new object[] { JoinType.Left, e.Pid == u.Id, JoinType.Left, u.Pid == r.Id, JoinType.Left, r.Pid == b.Id, JoinType.Left, b.Pid == d.Id }).
                Where((e, u, r, b, d) => e.Dorm_id == id).
                Where((e, u, r, b, d) => e.Is_active == stat).
                OrderBy((e, u, r, b, d) => e.Id, OrderByType.Desc).
                  Select((e, u, r, b, d) => new VBill
                  {
                      Id = e.Id,
                      Pid = e.Pid,
                      Building_id = b.Id,
                      Dorm_id = d.Id,
                      Note = e.Note,
                      Is_active = e.Is_active,
                      PNickname = r.Nickname,
                      Building_Nickname = b.Nickname,
                      Dorm_Nickname = d.Nickname,
                      Cold_water_cost = e.Cold_water_cost,
                      Electric_cost = e.Electric_cost,
                      Hot_water_cost = e.Hot_water_cost,
                      Post_date = e.Post_date,
                      Mod_date = e.Mod_date,
                      Cold_water_value = u.Cold_water_value,
                      Electric_value = u.Electric_value,
                      Hot_water_value = u.Hot_water_value
                  }).ToPageList(page, limit, ref totalCount);
            //return Db.Queryable<Entity.Building>().Where((b) => b.Nickname.Contains(where) || b.Note.Contains(where)).ToPageList(page, limit, ref totalCount);
        }
    }
}
