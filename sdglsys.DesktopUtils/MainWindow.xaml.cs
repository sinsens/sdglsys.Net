using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
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
            ResetTextBox();
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

        private async void BtnSearchUser_Click(object sender, RoutedEventArgs e)
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
                var u = DBInfo.DB.Db.Queryable<Entity.T_User>().Where(q => q.Login_name == tbxLogin_name.Text).First();
                if (u == null)
                {
                    await this.ShowMessageAsync("温馨提示", "未找到用户名为'" + tbxLogin_name.Text + "'的系统用户。");
                }
                else
                {
                    var user = new Entity.VUser();
                    user.Pid = u.Pid;
                    user.Id = u.Id;
                    user.Nickname = u.Nickname;
                    user.Login_name = u.Login_name;
                    user.Is_active = u.Is_active;
                    user.Role = u.Role;
                    var dorm = DBInfo.DB.Db.Queryable<Entity.T_Dorm>().Where(x => x.Id == user.Id).First();
                    if (dorm != null)
                    { user.DormName = dorm.Nickname; }
                    user.RoleName = user.Role < 3 ? (user.Role < 2 ? "辅助登记员" : "宿舍管理员") : "系统管理员";
                    userinfo.DataContext = user;
                }
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("发生错误", "错误信息：" + ex.Message);
            }
        }

        private void BtnConnect_ClickAsync(object sender, RoutedEventArgs e)
        {
            Connect();
        }

        /// <summary>
        /// 连接到数据库
        /// </summary>
        private async void Connect()
        {
            if (tbxServer.Text.Trim().Length < 5)
            {
                await this.ShowMessageAsync("温馨提示", "请输入数据库服务器IP或域名");
                return;
            }
            if (tbxDBName.Text.Trim().Length < 1) {
                await this.ShowMessageAsync("温馨提示", "请输入数据库名称");
                return;
            }
            if (tbxDBUserName.Text.Trim().Length < 1)
            {
                await this.ShowMessageAsync("温馨提示", "请输入数据库用户名");
                return;
            }
            if (sltDBType.Text.ToString().Length < 2)
            {
                await this.ShowMessageAsync("温馨提示", "请选择数据库类型");
                return;
            }
            try
            {
                var ConnectString = "Server={0};Database={1};UID={2};Password={3};Allow User Variables=True;AllowZeroDateTime=True;ConvertZeroDateTime=True;SslMode=none";
                DBInfo.DB = new DbHelper.DbContext(String.Format(ConnectString, tbxServer.Text.Trim(), tbxDBName.Text.Trim(), tbxDBUserName.Text.Trim(), tbxDBPassword.Password), sltDBType.Text);
                DBInfo.DB.Db.Open();
                await this.ShowMessageAsync("温馨提示", "连接数据库成功");
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "发生错误");
                await this.ShowMessageAsync("连接数据库失败！", "错误信息：" + ex.Message);
            }
        }

        private async void BtnResetPwd_Click(object sender, RoutedEventArgs e)
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
                var user = DBInfo.DB.Db.Queryable<Entity.T_User>().Where(u => u.Login_name == tbxLogin_name.Text).First();
                user.Pwd = BCrypt.Net.BCrypt.HashPassword(GetMD5(newpwd.Trim()), 4);/// 先做md5，再做bcrypt加密
                DBInfo.DB.Db.Updateable(user).ExecuteCommand();
                await this.ShowMessageAsync("温馨提示", "重置密码成功");
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("重置密码失败", "错误信息：" + ex.Message);
            }
        }

        private async void BtnCreateAdmin_Click(object sender, RoutedEventArgs e)
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
                // 检查用户名是否已存在
                if (DBInfo.DB.Db.Queryable<Entity.T_User>().Where(u => u.Login_name == loginname).Count() > 0)
                {
                    await this.ShowMessageAsync("温馨提示", "该用户名已存在");
                    return;
                }
                /// 先做md5，再做bcrypt加密
                pwd = BCrypt.Net.BCrypt.HashPassword(GetMD5(pwd), 4);
                var nickname = newNickname.Text.Trim().Length > 1 ? newNickname.Text.Trim() : loginname;
                var user = new Entity.T_User
                {
                    Login_name = loginname,
                    Pwd = pwd,
                    Role = 3,
                    Nickname = nickname,
                    Note = "使用桌面工具生成的系统角色"
                };
                DBInfo.DB.Db.Insertable(user).ExecuteCommand();
                await this.ShowMessageAsync("温馨提示", "创建系统角色成功");
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("重置密码失败", "错误信息：" + ex.Message);
            }
        }

        private void BtnGenBaseInfo_Click(object sender, RoutedEventArgs e)
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

        private async void BtnResetInput_Click(object sender, RoutedEventArgs e)
        {
            // 重新设置生成参数
            MessageDialogResult result = await this.ShowMessageAsync("温馨提示", "是否重新设置参数？", MessageDialogStyle.AffirmativeAndNegative);
            if (!result.Equals(MessageDialogResult.Affirmative))
                return;
            ResetTextBox();
            await this.ShowMessageAsync("温馨提示", "参数已重置");
        }

        /// <summary>
        /// 重新设置生成参数及表达式
        /// </summary>
        private void ResetTextBox()
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
            dgBuilding.Height = dgBuilding.MaxHeight = (double) this.Height / 2 - 280;
            dgRoom.Height = dgRoom.MaxHeight = (double) this.Height / 2 - dgBuilding.Height;
        }

        private async void BtnInsertBaseInfoToDatabase_Click(object sender, RoutedEventArgs e)
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


        ///<summary>
        ///给一个字符串生成MD5 Hash
        ///</summary>
        ///<param name="strText">字符串</param>
        ///<returns>Md5 Hash</returns>
        public static string GetMD5(string strText)
        {
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] fromData = System.Text.Encoding.UTF8.GetBytes(strText);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");
            }

            return byte2String;
        }

        /// <summary>
        /// 备份数据库选项卡-点击浏览按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSelectFilepathToBackup_Click(object sender, RoutedEventArgs e)
        {
            var f = new Microsoft.Win32.SaveFileDialog();
            f.DefaultExt = ".sql";
            f.FileName = DateTime.Now.ToString("yyyy_MM_dd_HH-mm-ss-ffff");
            f.Title = "请选择保存备份数据库文件的位置";
            f.Filter = "SQL文件|*.sql";
            f.ValidateNames = true;
            f.CreatePrompt = true;
            f.ShowDialog();
            tbxFilepathToBackup.Text = f.FileName;
        }

        /// <summary>
        /// 备份数据库选项卡-点击立即备份数据库按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnBackupDatabase_Click(object sender, RoutedEventArgs e)
        {
            await this.ShowMessageAsync("温馨提示", "该功能暂未完善");
            return;
            /*
            if (DBInfo.DB == null)
            {
                await this.ShowMessageAsync("温馨提示", "请先连接数据库");
                return;
            }

            if (tbxFilepathToBackup.Text.Length < 2)
                await this.ShowMessageAsync("温馨提示", "请先点击“浏览”按钮选择保存备份文件位置");
            var result = await this.ShowMessageAsync("温馨提示", "进行备份时将暂停业务处理，请确保在无人使用系统的时候进行备份操作。\n是否立即将数据库备份到：\n\n"+tbxFilepathToBackup.Text, MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Negative)
            {
                return; // 取消了操作
            }

            var t = DateTime.Now.Ticks;
            var controller = await this.ShowProgressAsync("请稍后", "正在备份数据库。。。", true, new MetroDialogSettings
            {
                AnimateShow = true,
            });
            controller.SetIndeterminate(); // 显示动画效果
            controller.SetCancelable(true);

            try
            {
                if (DBInfo.BackupDataBase(tbxFilepathToBackup.Text)) {
                    await this.ShowMessageAsync("温馨提示", "备份成功");
                }
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("进行数据库备份时发生错误", ex.Message);
            }
            await controller.CloseAsync();*/
        }
    }
}
