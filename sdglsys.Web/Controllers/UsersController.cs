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
    public class UsersController : Controller
    {
        // GET: Users
        [NeedLogin]
        public ActionResult Index(FormCollection collection)
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

                var users = new Users().GetByPages(page, limit, ref count, keyword, (int)Session["pid"]); // 获取列表
                ViewBag.keyword = keyword;
                ViewBag.count = count;  // 获取当前页数量
                ViewBag.page = page;  // 获取当前页
                return View(users);
            }
            catch (Exception)
            {
                throw;
            }

        }

        // GET: Users/Create
        [NeedLogin]
        public ActionResult Create()
        {
            var Dorm = new Dorms();
            ViewBag.dorms = Dorm.GetAllActive();
            return View();
        }

        // POST: Users/Create
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="collection"></param>
        [HttpPost]
        [NeedLogin]
        public void Create(FormCollection collection)
        {
            var msg = new Msg();

            try
            {
                var Db = new Users().Db;
                var Utils = new Utils.Utils();
                // 初始化对象
                Entity.T_User user = new Entity.T_User()
                {
                    User_nickname = collection["nickname"],
                    User_note = collection["note"],
                    User_phone = collection["phone"],
                    User_role = Convert.ToInt32(collection["role"]),
                    User_dorm_id = Convert.ToInt32(collection["pid"]),
                    User_login_name = collection["login_name"],
                    User_pwd = Utils.HashPassword(((string)Utils.GetAppSetting("DefaultPassword", typeof(string)))), // 设置默认密码
                };
                if (user.User_login_name.Trim().Length < 3) {
                    throw new Exception("用户名不能少于3个字符长度");
                }
                

                if (user.User_dorm_id == 0 && user.User_role < 3)
                {
                    throw new Exception("非系统管理员请选择所属园区");
                }
                if ((int)Session["role"] < 3 && (int)Session["role"] < user.User_role + 1)
                {
                    // 判断权限
                    throw new Exception("权限不足");
                }
                /// 检查用户名是否已存在
                
                if (Db.Queryable<Entity.T_User>().Count(x => x.User_login_name == user.User_login_name)>0)
                {
                    // 用户名已存在
                    throw new Exception("用户名已存在！如果列表不显示可能是未实际从数据库中删除。");
                }
                if (Db.Insertable(user).ExecuteCommand()>0)
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
                msg.Message = ex.Message;
                msg.Code = -1;
            }
            Response.Write(msg.ToJson());
            Response.End();
        }

        /// <summary>
        /// 修改系统用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Users/Edit/5
        [NeedLogin]
        public ActionResult Edit(int id)
        {
            var Dorm = new Dorms();
            ViewBag.dorms = Dorm.GetAll();
            var User = new Users();
            return View(User.FindById(id));
        }

        /// <summary>
        /// 修改系统用户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        // POST: Users/Edit/5；
        [HttpPost]
        [NotLowUser]
        public void Edit(int id, FormCollection collection)
        {
            var msg = new Msg();
            try
            {
                var User = new Users();
                var user = User.FindById(id);
                if (user==null)
                {
                    throw new Exception("该用户不存在！");
                }
                user.User_nickname = collection["nickname"];
                user.User_note = collection["note"];
                user.User_role = Convert.ToInt32(collection["role"]);
                user.User_dorm_id = Convert.ToInt32(collection["pid"]);
                user.User_login_name = collection["login_name"];
                user.User_phone = collection["phone"];
                user.User_is_active = Convert.ToBoolean(collection["is_active"]);

                if (user.User_dorm_id == 0 && user.User_role < 3)
                {
                    throw new Exception("非系统管理员请选择所属园区");
                }
                if ((int)Session["role"] < 3 && (int)Session["role"] < user.User_role + 1)
                {
                    // 判断权限
                    throw new Exception("权限不足");
                }
                if (User.Update(user))
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
                msg.Message = ex.Message;
            }
            Response.Write(msg.ToJson());
            Response.End();
        }

        // GET: Users/Delete/5
        [NotLowUser]
        public void Delete(int id)
        {
            var msg = new Msg();
            try
            {
                var User = new Users();
                var user = User.FindById(id);
                if (user == null)
                {
                    throw new Exception("该用户不存在！");
                }
                int myrole = (int)Session["role"];

                if (myrole < 3 && (myrole <= user.User_role || (int)Session["pid"] != user.User_role))
                {
                    // 如果不是系统管理员并且（为同级别角色或园区不同）
                    throw new Exception("权限不足！");
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

        // POST: Users/Delete/5
        [HttpPost]
        [NotLowUser]
        public void Delete(int id, FormCollection collection)
        {
            Delete(id);
        }

        // GET: Users/Reset/5
        [NotLowUser]
        public ActionResult Reset(int id)
        {
            var Dorm = new Dorms();
            ViewBag.dorms = Dorm.GetAll();
            var User = new Users();
            return View(User.FindById(id));
        }

        // POST: Users/Reset/5；重置默认密码
        [HttpPost]
        [NotLowUser]
        public void Reset(int id, FormCollection collection)
        {
            var msg = new Msg();
            try
            {
                var User = new Users();
                // 初始化对象

                var user = User.FindById(id);
                if (user == null)
                {
                    throw new Exception("该用户不存在！");
                }
                if ((int)Session["role"] < 3 && (int)Session["role"] < user.User_role + 1)
                {
                    // 判断权限
                    throw new Exception("权限不足");
                }
                else
                {
                    var Util = new Utils.Utils();
                    var pwd = (string)Util.GetAppSetting("DefaultPassword", typeof(string));
                    user.User_pwd = Util.HashPassword(pwd); // 设置默认密码
                    if (User.Update(user))
                    {
                        msg.Message = "重置默认密码成功，该角色的密码已设置为'" + pwd + "'";
                    }
                    else
                    {
                        throw new Exception("发生未知错误！");
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
    }
}
