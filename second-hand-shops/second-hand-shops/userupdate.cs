using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace second_hand_shops
{
    public partial class userupdate : Form
    {
        private MySqlConnection databaseConnection()
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=shop;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            return conn;
        }

        private string _username;

        public userupdate(string itemID, string username)
        {
            InitializeComponent(); 

            lblItemID.Text = itemID; 
            _username = username; 
            uchange.Text = username; 

            this.Load += new EventHandler(showinfo); 
        }


        private void showinfo(object sender, EventArgs e)
        {
            using (MySqlConnection conn = databaseConnection()) 
            {
                try
                {
                    conn.Open(); 

                    MySqlCommand command = new MySqlCommand("SELECT name, price, info, amount, pic, pic2, pic3, tag, status FROM item WHERE itemid = @itemid", conn);
                    command.Parameters.AddWithValue("@itemid", lblItemID.Text); 

                    using (MySqlDataReader reader = command.ExecuteReader()) 
                    {
                        if (reader.Read()) 
                        {
                            lbname.Text = reader.GetString("name");
                            lbprice.Text = reader.GetDecimal("price").ToString();
                            lbinfo.Text = reader.GetString("info");
                            lbamount.Text = reader.GetInt32("amount").ToString();
                            lbtag.Text = reader.GetString("tag");
                            lbstatus.Text = reader.GetString("status");

                            cname.Text = lbname.Text;
                            cprice.Text = lbprice.Text;
                            cinfo.Text = lbinfo.Text;
                            camount.Text = lbamount.Text;
                            ctag.Text = lbtag.Text;

                            byte[] itemPic1 = reader["pic"] as byte[];
                            byte[] itemPic2 = reader["pic2"] as byte[];
                            byte[] itemPic3 = reader["pic3"] as byte[];

                            if (itemPic1 != null)
                            {
                                pictureBox1.Image = ByteArrayToImage(itemPic1);
                            }
                            if (itemPic2 != null)
                            {
                                pictureBox2.Image = ByteArrayToImage(itemPic2);
                            }
                            if (itemPic3 != null)
                            {
                                pictureBox3.Image = ByteArrayToImage(itemPic3);
                            }
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error connecting to the database: " + ex.Message); 
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }


        private Image ByteArrayToImage(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length == 0)
            {
                return null;
            }

            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                try
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    return Image.FromStream(ms);
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show("Error creating image: " + ex.Message);
                    return null;
                }
            }
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox; 
            if (pictureBox != null) 
            {
                OpenFileDialog openFileDialog = new OpenFileDialog(); 
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"; 

                if (openFileDialog.ShowDialog() == DialogResult.OK) 
                {
                    pictureBox.ImageLocation = openFileDialog.FileName; 
                    pictureBox.Tag = openFileDialog.FileName; 
                }
            }
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            pictureBox_Click(sender, e);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            pictureBox_Click(sender, e);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            pictureBox_Click(sender, e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
        }



        private void button3_Click(object sender, EventArgs e)
        {
           
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cname.Text) || string.IsNullOrWhiteSpace(cprice.Text) ||
               string.IsNullOrWhiteSpace(cinfo.Text) || string.IsNullOrWhiteSpace(camount.Text) ||
               string.IsNullOrWhiteSpace(ctag.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(cprice.Text, "^[0-9]*$") ||
                !System.Text.RegularExpressions.Regex.IsMatch(camount.Text, "^[0-9]*$"))
            {
                MessageBox.Show("Please enter only numerical digits.");
                return;
            }

            if (!int.TryParse(camount.Text, out int camountValue))
            {
                MessageBox.Show("Please enter a valid number for amount.");
                return;
            }

            if (camountValue < 0)
            {
                MessageBox.Show("Amount cannot be negative.");
                return;
            }

            DialogResult result = MessageBox.Show("Confirm Your Info", "Message", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                using (MySqlConnection conn = databaseConnection())
                {
                    conn.Open();
                    MySqlCommand cmd;
                    if (camountValue == 0)
                    {
                        cmd = new MySqlCommand("DELETE FROM item WHERE itemid = @ItemID AND status != 'OUT'", conn);
                        cmd.Parameters.AddWithValue("@ItemID", lblItemID.Text);
                    }
                    else 
                    {
                        StringBuilder queryBuilder = new StringBuilder("UPDATE item SET name = @Name, price = @Price, info = @Info, amount = @Amount, tag = @Tag, status = @Status");
                        if (pictureBox1.Tag != null)
                        {
                            queryBuilder.Append(", pic = @Pic");
                        }
                        if (pictureBox2.Tag != null)
                        {
                            queryBuilder.Append(", pic2 = @Pic2");
                        }
                        if (pictureBox3.Tag != null)
                        {
                            queryBuilder.Append(", pic3 = @Pic3");
                        }
                        queryBuilder.Append(" WHERE itemid = @ItemID AND status != 'OUT'");

                        cmd = new MySqlCommand(queryBuilder.ToString(), conn);
                        cmd.Parameters.AddWithValue("@Name", cname.Text);
                        cmd.Parameters.AddWithValue("@Price", cprice.Text);
                        cmd.Parameters.AddWithValue("@Info", cinfo.Text);
                        cmd.Parameters.AddWithValue("@Amount", camount.Text);
                        cmd.Parameters.AddWithValue("@Tag", ctag.Text);
                        cmd.Parameters.AddWithValue("@ItemID", lblItemID.Text);
                        cmd.Parameters.AddWithValue("@Status", "WAIT");

                        if (pictureBox1.Tag != null)
                        {
                            byte[] imageBytes1 = File.ReadAllBytes(pictureBox1.Tag.ToString());
                            cmd.Parameters.AddWithValue("@Pic", imageBytes1);
                        }
                        if (pictureBox2.Tag != null)
                        {
                            byte[] imageBytes2 = File.ReadAllBytes(pictureBox2.Tag.ToString());
                            cmd.Parameters.AddWithValue("@Pic2", imageBytes2);
                        }
                        if (pictureBox3.Tag != null)
                        {
                            byte[] imageBytes3 = File.ReadAllBytes(pictureBox3.Tag.ToString());
                            cmd.Parameters.AddWithValue("@Pic3", imageBytes3);
                        }
                    }

                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show(camountValue == 0 ? "Data deleted successfully" : "Data updated successfully");
                        cname.Clear();
                        cprice.Clear();
                        cinfo.Clear();
                        camount.Clear();
                        ctag.Items.Clear();
                        pictureBox1.Image = null;
                        pictureBox2.Image = null;
                        pictureBox3.Image = null;

                        this.Hide();
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
            else
            {
                
                MessageBox.Show("Data update canceled");
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to delete this item?", "Confirm Delete", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                using (MySqlConnection conn = databaseConnection())
                {
                    string query = "DELETE FROM item WHERE itemid = @ItemID AND ststus != OUT";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ItemID", lblItemID.Text);

                        try
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Data deleted successfully");

                            lbname.Text = string.Empty;
                            lbprice.Text = string.Empty;
                            lbinfo.Text = string.Empty;
                            lbamount.Text = string.Empty;
                            lbtag.Text = string.Empty;
                            pictureBox1.Image = null;
                            pictureBox2.Image = null;
                            pictureBox3.Image = null;

                            this.Hide();
                        }
                        catch (MySqlException ex)
                        {
                            MessageBox.Show("Error: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Delete canceled");
            }
        }

    }
}
