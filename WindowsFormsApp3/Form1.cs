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
    public partial class Form1 : Form
    {
        public static string SellerName;
        public static int SellerID;
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataAdapter da;
        SqlDataReader dr;
        DataSet ds = new DataSet();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string sql = "Data Source= LAPTOP-7GNK1L4T\\SQLEXPRESS; Initial Catalog = DB1; Integrated Security=true";
                conn = new SqlConnection(sql);
                conn.Open();
                cmd = new SqlCommand("SELECT * FROM Sellers", conn);
                cmd.ExecuteNonQuery();
                da = new SqlDataAdapter(cmd);
                da.Fill(ds, "Sellers");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            comboBox1.SelectedIndex = 0;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() == "Admin")
            {
                if (textBox1.Text == "admin" && textBox2.Text == "admin")
                {
                    Form2 form2 = new Form2();
                    this.Hide();
                    form2.Show();
                }
                else
                {
                    MessageBox.Show("Wrong username or password");
                }
            }else if (comboBox1.SelectedItem.ToString() == "Seller")
            {
                try
                {
                    cmd = new SqlCommand($"SELECT * FROM Sellers WHERE SellerName= '{textBox1.Text}' AND SellerPassword = '{textBox2.Text}'", conn);
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        SellerName = textBox1.Text;
                        while (dr.Read())
                        {
                            SellerID = (int)dr[0];
                        }
                        Form5 form5 = new Form5();
                        this.Hide();
                        form5.Show();
                    }
                    else
                    {
                        MessageBox.Show("Wrong username or password");
                    }
                    dr.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
            }
        }
    }
}