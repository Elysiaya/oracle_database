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
    public partial class select : Form
    {
        public select()
        {
            InitializeComponent();
        }
        public string table_name;
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text == null)
            {
                MessageBox.Show("请输入表名");
            }
            else
            {
                table_name = this.textBox1.Text;
                this.Close();
            }
        }
    }
}
