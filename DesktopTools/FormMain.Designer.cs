namespace DesktopTools
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.连接ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mySQLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.AdminMgrToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.tbxRoomCode = new System.Windows.Forms.TextBox();
            this.tbxRoomNumPerFloor = new System.Windows.Forms.TextBox();
            this.tbxFloorNum = new System.Windows.Forms.TextBox();
            this.tbxPersonNum = new System.Windows.Forms.TextBox();
            this.tbxRoomNameFormat = new System.Windows.Forms.TextBox();
            this.cbxRoomCode = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.btnSaveDataToDB = new System.Windows.Forms.Button();
            this.btnGen = new System.Windows.Forms.Button();
            this.btnReloadDorm = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.园区信息管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.宿舍楼信息管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.连接ToolStripMenuItem,
            this.toolStripMenuItem1,
            this.关于ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 连接ToolStripMenuItem
            // 
            this.连接ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mySQLToolStripMenuItem});
            this.连接ToolStripMenuItem.Name = "连接ToolStripMenuItem";
            this.连接ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.连接ToolStripMenuItem.Text = "连接";
            // 
            // mySQLToolStripMenuItem
            // 
            this.mySQLToolStripMenuItem.Name = "mySQLToolStripMenuItem";
            this.mySQLToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.mySQLToolStripMenuItem.Text = "连接到数据库";
            this.mySQLToolStripMenuItem.Click += new System.EventHandler(this.mySQLToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AdminMgrToolStripMenuItem,
            this.园区信息管理ToolStripMenuItem,
            this.宿舍楼信息管理ToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(44, 21);
            this.toolStripMenuItem1.Text = "工具";
            // 
            // AdminMgrToolStripMenuItem
            // 
            this.AdminMgrToolStripMenuItem.Name = "AdminMgrToolStripMenuItem";
            this.AdminMgrToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.AdminMgrToolStripMenuItem.Text = "系统角色管理";
            this.AdminMgrToolStripMenuItem.Click += new System.EventHandler(this.AdminMgrToolStripMenuItem_Click);
            // 
            // 关于ToolStripMenuItem
            // 
            this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            this.关于ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.关于ToolStripMenuItem.Text = "关于";
            this.关于ToolStripMenuItem.Click += new System.EventHandler(this.关于ToolStripMenuItem_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.tbxRoomCode);
            this.groupBox1.Controls.Add(this.tbxRoomNumPerFloor);
            this.groupBox1.Controls.Add(this.tbxFloorNum);
            this.groupBox1.Controls.Add(this.tbxPersonNum);
            this.groupBox1.Controls.Add(this.tbxRoomNameFormat);
            this.groupBox1.Controls.Add(this.cbxRoomCode);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.listView1);
            this.groupBox1.Controls.Add(this.btnSaveDataToDB);
            this.groupBox1.Controls.Add(this.btnGen);
            this.groupBox1.Controls.Add(this.btnReloadDorm);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(12, 28);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(776, 256);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设置";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(97, 234);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 22;
            this.label4.Text = "已选择：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(512, 141);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(219, 11);
            this.label3.TabIndex = 21;
            this.label3.Text = "如：{0:d2}宿舍，生成：101,102,201,202";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(549, 177);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(191, 11);
            this.label2.TabIndex = 21;
            this.label2.Text = "如：A6B8{0}{1:d2}，生成：A6B8101";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("宋体", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(127, 214);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(104, 11);
            this.label12.TabIndex = 21;
            this.label12.Text = "只显示启用的宿舍楼";
            // 
            // tbxRoomCode
            // 
            this.tbxRoomCode.Enabled = false;
            this.tbxRoomCode.Location = new System.Drawing.Point(431, 173);
            this.tbxRoomCode.MaxLength = 20;
            this.tbxRoomCode.Name = "tbxRoomCode";
            this.tbxRoomCode.Size = new System.Drawing.Size(100, 21);
            this.tbxRoomCode.TabIndex = 17;
            this.tbxRoomCode.Text = "A6B8{0}{1:d2}";
            this.toolTip1.SetToolTip(this.tbxRoomCode, "宿舍自编号，用来快速查找的宿舍代号，不唯一,默认为空");
            // 
            // tbxRoomNumPerFloor
            // 
            this.tbxRoomNumPerFloor.Location = new System.Drawing.Point(383, 100);
            this.tbxRoomNumPerFloor.MaxLength = 3;
            this.tbxRoomNumPerFloor.Name = "tbxRoomNumPerFloor";
            this.tbxRoomNumPerFloor.Size = new System.Drawing.Size(100, 21);
            this.tbxRoomNumPerFloor.TabIndex = 14;
            this.tbxRoomNumPerFloor.Text = "20";
            this.toolTip1.SetToolTip(this.tbxRoomNumPerFloor, "该宿舍楼每层宿舍房间数量");
            // 
            // tbxFloorNum
            // 
            this.tbxFloorNum.Location = new System.Drawing.Point(383, 60);
            this.tbxFloorNum.MaxLength = 3;
            this.tbxFloorNum.Name = "tbxFloorNum";
            this.tbxFloorNum.Size = new System.Drawing.Size(100, 21);
            this.tbxFloorNum.TabIndex = 10;
            this.tbxFloorNum.Text = "3";
            this.toolTip1.SetToolTip(this.tbxFloorNum, "该宿舍楼一共有几层");
            // 
            // tbxPersonNum
            // 
            this.tbxPersonNum.Location = new System.Drawing.Point(595, 59);
            this.tbxPersonNum.MaxLength = 2;
            this.tbxPersonNum.Name = "tbxPersonNum";
            this.tbxPersonNum.Size = new System.Drawing.Size(100, 21);
            this.tbxPersonNum.TabIndex = 13;
            this.toolTip1.SetToolTip(this.tbxPersonNum, "每个宿舍住的人数，默认为0");
            // 
            // tbxRoomNameFormat
            // 
            this.tbxRoomNameFormat.Location = new System.Drawing.Point(395, 137);
            this.tbxRoomNameFormat.MaxLength = 20;
            this.tbxRoomNameFormat.Name = "tbxRoomNameFormat";
            this.tbxRoomNameFormat.Size = new System.Drawing.Size(100, 21);
            this.tbxRoomNameFormat.TabIndex = 15;
            this.tbxRoomNameFormat.Text = "{0:d2}宿舍";
            this.toolTip1.SetToolTip(this.tbxRoomNameFormat, "楼层数+房间序号的宿舍名称，如\"{0:d2}宿舍\"，则生成\"101,102,201,202");
            // 
            // cbxRoomCode
            // 
            this.cbxRoomCode.AutoSize = true;
            this.cbxRoomCode.Location = new System.Drawing.Point(305, 175);
            this.cbxRoomCode.Name = "cbxRoomCode";
            this.cbxRoomCode.Size = new System.Drawing.Size(120, 16);
            this.cbxRoomCode.TabIndex = 16;
            this.cbxRoomCode.Text = "宿舍自编号格式：";
            this.cbxRoomCode.UseVisualStyleBackColor = true;
            this.cbxRoomCode.CheckedChanged += new System.EventHandler(this.cbxRoomCode_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(300, 140);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 12);
            this.label7.TabIndex = 4;
            this.label7.Text = "宿舍名称格式：";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(524, 63);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 4;
            this.label9.Text = "宿舍人数：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(300, 103);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 12);
            this.label8.TabIndex = 4;
            this.label8.Text = "每层房间数：";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(300, 63);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 12);
            this.label5.TabIndex = 4;
            this.label5.Text = "楼层数：";
            // 
            // listView1
            // 
            this.listView1.Location = new System.Drawing.Point(83, 52);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.ShowGroups = false;
            this.listView1.Size = new System.Drawing.Size(196, 148);
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            this.listView1.ItemActivate += new System.EventHandler(this.listView1_ItemActivate);
            // 
            // btnSaveDataToDB
            // 
            this.btnSaveDataToDB.Location = new System.Drawing.Point(431, 208);
            this.btnSaveDataToDB.Name = "btnSaveDataToDB";
            this.btnSaveDataToDB.Size = new System.Drawing.Size(100, 23);
            this.btnSaveDataToDB.TabIndex = 20;
            this.btnSaveDataToDB.Text = "保存到数据库";
            this.btnSaveDataToDB.UseVisualStyleBackColor = true;
            this.btnSaveDataToDB.Click += new System.EventHandler(this.btnSaveDataToDB_Click);
            // 
            // btnGen
            // 
            this.btnGen.Location = new System.Drawing.Point(305, 208);
            this.btnGen.Name = "btnGen";
            this.btnGen.Size = new System.Drawing.Size(100, 23);
            this.btnGen.TabIndex = 20;
            this.btnGen.Text = "开始生成";
            this.btnGen.UseVisualStyleBackColor = true;
            this.btnGen.Click += new System.EventHandler(this.btnGen_Click);
            // 
            // btnReloadDorm
            // 
            this.btnReloadDorm.Location = new System.Drawing.Point(131, 20);
            this.btnReloadDorm.Name = "btnReloadDorm";
            this.btnReloadDorm.Size = new System.Drawing.Size(100, 23);
            this.btnReloadDorm.TabIndex = 2;
            this.btnReloadDorm.Text = "刷新宿舍楼信息";
            this.btnReloadDorm.UseVisualStyleBackColor = true;
            this.btnReloadDorm.Click += new System.EventHandler(this.btnReloadDorm_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 102);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "选择宿舍楼：";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 488);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(131, 17);
            this.toolStripStatusLabel2.Text = "toolStripStatusLabel2";
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 200;
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.InitialDelay = 200;
            this.toolTip1.ReshowDelay = 40;
            this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 290);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(776, 195);
            this.dataGridView1.TabIndex = 3;
            // 
            // 园区信息管理ToolStripMenuItem
            // 
            this.园区信息管理ToolStripMenuItem.Name = "园区信息管理ToolStripMenuItem";
            this.园区信息管理ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.园区信息管理ToolStripMenuItem.Text = "园区信息管理";
            // 
            // 宿舍楼信息管理ToolStripMenuItem
            // 
            this.宿舍楼信息管理ToolStripMenuItem.Name = "宿舍楼信息管理ToolStripMenuItem";
            this.宿舍楼信息管理ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.宿舍楼信息管理ToolStripMenuItem.Text = "宿舍楼信息管理";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 510);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMain";
            this.Text = "校园水电管理系统 - 基础数据管理工具";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 连接ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mySQLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于ToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnReloadDorm;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox cbxRoomCode;
        private System.Windows.Forms.TextBox tbxRoomCode;
        private System.Windows.Forms.TextBox tbxRoomNumPerFloor;
        private System.Windows.Forms.TextBox tbxFloorNum;
        private System.Windows.Forms.TextBox tbxPersonNum;
        private System.Windows.Forms.TextBox tbxRoomNameFormat;
        private System.Windows.Forms.Button btnGen;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSaveDataToDB;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem AdminMgrToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 园区信息管理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 宿舍楼信息管理ToolStripMenuItem;
    }
}

