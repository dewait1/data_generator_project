using DB.SqlConn;
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

namespace DataGenerator
{
    public partial class Form1 : Form
    {

        string selectedState;
        public DataSet1 ds = new DataSet1();
       

        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OracleConnection conn = DBUtils.GetDBConnection();

            label1.Text = "Get Connection: " + conn;          
            try
            {
                conn.Open();

                label1.Text = "Successful Connection";
            }
            catch (Exception ex)
            {
                label1.Text = "## ERROR: " + ex.Message;            
                return;
            }

            label1.Text = "Połączenie jest OK";

            Console.Read();
        }

        private void button2_Click(object sender, EventArgs e)
        {           
            Generator generator = new Generator();
            label1.Text = "Trwa generacja...";
            generator.Generate(selectedState, (int) numericUpDown1.Value);
            UpdateDataGrid();
            label1.Text = "OK";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FileController fc = new FileController();
            string selectedDataSet = comboBox2.SelectedItem.ToString();
            fc.OpenFile(selectedDataSet);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Generator generator = new Generator();
            label1.Text = generator.Delete(selectedState);
            UpdateDataGrid();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedState = comboBox1.SelectedItem.ToString();
            UpdateDataGrid();
        }

        private void UpdateDataGrid()
        {
            OracleConnection connection = DBUtils.GetDBConnection();
            connection.Open();
            OracleCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM " + selectedState;
            cmd.ExecuteNonQuery();
            OracleDataAdapter oda = new OracleDataAdapter(cmd);
            dataGridView1.DataSource = null;
            ds.Clear();
            dataGridView1.Update();
            dataGridView1.Refresh();
            oda.Fill(ds, selectedState);
            dataGridView1.DataSource = ds.Tables[selectedState].DefaultView;
        }

    }
}
