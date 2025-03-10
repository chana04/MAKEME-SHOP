using System;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data;
using System.Drawing;
using System.Diagnostics.Eventing.Reader;
using System.Text;

namespace second_hand_shops
{
    public partial class usersell : Form
    {
        private string _username;

        private MySqlConnection databaseConnection()
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=shop;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            return conn;
        }

        public usersell(string username)
        {
            InitializeComponent();
            _username = username;
            labeltext5.Text = _username;
            showyourlist();
            pendingdata.CellClick += pendingdata_CellClick;

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            tpic.ImageLocation = openFileDialog1.FileName;
        }

        private void tpic2_Click(object sender, EventArgs e)
        {
            openFileDialog2.ShowDialog();
            tpic2.ImageLocation = openFileDialog2.FileName;
        }

        private void tpic3_Click(object sender, EventArgs e)
        {
            openFileDialog3.ShowDialog();
            tpic3.ImageLocation = openFileDialog3.FileName;
        }
        private void guna2Button5_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.FileName.Contains("openFileDialog1") ||
                openFileDialog2.FileName.Contains("openFileDialog2") ||
                openFileDialog3.FileName.Contains("openFileDialog3"))
            {
                MessageBox.Show("Please select all required images."); 
                return;
            }

            if (string.IsNullOrWhiteSpace(tname.Text) || string.IsNullOrWhiteSpace(tprice.Text) ||
                string.IsNullOrWhiteSpace(tinfo.Text) || string.IsNullOrWhiteSpace(tamount.Text) ||
                string.IsNullOrWhiteSpace(ttag.Text))
            {
                MessageBox.Show("Please fill in all fields."); 
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(tname.Text, @"^[a-zA-Z0-9\s\(\)_-]+$"))
            {
                MessageBox.Show("Please enter a name with English letters, digits, spaces, and allowed special characters () _ -.");
                return;
            }

            string priceText = tprice.Text.Replace(",", "");
            if (!float.TryParse(priceText, out float price))
            {
                MessageBox.Show("Please enter a valid price.");
                return;
            }

            using (MySqlConnection conn = databaseConnection())
            {
                try
                {
                    

                    DialogResult result = MessageBox.Show("Confirm Your Info", "Message", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        string query = "INSERT INTO item (name, price, info, amount, seller, pic, pic2, pic3, tag, status) VALUES " +
                            "(@Name, @Price, @Info, @Amount, @Seller, @Pic, @Pic2, @Pic3, @Tag, @STA)";

                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Name", tname.Text);
                            cmd.Parameters.AddWithValue("@Price", price);
                            cmd.Parameters.AddWithValue("@Info", tinfo.Text);
                            cmd.Parameters.AddWithValue("@Amount", tamount.Text);
                            cmd.Parameters.AddWithValue("@Seller", _username); 
                            cmd.Parameters.AddWithValue("@Tag", ttag.Text);
                            cmd.Parameters.AddWithValue("@STA", "WAIT"); 

                            byte[] imageBytes = null;
                            if (!string.IsNullOrWhiteSpace(openFileDialog1.FileName))
                            {
                                using (var stream = new FileStream(openFileDialog1.FileName, FileMode.Open, FileAccess.Read))
                                {
                                    imageBytes = new byte[stream.Length];
                                    stream.Read(imageBytes, 0, (int)stream.Length);
                                }
                            }
                            cmd.Parameters.AddWithValue("@Pic", imageBytes); 

                            byte[] imageBytes2 = null;
                            if (!string.IsNullOrWhiteSpace(openFileDialog2.FileName))
                            {
                                using (var stream = new FileStream(openFileDialog2.FileName, FileMode.Open, FileAccess.Read))
                                {
                                    imageBytes2 = new byte[stream.Length];
                                    stream.Read(imageBytes2, 0, (int)stream.Length);
                                }
                            }
                            cmd.Parameters.AddWithValue("@Pic2", imageBytes2);

                            byte[] imageBytes3 = null;
                            if (!string.IsNullOrWhiteSpace(openFileDialog3.FileName))
                            {
                                using (var stream = new FileStream(openFileDialog3.FileName, FileMode.Open, FileAccess.Read))
                                {
                                    imageBytes3 = new byte[stream.Length];
                                    stream.Read(imageBytes3, 0, (int)stream.Length);
                                }
                            }
                            cmd.Parameters.AddWithValue("@Pic3", imageBytes3); 

                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Data inserted successfully"); 

                            tname.Clear();
                            tprice.Clear();
                            tinfo.Clear();
                            tamount.Items.Clear();
                            ttag.Items.Clear();
                            openFileDialog1.Reset();
                            openFileDialog2.Reset();
                            openFileDialog3.Reset();

                            userhome form4 = new userhome(_username);
                            form4.Show();
                            this.Hide();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Data insertion canceled"); 
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error: " + ex.Message); 
                }
            }
        }


        private void guna2Button2_Click_1(object sender, EventArgs e)
        {
            userhome form4 = new userhome(_username);
            form4.Show();
            this.Hide();
        }

        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            page1 form1 = new page1();
            form1.Show();
            this.Hide();
        }


        private void UpdateItemStatus()     //อัพเดทการส่งสินค้า
        {
            try
            {
                using (MySqlConnection conn = databaseConnection())
                {
                    conn.Open(); 

                    string updateQuery = "UPDATE cart SET status = @update WHERE orderid = @od AND seller = @usr";

                    using (MySqlCommand cmd = new MySqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@update", text.Text); 
                        cmd.Parameters.AddWithValue("@od", orderID.Text);  
                        cmd.Parameters.AddWithValue("@usr", _username);    

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Item status updated successfully"); 
                        }
                        else
                        {
                            MessageBox.Show("Failed to update item status"); 
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error updating item status: " + ex.Message);
            }
        }



        private void guna2Button11_Click(object sender, EventArgs e)
        {
            if (itemsellpn.Visible == false)
            {
                itemsellpn.Visible = true;
                itempn.Visible = false;
                shippn.Visible = false;
                itemsellpn.BringToFront();
            }
            else
            {
                itemsellpn.Visible = false;
            }
        }

        private void guna2Button12_Click(object sender, EventArgs e)
        {
            if (shippn.Visible == false)
            {

                shippn.Visible = true;
                itempn.Visible = false;
                itemsellpn.Visible = false;
                shippn.BringToFront();
                LoadOrders();
            }
            else
            {
                shippn.Visible = false;
            }
        }

        private void LoadOrders()   //ดึงส้นค้าที่สถานะเป็น รอการตรวจสอบจากผู้ขาย
        {
            using (MySqlConnection conn = databaseConnection())
            {
                try
                {
                    conn.Open(); 

                    string query = "SELECT * FROM cart WHERE seller = @Seller AND status = 'รอการตรวจสอบจากผู้ขาย'";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Seller", _username); 

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable(); 
                    adapter.Fill(dt); 

                    if (dt.Rows.Count > 0)
                    {
                        //ใส่เข้าไปในดาต้ากริด
                        pendingdata.DataSource = dt; 
                    }
                    else
                    {
                        MessageBox.Show("No pending orders found."); 
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error loading pending orders: " + ex.Message);
                }
            }
        }
        private void pendingdata_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = pendingdata.Rows[e.RowIndex];

                string orderid = row.Cells["orderid"].Value.ToString();
                string itemid = row.Cells["itemid"].Value.ToString();

                orderID.Text = orderid;
                itemID.Text = itemid;

                LoadCartData(orderid);      
                LoadAllUserInfo(orderid);   
                LoadItemINFO2();            
                LoadItemINFO();             
            }
        }

        private void LoadCartData(string orderId)
        {
            try
            {
                using (MySqlConnection conn = databaseConnection())
                {
                    conn.Open(); 

                    string query = "SELECT itemid, amount FROM cart WHERE orderid = @orderId AND seller = @sell";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@orderId", orderId);  
                    cmd.Parameters.AddWithValue("@sell", _username);   

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);  

                    itemsameorder.DataSource = dataTable;
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("An error occurred while fetching cart data: " + ex.Message);
            }
        }


        private void LoadAllUserInfo(string orderId)
        {
            try
            {
                using (MySqlConnection conn = databaseConnection())
                {
                    conn.Open(); 

                    string query = "SELECT username, name, housenum, district, province, code, date, ETC, pic, orderid, totalprice FROM userinfo WHERE orderid = @order";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@order", orderId); 

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable userTable = new DataTable();
                    adapter.Fill(userTable); 

                    datainfo.DataSource = userTable;
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("An error occurred while fetching user info: " + ex.Message);
            }
        }


        private void guna2Button4_Click(object sender, EventArgs e)    //ปุ่มยืนยันการอัพเดทสถานะสินค้า
        {
            if (text.Text == "ดำเนินการส่งแล้ว" || text.Text == "ยกเลิกสินค้าแล้ว")
            {
                UpdateItemStatus();

                pendingdata.DataSource = null;
                itemsameorder.DataSource = null;
                datainfo.DataSource = null;

                LoadOrders();
            }
            else
            {
                MessageBox.Show("กรุณาเลือกสถานะสินค้าเพื่ออัพเดทสถานะ");
            }
        }


        private void itempn_Paint(object sender, PaintEventArgs e)
        {

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
        private void showyourlist()
        {
            try
            {
                using (MySqlConnection connyour = databaseConnection())
                {
                    connyour.Open(); 

                    string queryyour = "SELECT name, itemid, amount ,pic,status FROM item WHERE seller = @username";
                    MySqlCommand commandyour = new MySqlCommand(queryyour, connyour);
                    commandyour.Parameters.AddWithValue("@username", _username); 

                    using (MySqlDataReader readeryour = commandyour.ExecuteReader())
                    {
                        while (readeryour.Read())
                        {
                            string name24 = readeryour["name"].ToString();
                            string id4 = readeryour["itemid"].ToString();
                            string amount4 = readeryour["amount"].ToString();
                            string status4 = readeryour["status"].ToString();
                            byte[] pic4 = null;

                            if (!readeryour.IsDBNull(readeryour.GetOrdinal("pic")))
                            {
                                pic4 = (byte[])readeryour["pic"]; 
                            }

                            UserControl1 us = new UserControl1();
                            us.ItemName = name24; 
                            us.ItemID = id4; 
                            us.Itemamount = amount4; 
                            us.Utext = _username; 
                            us.Itemstatus = status4; 
                            us.itemImage = ByteArrayToImage(pic4); 

                            itempn.Controls.Add(us);
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("An error occurred during command execution: " + ex.Message);
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            if (itempn.Visible == false)
            {
                itempn.Controls.Clear();
                itempn.Visible = true;
                shippn.Visible = false;
                itemsellpn.Visible = false;
                itempn.BringToFront();

                showyourlist();
            }
            else
            {
                itempn.Visible = false;
            }
        }
        private void LoadItemINFO2()  //ดึงข้ิอมูล 2
        {
            
            using (MySqlConnection conn = databaseConnection())
            {
                try
                {
                    conn.Open(); 

                    string sql = "SELECT pic FROM userinfo WHERE orderid = @orderID";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@orderID", orderID.Text); 

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                         
                            if (reader.Read())
                            {
                            
                                byte[] imageBytes = (byte[])reader["pic"];
                                using (MemoryStream ms = new MemoryStream(imageBytes))
                                {
                                    picNN.Image = Image.FromStream(ms);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Item not found.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        private void LoadItemINFO()   //ดึงข้อมูล1
        {
            using (MySqlConnection conn = databaseConnection())
            {
                try
                {
                    conn.Open(); 

                    string sql = "SELECT name, price, pic FROM item WHERE itemid = @itemid";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@itemid", itemID.Text);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                nameNN.Text = reader["name"].ToString();
                                priceNN.Text = reader["price"].ToString();

                                byte[] imageBytes = (byte[])reader["pic"];
                                using (MemoryStream ms = new MemoryStream(imageBytes))
                                {
                                    picNN2.Image = Image.FromStream(ms);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Item not found.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
    }
}