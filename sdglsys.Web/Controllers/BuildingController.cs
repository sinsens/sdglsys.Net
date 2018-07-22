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
    public class BuildingController : Controller
    {
        // GET: Dorm
        [NotLowUser]
        public ActionResult Index()
        {
            string keyword="";
            var b = new Buildings();
            int pageIndex = 1;
            int pageSize = 10;
            try
            {
                keyword = Request["keyword"]; // 搜索关键词
                pageIndex = Convert.ToInt32(Request["pageIndex"]); if (pageIndex < 1) pageIndex = 1;
                pageSize = Convert.ToInt32(Request["pageSize"]); if (pageSize > 99 || pageSize < 1) pageSize = 10;
            }
            catch
            {

            }
            
            int count = 0;
            ViewBag.keyword = keyword;
            ViewBag.buildings = b.getByPages(pageIndex, pageSize, ref count, keyword); // 获取列表
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
            var Dorm = new Dorms();
            ViewBag.dorms = Dorm.getAllActive();
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
                Entity.TBuilding building = new Entity.TBuilding()
                {
                    Nickname = collection["name"],
                    Note = collection["note"],
                    Vid = collection["vid"],
                    Pid = Convert.ToInt32(collection["pid"]),
                };
                var buildings = new Buildings();
                if (buildings.Add(building))
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
            var Dorm = new Dorms();
            ViewBag.dorms = Dorm.getAll();
            var Buidling = new Buildings();
            return View(Buidling.findById(id));
        }

        // POST: Dorm/Edit/5
        [HttpPost]
        [NotLowUser]
        public void Edit(int id, FormCollection collection)
        {
            var msg = new Msg();
            try
            {
                var buildings = new Buildings();
                var b = buildings.findById(id);
                if (b == null)
                {
                    msg.code = 404;
                    msg.msg = "该宿舍楼不存在！";
                }
                else {
                    b.Nickname = collection["name"];
                    b.Note = collection["note"];
                    b.Is_active = Convert.ToBoolean(collection["is_active"]);
                    b.Vid = collection["vid"];
                    b.Pid = Convert.ToInt32(collection["pid"]);

                    if (buildings.Update(b))
                    {
                        msg.msg = "保存成功！";
                    }
                    else
                    {
                        msg.msg = "发生未知错误，保存失败！";
                        msg.code = 500;
                    }
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
            var Buildings = new Buildings();
            var user = Buildings.findById(id);
            if (user == null)
            {
                msg.msg = "该园区不存在！";
                msg.code = 404;
            }
            else
            {
                if (Buildings.Delete(id))
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
            var db = new Buildings().Db;
            Response.Write(db.Queryable<TBuilding>().Where(d => d.Is_active == true).ToJson());
        }
    }
}
