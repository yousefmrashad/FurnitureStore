using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace WindowsFormsApp3
{
    public partial class Form5 : Form
    {
        Form1 form1 = new Form1();
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataAdapter da;
        private DataSet ds = new DataSet();

        int ProdID;

        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            try
            {
                string sql = "Data Source= LAPTOP-7GNK1L4T\\SQLEXPRESS; Initial Catalog = DB1; Integrated Security=true";
                conn = new SqlConnection(sql);
                conn.Open();

                label2.Text = form1.SellerName;
                label8.Text = DateTime.Now.ToShortDateString();
                label9.Text = "Total = 0 LE";

                cmd = new SqlCommand("SELECT * FROM Products", conn);
                cmd.ExecuteNonQuery();
                da = new SqlDataAdapter(cmd);
                da.Fill(ds, "Products");
                dataGridView3.DataSource = ds.Tables["Products"];
                dataGridView3.Refresh();

                cmd = new SqlCommand("SELECT * FROM Receipts", conn);
                cmd.ExecuteNonQuery();
                da = new SqlDataAdapter(cmd);
                da.Fill(ds, "Receipts");
                dataGridView1.DataSource = ds.Tables["Receipts"];
                dataGridView1.Refresh();

                cmd = new SqlCommand("SELECT * FROM ReceiptDetails", conn);
                cmd.ExecuteNonQuery();
                da = new SqlDataAdapter(cmd);
                da.Fill(ds, "ReceiptDetails");
                dataGridView2.DataSource = ds.Tables["ReceiptDetails"];
                dataGridView2.Refresh();

                MessageBox.Show("Connected");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 form1 = new Form1();
            form1.Show();
            form1.StartPosition = FormStartPosition.Manual;
            form1.Location = this.Location;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int index = e.RowIndex;
                DataGridViewRow selectedRow = dataGridView3.Rows[index];
                textBox2.Text = selectedRow.Cells[1].Value.ToString();
                textBox4.Text = selectedRow.Cells[3].Value.ToString();
                ProdID = int.Parse(selectedRow.Cells[0].Value.ToString());
            }
            catch
            {

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DataSet ds1 = new DataSet();
            cmd = new SqlCommand($"Select * FROM Products where CatID = {int.Parse(comboBox1.SelectedItem.ToString())}");
            cmd.ExecuteNonQuery();
            da.Fill(ds1, "Products");
            dataGridView3.DataSource = ds1.Tables["Products"]; 
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                //TODO:
                cmd = new SqlCommand($"INSERT INTO ReceiptDetails VALUES({textBox1.Text}, {ProdID}, {int.Parse(textBox3.Text)}", conn);
                cmd.ExecuteNonQuery();
                da.Dispose();
                cmd = new SqlCommand("SELECT * FROM ReceiptDetails", conn);
                cmd.ExecuteNonQuery();
                da = new SqlDataAdapter(cmd);
                da.Fill(ds, "ReceiptDetails");
                dataGridView2.DataSource = ds.Tables["ReceiptDetails"];
                dataGridView2.Refresh();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //TODO:
            label9.Text = $"Total is: {0}"; 
        }

        private void button3_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.ShowDialog();
        }

        private void printPreviewDialog1_Load(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            string doc = " ";
            if(dataGridView1.SelectedRows.Count > 0)
            {
                doc += dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                doc += "\n" + dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                doc += "\n" + dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                doc += "\n" + dataGridView1.SelectedRows[0].Cells[3].Value.ToString() + "LE";
            }
            e.Graphics.DrawString(doc, new Font("Times New Roman", 36, FontStyle.Bold), Brushes.Black, new Point(25, 25));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            cmd = new SqlCommand($"INSERT INTO Receipts VALUES({textBox1.Text}, {form1.SellerID}, {DateTime.Now.ToShortDateString()}", conn);
            cmd.ExecuteNonQuery();
            da.Dispose();
            cmd = new SqlCommand("SELECT * FROM Receipts", conn);
            cmd.ExecuteNonQuery();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds, "Receipts");
            dataGridView2.DataSource = ds.Tables["Receipts"];
            dataGridView2.Refresh();
        }
    }
}
