using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WindowsFormsApp3
{
    public partial class Form3 : Form
    {
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataAdapter da;

        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dB1DataSet.Products' table. You can move, or remove it, as needed.
            this.productsTableAdapter.Fill(this.dB1DataSet.Products);
            checkBox2.Checked = true;
            checkBox2.BackColor = Color.Gray;
            checkBox2.Enabled = false;    
            checkBox2.ForeColor = Color.White;
            try
            {
                string sql = "Data Source= LAPTOP-7GNK1L4T\\SQLEXPRESS; Initial Catalog=DB1; Integrated Security=true";
                conn = new SqlConnection(sql);
                conn.Open();
                cmd = new SqlCommand("SELECT * FROM Products", conn);
                cmd.ExecuteNonQuery();
                da = new SqlDataAdapter(cmd);
                da.Fill(dB1DataSet, "Products");
                dataGridView1.DataSource = dB1DataSet.Tables["Products"];
                MessageBox.Show("Connected");
                dataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            this.Hide();
            Form4 form4 = new Form4();
            form4.Show();   
            form4.StartPosition = FormStartPosition.Manual;         
            form4.Location = this.Location;
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.Hide();
            Form2 form2 = new Form2();
            form2.Show();
            form2.StartPosition = FormStartPosition.Manual;         
            form2.Location = this.Location;
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

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                cmd = new SqlCommand($"INSERT INTO Products VALUES({textBox1.Text},'{textBox2.Text}',{textBox3.Text},{textBox4.Text},'{comboBox1.SelectedItem.ToString()}')", conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("SELECT * FROM Products", conn);
                cmd.ExecuteNonQuery();
                da = new SqlDataAdapter(cmd);
                da.Fill(dB1DataSet, "Products");
                dataGridView1.DataSource = dB1DataSet.Tables["Products"];
                dataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                cmd = new SqlCommand($"UPDATE Products SET ProdName = '{textBox2.Text}', ProdQty = {textBox3.Text}, ProdPrice = {textBox4.Text}, ProdCat= '{comboBox1.SelectedItem.ToString()}' WHERE ProdID = {textBox1.Text}", conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("SELECT * FROM Products", conn);
                cmd.ExecuteNonQuery();
                da = new SqlDataAdapter(cmd);
                da.Fill(dB1DataSet, "Products");
                dataGridView1.DataSource = dB1DataSet.Tables["Products"];
                dataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                cmd = new SqlCommand($"DELETE FROM Products WHERE ProdID = {textBox1.Text}", conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("SELECT * FROM Products", conn);
                cmd.ExecuteNonQuery();
                da = new SqlDataAdapter(cmd);
                da.Fill(dB1DataSet, "Products");
                dataGridView1.DataSource = dB1DataSet.Tables["Products"];
                dataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            DataGridViewRow selectedRow = dataGridView1.Rows[index];
            textBox1.Text = selectedRow.Cells[0].Value.ToString();
            textBox2.Text = selectedRow.Cells[1].Value.ToString();
            textBox3.Text = selectedRow.Cells[2].Value.ToString();
            textBox4.Text = selectedRow.Cells[3].Value.ToString();
            comboBox1.Text = selectedRow.Cells[4].Value.ToString();
        }
    }
}