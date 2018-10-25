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
            string keyword = null;
            int page = 1;
            int limit = 10;
            int stat = 0;
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
                if (!int.TryParse(Request["stat"], out stat))
                {
                    stat = 0;
                }

                var vuseds = new Useds().GetByPages(page, limit, ref count, keyword, (int)Session["pid"]); // 获取列表
                ViewBag.keyword = keyword;
                ViewBag.count = count;  // 获取当前页数量
                ViewBag.page = page;  // 获取当前页

                return View(vuseds);
            }
            catch (Exception)
            {
                throw;
            }


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
            var vbuildings = new List<Entity.VBuilding>();
            try
            {
                var Buildings = new DbHelper.Buildings();
                vbuildings = ((int)Session["role"] < 3) ? Buildings.GetAllActiveById((int)Session["pid"]) : Buildings.GetAllActive();
                // 非系统管理员只能看到所属于园区的宿舍
            }
            catch (Exception)
            {
                throw;
            }

            return View(vbuildings);
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
            var Db = new DbContext().Db;
            try
            {
                var rate = new Rates().GetLast();
                // 验证费率信息
                if (rate == null)
                {
                    throw new Exception("请先到'系统设置-费率及基础配额设置'设置费率信息");
                }
                // 验证基础配额信息
                var quota = new Quotas().GetLast();
                if (quota == null)
                {
                    throw new Exception("请先到'系统设置-费率及基础配额设置'设置基础配额信息");
                }

                ///1.获取数据

                var cold_water_value = Convert.ToSingle(collection["cold_water_value"]);
                var hot_water_value = Convert.ToSingle(collection["hot_water_value"]);
                var electric_value = Convert.ToSingle(collection["electric_value"]);
                ///1.1判断输入数值
                if (cold_water_value < 0 || hot_water_value < 0 || electric_value < 0 || cold_water_value > 9999999 || hot_water_value > 9999999 || electric_value > 9999999)
                {
                    throw new Exception("数值输入有误，读表数值应在0~999999之间");
                }
                var Pid = Convert.ToInt32(collection["pid"]); // 宿舍ID
                var Note = collection["note"];
                if (Pid < 0 || Pid > 99999999)
                {
                    throw new Exception("宿舍ID输入有误，应在1~99999999之间");
                }
                ///1.2获取宿舍信息
                var room = Db.Queryable<T_Room>().Single(x=>x.Room_id==Pid&&x.Room_model_state);
                if (room == null)
                {
                    throw new Exception("该宿舍不存在");
                }
                if (room.Number < 1)
                {
                    throw new Exception("该宿舍无人居住，无需登记");
                }
                ///1.3判断该宿舍是否已登记, 避免重复操作
                if (new DbHelper.Useds().IsRecord(Pid))
                {
                    throw new Exception("该宿舍本月已经登记过了，无需再次登记");
                }

                ///2.获取上次读数
                var this_cold_water_value = cold_water_value;
                var this_hot_water_value = hot_water_value;
                var this_electric_value = electric_value;
                ///3.计算本次用量
                var usedinfo = Db.Queryable<T_Used_total>().Single(x=>x.Ut_room_id==Pid&&x.Ut_model_state);
                if (usedinfo != null)
                {
                    ///3.1判断本次数值是否大于等于上次数值
                    if (this_cold_water_value < usedinfo.Ut_cold_water_value || this_hot_water_value < usedinfo.Ut_hot_water_value || this_electric_value < usedinfo.Ut_electric_value)
                    {
                        throw new Exception("数值输入有误，本期水表电表数值应大于等于上期读表数值");
                    }
                    // 本次数值=本次读数-上次读数
                    this_cold_water_value -= usedinfo.Ut_cold_water_value;
                    this_hot_water_value -= usedinfo.Ut_hot_water_value;
                    this_electric_value -= usedinfo.Ut_electric_value;
                }
                else
                {
                    usedinfo = new T_Used_total();
                }
                usedinfo.Ut_dorm_id = room.Room_dorm_id;
                usedinfo.Ut_building_id = room.Room_building_id;
                usedinfo.Ut_room_id = Pid;
                usedinfo.Ut_hot_water_value = hot_water_value;
                usedinfo.Ut_cold_water_value = cold_water_value;
                usedinfo.Ut_electric_value = electric_value;

                ///3.1生成用量单
                var used = new T_Used()
                {
                    Used_electric_value = this_electric_value,
                    Used_hot_water_value = this_hot_water_value,
                    Used_cold_water_value = this_cold_water_value,
                    Used_building_id = room.Room_building_id,
                    Used_dorm_id = room.Room_dorm_id,
                    Used_note = Note,
                    Used_room_id = Pid,
                    Used_post_user_id = (int)Session["id"],
                };
                ///3.2扣除基础配额数据，最终使用量 =（本次读表数值-上次读表数值）-（基础配额*人数）
                if (quota != null && quota.Quota_is_active)
                {
                    this_cold_water_value -= quota.Quota_cold_water_value * room.Number;
                    this_hot_water_value -= quota.Quota_hot_water_value * room.Number;
                    this_electric_value -= quota.Quota_electric_value * room.Number;
                }
                Db.Ado.BeginTran(); // 开始事务
                ///3.3保存用量单
                var uid = Db.Insertable(used).ExecuteReturnEntity();

                if (uid.Used_id < 1)// 插入并更新自增ID
                {
                    throw new Exception("保存登记信息时发生错误！");
                }

                ///5.计算本次费用并生成账单，费用 = 最终使用量*费率，（无阶梯计费）
                var bill = new T_Bill();
                bill.Bill_used_id = uid.Used_id;
                bill.Bill_room_id = room.Room_id;
                bill.Bill_building_id = room.Room_building_id;
                bill.Bill_dorm_id = room.Room_dorm_id;
                ///6.超过基础配额的才计费
                if (this_cold_water_value > 0)
                { // 冷水费
                    bill.Bill_cold_water_cost = (decimal)this_cold_water_value * (decimal)rate.Rate_cold_water_value;
                }
                if (this_hot_water_value > 0)
                { // 热水费
                    bill.Bill_hot_water_cost = (decimal)this_hot_water_value * (decimal)rate.Rate_hot_water_value;
                }
                if (this_electric_value > 0)
                { // 电费
                    bill.Bill_electric_cost = (decimal)this_electric_value * (decimal)rate.Rate_electric_value;
                }

                bill.Bill_rates_id = rate.Rate_id;
                bill.Bill_quota_id = quota.Quota_id;
                ///7.保存所有数据

                ///7.1保存读表信息
                if (usedinfo.Ut_id < 1)
                {
                    if (Db.Insertable(usedinfo).ExecuteCommand() < 1)
                    {
                        throw new Exception("保存读表信息时发生错误！");
                    }

                }
                else if (Db.Updateable(usedinfo).ExecuteCommand() < 1)
                {
                    throw new Exception("更新读表信息时发生错误！");
                }
                ///7.2保存账单
                if (Db.Insertable(bill).ExecuteCommand() < 1)
                {
                    throw new Exception("添加账单信息时发生错误！");
                }

                Db.Ado.CommitTran();// 提交事务
                msg.Message = "登记成功！";
            }
            catch (Exception ex)
            {
                //发生错误，回滚事务
                Db.Ado.RollbackTran();
                msg.Code = -1;
                msg.Message = ex.Message;
            }
            Response.Write(msg.ToJson());
            Response.End();
        }



        /// <summary>
        /// 删除登记信息
        /// </summary>
        /// <param name="id"></param>
        // Post: Used/Delete/5
        [NeedLogin]
        [HttpPost]
        public void Delete(int id)
        {
            var msg = new Msg();
            try
            {
                new Useds().BllDelete(id);
                msg.Message = "删除成功！";
            }
            catch (Exception ex)
            {
                msg.Code = -1;
                msg.Message = ex.Message;
            }
            Response.Write(msg.ToJson());
            Response.End();
        }

        [NeedLogin]
        public void Last(FormCollection collection)
        {
            /// 获取宿舍的读表数值
            var msg = new Msg();
            try
            {
                var Used = new Useds_total();
                msg.Content = Used.Last(Convert.ToInt32(collection["pid"]));
                Response.Write(msg.ToJson());
                Response.End();
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// 宿舍水电表管理
        /// </summary>
        /// <returns></returns>
        [NotLowUser]
        public ActionResult UsedInfo()
        {
            var keyword = "";
            int page = 1;
            int limit = 10;
            int stat = 0;
            int count = 0;
            try
            {
                keyword = Request["keyword"]; // 搜索关键词
                page = Convert.ToInt32(Request["page"]); if (page < 1) page = 1;
                limit = Convert.ToInt32(Request["limit"]); if (limit > 99 || limit < 1) limit = 10;
                stat = Convert.ToInt32(Request["stat"]);

                var Useds_total = new Useds_total();
                ViewBag.useds = Useds_total.GetByPages(page, limit, ref count, keyword, (int)Session["pid"]);// 获取列表

                ViewBag.keyword = keyword;
                ViewBag.count = count;  // 获取当前页数量
                ViewBag.page = page;  // 获取当前页
                return View();
            }
            catch (Exception)
            {
                throw;
            }
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
            if ((int)Session["role"] < 3)
                ViewBag.rooms = Room.GetVRoomWithouT_UsedInfo((int)Session["pid"]); // 非系统管理员只能看到所属于园区的宿舍
            else
                ViewBag.rooms = Room.GetVRoomWithouT_UsedInfo();
            return View();
        }

        [NotLowUser]
        [HttpPost]
        public void CreateUsedInfo(FormCollection collection)
        {
            var msg = new Msg();
            var Db = new Useds_total().Db;
            try
            {
                var pid = Convert.ToInt32(collection["pid"]);
                var cold_water_value = Convert.ToSingle(collection["cold_water_value"]);
                var hot_water_value = Convert.ToSingle(collection["hot_water_value"]);
                var electric_value = Convert.ToSingle(collection["electric_value"]);
                var Note = collection["note"];

                ///1.1判断输入数值
                if (cold_water_value < 0 || hot_water_value < 0 || electric_value < 0)
                {
                    throw new Exception("数值输入有误，读表数值不能小于0");
                }
                else
                {

                    var usedinfo = Db.Queryable<T_Used_total>().Single(u => u.Ut_model_state && pid == u.Ut_room_id);
                    if (usedinfo == null)//如果读表数值为null，新建一个读表数值对象
                    {
                        usedinfo = new T_Used_total();
                        var room = Db.Queryable<Entity.T_Room>().Single(x => x.Room_id == pid && x.Room_model_state);
                        if (room == null)
                        {
                            throw new Exception("该宿舍不存在,id:" + pid);
                        }
                        usedinfo.Ut_dorm_id = room.Room_dorm_id;
                        usedinfo.Ut_building_id = room.Room_building_id;
                        usedinfo.Ut_room_id = pid;
                    }
                    usedinfo.Ut_note = Note;

                    usedinfo.Ut_hot_water_value = hot_water_value;
                    usedinfo.Ut_cold_water_value = cold_water_value;
                    usedinfo.Ut_electric_value = electric_value;

                    usedinfo.Ut_post_date = DateTime.Now;
                    ///1.2保存读表信息
                    Db.Ado.BeginTran(); //开始事务
                    if (usedinfo.Ut_id < 1) // 如果是新增的读表记录，就执行插入操作
                    {
                        Db.Insertable(usedinfo).ExecuteCommand();
                        //throw new Exception("插入读表记录时发生错误");
                    }
                    else if (Db.Updateable(usedinfo).ExecuteCommand() < 1)
                    {
                        throw new Exception("更新读表记录时发生错误"); // 否则执行更新操作
                    }
                    Db.Ado.CommitTran();// 提交事务
                    msg.Message = "保存成功";
                }
            }
            catch (Exception ex)
            {
                Db.Ado.RollbackTran();//发生错误，回滚操作
                msg.Code = -1;
                msg.Message = "添加读表信息时发生错误：" + ex.Message;
            }
            Response.Write(msg.ToJson());
            Response.End();
        }

        [NotLowUser]
        public ActionResult EditUsedInfo(int id)
        {
            return View(new Useds_total().FindVUsedById(id));
        }

        /// <summary>
        /// 删除宿舍读表信息
        /// </summary>
        /// <param name="id">记录</param>
        [NotLowUser]
        public void DeleteUsedInfo(int id)
        {
            var msg = new Msg();
            try
            {
                var Used = new Useds_total();
                var used = Used.FindById(id);
                if (used == null)
                {
                    throw new Exception("该宿舍读表信息不存在！");
                }
                else if (!Used.Delete(id))
                {
                    throw new Exception("删除宿舍读表信息时发生错误！");
                }
                else
                {
                    msg.Message = "删除成功！";
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
