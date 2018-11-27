using sdglsys.Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace DesktopTools
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            ShowMemory();
            StartPosition = FormStartPosition.CenterScreen;
            toolStripMenuItem1.Enabled = false;
            /*
                动态显示内存使用量
             */
            toolStripStatusLabel1.Text = "";
            Timer timer = new Timer
            {
                Interval = 1500
            };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private static string procName = Process.GetCurrentProcess().ProcessName;
        private PerformanceCounter pc = new PerformanceCounter("Process", "Private Bytes", procName);

        private void ShowMemory()
        {
            /*
             动态显示内存使用量
             */
            toolStripStatusLabel3.Text = "当前使用内存：" + (pc.NextValue() / 1024 / 1024).ToString() + "MB";
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            ShowMemory();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            if (File.Exists("info.dat"))
            {
                try
                {
                    using (var fs = new FileStream("info.dat", FileMode.Open))
                    {
                        var bf = new BinaryFormatter();
                        var info = bf.Deserialize(fs) as Info;
                        bf = null;
                        fs.Dispose();
                        GC.Collect();
                        if (info.DbInfo != null)
                        {
                            FormConnetToServer.DBName = info.DbInfo.DbName;
                            FormConnetToServer.DBType = info.DbInfo.DbType;
                            FormConnetToServer.UserName = info.DbInfo.Username;
                            FormConnetToServer.Server = info.DbInfo.Server;
                            FormConnetToServer.Pwd = info.DbInfo.Pwd;
                        }
                    }
                }
                catch
                {
                }
            }
            SetTip("请连接到数据库");
        }

        public void SetTip(string tip)
        {
            toolStripStatusLabel2.Text = DateTime.Now + " : " + tip;
        }

        private void MySQLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f = new FormConnetToServer
            {
                StartPosition = FormStartPosition.CenterParent
            };
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                var sql = DbContext.Client.Queryable<T_Used_total, T_Room, T_Building, T_Dorm>((u, r, b, d) => new object[] { JoinType.Left, u.Ut_room_id == r.Room_id, JoinType.Left, r.Room_building_id == b.Building_id, JoinType.Left, b.Building_dorm_id == d.Dorm_id }).OrderBy((u, r, b, d) => u.Ut_post_date, OrderByType.Desc).Where(u => u.Ut_model_state);
                var sqlstring = sql.Select((u, r, b, d) => new VUsed_total
                {
                    Id = u.Ut_id,
                    UT_Dorm_Nickname = d.Dorm_nickname,
                    UT_Building_Nickname = b.Building_nickname,
                    UT_Room_Nickname = r.Room_nickname,
                    UT_Note = u.Ut_note,
                    UT_Post_date = u.Ut_post_date,
                    UT_Cold_water_value = u.Ut_cold_water_value,
                    UT_Electric_value = u.Ut_electric_value,
                    UT_Hot_water_value = u.Ut_hot_water_value
                }).ToSql();
                //Clipboard.SetText(sqlstring.ToString());
                //MessageBox.Show(sqlstring.ToString());
                SetTip("已成功连接到数据库");
                groupBox1.Enabled = true;
                LoadDorm();
                SetTip("已加载园区信息");
                toolStripMenuItem1.Enabled = true;
            }
            else
            {
                groupBox1.Enabled = false;
                toolStripMenuItem1.Enabled = false;
            }
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("校园水电收费管理信息系统基础数据生成工具\n\n程序版本：" + Application.ProductVersion + "\n作者：Sinsen", "关于");
        }

        /// <summary>
        /// 选中的宿舍楼信息
        /// </summary>
        private T_Building selecT_Building = new T_Building();

        private List<VBuilding> vBuilding = new List<VBuilding>();

        /// <summary>
        /// 载入园区信息
        /// </summary>
        private void LoadDorm()
        {
            try
            {
                listView1.Items.Clear();
                SetTip("已清除宿舍楼信息");
                vBuilding = DbContext.Client.Queryable<sdglsys.Entity.T_Building, sdglsys.Entity.T_Dorm>((b, d) => new object[] { JoinType.Left, b.Building_dorm_id == d.Dorm_id }).
                  Where((b, d) => b.Building_is_active && b.Building_model_state).Select<VBuilding>().ToList();

                if (vBuilding == null || vBuilding.Count < 1)
                {
                    SetTip("请先添加宿舍楼信息");
                    return;
                }
                foreach (var b in vBuilding)
                {
                    listView1.Items.Add(b.Building_Vid + "|" + b.Building_Dorm_Nickname + b.Building_Nickname);
                }
                SetTip("已刷新宿舍楼信息");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void BtnReloadDorm_Click(object sender, EventArgs e)
        {
            LoadDorm();
        }

        private void CbxRoomCode_CheckedChanged(object sender, EventArgs e)
        {
            tbxRoomCode.Enabled = cbxRoomCode.Checked;
        }

        private void BtnGen_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbxPersonNum.Text))
            {
                tbxPersonNum.Text = "0";
            }
            if (string.IsNullOrWhiteSpace(tbxFloorNum.Text))
            {
                SetTip("请输入楼层数量！");
                tbxFloorNum.Focus();
            }
            else if (string.IsNullOrWhiteSpace(tbxRoomNumPerFloor.Text))
            {
                SetTip("请输入每层房间数量");
                tbxRoomNumPerFloor.Focus();
            }
            else if (string.IsNullOrWhiteSpace(tbxRoomNameFormat.Text))
            {
                SetTip("请输入宿舍名称格式");
                tbxRoomNameFormat.Focus();
            }
            else if (cbxRoomCode.Checked && string.IsNullOrWhiteSpace(tbxRoomCode.Text))
            {
                SetTip("请输入宿舍自编号格式文本");
                tbxRoomCode.Focus();
            }
            else if (selecT_Building == null || selecT_Building.Building_id < 1)
            {
                SetTip("请先选择宿舍楼");
            }
            else
            {
                GenRoomData();
            }
        }

        private DataTable table;

        private void GenRoomData()
        {
            SetTip("正在生成数据，请勿进行其他操作。。。");
            Enabled = false; // 禁止操作窗体控件
            var ticks = System.DateTime.Now.Ticks;
            table = new DataTable();
            table.Columns.Add("名称");
            table.Columns.Add("自编号");
            table.Columns.Add("园区ID");
            table.Columns.Add("宿舍楼ID");
            table.Columns.Add("人数");
            try
            {
                short NumFloor = short.Parse(tbxFloorNum.Text.Trim()); // 楼层数量
                short NumPerFloor = short.Parse(tbxRoomNumPerFloor.Text.Trim());// 每层房间数量
                short NumPerson = short.Parse(tbxPersonNum.Text.Trim()); // 宿舍人数
                string RoomCodeFormat = tbxRoomCode.Text.Trim(); // 宿舍自编号前缀
                string RoomNameFormat = tbxRoomNameFormat.Text.Trim(); // 宿舍名称格式

                /// 进入楼层数量循环
                for (int i = 1; i <= NumFloor; i++)
                {
                    /// 进入每层房间数量循环
                    for (int j = 1; j <= NumPerFloor; j++)
                    {
                        var row = table.NewRow();
                        row["名称"] = string.Format(i + RoomNameFormat, j);
                        row["自编号"] = cbxRoomCode.Checked ? string.Format(RoomCodeFormat, i, j) : "";
                        row["人数"] = NumPerson;
                        row["园区ID"] = selecT_Building.Building_dorm_id;
                        row["宿舍楼ID"] = selecT_Building.Building_id;
                        table.Rows.Add(row);
                    }
                }
                dataGridView1.DataSource = table;
                SetTip("生成成功，耗时：" + (DateTime.Now.Ticks - ticks) / 10000 + "ms");
            }
            catch (Exception ex)
            {
                SetTip(ex.Message);
            }
            GC.Collect(); // 进行垃圾回收
            Enabled = true; // 解除禁止
        }

        private void ListView1_ItemActivate(object sender, EventArgs e)
        {
            if (vBuilding.Count > 0)
            {
                foreach (var item in vBuilding)
                {
                    if (item.Building_Vid + "|" + item.Building_Dorm_Nickname + item.Building_Nickname == listView1.SelectedItems[0].Text)
                    {
                        SetTip(listView1.SelectedItems[0].Text);
                        selecT_Building.Building_id = item.Building_Id;
                        selecT_Building.Building_dorm_id = item.Building_Dorm_id;
                        label4.Text = "已选择：" + item.Building_Vid + "|" + item.Building_Dorm_Nickname + item.Building_Nickname;
                        /// 获取其他信息
                        label4.Text += "当前该宿舍楼共有宿舍数量：" + DbContext.Client.Queryable<T_Room>().Count(x => x.Room_building_id == item.Building_Id && x.Room_model_state);
                        break;
                    }
                }
            }
        }

        private void BtnSaveDataToDB_Click(object sender, EventArgs e)
        {
            if (table.Rows.Count < 1)
            {
                SetTip("请先生成数据");
                return;
            }

            DialogResult dialogResult = MessageBox.Show(this, "是否保存到数据库？", "温馨提示", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                SetTip("正在保存数据到数据库，请勿进行其他操作。。。");
                Enabled = false; // 禁止操作窗体控件
                var ticks = DateTime.Now.Ticks;
                var Ado = DbContext.Client.Ado;
                /// 开始事务
                Ado.BeginTran();
                try
                {
                    sbyte NumPerson = sbyte.Parse(tbxPersonNum.Text.Trim()); // 宿舍人数

                    /// 保存基础数据信息到数据库
                    foreach (DataRow item in table.Rows)
                    {
                        Ado.Context.Insertable<T_Room>(new T_Room
                        {
                            Room_dorm_id = selecT_Building.Building_dorm_id,
                            Room_building_id = selecT_Building.Building_id,
                            Room_nickname = item["名称"].ToString(),
                            Room_vid = item["自编号"].ToString(),
                            Number = NumPerson,
                        }).ExecuteCommand();
                    }
                    Ado.CommitTran();
                    SetTip("成功保存到数据库，耗时：" + (DateTime.Now.Ticks - ticks) / 1000 + "ms");
                }
                catch (Exception ex)
                {
                    Ado.RollbackTran();
                    SetTip("发生错误，已撤销提交。错误信息：" + ex.Message);
                }
            }
            else
            {
                SetTip("已取消保存");
            }
            Enabled = true; // 解除禁止
        }

        private void AdminMgrToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /// 打开系统角色管理窗口
            if (Application.OpenForms["FormAdminMgr"] == null || Application.OpenForms["FormAdminMgr"].IsDisposed)
            {
                new FormAdminMgr().Show();
            }
            else
            {
                Application.OpenForms["FormAdminMgr"].Activate();
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MessageBox.Show("是否退出本程序？", "温馨提示", MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void 园区信息管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /// 打开园区信息管理窗体
            if (Application.OpenForms["FormDormMgr"] == null || Application.OpenForms["FormDormMgr"].IsDisposed)
            {
                new FormDormMgr().Show();
            }
            else
            {
                Application.OpenForms["FormDormMgr"].Activate();
            }
        }
    }
}