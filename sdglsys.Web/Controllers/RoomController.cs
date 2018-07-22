using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sdglsys.DbHelper;
using sdglsys.Entity;
using sdglsys.Web;
namespace sdglsys.Web.Controllers
{
    public class RoomController : Controller
    {
        // GET: Dorm
        [NotLowUser]
        public ActionResult Index()
        {
            string keyword = "";
            int pageIndex = 1;
            int pageSize = 10;
            int count = 0;
            try
            {
                keyword = Request["keyword"]; // 搜索关键词
                pageIndex = Convert.ToInt32(Request["pageIndex"]); if (pageIndex < 1) pageIndex = 1;
                pageSize = Convert.ToInt32(Request["pageSize"]); if (pageSize > 99 || pageSize < 1) pageSize = 10;
            }
            finally
            {
            }
            var rooms = new Rooms();
            ViewBag.keyword = keyword;
            ViewBag.rooms = rooms.getByPages(pageIndex, pageSize, ref count, keyword); // 获取列表
            ViewBag.count = count;  // 获取当前页数量
            ViewBag.pageIndex = pageIndex;  // 获取当前页

            return View();
        }

        // GET: Dorm/Details/5
        [NotLowUser]
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Dorm/Create
        [NotLowUser]
        public ActionResult Create()
        {
            var Building = new Buildings();
            ViewBag.buildings = Building.getAllActive();
            return View();
        }

        // POST: Dorm/Create
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
                Entity.TRoom room = new Entity.TRoom()
                {
                    Dorm_id = Convert.ToInt32(collection["dorm_id"]),
                    Nickname = collection["name"],
                    Note = collection["note"],
                    Vid = collection["vid"],
                    Pid = Convert.ToInt32(collection["pid"]),
                };
                var Room = new Rooms();
                if (Room.Add(room))
                {
                    msg.msg = "添加成功！";
                }
                else
                {
                    msg.msg = "发生未知错误，添加失败！";
                    msg.code = 500;
                }
            }
            catch (Exception ex)
            {
                msg.code = 500;
                msg.msg = ex.Message;
            }
            finally
            {
                Response.Write(msg.ToJson());
                Response.End();
            }
        }

        // GET: Dorm/Edit/5
        [NotLowUser]
        public ActionResult Edit(int id)
        {
            var Buidling = new Buildings();
            var Room = new Rooms();
            ViewBag.buildings = Buidling.getAll();
            return View(Room.findById(id));
        }

        // POST: Dorm/Edit/5
        [HttpPost]
        [NotLowUser]
        public void Edit(int id, FormCollection collection)
        {
            var msg = new Msg();
            try
            {
                var Room = new Rooms();
                var r = Room.findById(id);
                if (r == null)
                {
                    msg.code = 404;
                    msg.msg = "该宿舍楼不存在！";
                }
                else {
                    r.Nickname = collection["name"];
                    r.Note = collection["note"];
                    r.Is_active = Convert.ToBoolean(collection["is_active"]);
                    r.Dorm_id = Convert.ToInt32(collection["dorm_id"]);
                    r.Vid = collection["vid"];
                    r.Pid = Convert.ToInt32(collection["pid"]);
                    msg.msg = (Room.Update(r)) ? "保存成功！" : "发生未知错误，保存失败！";
                }
                
            }
            catch (Exception ex)
            {
                msg.code = 500;
                msg.msg = ex.Message;
            }
            finally
            {
                Response.Write(msg.ToJson());
                Response.End();
            }
        }

        // GET: Dorm/Delete/5
        [NotLowUser]
        public void Delete(int id)
        {
            var msg = new Msg();
            var Room = new Rooms();
            var user = Room.findById(id);
            if (user == null)
            {
                msg.msg = "该园区不存在！";
                msg.code = 404;
            }
            else
            {
                if (Room.Delete(id))
                {
                    msg.msg = "删除成功！";
                }
                else
                {
                    msg.msg = "发生未知错误，删除失败！";
                }
            }
            Response.Write(msg.ToJson());
            Response.End();
        }

        // POST: Dorm/Delete/5
        [HttpPost]
        [NotLowUser]
        public void Delete(int id, FormCollection collection)
        {
            Delete(id);
        }

        public void List()
        {
            var db = new Rooms().Db;
            Response.Write(db.Queryable<TRoom>().Where(d => d.Is_active == true).ToJson());
        }

        [HttpPost]
        [NeedLogin]
        public void List(int pid)
        {
            var db = new Rooms().Db;
            Response.Write(db.Queryable<TRoom>().Where(d => d.Is_active == true&&d.Pid==pid).ToJson());
        }
    }
}
