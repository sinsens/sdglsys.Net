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
    public class UsedController : Controller
    {
        // GET: Used
        [NeedLogin]
        public ActionResult Index()
        {
            string keyword = "";
            var b = new Useds();
            int pageIndex = 1;
            int pageSize = 10;
            int stat = 0;
            int count = 0;
            try
            {
                keyword = Request[ "keyword" ]; // 搜索关键词
                pageIndex = Convert.ToInt32(Request[ "pageIndex" ]); if (pageIndex < 1) pageIndex = 1;
                pageSize = Convert.ToInt32(Request[ "pageSize" ]); if (pageSize > 99 || pageSize < 1) pageSize = 10;
                stat = Convert.ToInt32(Request[ "stat" ]);
            }
            finally
            {
            }
            ViewBag.useds = ((int) Session[ "role" ] < 3) ? b.getByPagesByDormId(pageIndex, pageSize, (int) Session[ "pid" ], ref count, keyword) : b.getByPages(pageIndex, pageSize, ref count, keyword); // 获取列表

            ViewBag.keyword = keyword;
            ViewBag.count = count;  // 获取当前页数量
            ViewBag.pageIndex = pageIndex;  // 获取当前页

            return View();
        }

        // GET: Used/Details/5
        [NeedLogin]
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Used/Create
        [NeedLogin]
        public ActionResult Create()
        {
            var B = new DbHelper.Buildings();
            if ((int) Session[ "role" ] < 3)
                ViewBag.building = B.getAllActiveById((int) Session[ "pid" ]); // 非系统管理员只能看到所属于园区的宿舍
            else
                ViewBag.building = B.getAllActive(); // 非系统管理员只能看到所属于园区的宿舍
            return View();
        }

        // POST: Used/Create
        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="collection"></param>
        [HttpPost]
        [NeedLogin]
        public void Create(FormCollection collection)
        {
            var msg = new Msg();
            msg.code = 400;
            var Db = new DbContext().Db;
            try
            {
                var rate = Db.Queryable<TRate>().OrderBy(r => r.Id, SqlSugar.OrderByType.Desc).First();
                // 验证费率信息
                if (rate == null)
                {
                    msg.msg = "请先到'系统设置-费率及基础配额设置'设置费率信息";
                    goto end;
                }
                // 验证基础配额信息
                var quota = Db.Queryable<TQuota>().OrderBy(r => r.Id, SqlSugar.OrderByType.Desc).First();
                if (quota == null)
                {
                    msg.msg = "请先到'系统设置-费率及基础配额设置'设置基础配额信息";
                    goto end;
                }

                var Used = new Useds();
                var Used_total = new Useds_total();
                var Bill = new Bills();

                ///1.获取数据
                var cold_water_value = Convert.ToSingle(collection[ "cold_water_value" ]);
                var hot_water_value = Convert.ToSingle(collection[ "hot_water_value" ]);
                var electric_value = Convert.ToSingle(collection[ "electric_value" ]);
                ///1.1判断输入数值
                if (cold_water_value < 0 || hot_water_value < 0 || electric_value < 0)
                {
                    msg.msg += "数值输入有误，读表数值不能小于0";
                    goto end;
                }
                var Dorm_id = Convert.ToInt32(collection[ "dorm_id" ]); // 园区ID
                var Building_id = Convert.ToInt32(collection[ "building_id" ]); // 宿舍楼ID
                var Pid = Convert.ToInt32(collection[ "pid" ]); // 宿舍ID
                var Note = collection[ "note" ];
                if (Dorm_id < 0 || Building_id < 0 || Pid < 0)
                {
                    msg.msg = "宿舍ID或宿舍楼ID或园区ID输入有误";
                    goto end;
                }
                ///1.2判断该宿舍是否已登记, 避免重复操作
                if (Used.IsRecord(Pid))
                {
                    msg.msg = "该宿舍本月已经登记过了，无需再次登记";
                    goto end;
                }

                ///2.获取上次读数
                var this_cold_water_value = cold_water_value;
                var this_hot_water_value = hot_water_value;
                var this_electric_value = electric_value;
                ///3.计算本次用量
                var usedinfo = Db.Queryable<TUsed_total>().Where(u => Pid == u.Pid).First();
                if (usedinfo != null)
                {
                    ///3.1判断本次数值是否大于等于上次数值
                    if (this_cold_water_value < usedinfo.Cold_water_value || this_hot_water_value < usedinfo.Hot_water_value || this_electric_value < usedinfo.Electric_value)
                    {
                        msg.msg = "数值输入有误，水表电表通常是不会装着转滴。";
                        goto end;
                    }
                    // 本次数值=本次读数-上次读数
                    this_cold_water_value -= usedinfo.Cold_water_value;
                    this_hot_water_value -= usedinfo.Hot_water_value;
                    this_electric_value -= usedinfo.Electric_value;
                }
                else
                {
                    usedinfo = new TUsed_total();
                }
                usedinfo.Dorm_id = Dorm_id;
                usedinfo.Building_id = Building_id;
                usedinfo.Pid = Pid;
                usedinfo.Hot_water_value = hot_water_value;
                usedinfo.Cold_water_value = cold_water_value;
                usedinfo.Electric_value = electric_value;
                ///3.2生成用量单
                var used = new TUsed()
                {
                    Electric_value = this_electric_value,
                    Hot_water_value = this_hot_water_value,
                    Cold_water_value = this_cold_water_value,
                    Building_id = Building_id,
                    Dorm_id = Dorm_id,
                    Note = Note,
                    Pid = Pid,
                    Post_uid = (int) Session[ "id" ],
                };
                Db.Ado.BeginTran(); // 开始事务
                ///3.3保存用量单
                var uid = Db.Insertable(used).ExecuteReturnEntity();

                if (uid.Id < 1)// 插入并更新自增ID
                {
                    msg.msg = "保存登记信息时发生错误！";
                    // 回滚事务
                    Db.Ado.RollbackTran();
                    goto end;
                }
                ///4.获取基础配额数据
                if (quota != null && quota.Is_active)
                {
                    this_cold_water_value -= quota.Cold_water_value;
                    this_hot_water_value -= quota.Hot_water_value;
                    this_electric_value -= quota.Electric_value;
                }

                ///5.计算本次费用并生成账单
                var bill = new TBill();
                bill.Pid = uid.Id;
                bill.Room_id = Pid;
                bill.Building_id = Building_id;
                bill.Dorm_id = Dorm_id;
                bill.Cold_water_cost = (decimal) this_cold_water_value * (decimal) rate.Cold_water_value;
                bill.Electric_cost = (decimal) this_electric_value * (decimal) rate.Electric_value;
                bill.Hot_water_cost = (decimal) this_hot_water_value * (decimal) rate.Hot_water_value;
                bill.Rates_id = rate.Id;
                bill.Quota_id = quota.Id;
                //msg.content = last;goto end;//#debug
                ///7.保存所有数据

                var flage = true;
                ///7.1保存读表信息
                if (usedinfo.Id < 1)
                {
                    if (Db.Insertable(usedinfo).ExecuteCommand() < 1)
                    {
                        flage = false;
                        msg.msg = "保存读表信息时发生错误！";
                    }

                }
                else if (Db.Updateable(usedinfo).ExecuteCommand() < 1)
                {
                    flage = false;
                    msg.msg = "更新读表信息时发生错误！";
                }
                ///7.2保存账单
                if (Db.Insertable(bill).ExecuteCommand() < 1)
                {
                    flage = false;
                    msg.msg += "添加账单信息时发生错误！";
                }

                if (flage)
                {
                    Db.Ado.CommitTran();// 提交事务
                    msg.code = 200;
                    msg.msg = "添加成功！";
                }
            }
            catch (Exception ex)
            {
                //发生错误，回滚事务
                Db.Ado.RollbackTran();
                msg.code = 500;
                msg.msg += ex.Message;
            }
        end:    // 不满足条件直接跳到这里
            Response.Write(msg.ToJson());
            Response.End();
        }



        /// <summary>
        /// 删除登记信息
        /// </summary>
        /// <param name="id"></param>
        // GET: Used/Delete/5
        [NeedLogin]
        public void Delete(int id)
        {
            var msg = new Msg();
            msg.code = 500;
            var Db = new DbContext().Db;
            var Used = new Useds();
            var Used_total = new Useds_total();
            try
            {
                ///0.开始事务
                Db.Ado.BeginTran();
                ///1.获取记录数据
                var used = Used.findById(id);
                if (used == null)
                {
                    msg.msg = "该记录已被删除";
                    goto end;
                }
                ///3.更新读表信息
                var last = Used_total.Last(used.Pid);
                if (last != null)
                { // 读表数值=读表数值-本次读数
                    last.Cold_water_value -= used.Cold_water_value;
                    last.Hot_water_value -= used.Hot_water_value;
                    last.Electric_value -= used.Electric_value;
                }
                else
                {
                    last = new TUsed_total();
                    last.Dorm_id = used.Dorm_id;
                    last.Building_id = used.Building_id;
                    last.Pid = used.Pid;
                }
                last.Post_date = DateTime.Now;
                ///4保存读表信息
                if (last.Id < 1)
                {
                    if (Db.Insertable(last).ExecuteCommand() < 1)
                    {
                        msg.msg = "更新读表信息时发生错误！";
                        Db.Ado.RollbackTran();
                    }

                }
                else if (Db.Updateable(last).ExecuteCommand() < 1)
                {
                    msg.msg = "更新读表信息时发生错误！";
                    Db.Ado.RollbackTran();
                }
                else
                {
                    Used.Delete(id);
                    Db.Ado.CommitTran();// 提交事务
                    msg.code = 200;
                    msg.msg = "删除成功！";
                }
            }
            catch (Exception ex)
            {
                Db.Ado.RollbackTran();//发生错误，回滚操作
                msg.msg += ex.Message;
            }
        end:    // 不满足条件直接跳到这里
            Response.Write(msg.ToJson());
            Response.End();
        }

        // POST: Used/Delete/5
        [HttpPost]
        [NeedLogin]
        public void Delete(int id, FormCollection collection)
        {
            this.Delete(id);
        }

        [NeedLogin]
        public void Last(FormCollection collection)
        {
            /// 获取宿舍的读表数值
            var msg = new Msg();
            var Used = new Useds_total();
            msg.content = Used.Last(Convert.ToInt32(collection[ "pid" ]));
            Response.Write(msg.ToJson());
            Response.End();
        }

        /// <summary>
        /// 宿舍水电表管理
        /// </summary>
        /// <returns></returns>
        [NotLowUser]
        public ActionResult UsedInfo()
        {
            var keyword = "";
            int pageIndex = 1;
            int pageSize = 10;
            int stat = 0;
            int count = 0;
            try
            {
                keyword = Request[ "keyword" ]; // 搜索关键词
                pageIndex = Convert.ToInt32(Request[ "pageIndex" ]); if (pageIndex < 1) pageIndex = 1;
                pageSize = Convert.ToInt32(Request[ "pageSize" ]); if (pageSize > 99 || pageSize < 1) pageSize = 10;
                stat = Convert.ToInt32(Request[ "stat" ]);
            }
            finally
            {
            }
            var b = new Useds_total();
            ViewBag.useds = ((int) Session[ "role" ] < 3) ? b.getByPagesByDormId(pageIndex, pageSize, (int) Session[ "pid" ], ref count, keyword) : ViewBag.useds = b.getByPages(pageIndex, pageSize, ref count, keyword);// 获取列表

            ViewBag.keyword = keyword;
            ViewBag.count = count;  // 获取当前页数量
            ViewBag.pageIndex = pageIndex;  // 获取当前页
            return View();
        }

        /// <summary>
        /// 宿舍水电表设置
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        [NotLowUser]
        public ActionResult CreateUsedInfo()
        {
            var Room = new Rooms();
            if ((int) Session[ "role" ] < 3)
                ViewBag.rooms = Room.GetVRoomWithoutUsedInfo((int) Session[ "pid" ]); // 非系统管理员只能看到所属于园区的宿舍
            else
                ViewBag.rooms = Room.GetVRoomWithoutUsedInfo();
            return View();
        }

        [NotLowUser]
        [HttpPost]
        public void CreateUsedInfo(int pid, FormCollection collection)
        {
            var msg = new Msg();
            var Db = new Useds_total().Db;
            msg.code = 400;
            try
            {
                var cold_water_value = Convert.ToSingle(collection[ "cold_water_value" ]);
                var hot_water_value = Convert.ToSingle(collection[ "hot_water_value" ]);
                var electric_value = Convert.ToSingle(collection[ "electric_value" ]);

                ///1.1判断输入数值
                if (cold_water_value < 0 || hot_water_value < 0 || electric_value < 0)
                {
                    msg.msg = "数值输入有误，读表数值不能小于0";
                    goto end;
                }
                var Dorm_id = Convert.ToInt32(collection[ "dorm_id" ]); // 园区ID
                var Building_id = Convert.ToInt32(collection[ "building_id" ]); // 宿舍楼ID
                var Pid = Convert.ToInt32(collection[ "pid" ]); ; // 宿舍ID
                var Note = collection[ "note" ];

                Db.Ado.BeginTran(); //开始事务
                var usedinfo = Db.Queryable<TUsed_total>().Where(u => pid == u.Pid).First();
                if (usedinfo == null)
                {
                    usedinfo = new TUsed_total();
                }
                usedinfo.Note = Note;
                usedinfo.Dorm_id = Dorm_id;
                usedinfo.Building_id = Building_id;
                usedinfo.Pid = Pid;
                usedinfo.Hot_water_value = hot_water_value;
                usedinfo.Cold_water_value = cold_water_value;
                usedinfo.Electric_value = electric_value;

                usedinfo.Post_date = DateTime.Now;
                var flage = true;
                ///7.1保存读表信息
                if (usedinfo.Id < 1)
                {
                    if (Db.Insertable(usedinfo).ExecuteCommand() < 1)
                    {
                        flage = false;
                        msg.msg = "更新读表信息时发生错误！";
                    }

                }
                else if (Db.Updateable(usedinfo).ExecuteCommand() < 1)
                {
                    flage = false;
                    msg.msg = "更新读表信息时发生错误！";
                }

                if (flage)
                {
                    msg.code = 200;
                    Db.Ado.CommitTran();// 提交事务
                    msg.msg = "保存成功";
                }
            }
            catch (Exception ex)
            {
                Db.Ado.RollbackTran();//发生错误，回滚操作
                msg.code = 500;
                msg.msg += ex.Message;
            }

        end:    // 不满足条件直接跳到这里
            Response.Write(msg.ToJson());
            Response.End();
        }

        [NotLowUser]
        public ActionResult EditUsedInfo(int id)
        {
            var Used = new Useds_total();
            return View(Used.FindById(id));
        }

        /// <summary>
        /// 删除宿舍读表信息
        /// </summary>
        /// <param name="id">记录</param>
        [NotLowUser]
        public void DeleteUsedInfo(int id)
        {
            var msg = new Msg();
            var Used = new Useds_total();
            var used = Used.findById(id);
            if (used == null)
            {
                msg.msg = "该宿舍读表信息不存在！";
                msg.code = 404;
            }
            else
            {
                if (!Used.Delete(id))
                {
                    msg.msg = "删除宿舍读表信息时发生错误！";
                    msg.code = 500;
                }
                else
                {
                    msg.msg = "删除成功！";
                }
            }
            Response.Write(msg.ToJson());
            Response.End();
        }


        /// <summary>
        /// 删除宿舍读表信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        [NotLowUser]
        [HttpPost]
        public void DeleteUsedInfo(int id, FormCollection collection)
        {
            DeleteUsedInfo(id);
        }
    }
}
