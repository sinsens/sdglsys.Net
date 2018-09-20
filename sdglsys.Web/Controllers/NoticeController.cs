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
    public class NoticeController : Controller
    {
        // GET: Notice
        [NotLowUser]
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
            var notices = new Notices();
            ViewBag.keyword = keyword;
            ViewBag.notices = notices.getByPages(page, limit, ref count, keyword); // 获取列表
            ViewBag.count = count;  // 获取当前页数量
            ViewBag.page = page;  // 获取当前页
            return View();
        }

        // GET: Notice/Details/5
        [NotLowUser]
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Notice/Create
        [NotLowUser]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Notice/Create
        /// <summary>
        /// 添加公告
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
                Entity.TNotice notice = new Entity.TNotice()
                {
                    Title = collection["title"],
                    Content = collection["content"],
                    Login_name = (string) Session["login_name"],
                    Post_date = DateTime.Now,
                };
                var Notice = new Notices();
                msg.msg = (Notice.Add(notice)) ? "发布公告成功！" : "发生未知错误，发布公告失败！";
            }
            catch (Exception ex)
            {
                msg.code = 500;
                msg.msg = ex.Message;
            }
            Response.Write(msg.ToJson());
            Response.End();
        }

        // GET: Notice/Edit/5
        [NotLowUser]
        public ActionResult Edit(int id)
        {
            var Notice = new Notices();
            return View(Notice.FindById(id));
        }

        // POST: Notice/Edit/5
        [HttpPost]
        [NotLowUser]
        public void Edit(int id, FormCollection collection)
        {
            var msg = new Msg();
            var Notice = new Notices();
            var notice = Notice.FindById(id);
            if (notice == null)
            {
                msg.code = 404;
                msg.msg = "该公告不存在！";
                Response.Write(msg.ToJson());
                Response.End();
            }
            try
            {
                notice.Content = collection["content"];
                notice.Title = collection["title"];
                notice.Mod_date = DateTime.Now;
                notice.Is_active = Convert.ToBoolean(collection["is_active"]);

                if (Notice.Update(notice))
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
            Response.Write(msg.ToJson());
            Response.End();

        }

        // GET: Notice/Delete/5
        [NotLowUser]
        public void Delete(int id)
        {
            var msg = new Msg();
            var User = new Notices();
            var user = User.FindById(id);
            if (user == null)
            {
                msg.msg = "该公告不存在！";
                msg.code = 404;
            }
            else
            {
                if (User.Delete(id))
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

        // POST: Notice/Delete/5
        [HttpPost]
        [NotLowUser]
        public void Delete(int id, FormCollection collection)
        {
            Delete(id);
        }


        /// <summary>
        /// 查看公告
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OutputCache(Duration = 60)]
        public ActionResult View(int id)
        {
            var db = new Notices().NoticeDb;
            var notice = db.GetById(id);
            if (notice == null)
                return HttpNotFound();
            return View(notice);
        }

        public ActionResult List()
        {
            var db = new Notices();
            int page = 1;
            int limit = 10;
            int count = 0;
            try
            {
                page = Convert.ToInt32(Request["page"]); if (page < 1) page = 1;
                limit = Convert.ToInt32(Request["limit"]); if (limit > 99 || limit < 1) limit = 10;
            }
            catch
            {
            }
            ViewBag.notices = db.getListByPages(page, limit, ref count);            
            ViewBag.count = count;  // 获取当前页数量
            ViewBag.page = page;  // 获取当前页
            
            return View();
        }

        /// <summary>
        /// 测试Layui Table模板引擎
        /// </summary>
        public ViewResult List2()
        {
            return View();
        }

        /// <summary>
        /// 获取公告列表
        /// 测试Layui Table模板引擎
        /// </summary>
        [OutputCache(Duration = 60)]
        [HttpPost]
        public void GetList()
        {
            //var data = new DbHelper.
            var msg = new ResponseData();
            var db = new Notices();
            int page = 1;
            int limit = 10;
            int count = 0;
            try
            {
                page = Convert.ToInt32(Request["page"]); if (page < 1) page = 1;
                limit = Convert.ToInt32(Request["limit"]); if (limit > 99 || limit < 1) limit = 10;
            }
            catch
            {}
            msg.data = db.getListByPages(page, limit, ref count);
            Response.Write(msg.ToJson());
            Response.End();
        }
    }
}
