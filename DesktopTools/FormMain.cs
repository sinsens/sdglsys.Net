using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using sdglsys.Entity;
using SqlSugar;

namespace DesktopTools
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            toolStripMenuItem1.Enabled = false;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            SetTip("请连接到数据库");
        }

        public void SetTip(string tip)
        {
            toolStripStatusLabel2.Text = DateTime.Now + " : " + tip;
        }

        private void mySQLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f = new FormConnetToServer();
            f.StartPosition = FormStartPosition.CenterParent;
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
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
        private TBuilding selectBuilding = new TBuilding();
        private List<VBuilding> vBuilding = new List<VBuilding>();
        /// <summary>
        /// 载入园区信息
        /// </summary>
        void LoadDorm()
        {
            try
            {
                listView1.Items.Clear();
                SetTip("已清除宿舍楼信息");
                vBuilding = DbContext.Client.Queryable<sdglsys.Entity.TBuilding, sdglsys.Entity.TDorm>((b, d) => new object[] { JoinType.Left, b.Pid == d.Id }).
                  Where((b, d) => b.Is_active == true).Select((b, d) => new VBuilding { Id = b.Id, Pid = b.Pid, Vid = b.Vid, Nickname = b.Nickname, Note = b.Note, PNickname = d.Nickname, Is_active = b.Is_active }).ToList();

                if (vBuilding == null || vBuilding.Count < 1)
                {
                    SetTip("请先添加宿舍楼信息");
                    return;
                }
                foreach (var b in vBuilding)
                {
                    listView1.Items.Add(b.Vid + "|" + b.PNickname + b.Nickname, b.Id);
                }
                SetTip("已刷新宿舍楼信息");
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void btnReloadDorm_Click(object sender, EventArgs e)
        {
            LoadDorm();
        }

        private void cbxRoomCode_CheckedChanged(object sender, EventArgs e)
        {
            tbxRoomCode.Enabled = cbxRoomCode.Checked;
        }

        private void btnGen_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(tbxPersonNum.Text))
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
            else if (selectBuilding == null || selectBuilding.Id < 1)
            {
                SetTip("请先选择宿舍楼");
            }
            else
            {
                GenRoomData();
            }
        }

        DataTable table;
        private void GenRoomData()
        {
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
                        row["园区ID"] = selectBuilding.Pid;
                        row["宿舍楼ID"] = selectBuilding.Id;
                        table.Rows.Add(row);
                    }
                }
                dataGridView1.DataSource = table;
                SetTip("生成成功，耗时：" + (DateTime.Now.Ticks - ticks) / 1000 + "ms");
            }
            catch (Exception ex)
            {
                SetTip(ex.Message);
            }
        }

        private void listView1_ItemActivate(object sender, EventArgs e)
        {
            if (vBuilding.Count > 0)
            {
                foreach (var item in vBuilding)
                {
                    if (item.Vid + "|" + item.PNickname + item.Nickname == listView1.SelectedItems[0].Text)
                    {
                        SetTip(listView1.SelectedItems[0].Text);
                        selectBuilding.Id = item.Id;
                        selectBuilding.Pid = item.Pid;
                        label4.Text = "已选择：" + item.Vid + "|" + item.PNickname + item.Nickname;
                        break;
                    }
                }
            }

        }

        private void btnSaveDataToDB_Click(object sender, EventArgs e)
        {
            if (table.Rows.Count < 1)
            {
                SetTip("请先生成数据");
                return;
            }
            DialogResult dialogResult = MessageBox.Show("是否保存到数据库？", "温馨提示", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                var ticks = DateTime.Now.Ticks;
                var Ado = DbContext.Client.Ado;
                /// 开始事务
                Ado.BeginTran();
                try
                {
                    short NumPerson = short.Parse(tbxPersonNum.Text.Trim()); // 宿舍人数

                    /// 保存基础数据信息到数据库
                    foreach (DataRow item in table.Rows)
                    {
                        Ado.Context.Insertable<TRoom>(new TRoom
                        {
                            Dorm_id = selectBuilding.Pid,
                            Pid = selectBuilding.Id,
                            Nickname = item["名称"].ToString(),
                            Vid = item["自编号"].ToString(),
                            Number = NumPerson,
                        }).ExecuteCommand();
                    }
                    Ado.CommitTran();
                    SetTip("成功保存到数据库，耗时："+ (DateTime.Now.Ticks - ticks) / 1000 + "ms");
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
        }

        FormAdminMgr formAdminMgr;
        private void AdminMgrToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (formAdminMgr == null || formAdminMgr.IsDisposed)
            {
                formAdminMgr = new FormAdminMgr();
                formAdminMgr.Show();
            }
            else {
                formAdminMgr.Activate();
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result =MessageBox.Show("是否退出本程序？", "温馨提示", MessageBoxButtons.YesNo);
            if (result == DialogResult.No) {
                e.Cancel = true;
            }
        }
    }
}
