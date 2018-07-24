using sdglsys.Entity;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace sdglsys.DbHelper
{
    public class Useds : DbContext
    {
        public List<Entity.TUsed> getAll()
        {
            return Db.Queryable<Entity.TUsed>().ToList();
        }
        

        /// <summary>
        /// 通过ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entity.TUsed findById(int id)
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
            return UsedDb.DeleteById(id);
        }

        /// <summary>
        /// 更新登记信息
        /// </summary>
        /// <param name="Room"></param>
        /// <returns></returns>
        public bool Update(Entity.TUsed used)
        {
            return UsedDb.Update(used);
        }

        /// <summary>
        /// 添加登记信息
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public bool Add(Entity.TUsed used)
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
            var i = Db.Queryable<TUsed>().
                Where(u => id == u.Pid && SqlFunc.Between(SqlFunc.Substring(u.Post_date, 0, 7),
                SqlFunc.Substring(DateTime.Now, 0, 7), SqlFunc.Substring(DateTime.Now, 0, 7))).Select(u => u.Pid).First();
            return i > 0 ? true : false;
        }

        /// <summary>
        /// 查找登记
        /// </summary>
        /// <param name="page">当前页数</param>
        /// <param name="limit">每页数量</param>
        /// <param name="totalCount">当前页结果数</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public List<VUsed> getByPages(int page, int limit, ref int totalCount, string where = null)
        {
            if (where == null)
                return Db.Queryable<TUser ,TUsed, TRoom, TBuilding, TDorm>((user,u, r, b, d) => new object[] {JoinType.Left,user.Id == u.Post_uid, JoinType.Left, u.Pid == r.Id, JoinType.Left, r.Pid == b.Id , JoinType.Left, b.Pid == d.Id }).
                    Where((user, u, r, b, d) => SqlFunc.IsNullOrEmpty(u.Id) == false).OrderBy((user, u, r, b, d)=>u.Id, OrderByType.Desc).
                  Select((user, u, r, b, d) => new VUsed
                  {
                      Id = u.Id,
                      Pid = r.Pid,
                      Building_id = b.Id,
                      Dorm_id = d.Id,
                      Note = u.Note,
                      Is_active = u.Is_active,
                      Post_Nickname = user.Nickname,
                      PNickname = r.Nickname,
                      Building_Nickname = b.Nickname,
                      Dorm_Nickname = d.Nickname,
                      Cold_water_value = u.Cold_water_value,
                      Electric_value = u.Electric_value,
                      Hot_water_value = u.Hot_water_value,
                      Post_date = u.Post_date
                  }).ToPageList(page, limit, ref totalCount);
            return Db.Queryable<TUser, TUsed, TRoom, TBuilding, TDorm>((user, u, r, b, d) => new object[] { JoinType.Left, user.Id == u.Post_uid, JoinType.Left, u.Pid == r.Id, JoinType.Left, r.Pid == b.Id, JoinType.Left, b.Pid == d.Id }).
                Where((user, u, r, b, d) => SqlFunc.IsNullOrEmpty(u.Id) == false).
                Where((user, u, r, b, d) => r.Nickname.Contains(where) || r.Note.Contains(where) || r.Vid.Contains(where)).
                OrderBy((user, u, r, b, d) => u.Id, OrderByType.Desc).
                  Select((user, u, r, b, d) => new VUsed
                  {
                      Id = u.Id,
                      Pid = r.Pid,
                      Building_id = b.Id,
                      Dorm_id = d.Id,
                      Note = u.Note,
                      Is_active = u.Is_active,
                      Post_Nickname = user.Nickname,
                      PNickname = r.Nickname,
                      Building_Nickname = b.Nickname,
                      Dorm_Nickname = d.Nickname,
                      Cold_water_value = u.Cold_water_value,
                      Electric_value = u.Electric_value,
                      Hot_water_value = u.Hot_water_value,
                      Post_date = u.Post_date
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
        public List<VUsed> getByPagesByDormId(int page, int limit,int id, ref int totalCount, string where = null)
        {
            if (where == null)
                return Db.Queryable<TUser, TUsed, TRoom, TBuilding, TDorm>((user, u, r, b, d) => new object[] { JoinType.Left, user.Id == u.Post_uid, JoinType.Left, u.Pid == r.Id, JoinType.Left, r.Pid == b.Id, JoinType.Left, b.Pid == d.Id }).
                    Where((user, u, r, b, d) => u.Dorm_id == id && SqlFunc.IsNullOrEmpty(u.Id) == false).OrderBy((user, u, r, b, d) => u.Id, OrderByType.Desc).
                    OrderBy((user, u, r, b, d) => u.Id, OrderByType.Desc).
                  Select((user, u, r, b, d) => new VUsed
                  {
                      Id = u.Id,
                      Pid = r.Pid,
                      Building_id = b.Id,
                      Dorm_id = d.Id,
                      Note = u.Note,
                      Is_active = u.Is_active,
                      Post_Nickname = user.Nickname,
                      PNickname = r.Nickname,
                      Building_Nickname = b.Nickname,
                      Dorm_Nickname = d.Nickname,
                      Cold_water_value = u.Cold_water_value,
                      Electric_value = u.Electric_value,
                      Hot_water_value = u.Hot_water_value,
                      Post_date = u.Post_date
                  }).ToPageList(page, limit, ref totalCount);
            return Db.Queryable<TUser, TUsed, TRoom, TBuilding, TDorm>((user, u, r, b, d) => new object[] { JoinType.Left, user.Id == u.Post_uid, JoinType.Left, u.Pid == r.Id, JoinType.Left, r.Pid == b.Id, JoinType.Left, b.Pid == d.Id }).
                Where((user, u, r, b, d) => SqlFunc.IsNullOrEmpty(u.Id) == false).
                Where((user, u, r, b, d) => r.Nickname.Contains(where) || r.Note.Contains(where) || r.Vid.Contains(where)).
                Where((user, u, r, b, d) => u.Dorm_id == id).
                OrderBy((user, u, r, b, d) => u.Id, OrderByType.Desc).
                  Select((user ,u, r, b, d) => new VUsed
                  {
                      Id = u.Id,
                      Pid = r.Pid,
                      Building_id = b.Id,
                      Dorm_id = d.Id,
                      Note = u.Note,
                      Is_active = u.Is_active,
                      Post_Nickname = user.Nickname,
                      PNickname = r.Nickname,
                      Building_Nickname = b.Nickname,
                      Dorm_Nickname = d.Nickname,
                      Cold_water_value = u.Cold_water_value,
                      Electric_value = u.Electric_value,
                      Hot_water_value = u.Hot_water_value,
                      Post_date = u.Post_date
                  }).ToPageList(page, limit, ref totalCount);
            //return Db.Queryable<Entity.Building>().Where((b) => b.Nickname.Contains(where) || b.Note.Contains(where)).ToPageList(page, limit, ref totalCount);
        }


        /// <summary>
        /// 获取用量统计信息
        /// </summary>
        /// <param name="_type">查找类型：1园区，2宿舍楼，3宿舍，5为所有园区，0为所有园区最近一年的统计，默认0</param>
        /// <param name="_id">相关类型的ID</param>
        /// <param name="_start">开始日期</param>
        /// <param name="_end">截至日期</param>
        /// <returns></returns>
        public Used_datas GetUsedDatas(int _type=0,int _id=0, DateTime _start=default(DateTime), DateTime _end= default(DateTime)) {
            var data = new Used_datas();
            var list = new List<used_data>();
            switch (_type) {
                case 1:
                    // 获取园区信息
                    var Dorm = new Dorms();
                    var dorm = Dorm.findById(_id);
                    if (dorm == null) {
                        data.info = "找不到编号为 "+ _id + " 的园区";
                        break;
                    }
                    data.title = "园区 " + dorm.Nickname + " 从 " + _start + " 到 " + _end + " 的统计图表";
                    list = Db.Queryable<TUsed>().Where(u => u.Is_active == true && u.Dorm_id == _id && SqlFunc.Between(u.Post_date, _start, _end)).GroupBy(u => SqlFunc.Substring(u.Post_date, 1, 7)).Select(u => new used_data
                    {
                        Date = SqlFunc.Substring(u.Post_date, 0, 7),
                        Cold_water_value = SqlFunc.AggregateSum(u.Cold_water_value),
                        Electric_value = SqlFunc.AggregateSum(u.Electric_value),
                        Hot_water_value = SqlFunc.AggregateSum(u.Hot_water_value)
                    }).ToList();
                    break;
                case 2:
                    // 获取宿舍楼信息
                    var B = new Buildings();
                    var b = B.findById(_id);
                    if (b == null)
                    {
                        data.info = "找不到编号为 " + _id + " 的宿舍楼";
                        break;
                    }
                    data.title = "宿舍楼 " + b.Nickname + " 从 " + _start + " 到 " + _end + " 的统计图表";
                    list = Db.Queryable<TUsed>().Where(u => u.Is_active == true && u.Building_id == _id && SqlFunc.Between(u.Post_date, _start, _end)).GroupBy(u => SqlFunc.Substring(u.Post_date, 1, 7)).Select(u => new used_data
                    {
                        Date = SqlFunc.Substring(u.Post_date, 0, 7),
                        Cold_water_value = SqlFunc.AggregateSum(u.Cold_water_value),
                        Electric_value = SqlFunc.AggregateSum(u.Electric_value),
                        Hot_water_value = SqlFunc.AggregateSum(u.Hot_water_value)
                    }).ToList();
                    break;
                case 3:
                    // 获取宿舍信息
                    var R = new Buildings();
                    var r = R.findById(_id);
                    if (r == null)
                    {
                        data.info = "找不到编号为 " + _id + " 的宿舍";
                        break;
                    }
                    data.title = "宿舍 " + r.Nickname + " 从 " + _start + " 到 " + _end + " 的统计图表";
                    list = Db.Queryable<TUsed>().Where(u => u.Is_active == true && u.Pid == _id && SqlFunc.Between(u.Post_date, _start, _end)).GroupBy(u => SqlFunc.Substring(u.Post_date, 1, 7)).Select(u => new used_data
                    {
                        Date = SqlFunc.Substring(u.Post_date, 0, 7),
                        Cold_water_value = SqlFunc.AggregateSum(u.Cold_water_value),
                        Electric_value = SqlFunc.AggregateSum(u.Electric_value),
                        Hot_water_value = SqlFunc.AggregateSum(u.Hot_water_value)
                    }).ToList();
                    break;
                case 5:
                    data.title = "所有园区从 " + _start + " 到 " + _end + " 的统计图表";
                    list = Db.Queryable<TUsed>().Where(u => u.Is_active == true && SqlFunc.Between(u.Post_date, _start, _end)).GroupBy(u => SqlFunc.Substring(u.Post_date, 1, 7)).Select(u => new used_data
                    {
                        Date = SqlFunc.Substring(u.Post_date, 0, 7),
                        Cold_water_value = SqlFunc.AggregateSum(u.Cold_water_value),
                        Electric_value = SqlFunc.AggregateSum(u.Electric_value),
                        Hot_water_value = SqlFunc.AggregateSum(u.Hot_water_value)
                    }).ToList();
                    break;
                default:
                    data.title = "所有园区最近一年的统计图表";
                    list = Db.Queryable<TUsed>().Where(u => u.Is_active == true && SqlFunc.Between(u.Post_date, DateTime.Now.AddYears(-1), DateTime.Now)).GroupBy(u => SqlFunc.Substring(u.Post_date, 1, 7)).Select(u => new used_data
                    {
                        Date = SqlFunc.Substring(u.Post_date, 0, 7),
                        Cold_water_value = SqlFunc.AggregateSum(u.Cold_water_value),
                        Electric_value = SqlFunc.AggregateSum(u.Electric_value),
                        Hot_water_value = SqlFunc.AggregateSum(u.Hot_water_value)
                    }).ToList();
                    break;
            }
            data.Add(list); // 加入返回列表
            if (list.Count < 1)
                data.info = "暂无相关数据";
            return data;
        }
    }
}
