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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            this.button3.Enabled = false;
        }
        private OracleCommand ocmd;  
        private OracleDataAdapter da;
        private DataSet ds;
        private OracleCommandBuilder cb;

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string sqlstr = "select FEATURE_ID,NAME,sdo_geometry.get_wkt(SHAPE)   from   SYSTEM.COLA_MARKETS";
                ocmd = new OracleCommand(sqlstr, Form1.conn);
                ocmd.CommandType = CommandType.Text;
                da = new OracleDataAdapter(ocmd);
                ds = new DataSet();
                da.Fill(ds);

                //加入自己操作数据库中数据的代码：
                this.dataGridView1.DataSource = ds.Tables[0];


                //ds.Dispose();
                //da.Dispose();
                //ocmd.Dispose();

                this.button3.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click_1(object sender, EventArgs e)
        {/*
            string block = " BEGIN " +
            $"insert into SYSTEM.COLA_MARKETS(NAME,SHAPE) VALUES (:name,:shape);" +
            "commit;" +
            " END; ";
            ocmd = new OracleCommand(block,Form1.conn);
            ocmd.CommandType = CommandType.Text;
            //插入名字
            OracleParameter name = new OracleParameter();
            name.Direction = ParameterDirection.Input;
            name.OracleDbType = OracleDbType.Varchar2;
            name.Value = "cola_a";
            ocmd.Parameters.Add(name);
            //插入空间数据
            OracleParameter shape = new OracleParameter();
            shape.Direction = ParameterDirection.Input;
            //shape.OracleDbTypeEx = OracleDbType.
            int x = ocmd.ExecuteNonQuery();
            if (x!=0)
            {
                MessageBox.Show($"插入成功");
            }*/
            int[] vs = { 3, 3, 4, 5, 6, 5, 6, 3, 3, 3 };
            insert("cola_b", vs);


        }

        private void button3_Click(object sender, EventArgs e)
        {
            cb = new OracleCommandBuilder(da);
            da.Update(ds.Tables[0]);

        }
        int insert(string NMAE,int[] SDO_ORDINATE_ARRAY)
        {
            string SDO_GTYPE = "2003";
            string SDO_SRID = "NULL";
            string SDO_POINT = "NULL";
            string SDO_ELEM_INFO_ARRAY = "1, 1003, 1";
            string SDO_ORDINATE_ARRAY_string = string.Join(",", SDO_ORDINATE_ARRAY);

            string sql = $@"INSERT into COLA_MARKETS(NAME,SHAPE) VALUES( 
                        '{NMAE}', 
                        MDSYS.SDO_GEOMETRY( 
                        {SDO_GTYPE}, 
                        {SDO_SRID}, 
                        {SDO_POINT}, 
                        MDSYS.SDO_ELEM_INFO_ARRAY({SDO_ELEM_INFO_ARRAY}), 
                        MDSYS.SDO_ORDINATE_ARRAY({SDO_ORDINATE_ARRAY_string})))";
            ocmd = new OracleCommand(sql, Form1.conn);
            ocmd.CommandType = CommandType.Text;
            int x = ocmd.ExecuteNonQuery();
            MessageBox.Show("插入成功");
            return x;

        }

    }
}
