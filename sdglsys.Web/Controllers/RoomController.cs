using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sdglsys.DbHelper;
using sdglsys.Entity;
using sdglsys.Web;
using SqlSugar;

namespace sdglsys.Web.Controllers
{
    public class RoomController : Controller
    {
        // GET: Dorm
        [NotLowUser]
        public ActionResult Index()
        {
            string keyword = null;
            int page = 1;
            int limit = 10;
            int count = 0;
            try
            {
                if (!string.IsNullOrWhiteSpace(Request["keyword"]))
                {
                    keyword = Request["keyword"]; // 搜索关键词
                }
                // 当前页码
                if (!string.IsNullOrWhiteSpace(Request["page"]))
                {
                    int.TryParse(Request["page"], out page);
                    page = page > 0 ? page : 1;
                }
                // 每页数量
                if (!string.IsNullOrWhiteSpace(Request["limit"]))
                {
                    int.TryParse(Request["limit"], out limit);
                    limit = limit > 0 ? limit : 10;
                }

                var vrooms = new Rooms().GetVRoomByPages(page, limit, ref count, keyword, (int)Session["pid"]);
                ViewBag.count = count;  // 获取当前页数量
                ViewBag.page = page;  // 获取当前页
                ViewBag.keyword = keyword;
                return View(vrooms);
            }
            catch(Exception)
            {
                throw;
            }
        }

        // GET: Room/Details/5
        [NotLowUser]
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Room/Create
        [NotLowUser]
        public ActionResult Create()
        {
            var Building = new Buildings();
            return View(Building.GetAllActive());
        }

        // POST: Room/Create
        /// <summary>
        /// 添加园区
        /// </summary>
        /// <param name="collection"></param>
        [HttpPost]
        [NotLowUser]
        public void Create(FormCollection collection)
        {
            var msg = new Msg();
            try
            {
                // 初始化对象
                Entity.T_Room room = new Entity.T_Room()
                {
                    Room_dorm_id = Convert.ToInt32(collection["dorm_id"]),
                    Room_nickname = collection["name"],
                    Room_note = collection["note"],
                    Room_vid = collection["vid"],
                    Number = Convert.ToSByte(collection["number"]),
                    Room_building_id = Convert.ToInt32(collection["pid"]),
                };
                var Room = new Rooms();
                if (Room.Add(room))
                {
                    msg.Message = "添加成功！";
                }
                else
                {
                    throw new Exception("发生未知错误，添加失败！");

                }
            }
            catch (Exception ex)
            {
                msg.Code = -1;
                msg.Message = ex.Message;
            }
            finally
            {
                Response.Write(msg.ToJson());
                Response.End();
            }
        }

        // GET: Room/Edit/5
        [NotLowUser]
        public ActionResult Edit(int id)
        {
            var Buidling = new Buildings();
            var Room = new Rooms();
            ViewBag.buildings = Buidling.GetAll();
            return View(Room.FindById(id));
        }

        // POST: Room/Edit/5
        [HttpPost]
        [NotLowUser]
        public void Edit(int id, FormCollection collection)
        {
            var msg = new Msg();
            try
            {
                var Room = new Rooms();
                var room = Room.FindById(id);
                if (room == null)
                {

                    throw new Exception("该宿舍不存在！");
                }
                else
                {
                    room.Room_nickname = collection["name"];
                    room.Room_note = collection["note"];
                    room.Room_is_active = Convert.ToBoolean(collection["is_active"]);
                    room.Room_dorm_id = Convert.ToInt32(collection["dorm_id"]);
                    room.Room_vid = collection["vid"];
                    room.Number = Convert.ToSByte(collection["number"]);
                    room.Room_building_id = Convert.ToInt32(collection["pid"]);
                    if (Room.Update(room)) { msg.Message = "保存成功！"; }
                    else
                    {
                        throw new Exception("发生未知错误，保存失败！");
                    }
                }
            }
            catch (Exception ex)
            {
                msg.Code = -1;
                msg.Message = ex.Message;
            }
            Response.Write(msg.ToJson());
            Response.End();
        }

        // GET: Room/Delete/5
        [NotLowUser]
        public void Delete(int id)
        {
            var msg = new Msg();
            try
            {
                var Room = new Rooms();
                var user = Room.FindById(id);
                if (user == null)
                {
                    throw new Exception("该园区不存在！");
                }
                else if (Room.Delete(id))
                {
                    msg.Message = "删除成功！";
                }
                else
                {
                    throw new Exception("发生未知错误，删除失败！");
                }
            }
            catch (Exception ex)
            {
                msg.Code = -1;
                msg.Message = ex.Message;
            }

            Response.Write(msg.ToJson());
            Response.End();
        }

        // POST: Room/Delete/5
        [HttpPost]
        [NotLowUser]
        public void Delete(int id, FormCollection collection)
        {
            Delete(id);
        }

        [NeedLogin]
        public void List()
        {
            var db = new Rooms().Db;
            Response.Write(db.Queryable<T_Used>().Where(u => u.Used_model_state && u.Used_is_active).ToJson());
        }

        [HttpPost]
        [NeedLogin]
        public void List(int pid)
        {
            var room = new Rooms();
            Response.Write(room.GetJsonAllNoRecordByBuilding(pid));
        }
    }
}
