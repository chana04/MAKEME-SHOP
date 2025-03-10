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
using MySql.Data.MySqlClient;
using System.Net;
using System.Net.Mail;

namespace second_hand_shops
{
    public partial class adminupdate : Form
    {
        private string _adminid;

        private MySqlConnection databaseConnection()
        {
            string connectionstring = "datasource=127.0.0.1;port=3306;username=root;password=;database=shop;";
            MySqlConnection conn = new MySqlConnection(connectionstring); 
            return conn; 
        }

        public adminupdate(string adminid)
        {
            InitializeComponent(); 
            _adminid = adminid; 
            name.Text = adminid; 

            using (MySqlConnection conn = databaseConnection())
            {
                conn.Open(); 
                MySqlCommand command = new MySqlCommand("SELECT name, price, info, amount, seller, pic,pic2,pic3, tag FROM item WHERE itemid = @itemid", conn);
                command.Parameters.AddWithValue("@itemid", _adminid);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read()) 
                    {
                        name.Text = reader.GetString("name");
                        price.Text = reader.GetDecimal("price").ToString();
                        label3.Text = reader.GetString("info");
                        itemid.Text = reader.GetInt32("amount").ToString();
                        seller.Text = reader.GetString("seller");
                        tag.Text = reader.GetString("tag");

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
            Form8 form8 = new Form8(); 
            form8.Show(); 
            this.Hide(); 
        }


        // ยืนยันสินค้า PASS
        private void button2_Click_1(object sender, EventArgs e)
        {
            using (MySqlConnection conn = databaseConnection())
            {
                conn.Open(); 
                MySqlCommand command = new MySqlCommand("UPDATE item SET status = @status WHERE itemid = @itemid", conn);
                command.Parameters.AddWithValue("@status", "PASS"); 
                command.Parameters.AddWithValue("@itemid", _adminid); 
                command.ExecuteNonQuery(); 

                try
                {
                    MessageBox.Show("Update Data successfully."); 
                }
                catch (Exception ex) 
                {
                    MessageBox.Show("Failed to Update Data: " + ex.Message); 
                }
            }

            string fromAddress = "iceza55yoo@gmail.com"; 
            string recipientEmail = "";

            using (MySqlConnection connmail = databaseConnection())
            {
                connmail.Open(); 
                MySqlCommand commandmail = new MySqlCommand("SELECT email FROM user WHERE username = @seller", connmail);
                commandmail.Parameters.AddWithValue("@seller", seller.Text); 

                using (MySqlDataReader reader = commandmail.ExecuteReader())
                {
                    if (reader.Read()) 
                    {
                        recipientEmail = reader.GetString("email"); 
                    }
                    else 
                    {
                        MessageBox.Show("Email not found for the specified seller."); 
                        return; 
                    }
                }
            }
            MessageBox.Show(recipientEmail); 
            string toAddress = recipientEmail; 
            string subject = "Update Status Notification"; 
            string body = "The status of the item " + _adminid + " has been updated to PASS."; 
            using (MailMessage mail = new MailMessage(fromAddress, toAddress, subject, body))
            {
                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")) 
                {
                    smtpClient.Port = 587; 
                    smtpClient.Credentials = new NetworkCredential("iceza55yoo@gmail.com", "qffn xbcg zzor qmaw"); 
                    smtpClient.EnableSsl = true;

                    try
                    {
                        smtpClient.Send(mail); 
                        MessageBox.Show("Email sent successfully."); 

                        Form8 form8 = new Form8(); 
                        form8.Show(); 
                        this.Hide(); 
                    }
                    catch (Exception ex) 
                    {
                        MessageBox.Show("Failed to send email: " + ex.Message); 
                    }
                }
            }
        }


        // ลบสินค้า
        private void button3_Click(object sender, EventArgs e)
        {
            string deleteReason = textboxReason.Text; 
            if (string.IsNullOrWhiteSpace(deleteReason)) 
            {
                MessageBox.Show("Please provide a reason for deletion."); 
                return; 
            }

            using (MySqlConnection conn = databaseConnection())
            {
                conn.Open(); 
                MySqlCommand command = new MySqlCommand("DELETE FROM item WHERE itemid = @itemid", conn);
                command.Parameters.AddWithValue("@itemid", _adminid); 
                command.ExecuteNonQuery(); 

                try
                {
                    MessageBox.Show("Item deleted successfully."); 
                }
                catch (Exception ex) 
                {
                    MessageBox.Show("Failed to delete item: " + ex.Message); 
                }
            }

            Form8 form8 = new Form8(); 
            form8.Show(); 
            this.Hide(); 
        }
    }
}
