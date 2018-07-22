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
        [IsAdmin]
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
            catch
            {
            }
            var notices = new Notices();
            ViewBag.keyword = keyword;
            ViewBag.notices = notices.getByPages(pageIndex, pageSize, ref count, keyword); // 获取列表
            ViewBag.count = count;  // 获取当前页数量
            ViewBag.pageIndex = pageIndex;  // 获取当前页
            return View();
        }

        // GET: Notice/Details/5
        [IsAdmin]
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
                    Login_name = (string)Session["login_name"],
                    Post_date = DateTime.Now,
                };
                var Notice = new Notices();
                if (Notice.Add(notice))
                {
                    msg.msg = "发布公告成功！";
                }
                else
                {
                    msg.msg = "发生未知错误，发布公告失败！";
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

        // GET: Notice/Edit/5
        [IsAdmin]
        public ActionResult Edit(int id)
        {
            var Notice = new Notices();
            return View(Notice.findById(id));
        }

        // POST: Notice/Edit/5
        [HttpPost]
        [IsAdmin]
        public void Edit(int id, FormCollection collection)
        {
            var msg = new Msg();
            var Notice = new Notices();
            var notice = Notice.findById(id);
            if (notice == null)
            {
                msg.code = 404;
                msg.msg = "该公告不存在！";
            }
            else
            {
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
                finally
                {
                    Response.Write(msg.ToJson());
                    Response.End();
                }
            }
        }

        // GET: Notice/Delete/5
        [IsAdmin]
        public void Delete(int id)
        {
            var msg = new Msg();
            var User = new Notices();
            var user = User.findById(id);
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
        [IsAdmin]
        public void Delete(int id, FormCollection collection)
        {
            Delete(id);
        }


        /// <summary>
        /// 查看公告
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult View(int id) {
            var db = new Notices().NoticeDb;
            var notice = db.GetById(id);
            if (notice == null)
                return HttpNotFound();
            return View(notice);
        }

        public ActionResult List() {
            var db = new Notices();
            int pageIndex = 1;
            int pageSize = 10;
            try
            {
                pageIndex = Convert.ToInt32(Request["pageIndex"]); if (pageIndex < 1) pageIndex = 1;
                pageSize = Convert.ToInt32(Request["pageSize"]); if (pageSize > 99 || pageSize < 1) pageSize = 10;
            }
            finally
            {
            }
            int count = 0;
            ViewBag.count = count;  // 获取当前页数量
            ViewBag.pageIndex = pageIndex;  // 获取当前页
            ViewBag.notices = db.getListByPages(pageIndex, pageSize, ref count);
            return View();
        }
    }
}
