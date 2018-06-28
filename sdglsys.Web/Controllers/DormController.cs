using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sdglsys.Entity;
using sdglsys.BLL;
namespace sdglsys.Web.Controllers
{
    public class DormController : Controller
    {
        // GET: Dorm
        [IsAdmin]
        public ActionResult Index()
        {
            var keyword = Request["keyword"]; // 搜索关键词
            var d = new BLL.Dorms();
            ViewBag.dorms = d.getAll(); // 获取所有园区
            var pageIndex = Request["pageIndex"] == null ? 1 : Convert.ToInt32(Request["pageIndex"]);
            var pageSize = Request["pageSize"] == null ? 10 : Convert.ToInt32(Request["pageSize"]);
            int count = 0;
            ViewBag.keyword = keyword;
            ViewBag.dorms = d.getByPages(pageIndex, pageSize, ref count, keyword); // 获取列表
            ViewBag.count = count;  // 获取当前页数量
            ViewBag.pageIndex = pageIndex;  // 获取当前页
            return View();
        }

        // GET: Dorm/Details/5
        [IsAdmin]
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Dorm/Create
        [AccountAuthorize]
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
            var msg = new BLL.Msg();
            try
            {
                // 初始化对象
                Entity.Dorm dorm = new Entity.Dorm()
                {
                    Nickname = collection["name"],
                    Note = collection["note"],
                    Type = Convert.ToBoolean(Convert.ToInt32(collection["type"])),
                };
                var Dorm = new BLL.Dorms();
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
            finally
            {
                Response.Write(msg.toJson());
                Response.End();
            }
        }

        // GET: Dorm/Edit/5
        [IsAdmin]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Dorm/Edit/5
        [HttpPost]
        [IsAdmin]
        public void Edit(int id, FormCollection collection)
        {
            var msg = new BLL.Msg();
            try
            {
                // 初始化对象
                Entity.Dorm dorm = new Entity.Dorm()
                {
                    Id = id,
                    Nickname = collection["name"],
                    Note = collection["note"],
                    Type = Convert.ToBoolean(Convert.ToInt32(collection["type"])),
                };
                var Dorm = new BLL.Dorms();
                if (Dorm.Update(dorm))
                {
                    msg.msg = "保存成功！";
                }
                else
                {
                    msg.msg = "发生未知错误，保存失败！";
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
                Response.Write(msg.toJson());
                Response.End();
            }
        }

        // GET: Dorm/Delete/5
        [AccountAuthorize]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Dorm/Delete/5
        [HttpPost]
        [AccountAuthorize]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
