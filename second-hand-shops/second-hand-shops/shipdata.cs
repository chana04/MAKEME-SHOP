using MySql.Data.MySqlClient;
using QRCodeGenerator;
using System;

using System.Drawing;
using System.IO;

using System.Windows.Forms;

namespace second_hand_shops
{
    public partial class Form1 : Form
    {
        private string _username;
        public Form1()
        {
            InitializeComponent();
        }

        private MySqlConnection databaseConnection()
        {
            string connectionstring = "datasource=127.0.0.1;port=3306;username=root;password=;database=shop;";
            MySqlConnection conn = new MySqlConnection(connectionstring);
            return conn;

        }
        public Form1(string ItemidID, string username)
        {
            InitializeComponent(); 
            text1.Text = ItemidID;
            _username = username; 
            text2.Text = username;

            try
            {
                using (MySqlConnection conn = databaseConnection())
                {
                    conn.Open(); 

                    string cartQuery = "SELECT itemid, amount, status FROM cart WHERE owner = @username AND status = @B";
                    MySqlCommand cartCommand = new MySqlCommand(cartQuery, conn);
                    cartCommand.Parameters.AddWithValue("@username", _username); 
                    cartCommand.Parameters.AddWithValue("@B", "P"); 

                    using (MySqlDataReader cartReader = cartCommand.ExecuteReader())
                    {
                        while (cartReader.Read())
                        {
                            int itemid = cartReader.GetInt32("itemid"); 
                            int amount = cartReader.GetInt32("amount"); 
                            string status = cartReader.GetString("status"); 

                            using (MySqlConnection conn2 = databaseConnection())
                            {
                                conn2.Open();

                                string itemQuery = "SELECT itemid, name, price, seller, info, amount, pic, pic2, pic3 FROM item WHERE itemid = @itemid";
                                MySqlCommand itemCommand = new MySqlCommand(itemQuery, conn2);
                                itemCommand.Parameters.AddWithValue("@itemid", itemid); 

                                using (MySqlDataReader itemReader = itemCommand.ExecuteReader())
                                {
                                    if (itemReader.Read()) 
                                    {
                                        label1.Text = itemReader.GetInt32("itemid").ToString(); 
                                        label4.Text = itemReader.GetString("seller"); 
                                        label3.Text = itemReader.GetDecimal("price").ToString(); 
                                        label5.Text = itemReader.GetString("name"); 
                                        label2.Text = amount.ToString(); 
                                        label6.Text = status.ToString(); 

                                        byte[] itemPic1 = itemReader["pic"] as byte[]; 
                                        byte[] itemPic2 = itemReader["pic2"] as byte[]; 
                                        byte[] itemPic3 = itemReader["pic3"] as byte[]; 

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
                        }
                    }
                }
            }
            catch (MySqlException ex) 
            {
                MessageBox.Show("An error occurred while fetching cart data: " + ex.Message);
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
                    Console.WriteLine("Error creating image: " + ex.Message);
                    return null;
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            MainForm qr = new MainForm(_username);
            qr.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection conn = databaseConnection())
                {
                    conn.Open(); 

                    string updateCartQuery = "UPDATE cart SET status = @status WHERE owner = @username AND itemid = @itemid";
                    MySqlCommand updateCartCommand = new MySqlCommand(updateCartQuery, conn);
                    updateCartCommand.Parameters.AddWithValue("@status", "DONE"); 
                    updateCartCommand.Parameters.AddWithValue("@username", _username); 
                    updateCartCommand.Parameters.AddWithValue("@itemid", Convert.ToInt32(label1.Text)); 

                    int rowsAffected = updateCartCommand.ExecuteNonQuery();
                    if (rowsAffected > 0) 
                    {
                        MessageBox.Show("Cart item updated successfully."); 
                    }
                    else 
                    {
                        MessageBox.Show("Cart item updated Cancel successfully."); 
                    }
                }
            }
            catch (MySqlException ex) 
            {
                MessageBox.Show("An error occurred while updating cart item: " + ex.Message);
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }
    }
}
