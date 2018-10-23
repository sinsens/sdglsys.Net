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
                var dorms = new Dorms().GetByPages(page, limit, ref count, keyword); // 获取列表

                ViewBag.keyword = keyword;
                ViewBag.count = count;  // 获取当前页数量
                ViewBag.page = page;  // 获取当前页
                return View(dorms);
            }
            catch(Exception)
            {
                throw;
            }

            
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
                Entity.T_Dorm dorm = new Entity.T_Dorm()
                {
                    Dorm_nickname = collection["name"],
                    Dorm_note = collection["note"],
                    Dorm_type = Convert.ToBoolean(Convert.ToInt32(collection["type"])),
                };
                var Dorm = new Dorms();
                if (Dorm.Add(dorm))
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
                    throw new Exception("该园区不存在");
                }
                dorm.Dorm_nickname = collection["name"];
                dorm.Dorm_note = collection["note"];
                dorm.Dorm_is_active = Convert.ToBoolean(collection["is_active"]);
                dorm.Dorm_type = Convert.ToBoolean(Convert.ToInt32(collection["type"]));
                if (Dorm.Update(dorm))
                {
                    msg.Message = "保存成功！";
                }
                else
                {
                    throw new Exception("发生未知错误！");
                }
            }
            catch (Exception ex)
            {
                msg.Code = -1;
                msg.Message = "保存失败：" + ex.Message;
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
            try
            {
                if (user == null)
                {
                    throw new Exception("该园区不存在！");
                }
                else
                {
                    if (User.Delete(id))
                    {
                        msg.Message = "删除成功！";
                    }

                    else
                    {
                        throw new Exception("发生未知错误，删除失败！");
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

        // POST: Dorm/Delete/5
        [HttpPost]
        [IsAdmin]
        public void Delete(int id, FormCollection collection)
        {
            Delete(id);
        }

        public void List()
        {
            try
            {

                using (var db = new Dorms().Db)
                {
                    Response.Write(db.Queryable<T_Dorm>().Where(d => d.Dorm_model_state && d.Dorm_is_active).ToJson());
                }
            }
            catch (Exception ex)
            {
                Response.Write(new Msg
                {
                     Message = ex.Message,
                     Code = -1
                }.ToJson());
            }
        }
    }
}
