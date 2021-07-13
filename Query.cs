using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
namespace 空间数据库
{
    public partial class Query : Form
    {
        public Query()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sql = @"select sdo_geometry.get_wkt(SDO_GEOM.SDO_INTERSECTION(c_a.SHAPE,c_c.SHAPE,0.005))
                        from COLA_MARKETS c_a,COLA_MARKETS c_c
                        where c_a.NAME = 'cola_a' and c_c.NAME = 'cola_c'";
            OracleCommand ocmd = new OracleCommand(sql, Login.conn);
            this.textBox1.Text = (string)ocmd.ExecuteScalar();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string sql = @"select SDO_GEOM.RELATE(c_b.SHAPE,'anyinteract',c_d.SHAPE,0.005)
                        from COLA_MARKETS c_b,COLA_MARKETS c_d
                        where c_b.NAME = 'cola_b' and c_d.NAME = 'cola_d'";
            OracleCommand ocmd = new OracleCommand(sql, Login.conn);
            this.textBox2.Text = (string)ocmd.ExecuteScalar();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string sql = @"select NAME,SDO_GEOM.SDO_AREA(SHAPE,0.005)
                          from COLA_MARKETS";
            OracleCommand ocmd = new OracleCommand(sql, Login.conn);

            OracleDataAdapter da = new OracleDataAdapter(ocmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            this.dataGridView1.DataSource = ds.Tables[0];

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string sql = @"select SDO_GEOM.SDO_DISTANCE(c_b.shape,c_d.shape,0.005)
                        from COLA_MARKETS c_b,COLA_MARKETS c_d
                        where c_b.NAME = 'cola_b' and c_d.NAME = 'cola_d'";
            OracleCommand ocmd = new OracleCommand(sql, Login.conn);
            this.textBox3.Text = ocmd.ExecuteScalar().ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string sql = @"select NAME,SDO_GEOM.validate_geometry_with_context(shape,0.005)
                          from COLA_MARKETS";
            OracleCommand ocmd = new OracleCommand(sql, Login.conn);

            OracleDataAdapter da = new OracleDataAdapter(ocmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            this.dataGridView2.DataSource = ds.Tables[0];
        }
    }
}
