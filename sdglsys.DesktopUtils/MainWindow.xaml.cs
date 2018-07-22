using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows;

namespace sdglsys.DesktopUtils
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            TrialChk();
            resetTextBox();
        }

        private async void TrialChk()
        {
            DateTime trial_end_date;
            DateTime.TryParse("2019-12-30 23:59", out trial_end_date);
            if (DateTime.Now >= trial_end_date)
            {
                await this.ShowMessageAsync("温馨提示", "非常抱歉地提示您，您可能未经授权就使用了我的程序，或者该程序已到期，已经无法使用，现在是：" + DateTime.Now + "\r\n如有任何疑问，请联系QQ：1278386874");
                Application.Current.Shutdown();
            }
        }

        private async void btnSearchUser_Click(object sender, RoutedEventArgs e)
        {
            if (DBInfo.DB == null)
            {
                await this.ShowMessageAsync("温馨提示", "请先连接数据库");
                return;
            }
            if (tbxLogin_name.Text.Length < 1)
            {
                await this.ShowMessageAsync("温馨提示", "请输入用户名");
                return;
            }

            try
            {
                var U = new DbHelper.Users();
                var user = U.findVUserByLoginName(tbxLogin_name.Text);
                if (user == null)
                {
                    await this.ShowMessageAsync("温馨提示", "未找到用户名为'" + tbxLogin_name.Text + "'的系统用户。");
                }
                userinfo.DataContext = user;
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("发生错误", "错误信息：" + ex.Message);
            }
        }

        private void btnConnect_ClickAsync(object sender, RoutedEventArgs e)
        {
            Connect();
        }

        /// <summary>
        /// 连接到数据库
        /// </summary>
        private async void Connect()
        {
            if (tbxConnectString.Text.Trim().Length < 10)
            {
                await this.ShowMessageAsync("温馨提示", "数据库连接字符串长度必须大于10个字符");
                return;
            }
            if (sltDBType.Text.ToString().Length < 2)
            {
                await this.ShowMessageAsync("温馨提示", "请选择数据库类型");
                return;
            }
            try
            {
                DBInfo.DB = new DbHelper.DbContext(tbxConnectString.Text, sltDBType.Text);
                DBInfo.DB.Db.Open();
                await this.ShowMessageAsync("温馨提示", "连接数据库成功");
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "发生错误");
                await this.ShowMessageAsync("连接数据库失败！", "错误信息：" + ex.Message);
            }
        }

        private async void btnResetPwd_Click(object sender, RoutedEventArgs e)
        {
            // 重置系统角色密码
            if (Login_name.Text.Length < 1)
            {
                await this.ShowMessageAsync("温馨提示", "请先查找系统用户");
                return;
            }

            string newpwd = await this.ShowInputAsync("请输入密码", "输入新密码");
            if (newpwd == null)
                return;
            if (newpwd.Length < 5)
            {
                await this.ShowMessageAsync("温馨提示", "新密码长度应至少为5位数");
                return;
            }
            try
            {
                var pwd = DbHelper.Utils.hashpwd(newpwd.Trim());
                var db = new DbHelper.Users().Db;
                var user = db.Queryable<Entity.TUser>().Where(u => u.Login_name == Login_name.Text).Select(u => new Entity.VUser
                {
                    Pid = u.Pid,
                    Id = u.Id,
                    Nickname = u.Nickname,
                    Login_name = u.Login_name,
                    Is_active = u.Is_active,
                    Note = u.Note,
                    Phone = u.Phone,
                    Reg_date = u.Reg_date,
                    Role = u.Role,
                }).First();
                if (user == null)
                    user = new Entity.VUser();
                else
                {
                    foreach (var item in db.Queryable<Entity.TDorm>().ToList())
                    {
                        if (item.Id == user.Pid)
                        {
                            user.DormName = item.Nickname;
                        }
                    }

                }
                user.RoleName = user.Role < 3 ? (user.Role < 2 ? "辅助登记员" : "宿舍管理员") : "系统管理员";
                user.Pwd = pwd;
                db.Updateable(user);
                await this.ShowMessageAsync("温馨提示", "重置密码成功");
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("重置密码失败", "错误信息：" + ex.Message);
            }
        }

        private async void btnCreateAdmin_Click(object sender, RoutedEventArgs e)
        {
            var loginname = newLogin_name.Text.Trim();
            var pwd = newPwd.Text.Trim();
            // 创建系统管理员角色
            if (loginname.Length < 3)
            {
                await this.ShowMessageAsync("温馨提示", "请输入3个字符以上长度的用户名");
                return;
            }
            if (pwd.Length < 5)
            {
                await this.ShowMessageAsync("温馨提示", "密码长度应至少为5位数");
                return;
            }
            try
            {
                var db = new DbHelper.Users().Db;
                if (db.Queryable<Entity.TUser>().Where(u => u.Login_name == loginname).Count() > 0)
                {
                    await this.ShowMessageAsync("温馨提示", "该用户名已存在");
                    return;
                }
                // 检查用户名是否已存在
                pwd = DbHelper.Utils.hashpwd(pwd);
                var nickname = newNickname.Text.Trim().Length > 1 ? newNickname.Text.Trim() : loginname;
                var user = new Entity.TUser
                {
                    Login_name = loginname,
                    Pwd = pwd,
                    Role = 3,
                    Nickname = nickname,
                    Note = "使用桌面工具生成的系统角色"
                };
                db.Insertable(user).ExecuteCommand();
                await this.ShowMessageAsync("温馨提示", "创建系统角色成功");
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("重置密码失败", "错误信息：" + ex.Message);
            }
        }

        private void btnGenBaseInfo_Click(object sender, RoutedEventArgs e)
        {
            GenBaseInfo();
        }

        private async void GenBaseInfo()
        {
            // 构建基础数据信息
            if (dormName.Text.Trim().Length < 1)
            {
                await this.ShowMessageAsync("温馨提示", "请先输入园区名称");
                return;
            }
            if (buildingNum.Text.Trim().Length < 1)
            {
                await this.ShowMessageAsync("温馨提示", "请输入宿舍楼数量");
                return;
            }
            if (buildingFloorNum.Text.Trim().Length < 1)
            {
                await this.ShowMessageAsync("温馨提示", "请输入宿舍楼楼层数");
                return;
            }
            if (roomNumOneFloor.Text.Trim().Length < 1)
            {
                await this.ShowMessageAsync("温馨提示", "请输入每层宿舍楼房间数");
                return;
            }
            if (!buildingNameExpr.Text.TrimEnd().Contains("{0"))
            {
                await this.ShowMessageAsync("温馨提示", "宿舍楼名称表达式输入有误，请重新输入");
                return;
            }
            if (roomNameExpr.Text.TrimEnd().Split('{').Length < 2)
            {
                await this.ShowMessageAsync("温馨提示", "宿舍名称表达式输入有误，请重新输入");
                return;
            }
            if (!buildingVidExpr.Text.TrimEnd().Contains("{0"))
            {
                await this.ShowMessageAsync("温馨提示", "宿舍楼编号表达式输入有误，请重新输入");
                return;
            }
            if (roomNameExpr.Text.TrimEnd().Split('{').Length < 2)
            {
                await this.ShowMessageAsync("温馨提示", "宿舍编号表达式输入有误，请重新输入");
                return;
            }
            try
            {

                var bNum = Convert.ToInt32(buildingNum.Text.Trim());//宿舍楼数量
                if (bNum > 99)
                {
                    await this.ShowMessageAsync("温馨提示", "输入的宿舍楼数量不能大于99，请重新输入");
                    return;
                }
                var fNum = Convert.ToInt32(buildingFloorNum.Text.Trim());//输入宿舍楼楼层数
                if (fNum > 199)
                {
                    await this.ShowMessageAsync("温馨提示", "输入的宿舍楼楼层数不能大于199，请重新输入");
                    return;
                }
                var rNum = Convert.ToInt32(roomNumOneFloor.Text.Trim());//每层宿舍楼房间数
                if (rNum > 99)
                {
                    await this.ShowMessageAsync("温馨提示", "输入的每层宿舍房间数不能大于99，请重新输入");
                    return;
                }
                if (bNum * fNum * rNum > 50000 || bNum * fNum * rNum < 5)
                {
                    await this.ShowMessageAsync("温馨提示", "宿舍楼数量*宿舍楼楼层数*每层房间数 的值应介于5~50000之间");
                    return;
                }
                // 要求用户输入园区类型
                var dormType = await this.ShowInputAsync("请输入园区类型", "数字0为女，其他数字或1为男，输入非数字字符串则无效");
                if (dormType == null || dormType.Length < 1)
                    return;
                try
                {
                    BaseInfoGenerator.DormType = Convert.ToBoolean(Convert.ToInt16(dormType));
                    await this.ShowMessageAsync("温馨提示", "输入的园区类型为：" + (BaseInfoGenerator.DormType ? "男" : "女"));
                }
                catch (Exception ex)
                {
                    await this.ShowMessageAsync("温馨提示", "请输入园区类型时发生错误：" + ex.Message);
                    return;
                }
                GenData(bNum, fNum, rNum);
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("数值输入有误", "错误信息：" + ex.Message);
            }
        }

        /// <summary>
        /// 计算数据
        /// </summary>
        /// <param name="bNum"></param>
        /// <param name="fNum"></param>
        /// <param name="rNum"></param>
        private async void GenData(int bNum, int fNum, int rNum)
        {
            var controller = await this.ShowProgressAsync("温馨提示", "正在进行计算，请稍等", true);
            controller.SetProgress(0.1F);
            await Task.Delay(500);
            BaseInfoGenerator.SetParams(dormName.Text.Trim(), bNum, fNum, rNum, buildingNameExpr.Text.TrimEnd(),
                roomNameExpr.Text.TrimEnd(), buildingVidExpr.Text.TrimEnd(), roomVidExpr.Text.TrimEnd());
            controller.SetProgress(0.15F);
            await Task.Delay(100);
            Func<string> gen = BaseInfoGenerator.Gen;
            controller.SetProgress(0.25F);
            await Task.Delay(200);
            controller.SetMessage(gen());
            controller.SetProgress(0.85F);
            await Task.Delay(500);
            BaseInfoGenerator.b.Columns.Add("Count");// 添加统计行
            BaseInfoGenerator.b.Rows[0]["Count"] = BaseInfoGenerator.b.Rows.Count;
            dgBuilding.DataContext = BaseInfoGenerator.b;
            controller.SetProgress(0.95F);
            await Task.Delay(200);
            BaseInfoGenerator.r.Columns.Add("Count");// 添加统计行
            BaseInfoGenerator.r.Rows[0]["Count"] = BaseInfoGenerator.r.Rows.Count;
            dgRoom.DataContext = BaseInfoGenerator.r;
            controller.SetProgress(1F);
            await Task.Delay(1500);
            await controller.CloseAsync();
        }

        private async void btnResetInput_Click(object sender, RoutedEventArgs e)
        {
            // 重新设置生成参数
            MessageDialogResult result = await this.ShowMessageAsync("温馨提示", "是否重新设置参数？", MessageDialogStyle.AffirmativeAndNegative);
            if (!result.Equals(MessageDialogResult.Affirmative))
                return;
            resetTextBox();
            await this.ShowMessageAsync("温馨提示", "参数已重置");
        }

        /// <summary>
        /// 重新设置生成参数及表达式
        /// </summary>
        private void resetTextBox()
        {
            buildingNameExpr.Text = "第{0:d2}栋";
            TextBoxHelper.SetWatermark(buildingNameExpr, "第{0:d2}栋");

            roomNameExpr.Text = "第{0}栋{1:d2}楼{1}{2:d3}宿舍";
            TextBoxHelper.SetWatermark(roomNameExpr, "第{0}栋{1:d2}楼{1}{2:d3}宿舍");

            buildingVidExpr.Text = "A1D{0:d2}";
            TextBoxHelper.SetWatermark(buildingVidExpr, "A1D{0:d2}");

            roomVidExpr.Text = "A1D{0:d2}F{1:d2}R{1:d2}{2:d3}";
            TextBoxHelper.SetWatermark(roomVidExpr, "A1D{0:d2}F{1:d2}R{1:d2}{2:d3}");
        }

        private void MetroWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // 自动调整数据窗口
            if (Height / 2 - 280 < 50)
                return;
            dgBuilding.Height = dgBuilding.MaxHeight = (double)this.Height / 2 - 280;
            dgRoom.Height = dgRoom.MaxHeight = (double)this.Height / 2 - dgBuilding.Height;
        }

        private async void btnInsertBaseInfoToDatabase_Click(object sender, RoutedEventArgs e)
        {
            //将生成的数据插入数据库
            if (DBInfo.DB == null)
            {
                await this.ShowMessageAsync("温馨提示", "请先连接数据库");
                return;
            }
            // 判断数据量
            if (BaseInfoGenerator.b == null || BaseInfoGenerator.r == null)
            {
                await this.ShowMessageAsync("温馨提示", "请先生成数据");
                return;
            }
            try
            {
                // 判断园区类型
                /*MessageDialogResult result = await this.ShowMessageAsync("温馨提示", "当前的园区类型为 " + (BaseInfoGenerator.DormType ? "男" : "女") + " 是否正确？", MessageDialogStyle.AffirmativeAndNegative);
                if (!result.Equals(MessageDialogResult.Affirmative))
                {
                    var dormType = await this.ShowInputAsync("请重新输入园区类型", "数字0为女，其他数字或1为男，输入非数字字符串则无效");

                    BaseInfoGenerator.DormType = Convert.ToBoolean(Convert.ToInt16(dormType));
                    await this.ShowMessageAsync("温馨提示", "输入的园区类型为：" + (BaseInfoGenerator.DormType ? "男" : "女"));
                }*/
                var msg = "当前生成的数据统计：\r\n园区名称：{0}，\n园区类型：{1}，\n生成的宿舍楼数量：{2}，\n" +
                    "每层宿舍楼宿舍数量：{3}，\n生成的宿舍数量：{4}，\n宿舍楼名称示例：{5}，\n宿舍楼编号示例：{6}，\n" +
                    "宿舍名称示例：{7}，\n宿舍编号示例：{8}，\n\r以上信息是否正确？点击OK将直接执行插入数据操作";
                msg = String.Format(msg, BaseInfoGenerator.DormName, (BaseInfoGenerator.DormType ? "男" : "女"), BaseInfoGenerator.BuildingNum,
                    BaseInfoGenerator.RoomNumOneFloor, BaseInfoGenerator.r.Rows.Count, BaseInfoGenerator.b.Rows[0]["Nickname"],
                    BaseInfoGenerator.b.Rows[0]["Vid"], BaseInfoGenerator.r.Rows[0]["Nickname"], BaseInfoGenerator.r.Rows[0]["Vid"]); // 格式化提示信息

                var result = await this.ShowMessageAsync("温馨提示", msg, MessageDialogStyle.AffirmativeAndNegative);
                if (result == MessageDialogResult.Negative)
                {
                    return;
                }

                var t = DateTime.Now.Ticks;
                var controller = await this.ShowProgressAsync("请稍后", "正在向数据库添加数据", true, new MetroDialogSettings
                {
                    AnimateShow = true,
                });
                controller.SetIndeterminate(); // 显示动画效果
                controller.SetCancelable(true);
                await Task.Delay(2500); // 假装程序执行需要时间QAQ
                Func<string> info = BaseInfoGenerator.SaveDataToDatabase;
                var tip = info();
                controller.SetTitle("成功将数据添加到数据库");
                controller.SetMessage(tip); // 显示成功提示信息
                await Task.Delay(3500); // 让成功提示信息停留3.5S
                await controller.CloseAsync();
                await this.ShowMessageAsync("成功将数据添加到数据库", tip); // 再显示一个弹窗对话框以防用户错过提示信息
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("温馨提示", "将数据添加到数据库时发生错误：" + ex.Message);
                return;
            }

        }
    }
}
