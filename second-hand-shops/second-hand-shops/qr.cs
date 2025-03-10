using System;
using System.Windows.Forms;
using Saladpuk.EMVCo.Contracts;
using Saladpuk.PromptPay.Facades;
using QRCoder;
using System.Drawing;
using MySql.Data.MySqlClient;
using second_hand_shops;
using System.Collections.Generic;
using Microsoft.Win32;
using System.IO;

namespace QRCodeGenerator
{
    public partial class MainForm : Form
    {
        private string _username; 

        
        private MySqlConnection databaseConnection()
        {
            string connectionstring = "datasource=127.0.0.1;port=3306;username=root;password=;database=shop;";
            MySqlConnection conn = new MySqlConnection(connectionstring); 
            return conn; 
        }

        public MainForm(string username)
        {
            InitializeComponent(); 
            _username = username; 

            showcart(); 

            showtotal(); 
        }

        private void showcart()
        {
            labelhead.Text = "cart item status"; 
            showtotal(); 
            flowLayoutPanel1.Controls.Clear(); 

            try
            {
                using (MySqlConnection conn = databaseConnection()) 
                {
                    conn.Open(); 

                    string cartQuery = "SELECT itemid, amount FROM cart WHERE owner = @username AND status = @B";
                    MySqlCommand cartCommand = new MySqlCommand(cartQuery, conn); 
                    cartCommand.Parameters.AddWithValue("@username", _username); 
                    cartCommand.Parameters.AddWithValue("@B", "B"); 

                    using (MySqlDataReader cartReader = cartCommand.ExecuteReader()) 
                    {
                        while (cartReader.Read()) 
                        {
                            int itemid = Convert.ToInt32(cartReader["itemid"]); 
                            int amount = Convert.ToInt32(cartReader["amount"]); 

                            using (MySqlConnection conn2 = databaseConnection()) 
                            {
                                conn2.Open(); 

                                string itemQuery = "SELECT price, pic FROM item WHERE itemid = @itemid";
                                MySqlCommand itemCommand = new MySqlCommand(itemQuery, conn2); 
                                itemCommand.Parameters.AddWithValue("@itemid", itemid); 

                                using (MySqlDataReader itemReader = itemCommand.ExecuteReader()) 
                                {
                                    if (itemReader.Read()) 
                                    {
                                        double price = itemReader["price"] == DBNull.Value ? 0 : Convert.ToDouble(itemReader["price"]);
                                        byte[] pic = itemReader["pic"] as byte[]; 

                                        cartitem carti = new cartitem(); 
                                        carti.ItemID = itemid.ToString(); 
                                        carti.Itemamount = amount.ToString(); 
                                        carti.itemImage = ByteArrayToImage(pic); 
                                        carti.Itemprice = price.ToString(); 
                                        carti.Itemuser = _username; 

                                        flowLayoutPanel1.Controls.Add(carti); 
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


        private Image ByteArrayToImage(byte[] byteArrayIn)
        {
            if (byteArrayIn == null || byteArrayIn.Length == 0)
                return null;

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(byteArrayIn))
            {
                return Image.FromStream(ms); 
            }
        }

        private void showtotal()
        {
            try
            {
                using (MySqlConnection conn = databaseConnection()) 
                {
                    conn.Open(); 

                    //จำนวนทั้งหมด
                    string countQuery = "SELECT IFNULL(SUM(c.amount), 0) AS totalItems FROM cart c WHERE c.owner = @username AND c.status = @B";
                    MySqlCommand countCommand = new MySqlCommand(countQuery, conn); 
                    countCommand.Parameters.AddWithValue("@username", _username); 
                    countCommand.Parameters.AddWithValue("@B", "B"); 

                    int totalItems = Convert.ToInt32(countCommand.ExecuteScalar()); 

                    //ราคารวม
                    string totalPriceQuery = "SELECT IFNULL(SUM(c.amount * i.price), 0) AS totalPrice FROM cart c INNER JOIN item i ON c.itemid = i.itemid WHERE c.owner = @username AND c.status = @B";
                    MySqlCommand totalPriceCommand = new MySqlCommand(totalPriceQuery, conn); 
                    totalPriceCommand.Parameters.AddWithValue("@username", _username); 
                    totalPriceCommand.Parameters.AddWithValue("@B", "B"); 

                    object totalPriceObj = totalPriceCommand.ExecuteScalar(); 
                    double totalPrice = Convert.ToDouble(totalPriceObj); 

                    price.Text = $"Cost : {totalPrice.ToString("0.00")}";

                    double vatRate = 0.07; 
                    double vat = totalPrice * vatRate; 

                    labelVAT.Text = $" VAT : {vat.ToString("0.00")}";

                    totalPrice += vat; 

                    double shippingCost = 45; 
                    labelShipping.Text = $"Shipping Cost : {shippingCost.ToString("0.00")}"; 

                    totalPrice += shippingCost; 

                    labelTotalPrice.Text = $"Total Cost : {totalPrice.ToString("0.00")}";
                    labelTotalItems.Text = $"Total Item : {totalItems}";
                }
            }
            catch (MySqlException ex) 
            {
                MessageBox.Show("An error occurred while fetching cart data: " + ex.Message); 
            }
        }

        private bool checkitem(object sender, EventArgs e) // เช็คสถานะสินค้า ถ้าเกินก็ลบสินค้าออก
        {
            MessageBox.Show("check item ..... ");
            try
            {
                using (MySqlConnection conn = databaseConnection()) 
                {
                    conn.Open(); 

                    
                    string checkQuery = "SELECT c.itemid, c.amount AS cartAmount, i.amount AS itemAmount " +
                                        "FROM cart c " +
                                        "INNER JOIN item i ON c.itemid = i.itemid " +
                                        "WHERE c.owner = @username AND c.status = @B";
                    MySqlCommand checkCommand = new MySqlCommand(checkQuery, conn); 
                    checkCommand.Parameters.AddWithValue("@username", _username); 
                    checkCommand.Parameters.AddWithValue("@B", "B"); 

                    //ลิสเก็บ tuple 
                    List<Tuple<int, int, string>> itemsToUpdate = new List<Tuple<int, int, string>>();

                    using (MySqlDataReader reader = checkCommand.ExecuteReader()) 
                    {
                        while (reader.Read()) 
                        {
                            int itemId = reader.GetInt32("itemid"); 
                            int cartAmount = reader.GetInt32("cartAmount"); 
                            int itemAmount = reader.GetInt32("itemAmount"); 

                            if (cartAmount > itemAmount)
                            {
                                int reduceAmount = cartAmount - itemAmount; 
                                MessageBox.Show($"Reduce {reduceAmount} {itemId}(s) from your cart."); 

                                // เพิ่มข้อมูลที่จะอัปเดตในรายการ
                                itemsToUpdate.Add(new Tuple<int, int, string>(itemId, itemAmount, _username));
                            }
                        }
                    }

                    foreach (var itemToUpdate in itemsToUpdate)
                    {
                        int itemId = itemToUpdate.Item1; 
                        int cartAmount = itemToUpdate.Item2; 
                        string username = itemToUpdate.Item3; 

                        string updateQuery = "UPDATE cart SET amount = @cartAmount WHERE itemid = @itemId AND owner = @username";
                        MySqlCommand updateCommand = new MySqlCommand(updateQuery, conn); 
                        updateCommand.Parameters.AddWithValue("@cartAmount", cartAmount); 
                        updateCommand.Parameters.AddWithValue("@itemId", itemId); 
                        updateCommand.Parameters.AddWithValue("@username", username); 
                        updateCommand.ExecuteNonQuery(); 
                    }

                    return true; 
                }
            }
            catch (MySqlException ex) 
            {
                MessageBox.Show("An error occurred while checking items: " + ex.Message); 
                return false; 
            }
        }

        private bool recheck(object sender, EventArgs e) 
        {
            try
            {
                using (MySqlConnection conn = databaseConnection()) 
                {
                    conn.Open(); 

                    //หาสินค้าหมด
                    string checkQuery = "SELECT itemid FROM cart WHERE owner = @username AND status = @B AND amount = 0";
                    MySqlCommand checkCommand = new MySqlCommand(checkQuery, conn); 
                    checkCommand.Parameters.AddWithValue("@username", _username); 
                    checkCommand.Parameters.AddWithValue("@B", "B"); 

                    List<int> itemsToRemove = new List<int>();

                    using (MySqlDataReader reader = checkCommand.ExecuteReader()) 
                    {
                        while (reader.Read()) 
                        {
                            int itemId = reader.GetInt32("itemid"); 
                            itemsToRemove.Add(itemId); 
                        }
                    }

                    if (itemsToRemove.Count > 0)
                    {
                        foreach (int itemId in itemsToRemove) 
                        {
                            string deleteQuery = "DELETE FROM cart WHERE itemid = @itemId AND owner = @username";
                            MySqlCommand deleteCommand = new MySqlCommand(deleteQuery, conn); 
                            deleteCommand.Parameters.AddWithValue("@itemId", itemId); 
                            deleteCommand.Parameters.AddWithValue("@username", _username); 
                            deleteCommand.ExecuteNonQuery(); 
                        }

                        MessageBox.Show($"Removed {itemsToRemove.Count} item(s) with zero quantity from your cart."); 
                        showcart(); 
                        return false; 
                    }
                    else
                    {
                        return true; 
                    }
                }
            }
            catch (MySqlException ex) 
            {
                MessageBox.Show("An error occurred while checking items: " + ex.Message); 
                return false; 
            }
        }
        private bool checkAndRemoveWaitItems(object sender, EventArgs e) // ตรวจสถานะ สินค้า
        {
            try
            {
                using (MySqlConnection conn = databaseConnection()) 
                {
                    conn.Open(); 

                    //ดูสินค้าในตะกร้า
                    string cartQuery = "SELECT itemid FROM cart WHERE owner = @username AND status = @B";
                    MySqlCommand cartCommand = new MySqlCommand(cartQuery, conn); 
                    cartCommand.Parameters.AddWithValue("@username", _username); 
                    cartCommand.Parameters.AddWithValue("@B", "B"); 

                    List<int> itemsToCheck = new List<int>(); 

                    using (MySqlDataReader cartReader = cartCommand.ExecuteReader()) 
                    {
                        while (cartReader.Read()) 
                        {
                            int itemId = cartReader.GetInt32("itemid"); 
                            itemsToCheck.Add(itemId); 
                        }
                    }

                    List<int> itemsToRemove = new List<int>(); 

                    //เช็คสถานะสินค้าในตะกร้า
                    foreach (int itemId in itemsToCheck) 
                    {
                        string itemQuery = "SELECT status FROM item WHERE itemid = @itemId";
                        MySqlCommand itemCommand = new MySqlCommand(itemQuery, conn); 
                        itemCommand.Parameters.AddWithValue("@itemId", itemId); 

                        object statusObj = itemCommand.ExecuteScalar(); 
                        if (statusObj != null && statusObj.ToString() == "WAIT") 
                        {
                            itemsToRemove.Add(itemId); 
                        }
                    }

                    // ลบสินค้าที่มีสถานะเป็น "WAIT"
                    foreach (int itemId in itemsToRemove)
                    {
                        string deleteQuery = "DELETE FROM cart WHERE itemid = @itemId AND owner = @username";
                        MySqlCommand deleteCommand = new MySqlCommand(deleteQuery, conn); 
                        deleteCommand.Parameters.AddWithValue("@itemId", itemId); 
                        deleteCommand.Parameters.AddWithValue("@username", _username); 
                        deleteCommand.ExecuteNonQuery(); 
                    }

                    if (itemsToRemove.Count > 0)
                    {
                        MessageBox.Show($"Removed {itemsToRemove.Count} item(s) with WRONG status from your cart."); 
                    }
                    else
                    {
                        
                    }

                    return true; 
                }
            }
            catch (MySqlException ex) 
            {
                MessageBox.Show("An error occurred while checking and removing items: " + ex.Message); 
                return false; 
            }
        }



        private void guna2Button1_Click(object sender, EventArgs e)
        {

            showcart();
        }


        private void guna2Button3_Click(object sender, EventArgs e)
        {

            userhome form4 = new userhome(_username);
            form4.Show();
            this.Hide();
        }


        private void guna2Button4_Click(object sender, EventArgs e) //qr code
        {

            qrcodehere.Visible = true;

            if (!HasItemsInCart())
            {
                MessageBox.Show("There are no items in your cart.");
                qrcodehere.Visible = false;
                return;
            }

            if (!checkitem(sender, e))
            {
                return;
            }

            if (!checkAndRemoveWaitItems(sender, e))
            {
                return;
            }

            if (!recheck(sender, e))
            {
                return;
            }

            showtotal();
            showcart();

            try
            {
                string Amountringking = labelTotalPrice.Text;
                double amount;

                if (!double.TryParse(labelTotalPrice.Text.Replace("Total Cost : ", ""), out amount))
                {
                    MessageBox.Show("Invalid amount. Please enter a valid number.");
                    qrcodehere.Visible = false;
                    return;
                }

                string qrData = PPay.StaticQR.MobileNumber("0953929205").Amount(amount).CreateCreditTransferQrCode();

                QRCoder.QRCodeGenerator qrGenerator = new QRCoder.QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrData, QRCoder.QRCodeGenerator.ECCLevel.H);
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(20);

                //ใส่ qr 
                qrCodePictureBox.Image = qrCodeImage;

                IPromptPayQrInfo promptPayInfo = PPay.Reader.ReadQrPromptPay(qrData);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while generating QR code: " + ex.Message);
                qrcodehere.Visible = false;
            }
        }


        private bool HasItemsInCart()   //นับจำนวนสินค้า ในตะกร้า
        {
            try
            {
                using (MySqlConnection conn = databaseConnection())
                {
                    conn.Open();

                    string query = "SELECT COUNT(*) FROM cart WHERE owner = @username AND status = @B";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", _username);
                    cmd.Parameters.AddWithValue("@B", "B");

                    int itemCount = Convert.ToInt32(cmd.ExecuteScalar());
                    return itemCount > 0;
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("An error occurred while checking the cart: " + ex.Message);
                return false;
            }
        }


        private void guna2Button5_Click(object sender, EventArgs e)
        {
            userdone form11 = new userdone(_username);
            form11.Show();
            this.Hide();
        }

        private void labelTotalPrice_Click(object sender, EventArgs e)
        {
            
        }

        private void price_Click(object sender, EventArgs e)
        {

        }

        private void labelVAT_Click(object sender, EventArgs e)
        {

        }
    }
}