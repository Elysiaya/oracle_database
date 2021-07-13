using Oracle.ManagedDataAccess.Client;
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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.textBox1.Text = "system";
            this.textBox2.Text = "zhangxv000";
            this.textBox3.Text = "orcl";
            this.textBox4.Text = "localhost";

        }
        public static OracleConnection conn = new OracleConnection();
        public static string UserId;
        public static string Password;
        public static string DataSource;
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    UserId = this.textBox1.Text;
                    Password = this.textBox2.Text;
                    string hostname = this.textBox4.Text;
                    string m_datasource = this.textBox3.Text;
                    DataSource = "(DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = " + hostname + " )(PORT = 1521)))  (CONNECT_DATA = (SERVICE_NAME = " + m_datasource + ") ))";
                    conn.ConnectionString = "User Id=" + UserId + ";Password=" + Password + ";Data Source=" + DataSource + ";";

                    conn.Open();
                }
                //打开第二个窗口
                Main form2 = new Main();
                form2.Show();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
    }
}
