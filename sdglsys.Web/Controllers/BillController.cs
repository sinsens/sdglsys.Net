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
    [AutoLogin]
    public class BillController : Controller
    {
        // GET: Used
        [NotLowUser]
        public ActionResult Index()
        {
            string keyword = null; // 关键词
            int page = 1; // 当前页数
            int limit = 10; // 每页显示数量
            int stat = -1;  // 账单状态
            int count = 0;  // 结果数量
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
                    stat = -1;
                }
                var vbills = new Bills().GetByPages(page, limit, ref count, keyword, (short)stat, (int)Session["pid"]); // 获取列表
                ViewBag.stat = stat;
                ViewBag.keyword = keyword;
                ViewBag.count = count;  // 当前页数量
                ViewBag.page = page;  // 获取当前页

                return View(vbills);
            }
            catch(Exception)
            {
                throw;
            }
            
        }

        /// <summary>
        /// 创建一个账单
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        [NotLowUser]
        public ActionResult Create()
        {
            var Building = new Buildings();
            ViewBag.buildings = (int) Session["role"] < 3 ? Building.GetAllActiveById((int) Session["pid"]) : Building.GetAllActive();
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
            SqlSugar.SqlSugarClient Db = null;
            try
            {
                var rate = new Rates().GetLast();
                // 验证费率信息
                var quota = new Quotas().GetLast();
                if (rate == null)
                {
                    throw new Exception("请先设置费率信息");
                }
                ///1.获取数据
                var cold_water_value = Convert.ToSingle(collection["cold_water_value"]);
                var hot_water_value = Convert.ToSingle(collection["hot_water_value"]);
                var electric_value = Convert.ToSingle(collection["electric_value"]);
                ///1.1判断输入数值
                if (cold_water_value < 0 || hot_water_value < 0 || electric_value < 0)
                {
                    throw new Exception("数值输入有误，费用不能小于0");
                }
                var Dorm_id = Convert.ToInt32(collection["dorm_id"]); // 园区ID
                var Building_id = Convert.ToInt32(collection["building_id"]); // 宿舍楼ID
                var Pid = Convert.ToInt32(collection["pid"]); // 宿舍ID
                var Note = collection["note"];
                if (Dorm_id < 0 || Building_id < 0 || Pid < 0)
                {
                    throw new Exception("宿舍ID或宿舍楼ID或园区ID输入有误");
                }
                if (Note.Trim().Length < 3)
                {
                    throw new Exception("手动添加账单时请输入至少3个字符作为说明");
                }
                /// 生成一个虚拟用量登记
                var used = new T_Used()
                {
                    Used_room_id = Pid,
                    Used_note = "手动添加账单后自动生成的登记信息，并不会更新读表数据",
                    Used_building_id = Building_id,
                    Used_dorm_id = Dorm_id,
                    Used_post_user_id = (int) Session["id"],
                };
                /// 开始事务
                Db = new DbContext().Db;
                Db.Ado.BeginTran();
                var u = Db.Insertable(used).ExecuteReturnIdentity();
                /// 生成账单
                var bill = new T_Bill()
                {
                    Bill_note = Note,
                    Bill_used_id = u,
                    Bill_room_id = Pid,
                    Bill_building_id = Building_id,
                    Bill_dorm_id = Dorm_id,
                    Bill_cold_water_cost = (decimal) cold_water_value,
                    Bill_electric_cost = (decimal) hot_water_value,
                    Bill_hot_water_cost = (decimal) electric_value,
                    Bill_rates_id = rate.Rate_id,
                    Bill_quota_id = quota.Quota_id,
                };
                /// 保存账单
                if (Db.Insertable(bill).ExecuteCommand() < 1)
                {
                    throw new Exception("添加账单信息时发生错误！");
                }

                Db.Ado.CommitTran();// 提交事务
                msg.Message = "添加成功！";
            }
            catch (Exception ex)
            {
                //发生错误，回滚事务
                if (Db != null) {
                    Db.Ado.RollbackTran();
                }
                msg.Code = -1;
                msg.Message = ex.Message;
            }
            Response.Write(msg.ToJson());
            Response.End();
        }

        /// <summary>
        /// 删除账单
        /// </summary>
        /// <param name="id"></param>
        // GET: Used/Delete/5
        [NotLowUser]

        public void Delete(int id)
        {
            var msg = new Msg();
            var Bill = new Bills();
            try
            {
                ///0.开始事务
                ///1.获取记录数据
                var bill = Bill.FindById(id);
                if (bill == null)
                {
                    throw new Exception("该账单已被删除");
                }
                else if (Bill.Delete(id))
                {
                    msg.Message = "删除成功！";
                }
                else {
                    msg.Message = "发生未知错误";
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

        // POST: Used/Delete/5
        [HttpPost]
        [NotLowUser]
        public void Delete(int id, FormCollection collection)
        {
            this.Delete(id);
        }

        /// <summary>
        /// 修改账单信息
        /// </summary>
        /// <param name="id"></param>
        // GET: Used/Delete/5
        [NotLowUser]
        [HttpPost]
        public void Edit(int id, FormCollection collection)
        {
            var msg = new Msg();
            var Db = new DbContext().Db;
            try
            {
                var bill = new Bills().FindById(id);
                if (bill == null)
                {
                    throw new Exception("该账单信息不存在！");
                }
                ///1.获取数据
                var cold_water_cost = Convert.ToDecimal(collection["cold_water_cost"]);
                var hot_water_cost = Convert.ToDecimal(collection["hot_water_cost"]);
                var electric_cost = Convert.ToDecimal(collection["electric_cost"]);
                ///1.1判断输入数值
                if (cold_water_cost < 0 || hot_water_cost < 0 || electric_cost < 0)
                {
                    throw new Exception("数值输入有误，费用不能小于0");
                }
                var Dorm_id = Convert.ToInt32(collection["dorm_id"]); // 园区ID
                var Building_id = Convert.ToInt32(collection["building_id"]); // 宿舍楼ID
                var Pid = Convert.ToInt32(collection["pid"]); // 宿舍ID
                var Note = collection["note"];
                var stat = Convert.ToSByte(collection["stat"]); // 账单状态
                if (Dorm_id < 0 || Building_id < 0 || Pid < 0)
                {
                    throw new Exception("宿舍ID或宿舍楼ID或园区ID输入有误");
                }
                if (Note.Trim().Length < 3)
                {
                    throw new Exception("修改账单时请输入至少3个字符作为说明");
                }
                ///修改账单数值
                bill.Bill_cold_water_cost = cold_water_cost;
                bill.Bill_hot_water_cost = hot_water_cost;
                bill.Bill_electric_cost = electric_cost;
                bill.Bill_note = Note;
                bill.Bill_is_active = stat;
                Db.Ado.BeginTran();
                ///7.2保存账单
                if (Db.Updateable(bill).ExecuteCommand() < 1)
                {
                    throw new Exception("保存账单信息时发生错误！");
                }
                Db.Ado.CommitTran();// 提交事务
                msg.Message = "保存成功！";
            }
            catch (Exception ex)
            {
                //发生错误，回滚事务
                Db.Ado.RollbackTran();
                msg.Code = -1;
                msg.Message = "修改账单时发生错误：" + ex.Message;
            }
            Response.Write(msg.ToJson());
            Response.End();
        }

        /// <summary>
        /// 修改账单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [NotLowUser]
        public ActionResult Edit(int id)
        {
            return View(new Bills().FindVBillById(id));
        }

        /// <summary>
        /// 结算该账单，设置账单为已付款状态
        /// </summary>
        /// <param name="id"></param>
        [NotLowUser]
        public void Pay(int id)
        {
            var msg = new Msg();
            try
            {
                var Db = new Bills().BillDb;
                var bill = Db.GetById(id);
                if (bill == null)
                {
                    throw new Exception("该账单不存在");
                }
                bill.Bill_is_active = 2;
                if (Db.Update(bill))
                {
                    msg.Message = "结算成功！";
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
    }
}
