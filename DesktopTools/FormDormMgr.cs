using System;
using System.Windows.Forms;

namespace DesktopTools
{
    public partial class FormDormMgr : Form
    {
        public FormDormMgr()
        {
            InitializeComponent();
            client = DbContext.Client;
        }

        private SqlSugar.SqlSugarClient client;

        private void FormDormMgr_Load(object sender, EventArgs e)
        {
            MessageBox.Show("功能尚未完善！");
            return;
            //Reload();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /// 保存数据;
            Save();
        }

        private void Save()
        {
            if (dataGridView1.Rows.Count < 1)
            {
                if (MessageBox.Show("数据为空，是否保存？", "温馨提示") == DialogResult.No)
                    return;
            }
            try
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    var row = dataGridView1.Rows[i];
                    sdglsys.Entity.T_Dorm dorm = new sdglsys.Entity.T_Dorm
                    {
                        Dorm_id = int.Parse(row.Cells[0].Value.ToString()),
                        Dorm_nickname = row.Cells[1].Value.ToString(),
                        Dorm_type = row.Cells[2].Value.ToString().Equals("男"),
                        Dorm_is_active = (bool)row.Cells[3].Value
                    };
                    client.Insertable(dorm).ExecuteCommand();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Reload();
        }

        private void Reload()
        {
            var dt = client.Queryable<sdglsys.Entity.T_Dorm>().Where(x => x.Dorm_model_state).ToDataTable();
            dataGridView1.AutoGenerateColumns = false;
            dt.Columns.Add("sex", typeof(string));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["sex"] = dt.Rows[i][2].Equals(true) ? "男" : "女";
            }
            dataGridView1.DataSource = dt;
        }
    }
}