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
    public class DormController : Controller
    {
        // GET: Dorm
        [IsAdmin]
        public ActionResult Index()
        {
            string keyword = "";
            int page = 1;
            int limit = 10;
            int count = 0;
            try
            {
                keyword = Request["keyword"]; // 搜索关键词
                page = Convert.ToInt32(Request["page"]); if (page < 1) page = 1;
                limit = Convert.ToInt32(Request["limit"]); if (limit > 99 || limit < 1) limit = 10;
            }
            catch
            {
            }

            ViewBag.keyword = keyword;
            ViewBag.dorms = new Dorms().getByPages(page, limit, ref count, keyword); // 获取列表
            ViewBag.count = count;  // 获取当前页数量
            ViewBag.page = page;  // 获取当前页
            return View();
        }

        // GET: Dorm/Details/5
        [IsAdmin]
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Dorm/Create
        [IsAdmin]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Dorm/Create
        /// <summary>
        /// 添加园区
        /// </summary>
        /// <param name="collection"></param>
        [HttpPost]
        [IsAdmin]
        public void Create(FormCollection collection)
        {
            var msg = new Msg();
            try
            {
                // 初始化对象
                Entity.TDorm dorm = new Entity.TDorm()
                {
                    Nickname = collection["name"],
                    Note = collection["note"],
                    Type = Convert.ToBoolean(Convert.ToInt32(collection["type"])),
                };
                var Dorm = new Dorms();
                if (Dorm.Add(dorm))
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
            Response.Write(msg.ToJson());
            Response.End();
        }

        // GET: Dorm/Edit/5
        [IsAdmin]
        public ActionResult Edit(int id)
        {
            var Dorm = new Dorms();
            return View(Dorm.FindById(id));
        }

        // POST: Dorm/Edit/5
        [HttpPost]
        [IsAdmin]
        public void Edit(int id, FormCollection collection)
        {
            var msg = new Msg();
            var Dorm = new Dorms();
            var dorm = Dorm.FindById(id);

            try
            {
                if (dorm == null)
                {
                    msg.code = 404;
                    throw new Exception("该园区不存在");
                }
                dorm.Nickname = collection["name"];
                dorm.Note = collection["note"];
                dorm.Is_active = Convert.ToBoolean(collection["is_active"]);
                dorm.Type = Convert.ToBoolean(Convert.ToInt32(collection["type"]));
                msg.msg = (Dorm.Update(dorm)) ? "保存成功！" : "发生未知错误，保存失败！";
            }
            catch (Exception ex)
            {
                msg.code = 500;
                msg.msg = "发生错误：" + ex.Message;
            }
            Response.Write(msg.ToJson());
            Response.End();
        }

        // GET: Dorm/Delete/5
        [IsAdmin]
        public void Delete(int id)
        {
            var msg = new Msg();
            var User = new Dorms();
            var user = User.FindById(id);
            if (user == null)
            {
                msg.msg = "该园区不存在！";
                msg.code = 404;
            }
            else
            {
                msg.msg = (User.Delete(id)) ? "删除成功！" : "发生未知错误，删除失败！";
            }
            Response.Write(msg.ToJson());
            Response.End();
        }

        // POST: Dorm/Delete/5
        [HttpPost]
        [IsAdmin]
        public void Delete(int id, FormCollection collection)
        {
            Delete(id);
        }

        public void List()
        {
            using (var db = new Dorms().Db)
            {
                Response.Write(db.Queryable<TDorm>().Where(d => d.Is_active == true).ToJson());
            }
        }
    }
}
