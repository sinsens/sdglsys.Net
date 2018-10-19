namespace DesktopTools
{
    partial class FormAdminMgr
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnAddNewAdmin = new System.Windows.Forms.Button();
            this.tbxAddPwd = new System.Windows.Forms.TextBox();
            this.tbxAddRealName = new System.Windows.Forms.TextBox();
            this.tbxAddUserName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnReloadUsers = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbxNewPwd = new System.Windows.Forms.TextBox();
            this.tbxModRealName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.btnAddNewAdmin);
            this.groupBox1.Controls.Add(this.tbxAddPwd);
            this.groupBox1.Controls.Add(this.tbxAddRealName);
            this.groupBox1.Controls.Add(this.tbxAddUserName);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(287, 372);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "添加系统管理员";
            // 
            // btnAddNewAdmin
            // 
            this.btnAddNewAdmin.Location = new System.Drawing.Point(79, 165);
            this.btnAddNewAdmin.Name = "btnAddNewAdmin";
            this.btnAddNewAdmin.Size = new System.Drawing.Size(75, 23);
            this.btnAddNewAdmin.TabIndex = 4;
            this.btnAddNewAdmin.Text = "确定";
            this.btnAddNewAdmin.UseVisualStyleBackColor = true;
            this.btnAddNewAdmin.Click += new System.EventHandler(this.btnAddNewAdmin_Click);
            // 
            // tbxAddPwd
            // 
            this.tbxAddPwd.Location = new System.Drawing.Point(79, 119);
            this.tbxAddPwd.MaxLength = 15;
            this.tbxAddPwd.Name = "tbxAddPwd";
            this.tbxAddPwd.Size = new System.Drawing.Size(100, 21);
            this.tbxAddPwd.TabIndex = 4;
            // 
            // tbxAddRealName
            // 
            this.tbxAddRealName.Location = new System.Drawing.Point(79, 79);
            this.tbxAddRealName.MaxLength = 15;
            this.tbxAddRealName.Name = "tbxAddRealName";
            this.tbxAddRealName.Size = new System.Drawing.Size(100, 21);
            this.tbxAddRealName.TabIndex = 2;
            // 
            // tbxAddUserName
            // 
            this.tbxAddUserName.Location = new System.Drawing.Point(79, 35);
            this.tbxAddUserName.MaxLength = 15;
            this.tbxAddUserName.Name = "tbxAddUserName";
            this.tbxAddUserName.Size = new System.Drawing.Size(100, 21);
            this.tbxAddUserName.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 123);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "密码：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "姓名：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "用户名：";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnReloadUsers);
            this.groupBox2.Controls.Add(this.btnSave);
            this.groupBox2.Controls.Add(this.comboBox1);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.tbxNewPwd);
            this.groupBox2.Controls.Add(this.tbxModRealName);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(310, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(287, 372);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "重置系统角色密码";
            // 
            // btnReloadUsers
            // 
            this.btnReloadUsers.Location = new System.Drawing.Point(226, 35);
            this.btnReloadUsers.Name = "btnReloadUsers";
            this.btnReloadUsers.Size = new System.Drawing.Size(55, 23);
            this.btnReloadUsers.TabIndex = 14;
            this.btnReloadUsers.Text = "刷新";
            this.btnReloadUsers.UseVisualStyleBackColor = true;
            this.btnReloadUsers.Click += new System.EventHandler(this.btnReloadUsers_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(98, 165);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 13;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(98, 37);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 20);
            this.comboBox1.TabIndex = 10;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(38, 123);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "密码：";
            // 
            // tbxNewPwd
            // 
            this.tbxNewPwd.Location = new System.Drawing.Point(98, 119);
            this.tbxNewPwd.MaxLength = 15;
            this.tbxNewPwd.Name = "tbxNewPwd";
            this.tbxNewPwd.Size = new System.Drawing.Size(100, 21);
            this.tbxNewPwd.TabIndex = 12;
            // 
            // tbxModRealName
            // 
            this.tbxModRealName.Location = new System.Drawing.Point(98, 79);
            this.tbxModRealName.MaxLength = 15;
            this.tbxModRealName.Name = "tbxModRealName";
            this.tbxModRealName.Size = new System.Drawing.Size(100, 21);
            this.tbxModRealName.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(38, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "用户名：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(38, 82);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "姓名：";
            // 
            // FormAdminMgr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 396);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FormAdminMgr";
            this.Text = "系统角色管理";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox tbxAddPwd;
        private System.Windows.Forms.TextBox tbxAddRealName;
        private System.Windows.Forms.TextBox tbxAddUserName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnAddNewAdmin;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox tbxNewPwd;
        private System.Windows.Forms.TextBox tbxModRealName;
        private System.Windows.Forms.Button btnReloadUsers;
    }
}