using sdglsys.Entity;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace sdglsys.DbHelper
{
    public class Useds : DbContext
    {
        /// <summary>
        /// 通过ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entity.T_Used FindById(int id)
        {
            return UsedDb.GetById(id);
        }

        /// <summary>
        /// 通过ID删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            var used = FindById(id);
            if (used != null)
            {
                used.Used_model_state = false;
                return Update(used);
            }
            return false;
        }

        public void BillDelete(int id)
        {
            /// 开始事务
            Db.Ado.BeginTran();
            try
            {
                ///1.1获取记录数据
                var used = Db.Context.Queryable<Entity.T_Used>().Single(u => u.Used_model_state && u.Used_id == id);
                if (used == null)
                {
                    throw new Exception("该记录已被删除");
                }

                ///2.判断账单状态
                var bill = Db.Ado.Context.Queryable<Entity.T_Bill>().Single(b => b.Bill_model_state && b.Bill_used_id == used.Used_id);
                if (bill != null && bill.Bill_is_active != 1)
                {
                    throw new Exception("关联账单的状态已被更改，无法删除");
                }
                else if (bill != null)
                {
                    bill.Bill_model_state = false; // 标记账单为已删除
                    if (!Db.Updateable(bill).ExecuteCommandHasChange())
                    {
                        throw new Exception("删除关联账单时发生错误");
                    }
                }

                ///3.更新读表信息
                var Used_total = new Useds_total();
                var last = Used_total.Last(used.Used_room_id);
                if (last != null)
                {
                    // 读表数值=读表数值-本次读数
                    last.Ut_cold_water_value -= used.Used_cold_water_value;
                    last.Ut_hot_water_value -= used.Used_hot_water_value;
                    last.Ut_electric_value -= used.Used_electric_value;

                    /// 避免产生负数
                    last.Ut_cold_water_value = last.Ut_cold_water_value >= 0 ? last.Ut_cold_water_value : 0;
                    last.Ut_hot_water_value = used.Used_hot_water_value >= 0 ? last.Ut_hot_water_value : 0;
                    last.Ut_electric_value = used.Used_electric_value >= 0 ? last.Ut_electric_value : 0;
                }
                else
                {
                    last = new T_Used_total();
                    last.Ut_dorm_id = used.Used_dorm_id;
                    last.Ut_building_id = used.Used_building_id;
                    last.Ut_room_id = used.Used_room_id;
                }
                last.Ut_post_date = DateTime.Now;

                ///4保存读表信息
                if (last.Ut_id < 1)
                {
                    if (Db.Insertable(last).ExecuteCommand() < 1)
                    {
                        throw new Exception("更新读表信息时发生错误！");
                    }
                }
                else if (Db.Updateable(last).ExecuteCommand() < 1)
                {
                    throw new Exception("更新读表信息时发生错误！");
                }
                used.Used_model_state = false; // 标记为已删除

                if (Db.Ado.Context.Updateable(used).ExecuteCommandHasChange())
                {
                    Db.Ado.CommitTran();// 提交事务
                }
                else
                {
                    throw new Exception("发生未知错误");
                }
            }
            catch (Exception)
            {
                Db.Ado.RollbackTran();
                throw;
            }
        }

        /// <summary>
        /// 更新登记信息
        /// </summary>
        /// <param name="Room"></param>
        /// <returns></returns>
        public bool Update(Entity.T_Used used)
        {
            used.Used_post_date = System.DateTime.Now; // 更新时间
            return UsedDb.Update(used);
        }

        /// <summary>
        /// 添加登记信息
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public bool Add(Entity.T_Used used)
        {
            return UsedDb.Insert(used);
        }

        /// <summary>
        /// 检测该宿舍本月是否已登记
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsRecord(int id)
        {
            var i = Db.Queryable<T_Used>().
                Where(u => u.Used_model_state && id == u.Used_room_id && SqlFunc.Between(SqlFunc.Substring(u.Used_post_date, 0, 7),
                SqlFunc.Substring(DateTime.Now, 0, 7), SqlFunc.Substring(DateTime.Now, 0, 7))).Select(u => u.Used_room_id).First();
            return i > 0;
        }

        /// <summary>
        /// 查找登记
        /// </summary>
        /// <param name="page">当前页数</param>
        /// <param name="limit">每页数量</param>
        /// <param name="totalCount">当前页结果数</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public List<VUsed> GetByPages(int page, int limit, ref int totalCount, string where = null, int dorm_id = 0)
        {
            var sql = Db.Queryable<T_Used, T_Room, T_Building, T_Dorm, T_User>((u, r, b, d, uu) => new object[] { JoinType.Left, u.Used_room_id == r.Room_id, JoinType.Left, r.Room_building_id == b.Building_id, JoinType.Left, b.Building_dorm_id == d.Dorm_id, JoinType.Left, u.Used_post_user_id == uu.User_id }).
                    Where((u, r, b, d, uu) => u.Used_model_state).OrderBy((u, r, b, d, uu) => u.Used_id, OrderByType.Desc);
            if (dorm_id != 0)
            {
                sql = sql.Where((u) => u.Used_dorm_id == dorm_id);
            }
            if (!string.IsNullOrWhiteSpace(where))
            {
                sql = sql.Where((u, r) => r.Room_nickname.Contains(where) || r.Room_vid.Contains(where) || r.Room_note.Contains(where));
            }
            return sql.Select((u, r, b, d, uu) => new VUsed
            {
                Used_Dorm_Nickname = d.Dorm_nickname,
                Used_Building_Nickname = b.Building_nickname,
                Used_Room_Nickname = r.Room_nickname,
                Used_Cold_water_value = u.Used_cold_water_value,
                Used_Hot_water_value = u.Used_hot_water_value,
                Used_Electric_value = u.Used_electric_value,
                Used_Id = u.Used_id,
                Used_Note = u.Used_note,
                Used_Post_date = u.Used_post_date,
                Used_Is_active = u.Used_is_active,
                Used_Post_User_Nickname = uu.User_nickname,
            }).ToPageList(page, limit, ref totalCount);
        }

        /// <summary>
        /// 获取用量统计信息
        /// </summary>
        /// <param name="_type">查找类型：1园区，2宿舍楼，3宿舍，5为所有园区，0为所有园区最近一年的统计，默认0</param>
        /// <param name="_id">相关类型的ID</param>
        /// <param name="_start">开始日期</param>
        /// <param name="_end">截至日期</param>
        /// <returns></returns>
        public Used_datas GetUsedDatas(int _type = 0, int _id = 0, DateTime _start = default(DateTime), DateTime _end = default(DateTime))
        {
            var data = new Used_datas();
            var list = new List<Used_data>();
            var sql = Db.Queryable<T_Used>().Where(u => u.Used_model_state && u.Used_is_active);
            switch (_type)
            {
                case 1:
                    // 获取园区信息
                    var Dorm = new Dorms();
                    var dorm = Dorm.FindById(_id);
                    if (dorm == null)
                    {
                        data.info = "找不到编号为 " + _id + " 的园区";
                        break;
                    }
                    data.title = "园区 " + dorm.Dorm_nickname + " 从 " + _start + " 到 " + _end + " 的统计图表";
                    sql = sql.Where(u => u.Used_dorm_id == _id);
                    break;

                case 2:
                    // 获取宿舍楼信息
                    var B = new Buildings();
                    var b = B.FindById(_id);
                    if (b == null)
                    {
                        data.info = "找不到编号为 " + _id + " 的宿舍楼";
                        break;
                    }
                    data.title = "宿舍楼 " + b.Building_nickname + " 从 " + _start + " 到 " + _end + " 的统计图表";
                    sql = sql.Where(u => u.Used_building_id == _id);
                    break;

                case 3:
                    // 获取宿舍信息
                    var R = new Rooms();
                    var r = R.FindById(_id);
                    if (r == null)
                    {
                        data.info = "找不到编号为 " + _id + " 的宿舍";
                        break;
                    }
                    data.title = "宿舍 " + r.Room_nickname + " 从 " + _start + " 到 " + _end + " 的统计图表";
                    sql = sql.Where(u => u.Used_room_id == _id);
                    break;

                case 5:
                    data.title = "所有园区从 " + _start + " 到 " + _end + " 的统计图表";
                    break;

                default:
                    data.title = "所有园区最近一年的统计图表";
                    _start = DateTime.Now.AddYears(-1);
                    _end = DateTime.Now;
                    break;
            }
            sql = sql.Where(u => SqlFunc.Between(u.Used_post_date, _start, _end));
            list = sql.GroupBy(u => SqlFunc.Substring(u.Used_post_date, 1, 7)).Select(u => new Used_data
            {
                Date = SqlFunc.Substring(u.Used_post_date, 0, 7),
                Cold_water_value = SqlFunc.AggregateSum(u.Used_cold_water_value),
                Electric_value = SqlFunc.AggregateSum(u.Used_electric_value),
                Hot_water_value = SqlFunc.AggregateSum(u.Used_hot_water_value)
            }).ToList();
            data.Add(list); // 加入返回列表
            if (list.Count < 1)
                data.info = "暂无相关数据";
            return data;
        }
    }
}