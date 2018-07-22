using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace sdglsys.DesktopUtils
{
    /// <summary>
    /// 基础数据生成工具
    /// </summary>
    public static class BaseInfoGenerator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dormName">园区名称</param>
        /// <param name="buildingNum">宿舍楼数量</param>
        /// <param name="buildingFloorNum">宿舍楼楼层数</param>
        /// <param name="roomNumOneFloor">每层房间数</param>
        /// <param name="afterBuilding">宿舍楼前缀</param>
        /// <param name="afterRoom">宿舍前缀</param>
        /// <param name="buildingVidExpr">宿舍楼编号表达式</param>
        /// <param name="roomVidExpr">宿舍编号表达式</param>
        public static void SetParams(string dormName, int buildingNum, int buildingFloorNum, int roomNumOneFloor,
            string buildingNameExpr, string roomNameExpr, string buildingVidExpr = "D{0:d2}", string roomVidExpr = "D{0:d2}F{1:d2}R{1:d2}{2:d3}")
        {
            BuildingNicknameExpr = buildingNameExpr;
            RoomNicknameExpr = roomNameExpr;
            DormName = dormName;
            BuildingNum = buildingNum;
            BuildingFloorNum = buildingFloorNum;
            RoomNumOneFloor = roomNumOneFloor;
            BuildingVidExpr = buildingVidExpr;
            RoomVidExpr = roomVidExpr;
        }

        /// <summary>
        /// 园区类别：false女,true男
        /// </summary>
        public static bool DormType { get; set; }
        public static int BuildingNum { get; set; }
        public static int BuildingFloorNum { get; set; }
        public static int RoomNumOneFloor { get; set; }
        public static string DormName { get; set; }
        public static string BuildingNicknameExpr { get; set; }
        public static string RoomNicknameExpr { get; set; }
        public static string BuildingVidExpr { get; set; }
        public static string RoomVidExpr { get; set; }

        /// <summary>
        /// 宿舍楼列表
        /// </summary>
        public static DataTable b;
        /// <summary>
        /// 宿舍列表
        /// </summary>
        public static DataTable r;

        /// <summary>
        /// 生成数据
        /// </summary>
        public static string Gen()
        {
            var t = DateTime.Now.Ticks;
            b = new DataTable();
            r = new DataTable();
            b.Columns.Add("Nickname");
            b.Columns.Add("Note");
            b.Columns.Add("Vid");
            r.Columns.Add("Nickname");
            r.Columns.Add("Note");
            r.Columns.Add("Vid");
            try
            {
                for (var i = 1; i < BuildingNum + 1; i++)
                {
                    var nick = string.Format(BuildingNicknameExpr, i);
                    //b.Rows.Add(nick, "使用桌面工具生成的宿舍楼信息", String.Format("D{0:d2}", i));
                    b.Rows.Add(nick, "使用桌面工具生成的宿舍楼信息", String.Format(BuildingVidExpr, i));

                    for (int j = 1; j < BuildingFloorNum + 1; j++)
                    {
                        for (var k = 1; k < RoomNumOneFloor + 1; k++)
                        {
                            var nickname = RoomNicknameExpr.Split('{').Length < 3 ? String.Format(RoomNicknameExpr, i, k) :
                                String.Format(RoomNicknameExpr, i, j, k);
                            //var vid = String.Format("D{0:d2}F{1:d2}R{1:d2}{2:d3}", i, j, k);
                            var vid = RoomVidExpr.Split('{').Length < 3 ? String.Format(RoomVidExpr, i, k) :
                                String.Format(RoomVidExpr, i, j, k);
                            r.Rows.Add(nickname, "使用桌面工具生成的宿舍楼信息", vid);
                        }
                    }
                }
                return "共生成宿舍楼 " + BaseInfoGenerator.b.Rows.Count + " 个，生成宿舍 " + BaseInfoGenerator.r.Rows.Count + " 个,生成耗时 " + (Math.Round((double)(DateTime.Now.Ticks - t) / 1000 / 10000, 5)) + " 秒";
            }
            catch (Exception)
            {
                throw new Exception();
            }
            
        }

        /// <summary>
        /// 保存数据到数据库
        /// </summary>
        /// <returns>提示信息</returns>
        public static string SaveDataToDatabase()
        {
            if (DBInfo.DB == null)
                return "请先连接数据库";
            if (DormName == null || DormName.Length < 1)
                return "请设置园区名称";
            if (b == null || r.Rows.Count < 1||r == null)
                return "请先生成数据";
            var t = DateTime.Now.Ticks;
            var db = DBInfo.DB.Db;
            var i = 0; // 宿舍楼数据指针
            var j = 0; // 宿舍数据指针
            try
            {
                db.Open();
                /// 开始事务
                db.Ado.BeginTran();
                /// 插入园区记录
                var dormId = db.Insertable(new Entity.TDorm
                {
                    Nickname = DormName,
                    Note = "使用桌面工具生成的园区信息",
                    Type = DormType
                }).ExecuteReturnIdentity();
                if (dormId < 1)
                {
                    db.Ado.RollbackTran();
                    return "插入园区信息时发生错误";
                }
                /// 循环插入宿舍楼数据和宿舍数据
                for (; i < b.Rows.Count; i++)
                {
                    var buildingId = db.Insertable(new Entity.TBuilding
                    {
                        Nickname = b.Rows[i]["Nickname"].ToString(),
                        Note = b.Rows[i]["Note"].ToString(),
                        Pid = dormId,
                        Vid = b.Rows[i]["Vid"].ToString()
                    }).ExecuteReturnIdentity();
                    if (buildingId < 1)
                    {
                        db.Ado.RollbackTran(); // 回滚事务
                        return "插入第 " + i + " 行宿舍楼数据时发生错误，已撤销更改";
                    }
                    while (j < r.Rows.Count)
                    {
                        var roomId = db.Insertable(new Entity.TRoom
                        {
                            Dorm_id = dormId,
                            Pid = buildingId,
                            Nickname = r.Rows[j]["Nickname"].ToString(),
                            Note = r.Rows[j]["Note"].ToString(),
                            Vid = r.Rows[j]["Vid"].ToString()
                        }).ExecuteReturnIdentity();
                        if (roomId < 1)
                        {
                            db.Ado.RollbackTran(); // 回滚事务
                            return "插入第 " + (j++) + " 行宿舍数据时发生错误，已撤销更改";
                        }
                        j++;
                        if (j > 0 && (j % (BuildingFloorNum * RoomNumOneFloor) == 0)) //宿舍楼楼层数*每层房间数/当前插入数=0(没有余数)，跳出本楼循环
                            break;
                    }
                }
                db.Ado.CommitTran(); // 提交事务
                return "操作完成，共插入 " + i + " 条宿舍楼数据， " + j + " 条宿舍数据，耗时 " + (Math.Round((double)(DateTime.Now.Ticks - t) / 1000 / 10000, 5)) + " 秒";
            }
            catch (Exception ex)
            {
                db.Ado.RollbackTran(); // 回滚事务
                //throw;
                return "插入第 " + (i++) + " 行宿舍数据时发生错误，已撤销更改，错误提示：" + ex.Message;
            }
        }
    }
}
