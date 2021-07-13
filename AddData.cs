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
    public partial class AddData : Form
    {
        public AddData()
        {
            InitializeComponent();
            this.comboBox1.SelectedIndex = 0;
        }
        public  List<double> points = new List<double>();
        public  string name;
        public  string mode;
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string s = this.textBox3.Text + "," + this.textBox4.Text + "\r\n";
            this.textBox2.Text += s;
            try
            {
                points.Add(Convert.ToDouble(this.textBox3.Text));
                points.Add(Convert.ToDouble(this.textBox4.Text));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                
            }


            this.textBox3.Text = "";
            this.textBox4.Text = "";

            this.textBox3.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (name == "")
            {
                MessageBox.Show("未输入名称");
            }
            else
            {
                name = this.textBox1.Text;
                mode = this.comboBox1.SelectedItem.ToString();
                this.Hide();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
