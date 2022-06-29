using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WindowsFormsApp3
{
    public partial class Form2 : Form
    {
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataAdapter da;
        private DataSet ds = new DataSet();

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            checkBox1.Checked = true;
            checkBox1.BackColor = Color.Gray;
            checkBox1.Enabled = false;
            checkBox1.ForeColor = Color.White;

            try
            {
                string sql = "Data Source= LAPTOP-7GNK1L4T\\SQLEXPRESS; Initial Catalog = DB1; Integrated Security=true";
                conn = new SqlConnection(sql);
                conn.Open();
                cmd = new SqlCommand("SELECT * FROM SELLERS", conn);
                cmd.ExecuteNonQuery();
                da = new SqlDataAdapter(cmd);
                da.Fill(ds, "Sellers");
                dataGridView1.DataSource = ds.Tables["Sellers"];
                dataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //Logout Button
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 form1 = new Form1();
            form1.Show();
            form1.StartPosition = FormStartPosition.Manual;         
            form1.Location = this.Location;
        }

        //Exit Button
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //Products Button
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            this.Hide();
            Form3 form3 = new Form3();
            form3.Show();
            form3.StartPosition = FormStartPosition.Manual;         
            form3.Location = this.Location;
        }

        //Categories Button
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            this.Hide();
            Form4 form4 = new Form4();
            form4.Show();
            form4.StartPosition = FormStartPosition.Manual;         
            form4.Location = this.Location;
        }


        //Add Button
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                cmd = new SqlCommand($"INSERT INTO Sellers VALUES({textBox1.Text},'{textBox2.Text}',{textBox3.Text},{textBox4.Text},'{textBox5.Text}')", conn);
                dml(cmd);
                clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //Edit Button
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                cmd = new SqlCommand($"UPDATE Sellers SET SellerName = '{textBox2.Text}', SellerAge = {textBox3.Text}, SellerPhone = {textBox4.Text}, SellerPassword = '{textBox5.Text}' WHERE SellerID = {textBox1.Text}", conn);
                dml(cmd);
                clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //Delete Button
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                cmd = new SqlCommand($"DELETE FROM Sellers WHERE SellerID = {textBox1.Text}", conn);
                dml(cmd);
                clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int index = e.RowIndex;
                DataGridViewRow selectedRow = dataGridView1.Rows[index];
                textBox1.Text = selectedRow.Cells[0].Value.ToString();
                textBox2.Text = selectedRow.Cells[1].Value.ToString();
                textBox3.Text = selectedRow.Cells[2].Value.ToString();
                textBox4.Text = selectedRow.Cells[3].Value.ToString();
                textBox5.Text = selectedRow.Cells[4].Value.ToString();
            }
            catch
            {

            }
        }

        void clear()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
        }

        void dml(SqlCommand cmd)
        {
            cmd.ExecuteNonQuery();
            ds.Clear();
            cmd = new SqlCommand("SELECT * FROM Sellers", conn);
            cmd.ExecuteNonQuery();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds, "Sellers");
            dataGridView1.DataSource = ds.Tables["Sellers"];
            dataGridView1.Refresh();
        }
    }
}