using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 空间数据库
{
    public partial class SQL : Form
    {
        public SQL()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public string sql;
        private void button1_Click(object sender, EventArgs e)
        {
            sql = this.textBox1.Text;
            Main.run_sql(sql);
            this.Close();
        }
    }
}
