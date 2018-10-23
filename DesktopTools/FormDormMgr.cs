using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        SqlSugar.SqlSugarClient client;
        private void FormDormMgr_Load(object sender, EventArgs e)
        {
            var dt = client.Queryable<sdglsys.Entity.T_Dorm>().Where(x => x.Dorm_model_state).ToList();
            
            dataGridView1.DataSource = dt;
        }
    }
}
