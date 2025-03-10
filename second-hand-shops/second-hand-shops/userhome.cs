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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;


namespace second_hand_shops
{
    public partial class userhome : Form
    {
        private string _username;

        private MySqlConnection databaseConnection()
        {
            string connectionstring = "datasource=127.0.0.1;port=3306;username=root;password=;database=shop;";
            MySqlConnection conn = new MySqlConnection(connectionstring);
            return conn;
        }


        public userhome(string username)
        {
            InitializeComponent(); 

            _username = username; 
            label4.Text = _username; 

            listBox1.Items.Add("รอการตรวจสอบ");
            listBox1.Items.Add("ดำเนินการส่งแล้ว");
            listBox1.Items.Add("การซื้อสำเร็จ");

            shipdata.CellClick += DataGridView1_CellClick;
            listBox1.SelectedIndexChanged += ListBox1_SelectedIndexChanged;

            showsentitem();
            showcart();
            showlist();
            shippingitem_data();

            boxbox.TextChanged += searchtextb_TextChanged;
            tagbox.SelectedIndexChanged += pricesearch;
            pricebox.SelectedIndexChanged += pricesearch;
        }




        private void showsentitem() //สินค้ารอส่ง
        {
            try
            {
                using (MySqlConnection conn = databaseConnection())
                {
                    conn.Open();

                    string countQuery = "SELECT IFNULL(SUM(c.amount), 0) AS totalItems FROM cart c WHERE c.owner = @username AND c.status = @B";
                    MySqlCommand countCommand = new MySqlCommand(countQuery, conn);
                    countCommand.Parameters.AddWithValue("@username", _username);
                    countCommand.Parameters.AddWithValue("@B", "ดำเนินการส่งแล้ว");

                    int totalItems = Convert.ToInt32(countCommand.ExecuteScalar());

                    
                    labelsentitem.Text = $"{totalItems}";
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("An error occurred while fetching cart data: " + ex.Message);
            }
        }

        private void showcart()  //สินค้าในตะกร้า ราคารวม
        {
            try
            {
               
                using (MySqlConnection conn = databaseConnection())
                {
                    conn.Open();

                    
                    string countQuery = "SELECT IFNULL(SUM(c.amount), 0) AS totalItems FROM cart c WHERE c.owner = @username AND c.status = @B";
                    MySqlCommand countCommand = new MySqlCommand(countQuery, conn);
                    countCommand.Parameters.AddWithValue("@username", _username);
                    countCommand.Parameters.AddWithValue("@B", "B");

                    
                    int totalItems = Convert.ToInt32(countCommand.ExecuteScalar());

                    
                    string priceQuery = "SELECT IFNULL(SUM(c.amount * i.price), 0) AS totalPrice FROM cart c INNER JOIN item i ON c.itemid = i.itemid WHERE c.owner = @username AND c.status = @B";
                    MySqlCommand priceCommand = new MySqlCommand(priceQuery, conn);
                    priceCommand.Parameters.AddWithValue("@username", _username);
                    priceCommand.Parameters.AddWithValue("@B", "B");
                    object totalPriceObj = priceCommand.ExecuteScalar();
                    double totalPrice = Convert.ToDouble(totalPriceObj);

                    
                    labelcartitem.Text = $"{totalPrice}";

                    
                    cibut.Text = totalItems.ToString();

                    
                    cibut.Visible = totalItems > 0;
                }
            }
            catch (MySqlException ex)
            {
                
                MessageBox.Show("An error occurred while fetching cart data: " + ex.Message);
            }
        }

        private void showlist()  //แสดงสินค้าที่ขาย
        {
            using (MySqlConnection conn = databaseConnection())
            {
                conn.Open();

                MySqlCommand command = new MySqlCommand("SELECT name, price, info, amount, seller, pic, tag FROM item WHERE seller != @username AND status = 'PASS' ORDER BY itemid DESC", conn);
                command.Parameters.AddWithValue("@username", _username);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string N = reader["name"].ToString();   
                        string P = reader["price"].ToString();  
                        string I = reader["info"].ToString();   
                        string A = reader["amount"].ToString();  
                        string S = reader["seller"].ToString(); 
                        byte[] pic = null;                       
                        string T = reader["tag"].ToString();    

                        if (!reader.IsDBNull(reader.GetOrdinal("pic")))
                        {
                            pic = (byte[])reader["pic"];  
                        }

                        listitem uis = new listitem();
                        uis.Name1 = N;
                        uis.Price = P;
                        uis.Icon = ByteArrayToImage(pic); 
                        uis.Info = I;
                        uis.Amount = A;
                        uis.Seller = S;
                        uis.Tag1 = T;
                        uis.Sc = _username;

                        productpanel.Controls.Add(uis);
                    }
                    conn.Close();  
                }
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
        private void searchtextb_TextChanged(object sender, EventArgs e)  //ค้นหา tag สินค้า
        {
            string searchText = boxbox.Text.Trim(); 

            if (!string.IsNullOrWhiteSpace(searchText)) 
            {
                MySqlConnection conn = databaseConnection(); 

                try
                {
                    conn.Open(); 
                                 
                    string query = "SELECT name, price, info, amount, seller, pic, tag FROM item WHERE (LOWER(name) LIKE @searchText OR LOWER(info) LIKE @searchText OR LOWER(seller) LIKE @searchText) AND seller != @username AND status = 'PASS'";
                    MySqlCommand command = new MySqlCommand(query, conn);
                    command.Parameters.AddWithValue("@searchText", "%" + searchText.ToLower() + "%"); 
                    command.Parameters.AddWithValue("@username", _username); 

                    productpanel.Controls.Clear(); 

                    using (MySqlDataReader search = command.ExecuteReader()) 
                    {
                        while (search.Read())
                        {
                            string N = search["name"].ToString();
                            string P = search["price"].ToString();
                            string I = search["info"].ToString();
                            string A = search["amount"].ToString();
                            string S = search["seller"].ToString();
                            byte[] pic = null;
                            string T = search["tag"].ToString();

                            if (!search.IsDBNull(search.GetOrdinal("pic")))
                            {
                                pic = (byte[])search["pic"];
                            }

                            listitem uis = new listitem();
                            uis.Name1 = N;
                            uis.Price = P;
                            uis.Icon = ByteArrayToImage(pic); 
                            uis.Info = I;
                            uis.Amount = A;
                            uis.Seller = S;
                            uis.Tag1 = T;
                            uis.Sc = _username;

                            productpanel.Controls.Add(uis);
                        }
                    }
                }
                catch (Exception ex) 
                {
                    MessageBox.Show("An error occurred during search: " + ex.Message);
                }
                finally
                {
                    conn.Close(); 
                }
            }
            else 
            {
                productpanel.Controls.Clear(); 
                showlist(); 
            }
        }

        private void pricesearch(object sender, EventArgs e) //ค้นหาจากราคา
        {
            string priceText = pricebox.SelectedItem?.ToString(); 
            string selectedTag = tagbox.SelectedItem?.ToString(); 
            MySqlConnection conn = databaseConnection(); 

            try
            {
                conn.Open(); 
                MySqlCommand command = new MySqlCommand(); 
                command.Connection = conn;

                if (!string.IsNullOrWhiteSpace(priceText) && !string.IsNullOrWhiteSpace(selectedTag))
                {
                    command.CommandText = "SELECT name, price, info, amount, seller, pic, tag FROM item WHERE price < @searchPrice AND tag = @searchTag AND seller != @username AND status = 'PASS'";
                    command.Parameters.AddWithValue("@searchPrice", priceText); 
                    command.Parameters.AddWithValue("@searchTag", selectedTag); 
                }
                else if (!string.IsNullOrWhiteSpace(priceText))
                {
                    command.CommandText = "SELECT name, price, info, amount, seller, pic, tag FROM item WHERE price < @searchPrice AND seller != @username AND status = 'PASS'";
                    command.Parameters.AddWithValue("@searchPrice", priceText);
                }
                else if (!string.IsNullOrWhiteSpace(selectedTag))
                {
                    command.CommandText = "SELECT name, price, info, amount, seller, pic, tag FROM item WHERE tag = @searchTag AND seller != @username AND status = 'PASS'";
                    command.Parameters.AddWithValue("@searchTag", selectedTag);
                }
                else
                {
                    productpanel.Controls.Clear();
                    showlist();
                    return;
                }

                command.Parameters.AddWithValue("@username", _username); 

                using (MySqlDataReader reader = command.ExecuteReader()) 
                {
                    productpanel.Controls.Clear(); 
                    while (reader.Read()) 
                    {
                        string N = reader["name"].ToString();
                        string P = reader["price"].ToString();
                        string I = reader["info"].ToString();
                        string A = reader["amount"].ToString();
                        string S = reader["seller"].ToString();
                        byte[] pic = null;
                        string T = reader["tag"].ToString();

                        if (!reader.IsDBNull(reader.GetOrdinal("pic")))
                        {
                            pic = (byte[])reader["pic"];
                        }

                        listitem uis = new listitem();
                        uis.Name1 = N;
                        uis.Price = P;
                        uis.Icon = ByteArrayToImage(pic); 
                        uis.Info = I;
                        uis.Amount = A;
                        uis.Seller = S;
                        uis.Tag1 = T;
                        uis.Sc = _username;

                        productpanel.Controls.Add(uis);
                    }
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show("An error occurred during the search: " + ex.Message);
            }
            finally
            {
                conn.Close(); 
            }
        }


        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btupay_Click(object sender, EventArgs e)
        {
            
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (productpanel.Visible == false)
            {
                productpanel.Visible = true;
                userpanel.Visible = false;
                panel3.Visible = true;
            }
            else
            {
                productpanel.Visible = false;
                panel3.Visible = false;
            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            if (userpanel.Visible == false)
            {
                userpanel.Visible = true;
                productpanel.Visible = false;
                panel3.Visible = false;
            }
            else
            {
                userpanel.Visible = false;
            }
        }

        private void button4_Click_1(object sender, EventArgs e) //รีเซ้ต
        {
            pricebox.ClearSelected();
            pricebox.SelectedItem = null;
            tagbox.ClearSelected();
            tagbox.SelectedItem = null;
            boxbox.Clear();
            productpanel.Controls.Clear();
            showlist();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            login form2 = new login();
            form2.Show();
            this.Hide();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            usersell form5 = new usersell(_username);
            form5.Show();
            this.Hide();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            MainForm qr = new MainForm(_username);
            qr.Show();
            this.Hide();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void cibut_Click(object sender, EventArgs e)
        {

        }

        private void shippingitem_data()    //ข้อมูลสินค้ารอส่ง
        {
            using (MySqlConnection conn = databaseConnection())
            {
                try
                {
                    conn.Open();

                    string query = "SELECT itemid, owner, day, status, amount, orderid, shipstatus FROM cart WHERE owner = @username";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", _username);

                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable table = new DataTable();
                        adapter.Fill(table);

                        shipdata.DataSource = table;
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void guna2Button10_Click(object sender, EventArgs e)
        {
            
            
        }

        private void guna2Button9_Click(object sender, EventArgs e)
        {
           
        }
        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = shipdata.Rows[e.RowIndex];

                string shipStatus = row.Cells["shipstatus"].Value.ToString(); 
                string orderidtxt = row.Cells["orderid"].Value.ToString();   
                string itemidtxt = row.Cells["itemid"].Value.ToString();     

                itemidt.Text = itemidtxt;
                shipid.Text = shipStatus;  
                orderidt.Text = orderidtxt; 

                LoadItemINFO();
            }
        }


        private void UpdateItemStatus()
        {
            try
            {
                using (MySqlConnection conn = databaseConnection())
                {
                    conn.Open(); 

                    string checkQuery = "SELECT status FROM cart WHERE shipstatus = @od";
                    MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn); 
                    checkCmd.Parameters.AddWithValue("@od", shipid.Text); 

                    using (MySqlDataReader reader = checkCmd.ExecuteReader())
                    {
                      
                        if (reader.Read())
                        {
                            string currentStatus = reader["status"].ToString();
                            string updateStatus = ""; 

                            if (currentStatus == "ดำเนินการส่งแล้ว")
                            {
                                updateStatus = "การซื้อสำเร็จ";
                            }
                            else if (currentStatus == "ยกเลิกสินค้าแล้ว")
                            {
                                updateStatus = "ยกเลิกคำสั่งซื้อสำเร็จ";
                            }
                            else
                            {
                                MessageBox.Show("สถานะของสินค้าชิ้นนี้ไม่ตรงกับเงื่อนไขที่กำหนด");
                                return;
                            }

                            reader.Close(); 

                            string updateQuery = "UPDATE cart SET status = @status WHERE shipstatus = @od";
                            using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn)) 
                            {
                                updateCmd.Parameters.AddWithValue("@status", updateStatus);
                                updateCmd.Parameters.AddWithValue("@od", shipid.Text);

                                int rowsAffected = updateCmd.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Item status updated successfully");
                                    shippingitem_data();
                                }
                                else
                                {
                                    MessageBox.Show("Failed to update item status");
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("ไม่พบข้อมูลสำหรับคำสั่งซื้อที่ระบุ");
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error updating item status: " + ex.Message);
            }
        }



        private void guna2Button14_Click(object sender, EventArgs e)
        {
            string shipid = this.shipid.Text;

            if (!string.IsNullOrEmpty(shipid))
            {
                UpdateItemStatus();

                this.shipid.Text = "";
            }
            else
            {
                MessageBox.Show("No order number specified.");
            }
        }



        private void labelsentitem_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            productpanel.Controls.Clear();

            showlist();
            showsentitem();
            showcart();
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedStatus = listBox1.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(selectedStatus))
            {
                FilterShipDataByStatus(selectedStatus);
            }
        }

        private void FilterShipDataByStatus(string status)   //ตรวจสอบ user และ สถานะ
        {
            try
            {
                using (MySqlConnection conn = databaseConnection())
                {
                    conn.Open(); 

                    string query = "SELECT itemid, owner, day, status, amount, orderid, shipstatus FROM cart WHERE owner = @username AND status = @status";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", _username); 
                        cmd.Parameters.AddWithValue("@status", status); 

                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable table = new DataTable();
                        adapter.Fill(table); 

                        shipdata.DataSource = table;
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void orderid_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void uritempanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }
        private void LoadItemINFO()    //ดึงข้อมุูลสินค้ามาแสดง
        {
            using (MySqlConnection conn = databaseConnection())
            {
                try
                {
                    conn.Open(); 

                    string sql = "SELECT name, price, pic FROM item WHERE itemid = @itemid";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@itemid", itemidt.Text);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                nameNN.Text = reader["name"].ToString();
                                priceNN.Text = reader["price"].ToString();

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


        private void guna2Button7_Click(object sender, EventArgs e)
        {
            shipdata.DataSource = null;
            shippingitem_data();
        }
    }
}
