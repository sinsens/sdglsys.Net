﻿using sdglsys.DbHelper;
using sdglsys.Entity;
using SqlSugar;
using System;
using System.Web.Mvc;

namespace sdglsys.Web.Controllers
{
    [AutoLogin]
    public class BuildingController : Controller
    {
        // GET: Dorm
        [NotLowUser]
        [HttpGet]
        public ActionResult Index()
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

                var vbuildings = new Buildings().GetByPages(page, limit, ref count, keyword, (int)Session["pid"]); // 获取列表
                ViewBag.keyword = keyword;
                ViewBag.count = count;  // 获取当前页数量
                ViewBag.page = page;  // 获取当前页

                return View(vbuildings);
            }
            catch (Exception)
            {
                throw;
            }
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
            return View(Dorm.GetAllActive());
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
                Entity.T_Building building = new Entity.T_Building()
                {
                    Building_nickname = collection["name"],
                    Building_note = collection["note"],
                    Building_vid = collection["vid"],
                    Building_dorm_id = Convert.ToInt32(collection["pid"]),
                };
                var buildings = new Buildings();
                if (buildings.Add(building))
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
        [NotLowUser]
        public ActionResult Edit(int id)
        {
            var Dorm = new Dorms();
            ViewBag.dorms = Dorm.GetAll();
            var Buidling = new Buildings();
            return View(Buidling.FindById(id));
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
                var b = buildings.FindById(id);
                if (b == null)
                {
                    throw new Exception("该宿舍楼不存在！");
                }
                else
                {
                    b.Building_nickname = collection["name"];
                    b.Building_note = collection["note"];
                    b.Building_is_active = Convert.ToBoolean(collection["is_active"]);
                    b.Building_vid = collection["vid"];
                    b.Building_dorm_id = Convert.ToInt32(collection["pid"]);

                    if (buildings.Update(b))
                    {
                        msg.Message = "保存成功";
                    }
                    else
                    {
                        throw new Exception("发生未知错误，保存失败");
                    }
                }
            }
            catch (Exception ex)
            {
                msg.Code = -1;
                msg.Message = ex.Message;
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
            try
            {
                var Buildings = new Buildings();
                var user = Buildings.FindById(id);
                if (user == null)
                {
                    throw new Exception("该宿舍楼不存在！");
                }
                else
                {
                    if (Buildings.Delete(id))
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
        [NotLowUser]
        public void Delete(int id, FormCollection collection)
        {
            Delete(id);
        }

        [NeedLogin]
        public void List()
        {
            var db = new Buildings().Db;
            Response.Write(db.Queryable<T_Building>().Where(d => d.Building_model_state && d.Building_is_active == true).ToJson());
        }

        [NeedLogin]
        public void List2()
        {
            /// 返回的数据格式
            /// {
            ///     "Building_id", int,
            ///     "Nickname": string
            /// }
            var db = new DbHelper.Buildings().Db;
            var res = db.Queryable<sdglsys.Entity.T_Building, Entity.T_Dorm>((b, d) => new object[] { JoinType.Left, b.Building_dorm_id == d.Dorm_id }).Where(b => b.Building_model_state && b.Building_is_active && b.Building_is_active && b.Building_model_state).Select((b, d) => new
            {
                b.Building_id,
                d.Dorm_nickname,
                b.Building_nickname
            }).ToJson();
            Response.Write(res);
        }
    }
}