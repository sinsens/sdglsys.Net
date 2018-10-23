using sdglsys.Entity;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace sdglsys.DbHelper
{
    public class Rooms : DbContext
    {
        /// <summary>
        /// 通过ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entity.T_Room FindById(int id)
        {
            return Db.Queryable<T_Room>().Where(x => x.Room_model_state && x.Room_id == id).Single(); ;
        }

        /// <summary>
        /// 通过ID删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            var room = FindById(id);
            if (room != null)
            {
                room.Room_model_state = false;
                return RoomDb.Update(room);
            }
            return false;
        }

        /// <summary>
        /// 更新宿舍信息
        /// </summary>
        /// <param name="Room"></param>
        /// <returns></returns>
        public bool Update(Entity.T_Room room)
        {
            return RoomDb.Update(room);
        }

        /// <summary>
        /// 添加宿舍信息
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public bool Add(Entity.T_Room room)
        {
            return RoomDb.Insert(room);
        }


        /// <summary>
        /// 按宿舍楼查找当月未登记宿舍并返回Json
        /// </summary>
        /// <param name="id">宿舍楼ID</param>
        /// <returns></returns>
        public string GetJsonAllNoRecordByBuilding(int id)
        {
            /// 先获取本月已登记的宿舍ID
            var rooms = Db.Queryable<T_Used>().
                Where(u => u.Used_model_state && SqlFunc.Between(SqlFunc.Substring(u.Used_post_date, 0, 7),
                SqlFunc.Substring(DateTime.Now, 0, 7), SqlFunc.Substring(DateTime.Now, 0, 7))).Select(u => u.Used_room_id).ToList();
            return Db.Queryable<T_Room>().Where((r) => r.Room_model_state && r.Number > 0 && r.Room_building_id == id && r.Room_is_active == true && !rooms.Contains(r.Room_id)).OrderBy(r => r.Room_vid).
                ToJson();
        }

        /// <summary>
        /// 返回本月未登记的宿舍数量
        /// </summary>
        /// <param name="dorm_id">园区ID：默认全部</param>
        /// <returns></returns>
        public short CountNoRecord(int dorm_id = 0)
        {
            var rooms = Db.Queryable<T_Used>().
                Where(u => u.Used_model_state && SqlFunc.Between(SqlFunc.Substring(u.Used_post_date, 0, 7),
                SqlFunc.Substring(DateTime.Now, 0, 7), SqlFunc.Substring(DateTime.Now, 0, 7))).Select(u => u.Used_room_id).ToList();
            if (dorm_id == 0)
                return (short)Db.Queryable<T_Room>().Where(r => r.Room_model_state && r.Number > 0 && r.Room_is_active == true && !rooms.Contains(r.Room_id))
                .OrderBy(r => r.Room_vid).Count();
            return (short)Db.Queryable<T_Room>().Where((r) => r.Number > 0 && r.Room_dorm_id == dorm_id && r.Room_is_active == true && !rooms.Contains(r.Room_id))
                .OrderBy(r => r.Room_vid).Count();
        }

        /// <summary>
        /// 查找没有读表信息的宿舍
        /// </summary>
        /// <returns></returns>
        public List<VRoom> GetVRoomWithouT_UsedInfo()
        {

            /// 先获取已存在于宿舍读表数值的宿舍ID
            var rooms = Db.Queryable<T_Used_total>().Where(u => u.Ut_model_state).Select(u => u.Ut_room_id).ToList();

            return Db.Queryable<T_Room, T_Building, T_Dorm>((r, b, d) => new object[] {
                JoinType.Left, r.Room_building_id == b.Building_id,
                JoinType.Left, b.Building_dorm_id == d.Dorm_id }).Where((r, b, d) => r.Room_model_state && r.Room_is_active == true && !rooms.Contains(r.Room_id)).OrderBy(r => r.Room_vid).Select((r, b, d) => new Entity.VRoom
                {
                    Number = r.Number,
                    Room_Building_id = r.Room_building_id,
                    Room_Building_Nickname = b.Building_nickname,
                    Room_Dorm_id = r.Room_dorm_id,
                    Room_Dorm_Nickname = d.Dorm_nickname,
                    Room_Id = r.Room_id,
                    Room_Is_active = r.Room_is_active,
                    Room_Nickname = r.Room_nickname,
                    Room_Note = r.Room_note,
                    Room_Vid = r.Room_vid
                }).ToList();
        }

        /// <summary>
        /// 查找没有读表信息的宿舍
        /// </summary>
        /// <param name="id">园区ID</param>
        /// <returns></returns>
        public List<VRoom> GetVRoomWithouT_UsedInfo(int id)
        {
            /// 先获取已存在于宿舍读表数值的宿舍ID
            var rooms = Db.Queryable<T_Used_total>().Where(u => u.Ut_model_state && u.Ut_dorm_id == id).Select(u => u.Ut_room_id).ToList();

            return Db.Queryable<T_Room, T_Building, T_Dorm>((r, b, d) => new object[] {
                JoinType.Left, r.Room_building_id == b.Building_id,
                JoinType.Left, b.Building_dorm_id == d.Dorm_id }).Where((r, b, d) => r.Room_is_active && r.Room_model_state && !rooms.Contains(r.Room_id)).OrderBy(r => r.Room_vid).Select((r, b, d) => new Entity.VRoom
                {
                    Number = r.Number,
                    Room_Building_id = r.Room_building_id,
                    Room_Building_Nickname = b.Building_nickname,
                    Room_Dorm_id = r.Room_dorm_id,
                    Room_Dorm_Nickname = d.Dorm_nickname,
                    Room_Id = r.Room_id,
                    Room_Is_active = r.Room_is_active,
                    Room_Nickname = r.Room_nickname,
                    Room_Note = r.Room_note,
                    Room_Vid = r.Room_vid
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
                return (short)RoomDb.Count(r => r.Room_is_active && r.Room_model_state);
            return (short)RoomDb.Count(r => r.Room_is_active && r.Room_model_state && r.Room_dorm_id == dorm_id);
        }

        /// <summary>
        /// 返回人数大于1人的宿舍数量
        /// </summary>
        /// <param name="dorm_id">园区ID：默认全部</param>
        /// <returns></returns>
        public short CountWithPeople(int dorm_id = 0)
        {
            if (dorm_id == 0)
                return (short)RoomDb.Count(r => r.Room_is_active && r.Room_model_state);
            return (short)RoomDb.Count(r => r.Room_is_active && r.Room_model_state && r.Room_dorm_id == dorm_id && r.Number > 0);
        }

        /// <summary>
        /// 获取宿舍信息
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="limit">每页数量</param>
        /// <param name="count">ref in: 数量</param>
        /// <param name="where">关键词</param>
        /// <param name="dorm_id">园区id：默认0</param>
        /// <returns></returns>
        public List<Entity.VRoom> GetVRoomByPages(int page, int limit, ref int count, string where = null, int dorm_id = 0) {
            var sql = Db.Queryable<T_Room, T_Building, T_Dorm>((r, b, d) => new object[] { JoinType.Left, r.Room_building_id == b.Building_id, JoinType.Left, b.Building_dorm_id == d.Dorm_id }).Where((r, b, d) => r.Room_model_state && b.Building_model_state && d.Dorm_model_state);

            if (dorm_id != 0)
            {
                sql = sql.Where(r => r.Room_dorm_id == dorm_id);
            }
            if (!string.IsNullOrWhiteSpace(where)) {
                sql = sql.Where(r => r.Room_nickname.Contains(where) || r.Room_vid.Contains(where) || r.Room_note.Contains(where));
            }
            return sql.OrderBy(r => r.Room_id, OrderByType.Desc).Select((r, b, d) => new Entity.VRoom
            {
                Number = r.Number,
                Room_Building_id = r.Room_building_id,
                Room_Building_Nickname = b.Building_nickname,
                Room_Dorm_id = r.Room_dorm_id,
                Room_Dorm_Nickname = d.Dorm_nickname,
                Room_Id = r.Room_id,
                Room_Is_active = r.Room_is_active,
                Room_Nickname = r.Room_nickname,
                Room_Note = r.Room_note,
                Room_Vid = r.Room_vid
            }).ToPageList(page, limit, ref count);
        }
    }
}
