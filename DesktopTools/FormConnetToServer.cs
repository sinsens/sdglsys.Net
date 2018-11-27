using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace DesktopTools
{
    public partial class FormConnetToServer : Form
    {
        internal static string DBName = "sdglsys";
        internal static string Server = "127.0.0.1";
        internal static string UserName;
        internal static string Pwd;
        internal static string DBType = "MySQL";
        private string title;

        public FormConnetToServer()
        {
            InitializeComponent();
            /// 初始化数据
            comboBox1.SelectedIndex = 0;
            title = Text;
            tbxDBName.Text = DBName;
            tbxPwd.Text = Pwd;
            tbxServer.Text = Server;
            tbxDBName.Text = DBName;
            tbxUserName.Text = UserName;
            comboBox1.SelectedIndex = comboBox1.Items.IndexOf(DBType);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbxDBName.Text) || string.IsNullOrWhiteSpace(tbxServer.Text) || string.IsNullOrWhiteSpace(tbxUserName.Text))
            {
                MessageBox.Show("输入不能为空！");
                return;
            }
            try
            {
                //var ConnectString = "Server={0};Database={1};UID={2};Password={3};Allow User Variables=True;AllowZeroDateTime=True;ConvertZeroDateTime=True;SslMode=none";
                var ConnectString = "Server={0};Database={1};UID={2};Password={3};";
                DbContext.Init(String.Format(ConnectString, tbxServer.Text.Trim(), tbxDBName.Text.Trim(), tbxUserName.Text.Trim(), tbxPwd.Text), comboBox1.Text);
                DbContext.Client.Open();
                MessageBox.Show("连接数据库成功");
                /// 保存输入数据
                Server = tbxServer.Text;
                DBName = tbxDBName.Text;
                Pwd = tbxPwd.Text;
                UserName = tbxUserName.Text;
                DBType = comboBox1.Text;
                Text = title + " - 已保存";
                Info info = new Info();
                info.DbInfo = new DbInfo();
                info.DbInfo.DbName = DBName;
                info.DbInfo.DbType = DBType;
                info.DbInfo.Server = Server;
                info.DbInfo.Username = UserName;
                info.DbInfo.Pwd = Pwd;
                var fs = new FileStream("info.dat", FileMode.Create);
                var bf = new BinaryFormatter();
                bf.Serialize(fs, info);
                bf = null;
                fs.Close();
                DialogResult = DialogResult.OK;
            }
            catch// (Exception ex)
            {
                throw;// ex;
                //MessageBox.Show(ex.Source + ex.Message);
            }
        }

        private void FormConnetToServer_Load(object sender, EventArgs e)
        {
        }
    }
}