using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DesktopTools
{
    public partial class FormAdminMgr : Form
    {
        public FormAdminMgr()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 系统角色信息列表
        /// </summary>
        private List<sdglsys.Entity.T_User> users = new List<sdglsys.Entity.T_User>();

        /// <summary>
        /// 已选中系统角色
        /// </summary>
        private sdglsys.Entity.T_User selectedUser = new sdglsys.Entity.T_User();

        /// <summary>
        /// 加载系统角色并添加到ComboBox
        /// </summary>
        private void LoadUser()
        {
            users = DbContext.Client.Queryable<sdglsys.Entity.T_User>().ToList();
            if (users != null && users.Count() > 0)
            {
                comboBox1.Items.Clear();
                foreach (var u in users)
                {
                    comboBox1.Items.Add(u.User_login_name);
                }
                comboBox1.SelectedIndex = 0;
            }
        }

        private void btnAddNewAdmin_Click(object sender, EventArgs e)
        {
            /// 检查输入
            if (string.IsNullOrWhiteSpace(tbxAddUserName.Text))
            {
                MessageBox.Show("请输入用户名");
                tbxAddUserName.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(tbxAddRealName.Text))
            {
                MessageBox.Show("请输入姓名");
                tbxAddRealName.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(tbxAddPwd.Text))
            {
                MessageBox.Show("请输入密码");
                tbxAddPwd.Focus();
                return;
            }
            if (tbxAddPwd.Text.Length < 6)
            {
                MessageBox.Show("请输入6位数以上长度的密码");
                tbxAddPwd.Focus();
                return;
            }
            /// 检查用户名
            var res = DbContext.Client.Queryable<sdglsys.Entity.T_User>().Where(x => x.User_model_state && x.User_login_name == tbxAddUserName.Text.Trim());
            if (res.Count() > 0)
            {
                MessageBox.Show("该用户名已存在");
                tbxAddUserName.Focus();
                return;
            }
            /// 生成系统角色信息
            var user = new sdglsys.Entity.T_User
            {
                User_login_name = tbxAddUserName.Text.Trim(),
                User_nickname = tbxAddRealName.Text.Trim(),
                User_role = 3,
                User_pwd = new sdglsys.Utils.Utils().HashPassword(tbxAddPwd.Text.Trim())
            };

            /// 保存到数据库
            try
            {
                if (DbContext.Client.Insertable(user).ExecuteCommand() > 0)
                {
                    MessageBox.Show("添加系统角色成功");
                }
                else
                {
                    MessageBox.Show("添加系统角色失败，发生未知错误");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnReloadUsers_Click(object sender, EventArgs e)
        {
            LoadUser();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBox1.Text.ToString()))
            {
                MessageBox.Show("当前未选择任何系统角色");
                comboBox1.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(tbxNewPwd.Text) || tbxNewPwd.Text.Trim().Length < 6)
            {
                MessageBox.Show("新密码不能少于6个字符长度");
                tbxNewPwd.Focus();
                return;
            }
            if (DialogResult.No == MessageBox.Show("是否重置系统角色 '" + comboBox1.Text.ToString() + "' 的密码为 '" + tbxNewPwd.Text.Trim() + "' ？", "温馨提示", MessageBoxButtons.YesNo))
            {
                return;
            }

            foreach (var user in users)
            {
                if (comboBox1.Text.Equals(user.User_login_name))
                {
                    try
                    {
                        /// 设置当前选择系统角色
                        user.User_pwd = new sdglsys.Utils.Utils().HashPassword(tbxNewPwd.Text.Trim());
                        if (DbContext.Client.Updateable(user).ExecuteCommand() > 0)
                        {
                            MessageBox.Show("重置密码成功");
                        }
                        else
                        {
                            MessageBox.Show("发生未知错误");
                        }
                        break;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            /// 显示系统角色姓名
            foreach (var user in users)
            {
                if (comboBox1.Text.Equals(user.User_login_name))
                {
                    tbxModRealName.Text = user.User_nickname;
                    break;
                }
            }
        }
    }
}