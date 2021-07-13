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
using System.Text.RegularExpressions;

namespace 空间数据库
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();

        }
        private void Form2_Load(object sender, EventArgs e)
        {
            flash();
        }
        private OracleCommand ocmd;  
        private OracleDataAdapter da;
        private DataSet ds;
        private string table_name = "SYSTEM.COLA_MARKETS";
        /// <summary>
        /// 连接数据库获取数据，刷新数据框
        /// </summary>
        void flash(string table_name1= "SYSTEM.COLA_MARKETS")
        {
            this.label1.Text = "当前表：" + table_name1;
            try
            {

                string sqlstr;
                if (table_name1 == "SYSTEM.COLA_MARKETS")
                {
                    sqlstr = $"select FEATURE_ID,NAME,sdo_geometry.get_wkt(SHAPE) from {table_name}";
                    ocmd = new OracleCommand(sqlstr, Login.conn);
                    ocmd.CommandType = CommandType.Text;
                    da = new OracleDataAdapter(ocmd);
                    ds = new DataSet();
                    da.Fill(ds);

                    //加入自己操作数据库中数据的代码：
                    this.dataGridView1.DataSource = ds.Tables[0];

                    //画图
                    draw_init();
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        string name = (string)item.ItemArray[1];
                        string zuobiao = (string)item.ItemArray[2];
                        //虽然这里看起来很罗嗦，但是不能改，一改就崩
                        string i1 = zuobiao.Split('(')[2];
                        string i2 = i1.Split(')')[0];
                        string leixing = zuobiao.Split('(')[0];
                        List<double> vs = new List<double> { };
                        MatchCollection a = new Regex(@"\d*\.\d*").Matches(i2);

                        foreach (Match c in a)
                        {
                            string d = c.Value;
                            vs.Add(Convert.ToDouble(d));
                        }
                        DrawFeature(name, vs.ToArray(), leixing);
                    }
                }
                else
                {
                    sqlstr = $"select * from {table_name1}";
                    ocmd = new OracleCommand(sqlstr, Login.conn);
                    ocmd.CommandType = CommandType.Text;
                    da = new OracleDataAdapter(ocmd);
                    ds = new DataSet();
                    da.Fill(ds);

                    //加入自己操作数据库中数据的代码：
                    this.dataGridView1.DataSource = ds.Tables[0];
                }


                //ds.Dispose();
                //da.Dispose();
                //ocmd.Dispose();

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
        private void toolStripButton2_Click(object sender, EventArgs e)
        {/*#region
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


            //新建窗口
            AddData addData = new AddData();
            addData.ShowDialog();
            double[] vs = addData.points.ToArray();
            string name = addData.name;
            string mode = addData.mode;
            if (vs.Length!=0)
            {

                insert(name, vs,mode);
                flash();
            }

        }

        /// <summary>
        /// 插入数据 增
        /// </summary>
        /// <param name="NMAE"></param>
        /// <param name="SDO_ORDINATE_ARRAY"></param>
        /// <returns></returns>
        int insert(string NMAE,double[] SDO_ORDINATE_ARRAY,string mode)
        {
            string SDO_GTYPE = "2003";
            string SDO_SRID = "NULL";
            string SDO_POINT = "NULL";
            string SDO_ELEM_INFO_ARRAY = null ;
            if (mode == "多边形")
            {
                SDO_ELEM_INFO_ARRAY = "1, 1003, 1";
            }
            else if(mode == "圆")
            {
                SDO_ELEM_INFO_ARRAY = "1, 1003, 4";
            }
            string SDO_ORDINATE_ARRAY_string = string.Join(",", SDO_ORDINATE_ARRAY);

            string sql = $@"INSERT into COLA_MARKETS(NAME,SHAPE) VALUES( 
                        '{NMAE}', 
                        MDSYS.SDO_GEOMETRY( 
                        {SDO_GTYPE}, 
                        {SDO_SRID}, 
                        {SDO_POINT}, 
                        MDSYS.SDO_ELEM_INFO_ARRAY({SDO_ELEM_INFO_ARRAY}), 
                        MDSYS.SDO_ORDINATE_ARRAY({SDO_ORDINATE_ARRAY_string})))";
            ocmd = new OracleCommand(sql, Login.conn);
            ocmd.CommandType = CommandType.Text;
            int x = -1;
            try {x = ocmd.ExecuteNonQuery(); }
            catch {}

            MessageBox.Show("插入成功");
            return x;

        }
        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                OracleCommandBuilder cb = new OracleCommandBuilder(da);
                da.Update(ds);
                MessageBox.Show("保存成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            select select = new select();
            select.ShowDialog();
            
            string x = select.table_name;
            if (x != "")
            {
                flash(x);
            }
        }
        /// <summary>
        /// 初始化画布
        /// </summary>
        /// 

        Graphics g1;
        Font f;
        void draw_init()
        {
            g1 = this.pictureBox1.CreateGraphics();
            g1.Clear(Color.White);
            Pen p1 = new Pen(Color.Black, 2);//画笔

            Pen p2 = new Pen(Color.FromArgb(96, 96, 96), 1);
            f = new Font("宋体", 10);//字体
            //画线

            int a = pictureBox1.Size.Height;
            int b = pictureBox1.Size.Width;
            g1.DrawLine(p1, 0, a, 0, 0);//画线
            //g1.DrawLine(p1, 0, a, b, a);
            g1.DrawLine(p1, 0, 0, 5, 10);
            //g1.DrawLine(p1, b, a, b - 10, a - 5);
            //g1.DrawString("O", f, Brushes.Black, 3, a - 20);
            //g1.DrawString("正北X", f, Brushes.Black, 3, 3);
            //g1.DrawString("正东Y", f, Brushes.Black, b - 50, a - 20);

            //坐标网绘制
            for (int i = 0; i < 500; i+=25)
            {

                g1.DrawLine(p2, 0, i, 500, i);
                g1.DrawString((i / 25).ToString(), f, Brushes.Black, i, 500-13);
                g1.DrawLine(p2, i, 0, i, 500);
                g1.DrawString((i / 25).ToString(), f, Brushes.Black, 0, 500 - i);
            }


        }
        /// <summary>
        /// 画图
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="leixing"></param>
        Pen[] pens = new Pen[] { new Pen(Color.Red, 2), new Pen(Color.Blue, 2), new Pen(Color.Green, 2), new Pen(Color.Black, 2), new Pen(Color.DarkViolet, 2), new Pen(Color.DarkBlue, 2), new Pen(Color.DarkGreen, 2), new Pen(Color.DeepPink, 2), new Pen(Color.Orange, 2) };
        Brush[] brushes = new Brush[] { Brushes.Red, Brushes.Blue, Brushes.Green, Brushes.Black, Brushes.DarkViolet, Brushes.DarkBlue, Brushes.DarkGreen, Brushes.DeepPink, Brushes.Orange };
        int penindex = 0;
        void DrawFeature(string name, double[] data,string leixing)
        {

            List<PointF> points = new List<PointF> { };
            List<double> xlist = new List<double> { };
            List<double> ylist = new List<double> { };
            for (int i = 0; i < data.Length - 1; i += 2)
            {
                double x = data[i];
                xlist.Add(x);
                double y = data[i + 1];
                ylist.Add(y);
                points.Add(new PointF((float)x*25, 500-(float)y*25));
            }
            if (leixing == "POLYGON ")
            {
                g1.DrawLines(pens[penindex], points.ToArray());
                double[] center = { xlist.Min() * 25 + 5, 500 - ylist.Min() * 25 - 15 };
                g1.DrawString(name, f, brushes[penindex], new PointF((float)center[0], (float)center[1]));
            }
            else if (leixing == "CURVEPOLYGON ")
            {
                float xmin = (float)xlist.Min();
                float xmax = (float)xlist.Max();
                float ymin = (float)ylist.Min();
                float ymax = (float)ylist.Max();
                g1.DrawEllipse(pens[penindex], xmin*25, 500-ymax*25,(xmax-xmin)*25,(ymax-ymin)*25);
                g1.DrawString(name, f,brushes[penindex], new PointF(25*(xmin+xmax)/2,500-25*(ymin+ymax)/2));
            }
            if (penindex < pens.Length - 1)
            {
                penindex += 1; }
            else
            {
                penindex = 0;
            }
        }
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int run_sql(string sql)
        {
            try
            {
                OracleCommand ocmd = new OracleCommand(sql, Login.conn);
                int x = ocmd.ExecuteNonQuery();
                return x;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return -1;
            }

        }
        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            string sql = $"delete from {table_name}";
            var result= MessageBox.Show($"此功能将会删除{table_name}表中所有记录，如果想要删除单行或多行内容请选中该行单击delete按键删除点击保存按钮，是否依然删除全部数据？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                run_sql(sql);
            }
            flash(table_name);
        }
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            SQL x = new SQL();
            x.ShowDialog();

            string sql = x.sql;
            run_sql(sql);
        }

        private void 增加表中数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton2_Click(sender, e);
        }

        private void 删除表中数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton3_Click(sender, e);
        }

        private void 更新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton1_Click(sender, e);
        }

        private void 查询表中数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton4_Click(sender, e);
        }

        private void 创建表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQL sQL = new SQL();
            sQL.textBox1.Text = @"CREATE TABLE COLA_MARKETS (
                                    feature_id NUMBER PRIMARY KEY,
                                    name VARCHAR2(32),
                                    shape MDSYS.SDO_GEOMETRY); ";
            sQL.ShowDialog();
            run_sql(sQL.textBox1.Text);
        }

        private void 删除表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sql;
            select select = new select();
            select.Text = "删除表";
            select.label1.Text = "请输入需要删除的表名";
            select.ShowDialog();

            string table_name = select.table_name;
            sql = $"drop table {table_name}";
            run_sql(sql);
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            Query query = new Query();
            query.ShowDialog();


        }

        private void 添加元数据视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQL sQL = new SQL();
            sQL.textBox1.Text = @"insert into user_sdo_geom_metadata(TABLE_NAME,COLUMN_NAME,DIMINFO,SRID) 
                                        values(
                                        'COLA_MARKETS',
                                        'SHAPE',
                                        mdsys.sdo_dim_array(
                                        MDSYS.SDO_DIM_ELEMENT('X', 0, 100, 0.05),
                                        MDSYS.SDO_DIM_ELEMENT('Y', 0, 100, 0.05)),
                                        NULL--SRID
                                        ); ";
            sQL.ShowDialog();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            flash();
        }

    }
}
