using sdglsys.DbHelper;
using sdglsys.Entity;
using SqlSugar;
using System;
using System.Linq;

namespace sdglsys.XUtils
{
    class Program : DbContext
    {
        static void Main(string[] args)
        {
            var Db = new DbContext().Db;
            /*
            var o = Db.Queryable<TUsed, TRoom, TBuilding, TDorm>((u, r, b, d) => new object[] {
                JoinType.Left,u.Pid==r.Id,
                JoinType.Left, r.Pid == b.Id,
                JoinType.Left, b.Pid == d.Id }).Where((u, r, b, d) => r.Is_active == true).
                Where("r.id not in (select pid from used where DATE_FORMAT(used.post_date, '%Y-%m') = DATE_FORMAT(NOW(),'%Y-%m'))").
              Select((u, r, b, d) => new VRoom
              {
                  Id = r.Id,
                  Pid = r.Pid,
                  Vid = r.Vid,
                  Nickname = r.Nickname,
                  Note = r.Note,
                  PNickname = b.Nickname,
                  Is_active = r.Is_active,
                  Dorm_Nickname = d.Nickname
              }).ToSql().ToString();
            var t = Db.Queryable<TRoom, TBuilding, TDorm>((r, b, d) => new object[] {
                JoinType.Left, r.Pid == b.Id,
                JoinType.Left, b.Pid == d.Id }).Where((r, b, d) => r.Is_active == true).
                Where("r.id not in (select pid from used where DATE_FORMAT(used.post_date, '%Y-%m') = DATE_FORMAT(NOW(),'%Y-%m'))").
              Select((r, b, d) => new VRoom
              {
                  Id = r.Id,
                  Pid = r.Pid,
                  Vid = r.Vid,
                  Nickname = r.Nickname,
                  Note = r.Note,
                  PNickname = b.Nickname,
                  Is_active = r.Is_active,
                  Dorm_Nickname = d.Nickname
              }).ToSql().ToString();

            //Console.WriteLine(o);
            //Console.WriteLine(t);
            */
            /*
            var used = new Used_data();
            var used_data = new DbHelper.Used_datas();
            Console.WriteLine(sdglsys.DbHelper.Utils.ToJson(used));
            Console.WriteLine(sdglsys.DbHelper.Utils.ToJson(used_data));
            used_data.title = "2342342423";
            used_data.info = "2342342423";
            used_data.Add(new Used_data
            {
                 Cold_water_value = 2312,
                 Date = "2018-12",
                  Electric_value=234.234F,
                   Hot_water_value= 2342.23F
            });
            used_data.Add(new Used_data
            {
                Cold_water_value = 2312,
                Date = "2018-11",
                Electric_value = 234.234F,
                Hot_water_value = 2342.23F
            });
            used_data.Add(new System.Collections.Generic.List<Used_data>()
            {
                new Used_data
            {
                Cold_water_value = 2312,
                Date = "2018-11",
                Electric_value = 234.234F,
                Hot_water_value = 2342.23F
            },new Used_data
            {
                Cold_water_value = 2312,
                Date = "2018-11",
                Electric_value = 234.234F,
                Hot_water_value = 2342.23F
            }
            });
            Console.WriteLine(sdglsys.DbHelper.Utils.ToJson(used_data));
            */

            /*
            var a = Db.Queryable<TUsed>().Where(u => u.Is_active == true && u.Dorm_id == 2 && SqlFunc.Between(u.Post_date,DateTime.Now,DateTime.Now)).GroupBy(u=>SqlFunc.Substring(u.Post_date,1,7)).Select(u => new Used_data
            {
                Date = SqlFunc.Substring(u.Post_date,1,7),
                Cold_water_value =SqlFunc.AggregateSum(u.Cold_water_value),
                Electric_value = SqlFunc.AggregateSum(u.Electric_value),
                Hot_water_value = SqlFunc.AggregateSum(u.Hot_water_value)
            }).ToSql().ToString();
            Console.WriteLine(DbHelper.Utils.ToJson(a));
            */
            /*
            Console.WriteLine("去年的现在是" + DateTime.Now.AddYears(-1));
            var udata = new Useds();
            var data = udata.GetUsedDatas();
            Console.WriteLine(data.ToJson());

            */
            /*
            /// 查找本月未登记的宿舍
            /// 先获取本月已登记宿舍
            var rooms = Db.Queryable<TUsed>().
                Where(u => SqlFunc.Between(SqlFunc.Substring(u.Post_date, 0, 7), 
                SqlFunc.Substring(DateTime.Now, 0, 7), SqlFunc.Substring(DateTime.Now, 0, 7))).Select(u=>u.Pid).ToList();

            var j = Db.Queryable<TRoom, TBuilding, TDorm>((r, b, d) => new object[] {
                JoinType.Left, r.Pid == b.Id,
                JoinType.Left, b.Pid == d.Id }).Where((r, b, d) => r.Is_active == true && !rooms.Contains(r.Id)).
              Select((r, b, d) => new VRoom
              {
                  Id = r.Id,
                  Pid = r.Pid,
                  Dorm_id = r.Dorm_id,
                  Vid = r.Vid,
                  Nickname = r.Nickname,
                  Note = r.Note,
                  PNickname = b.Nickname,
                  Is_active = r.Is_active,
                  Dorm_Nickname = d.Nickname
              }).ToList();

            Console.WriteLine(rooms);
            Console.WriteLine(sdglsys.DbHelper.Utils.ToJson(j));
            */
            /// 判断宿舍本月是否已登记
            /*
            var i = Db.Queryable<TUsed>().
                Where(u => 1 == u.Pid && SqlFunc.Between(SqlFunc.Substring(u.Post_date, 0, 7),
                SqlFunc.Substring(DateTime.Now, 0, 7), SqlFunc.Substring(DateTime.Now, 0, 7))).Select(u => u.Pid).First();
            Console.WriteLine(i);
            */

            /// 添加宿舍读表数值记录
            /*
            var usedinfo = Db.Queryable<TUsed_total>().Where(u => 1 == u.Pid).First();
            if (usedinfo == null)
            {
                usedinfo = new TUsed_total();
            }
            
            if (usedinfo == null)
            {
                usedinfo = new TUsed_total();
            }
            usedinfo.Note = "";
            usedinfo.Dorm_id = 0;
            usedinfo.Building_id = 0;
            usedinfo.Pid = 0;
            usedinfo.Hot_water_value = 0;
            usedinfo.Cold_water_value = 0;
            usedinfo.Electric_value = 0;
            usedinfo.Post_date = DateTime.Now;

            ///7.1保存读表信息
            if (usedinfo.Id < 1)
                Console.WriteLine(Db.Insertable(usedinfo).ExecuteCommand().ToString());
            
            var Used = new Useds();
            var list = Used.GetUsedDatas(5,0, DateTime.Parse("2012-07-01 15:00:55"),DateTime.Parse("2018-07-02 15:00:55"));
            Console.WriteLine(sdglsys.DbHelper.Utils.ToJson(list));
            */
            while (true) {
                MainUsage();
                Console.Write("请输入选项：");
                var type = Console.ReadLine();
                switch (type)
                {
                    case "init_db":
                        InitDb();
                        break;
                    case "create_admin":
                        CreateAdmin();
                        break;
                    case "q": // 退出
                        return;
                }
            }
        }

        /// <summary>
        /// 打印字符
        /// </summary>
        /// <param name="c">字符</param>
        /// <param name="count">打印次数</param>
        static void PrintChar(char c, int count) {
            Console.WriteLine();
            for (var i=0;i<count;i++)
            {
                Console.Write(c);
            }
            Console.WriteLine();
        }

        /// <summary>
        /// 显示主界面帮助信息
        /// </summary>
        static void MainUsage() {
            Console.Title = "校园水电管理命令行工具";
            Console.Clear();
            PrintChar('*', 50);
            Console.WriteLine("选项列表：");
            //Console.WriteLine("\tinit_db :初始化数据库");
            Console.WriteLine("\tcreate_admin :创建系统管理员角色");
            Console.WriteLine("\tq :退出");
            PrintChar('*', 50);
        }

        /// <summary>
        /// 初始化数据库选项
        /// </summary>
        static void InitDb() {
            Console.Title = "初始化数据库";
            Console.Clear();
            Print("该功能暂时不可用。。。");
            Print("按回车键返回");
            Console.Read();
            return;
            /*
            PrintChar('*', 50);
            PrintChar('!', 30);
            Console.WriteLine("如果您以前进行过初始化数据操作，再次初始化数据可能会导致以前的数据丢失！");
            Console.WriteLine("是否继续初始化数据操作。");
            Console.WriteLine("\tc :继续\tq :退出");
            PrintChar('*', 50);
            while (true) {
                Console.Write("请输入：");
                var o = Console.ReadLine();
                if (o == "c")
                {
                    PrintChar('.', 10);
                    var db = new DbTools();
                    if (db.InitDb())
                    {
                        Print("初始化数据库成功！\n输入q返回上一界面");
                    }
                }
                else if(o=="q"){
                    return;
                }
            }*/
        }


        /// <summary>
        /// 创建系统管理员选项
        /// </summary>
        static void CreateAdmin() {
            Console.Title = "创建系统管理员角色";
            Console.Clear();
            PrintChar('*', 50);
            Console.WriteLine("新建一个系统管理员");
            Print("在任意输入阶段输入q(小写)字符退出");
            PrintChar('*', 50);
            while (true)
            {
                start:
                Print("请输入用户名：", false);
                var login_name = Console.ReadLine().Trim();
            pwd:
                var pwd = InputPwd();
                
                if (login_name == "q" || pwd == "q") {
                    Print("您取消了创建管理员操作，按回车键返回上一界面");
                    Console.Read();
                    return;
                }
                if (login_name.Trim().Length < 3)
                {
                    Print("用户名长度不能小于3个字符");
                    goto start;
                }
                else if (pwd.Length < 5)
                {
                    Print("密码长度不能小于5个字符");
                    goto pwd;
                }
                var pwd2 = InputPwd("请再次输入密码：");
                if (pwd!=pwd2){
                    Print("两次密码输入不一致");
                    goto pwd;
                }
                Print("正在创建用户名为 '" +login_name+ "' 的系统管理员角色");
                var db = new DbTools();
                try {
                    if (db.CreateAdmin(login_name, pwd))
                    {
                        Print("创建系统管理员成功！\n输入q返回上一级界面。\n");
                        Console.Read();
                    }
                    else {
                        Print("创建系统管理员失败。发生未知错误。\n输入q返回上一级界面。\n");
                        Console.Read();
                    }
                } catch (Exception ex){
                    Print("创建系统管理员失败。发生异常：\n" + ex.Message);
                    Print("输入q返回上一级界面。");
                }
                
            }
        }

        /// <summary>
        /// 打印字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="new_line">打印完是否换行，默认换行</param>
        static void Print(string str, bool new_line= true) {
            if (new_line)
            {
                Console.WriteLine("\n"+str);
            }
            else {
                Console.Write("\n"+str);
            }
        }

        /// <summary>
        /// 获取输入的密码
        /// </summary>
        /// <param name="msg">提示文本</param>
        static string InputPwd(string msg=null) {
            string str = "";
            Print("提示：输入的密码并不会显示出来，按回车键结束密码输入。");
            if (msg == null)
            {
                Print("请输入密码：", false);
            }
            else {
                Print(msg, false);
            }
            while (true) {
                var key = Console.ReadKey(true); // 不回显输入的
                if (key.Key == ConsoleKey.Enter)
                    return str.Trim();
                if (key.Key == ConsoleKey.Backspace)
                {
                    if(str.Length>0)
                        str = (string)str.Substring(0,str.Length-1);
                }
                else if (key.Key < ConsoleKey.D0 || key.Key > ConsoleKey.NumPad9)
                {
                    // 不捕获其他功能键
                }
                else
                {
                    str += key.KeyChar;
                    //Print("*", false);
                }
            }
        }
    }
}
