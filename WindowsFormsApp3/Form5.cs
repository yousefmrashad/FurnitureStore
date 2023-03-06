﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing.Configuration;


namespace WindowsFormsApp3
{
    public partial class Form5 : Form
    {
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataAdapter da;
        private SqlDataReader dr;
        private DataSet ds = new DataSet();

        int ProdID;
        private double CurrentTotal = 0;
        int ReceiptNo = 0;

        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            try
            {
                string sql =
                    "Data Source= Yousef-Laptop\\SQLEXPRESS; Initial Catalog = DB1; Integrated Security=true";
                conn = new SqlConnection(sql);
                conn.Open();

                label2.Text = Form1.SellerName;
                label8.Text = DateTime.Now.ToShortDateString();
                label9.Text = "Total = 0 LE";

                refresh();

                ReceiptNo = dataGridView1.Rows.Count;
                textBox1.Text = ReceiptNo.ToString();
                comboBox1.SelectedIndex = 0;
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

        //Print Button
        private void button3_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            try
            {
                string doc = "";
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    doc += "Receipt ID: " + dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                    cmd = new SqlCommand(
                        $"SELECT SellerName FROM Sellers WHERE SellerID = {int.Parse(dataGridView1.SelectedRows[0].Cells[1].Value.ToString())}",
                        conn);
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        doc += "\n" + "Seller Name: " + dr[0].ToString();
                    }

                    dr.Close();
                    doc += "\n" + dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                    doc += "\n" + dataGridView1.SelectedRows[0].Cells[3].Value.ToString() + "LE";
                }

                e.Graphics.DrawString(doc, new Font("Times New Roman", 36, FontStyle.Bold), Brushes.Black,
                    new Point(25, 25));
            }
            catch
            {

            }
        }

        //Submit Button
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.Enabled = true;
                cmd = new SqlCommand(
                $"UPDATE Receipts SET ReceiptTotal = ReceiptTotal + {CurrentTotal} WHERE ReceiptID = {int.Parse(textBox1.Text)}",
                conn);
                cmd.ExecuteNonQuery();
                refresh();
                CurrentTotal = 0;
                label9.Text = $"Total is: {CurrentTotal} LE";
                clear();
            }
            catch
            {

            }
        }

        //Refresh Button
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedIndex >= 0)
                {
                    cmd = new SqlCommand($"SELECT * FROM Products WHERE ProdCat = {comboBox1.SelectedIndex + 1}", conn);
                    cmd.ExecuteNonQuery();
                    ds.Tables.Remove("Products");
                    da = new SqlDataAdapter(cmd);
                    da.Fill(ds, "Products");
                    dataGridView3.DataSource = ds.Tables["Products"];
                    dataGridView3.Refresh();
                }
                else
                {
                    cmd = new SqlCommand($"SELECT * FROM Products", conn);
                    cmd.ExecuteNonQuery();
                    ds.Tables.Remove("Products");
                    da = new SqlDataAdapter(cmd);
                    da.Fill(ds, "Products");
                    dataGridView3.DataSource = ds.Tables["Products"];
                    dataGridView3.Refresh();
                }
            }
            catch
            {

            }
        }

        //Add Button
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.Enabled = false;

                if (int.Parse(dataGridView3.Rows[ProdID - 1].Cells[2].Value.ToString()) - int.Parse(textBox3.Text) < 0)
                {
                    MessageBox.Show("Quantitiy is not suffecient");
                    clear();
                    return;
                }

                cmd = new SqlCommand(
                    $"INSERT INTO ReceiptDetails VALUES({int.Parse(textBox1.Text)}, {ProdID}, {int.Parse(textBox3.Text)})",
                    conn);
                cmd.ExecuteNonQuery();
                ds.Tables.Remove("ReceiptDetails");
                cmd = new SqlCommand(
                    $"SELECT ProdID, Qty FROM ReceiptDetails WHERE ReceiptID = {int.Parse(textBox1.Text)}", conn);
                cmd.ExecuteNonQuery();
                da = new SqlDataAdapter(cmd);
                da.Fill(ds, "ReceiptDetails");
                dataGridView2.DataSource = ds.Tables["ReceiptDetails"];
                dataGridView2.Refresh();

                cmd = new SqlCommand(
                    $"UPDATE Products SET ProdQty = ProdQty - {int.Parse(textBox3.Text)} WHERE ProdID = {ProdID}", conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand($"SELECT * FROM Products", conn);
                ds.Tables.Remove("Products");
                da = new SqlDataAdapter(cmd);
                da.Fill(ds, "Products");
                dataGridView3.DataSource = ds.Tables["Products"];
                dataGridView3.Refresh();
                CurrentTotal += int.Parse(textBox3.Text) * double.Parse(textBox4.Text);
                label9.Text = $"Total is: {CurrentTotal.ToString()} LE";
                clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show("Choose Product");
            }
        }

        //New Receipt Button
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                textBox1.Text = ReceiptNo.ToString();
                cmd = new SqlCommand(
                    $"INSERT INTO Receipts VALUES({ReceiptNo}, {Form1.SellerID}, CONVERT (DATE, GETDATE()), {0})", conn);
                cmd.ExecuteNonQuery();
                refresh();
                ReceiptNo++;
            }
            catch
            {

            }
        }

        //Reload Button
        private void button8_Click(object sender, EventArgs e)
        {
            refresh();
        }

        // Receipts GridView
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int index = e.RowIndex;
                DataGridViewRow selectedRow = dataGridView1.Rows[index];

                int i;
                i = int.Parse(selectedRow.Cells[0].Value.ToString());
                textBox1.Text = i.ToString();
                cmd = new SqlCommand($"SELECT ProdID, Qty FROM ReceiptDetails WHERE ReceiptID = {i}", conn);
                cmd.ExecuteNonQuery();
                ds.Tables.Remove("ReceiptDetails");
                da = new SqlDataAdapter(cmd);
                da.Fill(ds, "ReceiptDetails");
                dataGridView2.DataSource = ds.Tables["ReceiptDetails"];
                dataGridView2.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Products GridView
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

        void clear()
        {
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
        }

        void refresh()
        {
            try
            {
                ds.Clear();
                comboBox1.Items.Clear();

                cmd = new SqlCommand("SELECT * FROM Receipts", conn);
                cmd.ExecuteNonQuery();
                da = new SqlDataAdapter(cmd);
                da.Fill(ds, "Receipts");
                dataGridView1.DataSource = ds.Tables["Receipts"];
                dataGridView1.Refresh();

                cmd = new SqlCommand("SELECT * FROM Products", conn);
                cmd.ExecuteNonQuery();
                da = new SqlDataAdapter(cmd);
                da.Fill(ds, "Products");
                dataGridView3.DataSource = ds.Tables["Products"];
                dataGridView3.Refresh();

                cmd = new SqlCommand("SELECT * FROM ReceiptDetails", conn);
                cmd.ExecuteNonQuery();
                da = new SqlDataAdapter(cmd);
                da.Fill(ds, "ReceiptDetails");
                dataGridView2.DataSource = ds.Tables["ReceiptDetails"];
                dataGridView2.Refresh();

                cmd = new SqlCommand("SELECT * FROM Categories", conn);
                cmd.ExecuteNonQuery();
                da = new SqlDataAdapter(cmd);
                da.Fill(ds, "Categories");
                foreach (DataRow dr in ds.Tables["Categories"].Rows)
                {
                    comboBox1.Items.Add(dr["CatName"].ToString());
                }
            }
            catch
            {

            }
        }
    }
}
