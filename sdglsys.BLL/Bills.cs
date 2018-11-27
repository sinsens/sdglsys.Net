using sdglsys.Entity;
using SqlSugar;
using System.Collections.Generic;

namespace sdglsys.DbHelper
{
    public class Bills : DbContext
    {
        /// <summary>
        /// 通过ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entity.T_Bill FindById(int id)
        {
            return BillDb.GetById(id);
        }

        /// <summary>
        /// 通过ID查询账单视图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entity.VBill FindVBillById(int id)
        {
            return Db.Queryable<T_Bill, T_Used, T_Room, T_Building, T_Dorm>((e, u, r, b, d) => new object[] { JoinType.Left, e.Bill_room_id == u.Used_id, JoinType.Left, u.Used_room_id == r.Room_id, JoinType.Left, r.Room_building_id == b.Building_id, JoinType.Left, b.Building_dorm_id == d.Dorm_id }).
                Where(e => e.Bill_id == id && e.Bill_model_state).Select((e, u, r, b, d) => new VBill
                {
                    Bill_Id = e.Bill_id,
                    Bill_Room_Nickname = r.Room_nickname,
                    Bill_Building_Nickname = b.Building_nickname,
                    Bill_Dorm_Nickname = d.Dorm_nickname,
                    Bill_Electric_cost = e.Bill_electric_cost,
                    Bill_Hot_water_cost = e.Bill_hot_water_cost,
                    Bill_Cold_water_cost = e.Bill_cold_water_cost,
                    Bill_Electric_value = u.Used_electric_value,
                    Bill_Cold_water_value = u.Used_cold_water_value,
                    Bill_Hot_water_value = u.Used_hot_water_value,
                    Bill_Is_active = e.Bill_is_active,
                    Bill_Note = e.Bill_note,
                    Bill_Post_date = e.Bill_post_date,
                    Bill_Mod_date = e.Bill_mod_date
                }).Single();
        }

        /// <summary>
        /// 通过ID删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            var bill = FindById(id);
            if (bill != null)
            {
                bill.Bill_model_state = false;
                return BillDb.Update(bill);
            }
            return false;
        }

        /// <summary>
        /// 更新账单信息
        /// </summary>
        /// <param name="Room"></param>
        /// <returns></returns>
        public bool Update(Entity.T_Bill bill)
        {
            bill.Bill_mod_date = System.DateTime.Now;
            return BillDb.Update(bill);
        }

        /// <summary>
        /// 添加登记信息
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public bool Add(Entity.T_Bill bill)
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
        /// <param name="stat">账单状态：-1全部，0已注销，1已登记，2已结算</param>
        /// <param name="pid">园区ID：0</param>
        /// <returns></returns>
        public List<VBill> GetByPages(int page, int limit, ref int totalCount, string where = null, short stat = -1, int pid = 0)
        {
            var sql = Db.Queryable<T_Bill, T_Used, T_Room, T_Building, T_Dorm>((e, u, r, b, d) => new object[] { JoinType.Left, e.Bill_used_id == u.Used_id, JoinType.Left, u.Used_room_id == r.Room_id, JoinType.Left, r.Room_building_id == b.Building_id, JoinType.Left, b.Building_dorm_id == d.Dorm_id }).
                    OrderBy((e, u, r, b, d) => e.Bill_id, OrderByType.Desc).Where(e => e.Bill_model_state);

            if (where != null)
            {
                sql = sql.Where((e, u, r, b, d) => r.Room_vid.Contains(where) || r.Room_nickname.Contains(where) || b.Building_vid.Contains(where) || b.Building_nickname.Contains(where) || d.Dorm_nickname.Contains(where));
            }

            if (stat != -1)
            {
                sql = sql.Where((e, u, r, b, d) => e.Bill_is_active == stat);
            }
            if (pid != 0)
            {
                sql = sql.Where((e, u, r, b, d) => pid == e.Bill_dorm_id);
            }
            return sql.Select((e, u, r, b, d) => new VBill
            {
                Bill_Id = e.Bill_id,
                Bill_Room_Nickname = r.Room_nickname,
                Bill_Building_Nickname = b.Building_nickname,
                Bill_Dorm_Nickname = d.Dorm_nickname,
                Bill_Electric_cost = e.Bill_electric_cost,
                Bill_Hot_water_cost = e.Bill_hot_water_cost,
                Bill_Cold_water_cost = e.Bill_cold_water_cost,
                Bill_Electric_value = u.Used_electric_value,
                Bill_Cold_water_value = u.Used_cold_water_value,
                Bill_Hot_water_value = u.Used_hot_water_value,
                Bill_Is_active = e.Bill_is_active,
                Bill_Note = e.Bill_note,
                Bill_Post_date = e.Bill_post_date,
                Bill_Mod_date = e.Bill_mod_date
            }).ToPageList(page, limit, ref totalCount);
        }
    }
}