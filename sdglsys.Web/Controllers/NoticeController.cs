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

                var notices = new Notices().GetByPages(page, limit, ref count, keyword); // 获取列表
                ViewBag.keyword = keyword;
                ViewBag.count = count;  // 获取当前页数量
                ViewBag.page = page;  // 获取当前页
                return View(notices);
            }
            catch(Exception)
            {
                throw;
            }
            
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
                Entity.T_Notice notice = new Entity.T_Notice()
                {
                    Notice_title = collection["title"],
                    Notice_content = collection["content"],
                    Notice_login_name = (string)Session["login_name"],
                    //Notice_post_date = DateTime.Now,
                };
                var Notice = new Notices();
                if (Notice.Add(notice)) {
                    msg.Message = "发布公告成功！";
                }
                else {
                    throw new Exception("发生未知错误，发布公告失败！");
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
            try
            {
                var Notice = new Notices();
                var notice = Notice.FindById(id);
                if (notice == null)
                {
                    throw new Exception("该公告不存在！");
                }
                notice.Notice_content = collection["content"];
                notice.Notice_title = collection["title"];
                notice.Notice_mod_date = DateTime.Now;
                notice.Notice_is_active = Convert.ToBoolean(collection["is_active"]);

                if (Notice.Update(notice))
                {
                    msg.Message = "保存成功！";
                }
                else
                {
                    throw new Exception("发生未知错误，保存失败！");
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

        // GET: Notice/Delete/5
        [NotLowUser]
        public void Delete(int id)
        {
            var msg = new Msg();
            try
            {
                var User = new Notices();
                var user = User.FindById(id);
                if (user == null)
                {
                    throw new Exception("该公告不存在！");
                }
                else if (User.Delete(id))
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
            
            int page = 1;
            int limit = 10;
            int count = 0;
            try
            {
                var Notice = new Notices();
                page = Convert.ToInt32(Request["page"]); if (page < 1) page = 1;
                limit = Convert.ToInt32(Request["limit"]); if (limit > 99 || limit < 1) limit = 10;
                ViewBag.notices = Notice.GetListByPages(page, limit, ref count);
            }
            catch
            {
            }
            
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
            int page = 1;
            int limit = 10;
            int count = 0;
            try
            {
                var Notice = new Notices();
                page = Convert.ToInt32(Request["page"]); if (page < 1) page = 1;
                limit = Convert.ToInt32(Request["limit"]); if (limit > 99 || limit < 1) limit = 10;
                msg.data = Notice.GetListByPages(page, limit, ref count);
            }
            catch
            { }
            
            Response.Write(msg.ToJson());
            Response.End();
        }
    }
}
