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
            string keyword = "";
            var d = new Dorms();
            ViewBag.dorms = d.getAll(); // 获取所有园区
            int page = 1;
            int limit = 10;
            try
            {
                keyword = Request[ "keyword" ]; // 搜索关键词
                page = Convert.ToInt32(Request[ "page" ]); if (page < 1) page = 1;
                limit = Convert.ToInt32(Request[ "limit" ]); if (limit > 99 || limit < 1) limit = 10;
            }
            catch
            {
            }
            var u = new Users();
            int count = 0;
            // 系统管理员
            if ((int) Session[ "role" ] < 3)
            {
                ViewBag.users = u.getByPages(page, limit, ref count, (int) Session[ "pid" ], keyword); // 获取列表
            }
            else
            {
                ViewBag.users = u.getByPages(page, limit, ref count, keyword); // 获取列表
            }
            ViewBag.keyword = keyword;
            ViewBag.count = count;  // 获取当前页数量
            ViewBag.page = page;  // 获取当前页
            return View();
        }

        // GET: Users/Create
        [NeedLogin]
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
        [NeedLogin]
        public void Create(FormCollection collection)
        {
            var msg = new Msg();
            try
            {
                // 初始化对象
                Entity.TUser user = new Entity.TUser()
                {
                    Nickname = collection[ "nickname" ],
                    Note = collection[ "note" ],
                    Phone = collection[ "phone" ],
                    Role = Convert.ToInt32(collection[ "role" ]),
                    Pid = Convert.ToInt32(collection[ "pid" ]),
                    Login_name = collection[ "login_name" ],
                    Pwd = Utils.hashpwd(Utils.GetMD5((string) Utils.GetAppSetting("DefaultPassword", typeof(string)))), // 设置默认密码
                };

                var User = new Users();

                if (user.Pid == 0 && user.Role < 3)
                {
                    msg.msg = "非系统管理员请选择所属园区";
                    msg.code = 400;
                }
                else if ((int) Session[ "role" ] < 3 && (int) Session[ "role" ] < user.Role + 1)
                {
                    // 判断权限
                    msg.code = 403;
                    msg.msg = "权限不足";
                }
                else if (User.findByLoginName(user.Login_name) != null)
                {
                    // 用户名已存在
                    msg.msg = "用户名已存在！";
                    msg.code = 400;
                }
                else
                {
                    msg.msg = (User.Add(user)) ? "添加成功！" : "发生未知错误，添加失败！";
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
            ViewBag.dorms = Dorm.getAll();
            var User = new Users();
            return View(User.findById(id));
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
                var user = User.findById(id);
                if (User.findById(user.Id) == null)
                {
                    msg.msg = "该用户不存在！";
                    msg.code = 404;
                }
                else
                {
                    user.Nickname = collection[ "nickname" ];
                    user.Note = collection[ "note" ];
                    user.Role = Convert.ToInt32(collection[ "role" ]);
                    user.Pid = Convert.ToInt32(collection[ "pid" ]);
                    user.Login_name = collection[ "login_name" ];
                    user.Phone = collection[ "phone" ];
                    user.Is_active = Convert.ToBoolean(collection[ "is_active" ]);

                    if (user.Pid == 0 && user.Role < 3)
                    {
                        msg.msg = "非系统管理员请选择所属园区";
                        msg.code = 400;
                    }
                    else if ((int) Session[ "role" ] < 3 && (int) Session[ "role" ] < user.Role + 1)
                    {
                        // 判断权限
                        msg.code = 403;
                        msg.msg = "权限不足";
                    }
                    else
                    {
                        msg.msg = (User.Update(user)) ? "保存成功！" : "发生未知错误！";
                    }
                }
            }
            catch (Exception ex)
            {
                msg.code = 500;
                msg.msg = "保存用户信息时发生错误：" + ex.Message;
            }
            Response.Write(msg.ToJson());
            Response.End();
        }

        // GET: Users/Delete/5
        [NotLowUser]
        public void Delete(int id)
        {
            var msg = new Msg();
            var User = new Users();
            var user = User.findById(id);
            if (user == null)
            {
                msg.msg = "该用户不存在！";
                msg.code = 404;
            }
            else
            {
                int myrole = (int) Session[ "role" ];

                if (myrole < 3 && (myrole <= user.Role || (int) Session[ "pid" ] != user.Pid))
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
            ViewBag.dorms = Dorm.getAll();
            var User = new Users();
            return View(User.findById(id));
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
                var user = User.findById(id);

                if ((int) Session[ "role" ] < 3 && (int) Session[ "role" ] < user.Role + 1)
                {
                    // 判断权限
                    msg.code = 403;
                    msg.msg = "权限不足";
                }
                else if (User.findById(user.Id) == null)
                {
                    msg.msg = "该用户不存在！";
                    msg.code = 404;
                }
                else
                {
                    var pwd = (string) Utils.GetAppSetting("DefaultPassword", typeof(string));
                    user.Pwd = Utils.hashpwd(Utils.GetMD5(pwd)); // 设置默认密码
                    msg.msg = (User.Update(user)) ? "重置默认密码成功，该角色的密码已设置为'" + pwd + "'" : "发生未知错误！";
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
    }
}
