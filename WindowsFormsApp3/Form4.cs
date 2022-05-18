using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace WindowsFormsApp3
{
    public partial class Form4 : Form
    {
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataAdapter da;

        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dB1DataSet.Categories' table. You can move, or remove it, as needed.
            this.categoriesTableAdapter.Fill(this.dB1DataSet.Categories);
            checkBox3.Checked = true;
            checkBox3.BackColor = Color.Gray;
            checkBox3.Enabled = false;
            checkBox3.ForeColor = Color.White;

            try
            {
                string sql = "Data Source= LAPTOP-7GNK1L4T\\SQLEXPRESS; Initial Catalog = DB1; Integrated Security=true";
                conn = new SqlConnection(sql);
                conn.Open();
                cmd = new SqlCommand("SELECT * FROM Categories", conn);
                cmd.ExecuteNonQuery();
                da = new SqlDataAdapter(cmd);
                da.Fill(dB1DataSet, "Categories");
                dataGridView1.DataSource = dB1DataSet.Tables["Categories"];
                MessageBox.Show("Connected");
                dataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            this.Hide();
            Form3 form3 = new Form3();
            form3.Show();
            form3.StartPosition = FormStartPosition.Manual;         
            form3.Location = this.Location;
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
                cmd = new SqlCommand($"INSERT INTO Categories VALUES({textBox1.Text},'{textBox2.Text}','{textBox3.Text}')", conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("SELECT * FROM Categories", conn);
                cmd.ExecuteNonQuery();
                da = new SqlDataAdapter(cmd);
                da.Fill(dB1DataSet, "Categories");
                dataGridView1.DataSource = dB1DataSet.Tables["Categories"];
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
                cmd = new SqlCommand($"UPDATE Categories SET CatName = '{textBox2.Text}', CatDesc = '{textBox3.Text}' WHERE CatID = {textBox1.Text}", conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("SELECT * FROM Categories", conn);
                cmd.ExecuteNonQuery();
                da = new SqlDataAdapter(cmd);
                da.Fill(dB1DataSet, "Categories");
                dataGridView1.DataSource = dB1DataSet.Tables["Categories"];
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
                cmd = new SqlCommand($"DELETE FROM Categories WHERE CatID = {textBox1.Text}", conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("SELECT * FROM Categories", conn);
                cmd.ExecuteNonQuery();
                da = new SqlDataAdapter(cmd);
                da.Fill(dB1DataSet, "Categories");
                dataGridView1.DataSource = dB1DataSet.Tables["Categories"];
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
        }
    }
}