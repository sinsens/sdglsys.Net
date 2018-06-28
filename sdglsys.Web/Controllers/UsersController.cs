using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sdglsys.Entity;
using sdglsys.BLL;
namespace sdglsys.Web.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        [AccountAuthorize]
        public ActionResult Index(FormCollection collection)
        {
            var keyword = Request["keyword"]; // 搜索关键词
            var d = new BLL.Dorms();
            ViewBag.dorms = d.getAll(); // 获取所有园区
            var pageIndex = Request["pageIndex"] == null ? 1 : Convert.ToInt32(Request["pageIndex"]);
            var pageSize = Request["pageSize"] == null ? 10 : Convert.ToInt32(Request["pageSize"]);
            var u = new BLL.Users();
            int count = 0;
            // 系统管理员
            if ((int)Session["role"] < 3)
            {
                ViewBag.users = u.getByPages(pageIndex, pageSize, ref count, (int)Session["pid"], keyword); // 获取列表
            }
            else {
                ViewBag.users = u.getByPages(pageIndex, pageSize, ref count, keyword); // 获取列表
            }
            ViewBag.keyword = keyword;
            ViewBag.count = count;  // 获取当前页数量
            ViewBag.pageIndex = pageIndex;  // 获取当前页
            return View();
        }

        // GET: Users/Details/5
        [AccountAuthorize]
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Users/Create
        [AccountAuthorize]
        public ActionResult Create()
        {
            var dorm = new Dorms();
            ViewBag.dorms = dorm.getAllActive();
            return View();
        }

        // POST: Users/Create
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="collection"></param>
        [HttpPost]
        [AccountAuthorize]
        public void Create(FormCollection collection)
        {
            var msg = new BLL.Msg();
            try
            {
                // 初始化对象
                Entity.Users user = new Entity.Users()
                {
                    Nickname = collection["nickname"],
                    Note = collection["note"],
                    Role = Convert.ToInt32(collection["role"]),
                    Pid = Convert.ToInt32(collection["pid"]),
                    Login_name = collection["login_name"],
                    Pwd = Utils.hashpwd((string)Utils.getSetting("default_pwd", typeof(string))), // 设置默认密码
                };
                
                var User = new BLL.Users();
                // 判断权限
                if ((int)Session["role"] < 3 && (int)Session["role"] < user.Role + 1)
                {
                    msg.code = 403;
                    msg.msg = "权限不足";
                }
                else if (User.findByLoginName(user.Login_name) != null) {
                    // 用户名已存在
                    msg.msg = "用户名已存在！";
                    msg.code = 400;
                }
                else
                {

                    if (User.Add(user))
                    {
                        msg.msg = "添加成功！";
                    }
                    else
                    {
                        msg.msg = "发生未知错误，添加失败！";
                        msg.code = 500;
                    }
                }
            }
            catch(Exception ex)
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

        // GET: Users/Edit/5
        [AccountAuthorize]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Users/Edit/5
        [HttpPost]
        [AccountAuthorize]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Users/Delete/5
        [AccountAuthorize]
        public void Delete(int id)
        {
            var msg = new BLL.Msg();
            var User = new BLL.Users();
            var user = User.findById(id);
            if (user == null)
            {
                msg.msg = "该用户不存在！";
                msg.code = 404;
            }
            else {
                int myrole = (int)Session["role"];

                if (myrole < 3 && (myrole <= user.Role || (int)Session["pid"] != user.Pid))
                { // 如果不是系统管理员并且（为同级别角色或园区不同）
                    msg.msg = "权限不足！";
                    msg.code = 403;
                }
                else if (User.Delete(id))
                {
                    msg.msg = "删除成功！";
                }
                else {
                    msg.msg = "发生未知错误，删除失败！";
                }
            }
            Response.Write(msg.toJson());
            Response.End();
        }

        // POST: Users/Delete/5
        [HttpPost]
        [AccountAuthorize]
        public void Delete(int id, FormCollection collection)
        {
            var msg = new BLL.Msg();
            var User = new BLL.Users();
            var user = User.findById(id);
            if (user == null)
            {
                msg.msg = "该用户不存在！";
                msg.code = 404;
            }
            else
            {
                int myrole = (int)Session["role"];

                if (myrole < 3 && (myrole <= user.Role || (int)Session["pid"] != user.Pid))
                { // 如果不是系统管理员并且（为同级别角色或园区不同）
                    msg.msg = "权限不足！";
                    msg.code = 403;
                }
                else if (User.Delete(id))
                {
                    msg.msg = "删除成功！";
                }
                else
                {
                    msg.msg = "发生未知错误，删除失败！";
                }
            }
            Response.Write(msg.toJson());
            Response.End();
        }
    }
}
