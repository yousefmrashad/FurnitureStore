using System;
using System.Data;
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
        private DataSet ds = new DataSet();

        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
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
                da.Fill(ds, "Products");
                dataGridView1.DataSource = ds.Tables["Products"];
                cmd = new SqlCommand("SELECT * FROM Categories", conn);
                cmd.ExecuteNonQuery();
                da = new SqlDataAdapter(cmd);
                da.Fill(ds, "Categories");
                foreach (DataRow dr in ds.Tables["Categories"].Rows)
                {
                    comboBox1.Items.Add(dr["CatName"].ToString());
                }
                dataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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

        //Sellers Button
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.Hide();
            Form2 form2 = new Form2();
            form2.Show();
            form2.StartPosition = FormStartPosition.Manual;         
            form2.Location = this.Location;
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

        //Add Button
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                cmd = new SqlCommand($"INSERT INTO Products VALUES({textBox1.Text},'{textBox2.Text}',{textBox3.Text},{textBox4.Text}, {comboBox1.SelectedIndex+1})", conn);
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
                cmd = new SqlCommand($"UPDATE Products SET ProdName = '{textBox2.Text}', ProdQty = {textBox3.Text}, ProdPrice = {textBox4.Text}, ProdCat= {comboBox1.SelectedIndex + 1} WHERE ProdID = {textBox1.Text}", conn);
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
                cmd = new SqlCommand($"DELETE FROM Products WHERE ProdID = {textBox1.Text}", conn);
                dml(cmd);
                clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void clear()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
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
                comboBox1.Text = comboBox1.Items[int.Parse(selectedRow.Cells[4].Value.ToString()) - 1].ToString();
            }
            catch
            {

            }
        }
        void dml(SqlCommand cmd)
        {
            cmd.ExecuteNonQuery();
            ds.Clear();
            cmd = new SqlCommand("SELECT * FROM Products", conn);
            cmd.ExecuteNonQuery();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds, "Products");
            dataGridView1.DataSource = ds.Tables["Products"];
            dataGridView1.Refresh();
        }
    }
}