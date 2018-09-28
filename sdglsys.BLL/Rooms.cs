using sdglsys.Entity;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace sdglsys.DbHelper
{
    public class Rooms : DbContext
    {
        public List<Entity.TRoom> getAll()
        {
            return Db.Queryable<Entity.TRoom>().ToList();
        }

        /// <summary>
        /// 获取已启用的宿舍
        /// </summary>
        /// <returns></returns>
        public List<Entity.TRoom> getAllActive()
        {
            return Db.Queryable<Entity.TRoom>().Where(a => a.Is_active == true).ToList();
        }

        /// <summary>
        /// 获取已启用的宿舍
        /// </summary>
        /// <param name="id">园区</param>
        /// <returns></returns>
        public List<Entity.TRoom> getAllActive(int id)
        {
            return Db.Queryable<Entity.TRoom>().Where(a => a.Is_active == true && a.Dorm_id == id).ToList();
        }

        /// <summary>
        /// 获取已启用的宿舍
        /// </summary>
        /// <returns></returns>
        public List<Entity.VRoom> getAllVRoomActive()
        {
            return Db.Queryable<TRoom, TBuilding, TDorm>((r, b, d) => new object[] { JoinType.Left, r.Pid == b.Id, JoinType.Left, b.Pid == d.Id }).OrderBy(r => r.Vid).
                  Select((r, b, d) => new VRoom
                  {
                      Id = r.Id,
                      Pid = r.Pid,
                      Vid = r.Vid,
                      Dorm_id = r.Dorm_id,
                      Nickname = r.Nickname,
                      Note = r.Note,
                      PNickname = b.Nickname,
                      Is_active = r.Is_active,
                      Dorm_Nickname = d.Nickname
                  }).ToList();
        }

        /// <summary>
        /// 获取已启用的宿舍
        /// </summary>
        /// <param name="id">园区</param>
        /// <returns></returns>
        public List<Entity.VRoom> getAllVRoomActive(int id)
        {
            return Db.Queryable<TRoom, TBuilding, TDorm>((r, b, d) => new object[] { JoinType.Left, r.Pid == b.Id, JoinType.Left, b.Pid == d.Id }).OrderBy(r => r.Vid).
                Where(r => r.Dorm_id == id).
                  Select((r, b, d) => new VRoom
                  {
                      Id = r.Id,
                      Pid = r.Pid,
                      Vid = r.Vid,
                      Dorm_id = r.Dorm_id,
                      Nickname = r.Nickname,
                      Note = r.Note,
                      PNickname = b.Nickname,
                      Is_active = r.Is_active,
                      Dorm_Nickname = d.Nickname
                  }).ToList();
        }

        /// <summary>
        /// 通过ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entity.TRoom FindById(int id)
        {
            return RoomDb.GetById(id);
        }

        /// <summary>
        /// 通过ID删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            return RoomDb.DeleteById(id);
        }

        /// <summary>
        /// 更新宿舍信息
        /// </summary>
        /// <param name="Room"></param>
        /// <returns></returns>
        public bool Update(Entity.TRoom room)
        {
            return RoomDb.Update(room);
        }

        /// <summary>
        /// 添加宿舍信息
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public bool Add(Entity.TRoom room)
        {
            return RoomDb.Insert(room);
        }

        /// <summary>
        /// 查找宿舍
        /// </summary>
        /// <param name="page">当前页数</param>
        /// <param name="limit">每页数量</param>
        /// <param name="totalCount">当前页结果数</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public List<VRoom> getByPages(int page, int limit, ref int totalCount, string where = null)
        {
            if (where == null)
                return Db.Queryable<TRoom, TBuilding, TDorm>((r, b, d) => new object[] { JoinType.Left, r.Pid == b.Id, JoinType.Left, b.Pid == d.Id }).OrderBy(r => r.Id, OrderByType.Desc).
                  Select((r, b, d) => new VRoom
                  {
                      Id = r.Id,
                      Pid = r.Pid,
                      Vid = r.Vid,
                      Nickname = r.Nickname,
                      Note = r.Note,
                      PNickname = b.Nickname,
                      Is_active = r.Is_active,
                      Dorm_Nickname = d.Nickname,
                      Number = r.Number
                  }).ToPageList(page, limit, ref totalCount);
            return Db.Queryable<TRoom, TBuilding, TDorm>((r, b, d) => new object[] { JoinType.Left,
                 r.Pid == b.Id, JoinType.Left,b.Pid == d.Id }).Where((r) => r.Nickname.Contains(where) || r.Note.Contains(where) || r.Vid.Contains(where)).OrderBy(r => r.Id, OrderByType.Desc).
                 Select((r, b, d) => new VRoom
                 {
                     Id = r.Id,
                     Pid = r.Pid,
                     Vid = r.Vid,
                     Nickname = r.Nickname,
                     Note = r.Note,
                     PNickname = b.Nickname,
                     Is_active = r.Is_active,
                     Dorm_Nickname = d.Nickname,
                     Number = r.Number
                 }).ToPageList(page, limit, ref totalCount);

        }

        /// <summary>
        /// 查找宿舍
        /// </summary>
        /// <param name="pid">园区ID</param>
        /// <param name="page">当前页数</param>
        /// <param name="limit">每页数量</param>
        /// <param name="totalCount">当前页结果数</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public List<VRoom> getByPages(int pid, int page, int limit, ref int totalCount, string where = null)
        {
            if (where == null)
                return Db.Queryable<TRoom, TBuilding, TDorm>((r, b, d) => new object[] { JoinType.Left, r.Pid == b.Id, JoinType.Left, b.Pid == d.Id }).Where(r => r.Dorm_id == pid).OrderBy(r => r.Id, OrderByType.Desc).
                  Select((r, b, d) => new VRoom
                  {
                      Id = r.Id,
                      Pid = r.Pid,
                      Vid = r.Vid,
                      Nickname = r.Nickname,
                      Note = r.Note,
                      PNickname = b.Nickname,
                      Is_active = r.Is_active,
                      Dorm_Nickname = d.Nickname
                  }).ToPageList(page, limit, ref totalCount);
            return Db.Queryable<TRoom, TBuilding, TDorm>((r, b, d) => new object[] { JoinType.Left,
                 r.Pid == b.Id, JoinType.Left,b.Pid == d.Id }).Where(r => r.Dorm_id == pid).Where((r) => r.Nickname.Contains(where) || r.Note.Contains(where) || r.Vid.Contains(where)).OrderBy(r => r.Id, OrderByType.Desc).
                 Select((r, b, d) => new VRoom
                 {
                     Id = r.Id,
                     Pid = r.Pid,
                     Vid = r.Vid,
                     Nickname = r.Nickname,
                     Note = r.Note,
                     PNickname = b.Nickname,
                     Is_active = r.Is_active,
                     Dorm_Nickname = d.Nickname
                 }).ToPageList(page, limit, ref totalCount);

        }


        /// <summary>
        /// 按园区查找当月未登记宿舍
        /// </summary>
        /// <param name="id">园区ID</param>
        /// <returns></returns>
        public List<VRoom> getAllNoRecordByDorm(int id)
        {
            /// 先获取本月已登记的宿舍ID
            var rooms = Db.Queryable<TUsed>().
                Where(u => SqlFunc.Between(SqlFunc.Substring(u.Post_date, 0, 7),
                SqlFunc.Substring(DateTime.Now, 0, 7), SqlFunc.Substring(DateTime.Now, 0, 7))).Select(u => u.Pid).ToList();

            return Db.Queryable<TRoom, TBuilding, TDorm>((r, b, d) => new object[] {
                JoinType.Left, r.Pid == b.Id,
                JoinType.Left, b.Pid == d.Id }).Where((r, b, d) => r.Dorm_id == id && r.Is_active == true && !rooms.Contains(r.Id)).OrderBy(r => r.Vid).
              Select((r, b, d) => new VRoom
              {
                  Id = r.Id,
                  Pid = r.Pid,
                  Dorm_id = r.Dorm_id,
                  Vid = r.Vid,
                  Nickname = r.Nickname,
                  Note = r.Note,
                  PNickname = b.Nickname,
                  Is_active = r.Is_active,
                  Dorm_Nickname = d.Nickname
              }).ToList();
        }

        /// <summary>
        /// 按宿舍楼查找当月未登记宿舍
        /// </summary>
        /// <param name="id">宿舍楼ID</param>
        /// <returns></returns>
        public List<VRoom> getAllNoRecordByBuilding(int id)
        {
            /// 先获取本月已登记的宿舍ID
            var rooms = Db.Queryable<TUsed>().
                Where(u => SqlFunc.Between(SqlFunc.Substring(u.Post_date, 0, 7),
                SqlFunc.Substring(DateTime.Now, 0, 7), SqlFunc.Substring(DateTime.Now, 0, 7))).Select(u => u.Pid).ToList();

            return Db.Queryable<TRoom, TBuilding, TDorm>((r, b, d) => new object[] {
                JoinType.Left, r.Pid == b.Id,
                JoinType.Left, b.Pid == d.Id }).Where((r, b, d) => r.Pid == id && r.Is_active == true && !rooms.Contains(r.Id)).OrderBy(r => r.Vid).
              Select((r, b, d) => new VRoom
              {
                  Id = r.Id,
                  Pid = r.Pid,
                  Dorm_id = r.Dorm_id,
                  Vid = r.Vid,
                  Nickname = r.Nickname,
                  Note = r.Note,
                  PNickname = b.Nickname,
                  Is_active = r.Is_active,
                  Dorm_Nickname = d.Nickname
              }).ToList();
        }

        /// <summary>
        /// 按宿舍楼查找当月未登记宿舍并返回Json
        /// </summary>
        /// <param name="id">宿舍楼ID</param>
        /// <returns></returns>
        public string getJsonAllNoRecordByBuilding(int id)
        {
            /// 先获取本月已登记的宿舍ID
            var rooms = Db.Queryable<TUsed>().
                Where(u => SqlFunc.Between(SqlFunc.Substring(u.Post_date, 0, 7),
                SqlFunc.Substring(DateTime.Now, 0, 7), SqlFunc.Substring(DateTime.Now, 0, 7))).Select(u => u.Pid).ToList();
            return Db.Queryable<TRoom>().Where((r) => r.Number > 0 && r.Pid == id && r.Is_active == true && !rooms.Contains(r.Id)).OrderBy(r => r.Vid).
                ToJson();
        }

        /// <summary>
        /// 返回本月未登记的宿舍数量
        /// </summary>
        /// <param name="dorm_id">园区ID：默认全部</param>
        /// <returns></returns>
        public short CountNoRecord(int dorm_id = 0) {
            var rooms = Db.Queryable<TUsed>().
                Where(u => SqlFunc.Between(SqlFunc.Substring(u.Post_date, 0, 7),
                SqlFunc.Substring(DateTime.Now, 0, 7), SqlFunc.Substring(DateTime.Now, 0, 7))).Select(u => u.Pid).ToList();
            if(dorm_id==0)
                return (short) Db.Queryable<TRoom>().Where((r) => r.Number > 0 && r.Is_active == true && !rooms.Contains(r.Id))
                .OrderBy(r => r.Vid).Count();
            return (short)Db.Queryable<TRoom>().Where((r) => r.Number > 0 && r.Dorm_id == dorm_id && r.Is_active == true && !rooms.Contains(r.Id))
                .OrderBy(r => r.Vid).Count();
        }

        /// <summary>
        /// 查找没有读表信息的宿舍
        /// </summary>
        /// <returns></returns>
        public List<VRoom> GetVRoomWithoutUsedInfo()
        {

            /// 先获取已存在于宿舍读表数值的宿舍ID
            var rooms = Db.Queryable<TUsed_total>().Select(u => u.Pid).ToList();

            return Db.Queryable<TRoom, TBuilding, TDorm>((r, b, d) => new object[] {
                JoinType.Left, r.Pid == b.Id,
                JoinType.Left, b.Pid == d.Id }).Where((r, b, d) => r.Is_active == true && !rooms.Contains(r.Id)).OrderBy(r => r.Vid).
              Select((r, b, d) => new VRoom
              {
                  Id = r.Id,
                  Pid = r.Pid,
                  Dorm_id = r.Dorm_id,
                  Vid = r.Vid,
                  Nickname = r.Nickname,
                  Note = r.Note,
                  PNickname = b.Nickname,
                  Is_active = r.Is_active,
                  Dorm_Nickname = d.Nickname
              }).ToList();
        }

        /// <summary>
        /// 查找没有读表信息的宿舍
        /// </summary>
        /// <param name="id">园区ID</param>
        /// <returns></returns>
        public List<VRoom> GetVRoomWithoutUsedInfo(int id)
        {
            /// 先获取已存在于宿舍读表数值的宿舍ID
            var rooms = Db.Queryable<TUsed_total>().Select(u => u.Dorm_id).ToList();

            return Db.Queryable<TRoom, TBuilding, TDorm>((r, b, d) => new object[] {
                JoinType.Left, r.Pid == b.Id,
                JoinType.Left, b.Pid == d.Id }).Where((r, b, d) => r.Is_active == true && !rooms.Contains(r.Id)).OrderBy(r => r.Vid).
              Select((r, b, d) => new VRoom
              {
                  Id = r.Id,
                  Pid = r.Pid,
                  Dorm_id = r.Dorm_id,
                  Vid = r.Vid,
                  Nickname = r.Nickname,
                  Note = r.Note,
                  PNickname = b.Nickname,
                  Is_active = r.Is_active,
                  Dorm_Nickname = d.Nickname
              }).ToList();
        }

        /// <summary>
        /// 返回宿舍数量
        /// </summary>
        /// <param name="dorm_id">园区ID：默认全部</param>
        /// <returns></returns>
        public short Count(int dorm_id = 0)
        {
            if (dorm_id == 0)
                return (short) RoomDb.Count(r => r.Is_active == true);
            return (short) RoomDb.Count(r => r.Is_active == true && r.Dorm_id == dorm_id);
        }

        /// <summary>
        /// 返回人数大于1人的宿舍数量
        /// </summary>
        /// <param name="dorm_id">园区ID：默认全部</param>
        /// <returns></returns>
        public short CountWithPeople(int dorm_id = 0)
        {
            if (dorm_id == 0)
                return (short) RoomDb.Count(r => r.Is_active == true && r.Number > 0);
            return (short) RoomDb.Count(r => r.Is_active == true && r.Dorm_id == dorm_id && r.Number > 0);
        }
    }
}
