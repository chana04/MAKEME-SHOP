using Aspose.CAD.FileFormats.Collada.FileParser.Elements;
using MySql.Data.MySqlClient;
using QRCodeGenerator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace second_hand_shops
{
    public partial class userdel : Form
    {
        private string _username;
        private int cartAmount = 0;

        private MySqlConnection databaseConnection()
        {
            string connectionstring = "datasource=127.0.0.1;port=3306;username=root;password=;database=shop;";
            MySqlConnection conn = new MySqlConnection(connectionstring);
            return conn;

        }
        public userdel(string ItemidID, string username)
        {
            InitializeComponent();

            text1.Text = ItemidID;
            _username = username;
            text2.Text = username;
            textBoxAmount.Text = "1";

            mycart();

            using (MySqlConnection conn = databaseConnection())
            {
                conn.Open();

                MySqlCommand command = new MySqlCommand("SELECT name, price, info, amount, seller, pic,pic2,pic3, tag FROM item WHERE itemid = @itemid", conn);

                command.Parameters.AddWithValue("@itemid", ItemidID);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        textname.Text = reader.GetString("name");
                        textprice.Text = reader.GetDecimal("price").ToString();
                        textinfo.Text = reader.GetString("info");
                        texta.Text = reader.GetInt32("amount").ToString();
                        texts.Text = reader.GetString("seller");
                        textt.Text = reader.GetString("tag");

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

        private void mycart()
        {
            using (MySqlConnection amountconn = databaseConnection())
            {
                amountconn.Open();

                string amountQuery = "SELECT amount FROM cart WHERE itemid = @itemid";
                MySqlCommand amountCommand = new MySqlCommand(amountQuery, amountconn);
                amountCommand.Parameters.AddWithValue("@itemid", text1.Text);

                object result = amountCommand.ExecuteScalar();

                if (result != null)
                {
                    cartAmount = Convert.ToInt32(result);

                    if (cartAmount > 0)
                    {
                        textc.Text = cartAmount.ToString();
                    }
                }
                else
                {
                    textc.Text = "0";
                    cartAmount = 0;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)  //ตรวจจำนวนใน cart ลบจำนวนออกตามที่ผู้ใช้กรอก
        {
            string itemId = text1.Text;
            int amountToChange = -1 * int.Parse(textBoxAmount.Text); 
            string owner = _username; 

            using (MySqlConnection conn = databaseConnection())
            {
             
                conn.Open();

                MySqlCommand checkCommand = new MySqlCommand("SELECT amount FROM cart WHERE itemid = @itemid AND owner = @owner", conn);
                checkCommand.Parameters.AddWithValue("@itemid", itemId);
                checkCommand.Parameters.AddWithValue("@owner", owner);

                object result = checkCommand.ExecuteScalar();

                if (result != null)
                {
                    int currentAmount = Convert.ToInt32(result);

                    if (currentAmount + amountToChange <= 0)
                    {
                        MySqlCommand deleteCommand = new MySqlCommand("DELETE FROM cart WHERE itemid = @itemid AND owner = @owner", conn);
                        deleteCommand.Parameters.AddWithValue("@itemid", itemId);
                        deleteCommand.Parameters.AddWithValue("@owner", owner);
                        deleteCommand.ExecuteNonQuery(); 
                        MessageBox.Show("Item removed from cart."); 

                        MainForm qr = new MainForm(_username);
                        qr.Show();
                        this.Hide(); 
                    }
                    else
                    {
                        MySqlCommand updateCommand = new MySqlCommand("UPDATE cart SET amount = @amount WHERE itemid = @itemid AND owner = @owner", conn);
                        updateCommand.Parameters.AddWithValue("@amount", currentAmount + amountToChange); 
                        updateCommand.Parameters.AddWithValue("@itemid", itemId);
                        updateCommand.Parameters.AddWithValue("@owner", owner);
                        updateCommand.ExecuteNonQuery();
                        MessageBox.Show("Item amount updated in cart."); 

                        MainForm qr = new MainForm(_username);
                        qr.Show();
                        this.Hide(); 
                    }
                }
                else
                {
                    MessageBox.Show("Cannot remove non-existent item from cart.");
                }
            }
            mycart();
        }


        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
