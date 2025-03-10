using MySql.Data.MySqlClient;
using QRCoder;
using Saladpuk.EMVCo.Contracts;
using Saladpuk.PromptPay.Facades;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;


namespace second_hand_shops
{
    public partial class Form8 : Form
    {
        public Form8()
        {
            InitializeComponent();
            showyourlist();

            checkstatus.CellClick += checkstatus_CellClick;
            checkbill.CellClick += checkbill_CellClick;

            LoadAllUserInfo();
        }

        private MySqlConnection databaseConnection()
        {
            string connectionstring = "datasource=127.0.0.1;port=3306;username=root;password=;database=shop;";
            MySqlConnection conn = new MySqlConnection(connectionstring);
            return conn;
        }

        private void showyourlist() // ฟังก์ชันแสดงรายการสินค้าที่มีสถานะ 'WAIT'
        {
            try
            {
                using (MySqlConnection conad1 = databaseConnection()) 
                {
                    conad1.Open(); 

                    // ดึงข้อมูลสินค้าที่มีสถานะ 'WAIT'
                    string qad1 = "SELECT itemid, name, seller, price, info, amount, seller, pic, pic2, pic3, tag, status FROM item WHERE status = 'WAIT'";
                    MySqlCommand comad1 = new MySqlCommand(qad1, conad1); 

                    using (MySqlDataReader rea1 = comad1.ExecuteReader()) 
                    {
                        while (rea1.Read()) 
                        {
                         
                            string idadmin = rea1["itemid"].ToString();
                            string namedmin = rea1["name"].ToString();
                            string priceadmin = rea1["price"].ToString();
                            string infoadmin = rea1["info"].ToString();
                            string amountadmin = rea1["amount"].ToString();
                            string statusadmin = rea1["status"].ToString();
                            string selleradmin = rea1["seller"].ToString();
                            byte[] picadmin = null;

                            if (!rea1.IsDBNull(rea1.GetOrdinal("pic")))
                            {
                                picadmin = (byte[])rea1["pic"];
                            }

                            userinfo fk = new userinfo();
                            fk.Aid = idadmin; 
                            fk.Aname = namedmin; 
                            fk.Aprice = priceadmin; 
                            fk.Ainfo = infoadmin; 
                            fk.Aamount = amountadmin; 
                            fk.Astatus = statusadmin; 
                            fk.Ausername = selleradmin; 

                            fk.Aicon = ByteArrayToImage(picadmin);

                            adminflow.Controls.Add(fk);
                        }
                    }
                }
            }
            catch (MySqlException ex) 
            {
                MessageBox.Show("An error occurred during command execution: " + ex.Message);
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



        private void LoadAllUserInfo() // ฟังก์ชันสำหรับโหลดข้อมูลผู้ใช้ทั้งหมดที่มีสถานะ 'D'
        {
            try
            {
                using (MySqlConnection conn = databaseConnection()) 
                {
                    conn.Open(); 

                    //ดึงข้อมูลผู้ใช้ที่มีสถานะ 'D'
                    string query = "SELECT username, name, housenum, district, province, code, date, ETC, pic, orderid, totalprice FROM userinfo WHERE status = 'D'";
                    MySqlCommand cmd = new MySqlCommand(query, conn); 
                    cmd.Parameters.AddWithValue("@d", null); 

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable userTable = new DataTable(); 
                    adapter.Fill(userTable); 

                    //เอาข้อมูลใส่ใน datagrid
                    checkstatus.DataSource = userTable;
                }
            }
            catch (MySqlException ex) 
            {
                MessageBox.Show("An error occurred while fetching user info: " + ex.Message);
            }
        }


        private void LoadCartData() // ฟังก์ชันสำหรับโหลดข้อมูลจากตะกร้าสินค้า
        {
            try
            {
                using (MySqlConnection conn = databaseConnection()) 
                {
                    conn.Open(); 

                    //ดึงข้อมูลจากตะกร้าาที่มีสถานะ 'รอการตรวจสอบ'
                    string query = "SELECT orderid, itemid, amount, time, status, shipstatus FROM cart WHERE status = 'รอการตรวจสอบ'";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn); 

                    DataTable dataTable = new DataTable(); 
                    adapter.Fill(dataTable);

                    //เอาข้อมูลใส่ใน datagrid
                    checkbill.DataSource = dataTable;
                }
            }
            catch (MySqlException ex) 
            {
                MessageBox.Show("An error occurred while fetching cart data: " + ex.Message);
            }
        }


        private void LoadCartData(string orderId) // ฟังก์ชันสำหรับโหลดข้อมูลจากตะกร้าสินค้าตาม orderid ที่กำหนด
        {
            try
            {
                using (MySqlConnection conn = databaseConnection()) 
                {
                    conn.Open(); 
                    // ดึงข้อมูลจากcart ด้วย orderid และ มีสถานะ 'รอการตรวจสอบ'
                    string query = "SELECT orderid, itemid, amount, time, status, shipstatus FROM cart WHERE orderid = @orderId AND status = 'รอการตรวจสอบ'";
                    MySqlCommand cmd = new MySqlCommand(query, conn); 
                    cmd.Parameters.AddWithValue("@orderId", orderId); 

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd); 
                    DataTable cartTable = new DataTable(); 
                    adapter.Fill(cartTable);

                    //เอาข้อมูลใส่ใน datagrid
                    checkbill.DataSource = cartTable;
                }
            }
            catch (MySqlException ex) 
            {
                MessageBox.Show("An error occurred while fetching cart data: " + ex.Message);
            }
        }

        // คลิก datagrid checkstatus
        private void checkstatus_CellClick(object sender, DataGridViewCellEventArgs e) 
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow row = checkstatus.Rows[e.RowIndex]; 

                // ดึงค่า orderid และ totalprice 
                string orderId = row.Cells["orderid"].Value?.ToString();
                string ttprice = row.Cells["totalprice"].Value?.ToString();

                orderidtxt.Text = orderId;
                totalpricetxt.Text = ttprice;

                if (row.Cells["pic"].Value != DBNull.Value)
                {
                    byte[] picBytes = (byte[])row.Cells["pic"].Value; 
                    pictureBox1.Image = ByteArrayToImage(picBytes); 
                }
                else
                {
                    pictureBox1.Image = null; 
                }

                if (!string.IsNullOrEmpty(orderId))
                {
                    LoadCartData(orderId); 
                }
            }
        }

        // คลิก datagrid checkbill
        private void checkbill_CellClick(object sender, DataGridViewCellEventArgs e) 
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow row = checkbill.Rows[e.RowIndex]; 

                string timestamp = row.Cells["time"].Value?.ToString();

                timetxt.Text = timestamp;
            }
        }


        //กดปุ่ม
        private void guna2Button3_Click(object sender, EventArgs e) 
        {
            if (panel1.Visible == false)
            {
                panel1.Visible = true; 
                panel1.BringToFront(); 
                adminflow.Visible = false; 
                panel2.Visible = false; 
                LoadCartData(); 
            }
            else
            {
                panel1.Visible = false; 
            }
        }

        private void guna2Button4_Click(object sender, EventArgs e) 
        {
            page1 form1 = new page1(); 
            form1.Show(); 
            this.Hide(); 
        }

        // โหลดคำสั่งซื้อสมบูรณ์
        private void LoadCompletedAndCancelledOrders() 
        {
            try
            {
                using (MySqlConnection conn = databaseConnection()) 
                {
                    conn.Open();

                    // ดึงข้อมูลสถานะ การซื้อสำเร็จ ยกเลิกคำสั่งซื้อสำเร็จ
                    string query = "SELECT seller,orderid, itemid, amount, time, status, shipstatus FROM cart WHERE status IN ('การซื้อสำเร็จ', 'ยกเลิกคำสั่งซื้อสำเร็จ')";
                    MySqlCommand cmd = new MySqlCommand(query, conn); 
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd); 

                    DataTable dataTable = new DataTable(); 
                    adapter.Fill(dataTable);

                    //เอาข้อมูลใส่ใน datagrid
                    payadmin.DataSource = dataTable; 
                }
            }
            catch (MySqlException ex) 
            {
                MessageBox.Show("An error occurred while fetching completed and cancelled orders: " + ex.Message);
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e) 
        {
            if (panel2.Visible == false)
            {
                panel2.Visible = true; 
                panel2.BringToFront(); 
                adminflow.Visible = false; 
                panel1.Visible = false; 
                LoadCompletedAndCancelledOrders(); 
            }
            else
            {
                panel2.Visible = false; 
            }
        }

        private void searchbox_SelectedIndexChanged(object sender, EventArgs e) 
        {
            // ตรวจสอบว่า searchbox ไม่มีค่าหรือไม่
            if (string.IsNullOrEmpty(searchbox.Text))
            {
                LoadCompletedAndCancelledOrders(); // ถ้าไม่มีค่าให้โหลดข้อมูลคำสั่งซื้อที่เสร็จสมบูรณ์และยกเลิก
                return;
            }

            using (MySqlConnection conn = databaseConnection()) 
            {
                conn.Open();

                string query = "SELECT seller, orderid, itemid, amount, time, status, shipstatus FROM cart WHERE status = @sc";
                MySqlCommand cmd = new MySqlCommand(query, conn); 
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd); 
                cmd.Parameters.AddWithValue("@sc", searchbox.Text); 

                if (searchbox.Text == "การซื้อสำเร็จ") //panel sell
                {
                    if (sell.Visible == false) 
                    {
                        sell.Visible = true; 
                    }
                }
                else
                {
                    sell.Visible = false; 
                }

                if (searchbox.Text == "ยกเลิกคำสั่งซื้อสำเร็จ") //panel cus
                {
                    if (cus.Visible == false) 
                    {
                        cus.Visible = true; 
                    }
                }
                else
                {
                    cus.Visible = false; 
                }

                DataTable dataTable = new DataTable(); 
                adapter.Fill(dataTable);

                //เอาข้อมูลใส่ใน datagrid
                payadmin.DataSource = dataTable;
            }
        }

 
        private void payadmin_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow row = payadmin.Rows[e.RowIndex];

                string orderId = row.Cells["seller"].Value?.ToString();

                nameup.Text = orderId;

                loaduserinfo();
                loadcustomer();
            }
        }

        private void loaduserinfo() //ดึงข้อมูลผู้ใช้ตามชื่อผู้ขาย
        {
            try
            {
                using (MySqlConnection conn = databaseConnection())
                {
                    conn.Open();

                    string query = "SELECT * FROM user WHERE username = @sellerdog";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@sellerdog", nameup.Text);

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable cartTable = new DataTable();
                    adapter.Fill(cartTable);

                    //เอาข้อมูลใส่ใน datagrid
                    usinfo.DataSource = cartTable;
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("An error occurred while fetching cart data: " + ex.Message);
            }
        }

        // ดึงข้อมูลการซื้อสำเร็จ
        private void loadmoney()
        {
            try
            {
                using (MySqlConnection conn = databaseConnection())
                {
                    conn.Open();

                    string query = "SELECT * FROM cart  WHERE seller = @sellerdog AND status = @statusdog";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@sellerdog", namesf.Text);
                    cmd.Parameters.AddWithValue("@statusdog", "การซื้อสำเร็จ");

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable cartTable = new DataTable();
                    adapter.Fill(cartTable);

                    //เอาข้อมูลใส่ใน datagrid
                    ustotal.DataSource = cartTable;
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("An error occurred while fetching cart data: " + ex.Message);
            }
        }
        // ดึงข้อมูลการซื้อที่ไม่สำเร็จ
        private void loadcustomer()
        {
            try
            {
                using (MySqlConnection conn = databaseConnection())
                {
                    conn.Open();

                    string query = "SELECT * FROM cart WHERE seller = @sellerdog AND status = @statusdog";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@sellerdog", nameup.Text);
                    cmd.Parameters.AddWithValue("@statusdog", "ยกเลิกคำสั่งซื้อสำเร็จ");

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable cartTable = new DataTable();
                    adapter.Fill(cartTable);

                    //เอาข้อมูลใส่ใน datagrid
                    infocancle.DataSource = cartTable;
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("An error occurred while fetching cart data: " + ex.Message);
            }
        }

        // ดูยอดรวมที่ต้องจ่าย
        private void totalmoney()
        {
            {
                using (MySqlConnection conn = databaseConnection())
                {
                    try
                    {
                        conn.Open();

                        string sql = "SELECT price, amount FROM cart WHERE seller = @use AND status = @Uid";
                        MySqlCommand cmd = new MySqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@use", nameup.Text);
                        cmd.Parameters.AddWithValue("@Uid", "การซื้อสำเร็จ");

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            decimal totalPrice = 0; 

                            while (reader.Read())
                            {
                                decimal price = reader.GetDecimal(reader.GetOrdinal("price")); 
                                int amount = reader.GetInt32(reader.GetOrdinal("amount")); 
                                totalPrice += price * amount; 
                            }

                            // แสดงยอดรวมใน totalm
                            totalm.Text = totalPrice.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        // แสดงข้อความแสดงข้อผิดพลาดถ้าเกิดข้อผิดพลาดระหว่างการดึงข้อมูล
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        // ปิดการเชื่อมต่อกับฐานข้อมูล
                        conn.Close();
                    }
                }
            }
        }


        private void qrgen()
        {
            try
            {
                string Amountringking = totalm.Text; // รับจำนวนเงินที่ต้องการชำระ
                double amount;

                if (!double.TryParse(totalm.Text.Replace("Total Cost : ", ""), out amount))
                {
                    MessageBox.Show("Invalid amount. Please enter a valid number."); 
                    return; 
                }

                string qrData = PPay.StaticQR.MobileNumber(tel.Text).Amount(amount).CreateCreditTransferQrCode();

                QRCoder.QRCodeGenerator qrGenerator = new QRCoder.QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrData, QRCoder.QRCodeGenerator.ECCLevel.H);
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(20); 

                pictureBox2.Image = qrCodeImage;

                IPromptPayQrInfo promptPayInfo = PPay.Reader.ReadQrPromptPay(qrData);
            }
            catch (Exception ex)
            {
              
                MessageBox.Show("An error occurred while generating QR code: " + ex.Message);
                panel3.Visible = false; 
            }
        }

        //อัปเดตสถานะคำสั่งซื้อ
        private void guna2Button6_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = databaseConnection()) 
            {
                try
                {
                    conn.Open(); 

                   
                    string sql = @"
            UPDATE cart 
            SET status = CASE 
                WHEN status = 'การซื้อสำเร็จ' THEN 'รายการสำเร็จ'
                WHEN status = 'ยกเลิกคำสั่งซื้อสำเร็จ' THEN 'ยกเลิกรายการสำเร็จ'
            END 
            WHERE seller = @namemt AND status IN ('การซื้อสำเร็จ', 'ยกเลิกคำสั่งซื้อสำเร็จ')";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@namemt", nameup.Text); 

                    int rowsAffected = cmd.ExecuteNonQuery(); 

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("อัพเดทสำเร็จ");
                        payadmin.DataSource = null;
                        usinfo.DataSource = null;
                        ustotal.DataSource = null;
                        LoadCompletedAndCancelledOrders(); 
                        panel3.Visible = false; 
                                                
                        nameup.Text = ".";
                        tel.Text = ".";
                        namesf.Text = ".";
                        totalm.Text = ".";
                    }
                    else
                    {
                        MessageBox.Show("ไม่มีข้อมูลที่ตรงกับเงื่อนไข");
                    }
                }
                catch (Exception ex)
                {
                    
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        //สร้าง qr 
        private void guna2Button7_Click(object sender, EventArgs e)
        {
            if (panel3.Visible == false)
            {
                panel3.Visible = true; 

                totalmoney(); 
                qrgen(); 
            }
            else
            {
                panel3.Visible = false; 
            }
        }

        //คลิก datagrid
        private void dataGridView5_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow row = infocancle.Rows[e.RowIndex];

                string sellerId = row.Cells["owner"].Value?.ToString();
                string orderId = row.Cells["price"].Value?.ToString();

                namesf.Text = sellerId;
                totalm.Text = orderId;

                loaduserinfo2();
                loadcustomer();
            }
        }

        //ดึง user ที่ตรงกับ namef
        private void loaduserinfo2()
        {
            try
            {
                using (MySqlConnection conn = databaseConnection())
                {
                    conn.Open(); 

                    string query = "SELECT * FROM user WHERE username = @sellerdog";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@sellerdog", namesf.Text); 

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable cartTable = new DataTable();
                    adapter.Fill(cartTable); 

                    //เติมข้อมูลใส่ datagrid
                    datacancle.DataSource = cartTable; 
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("An error occurred while fetching cart data: " + ex.Message);
            }
        }

        private void datacancle_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow row = datacancle.Rows[e.RowIndex];

                string telle = row.Cells["telephone"].Value?.ToString();

                tel.Text = telle;
            }
        }

        private void infocancle_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        private void usinfo_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow row = usinfo.Rows[e.RowIndex];

                string username = row.Cells["username"].Value?.ToString();
                string telephone = row.Cells["telephone"].Value?.ToString();

             
                namesf.Text = username;
                tel.Text = telephone;

                loadmoney();
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = databaseConnection())
            {
                try
                {
                    conn.Open(); 

                    string sql = @"
            UPDATE cart 
            SET status = 'รอการตรวจสอบจากผู้ขาย'
            WHERE orderid = @orderid";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@orderid", orderidtxt.Text); 

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("สถานะได้รับการอัพเดทเป็น 'รอการตรวจสอบจากผู้ขาย'");

                        string sql2 = @"
                UPDATE userinfo
                SET status = 'P'
                WHERE orderid = @orderid";

                        MySqlCommand cmd2 = new MySqlCommand(sql2, conn);
                        cmd2.Parameters.AddWithValue("@orderid", orderidtxt.Text);
                        cmd2.ExecuteNonQuery(); 

                        checkstatus.Controls.Clear();
                        checkbill.Controls.Clear();

                        LoadAllUserInfo();
                        LoadCartData();
                    }
                    else
                    {
                        MessageBox.Show("ไม่มีข้อมูลที่ตรงกับเงื่อนไข");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }


        
        private void guna2Button8_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = databaseConnection())
            {
                try
                {
                    conn.Open(); 

                    string sql = @"
            UPDATE cart 
            SET status = 'ยกเลิกสินค้าแล้ว'
            WHERE orderid = @orderid";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@orderid", orderidtxt.Text);

                    int rowsAffected = cmd.ExecuteNonQuery(); 

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("สถานะได้รับการอัพเดทเป็น 'ยกเลิกสินค้าแล้ว'");

                        string sql2 = @"
                UPDATE userinfo
                SET status = 'P'
                WHERE orderid = @orderid";

                        MySqlCommand cmd2 = new MySqlCommand(sql2, conn);
                        cmd2.Parameters.AddWithValue("@orderid", orderidtxt.Text);
                        cmd2.ExecuteNonQuery(); 

                        checkstatus.Controls.Clear();
                        checkbill.Controls.Clear();
                        LoadAllUserInfo(); 
                        LoadCartData(); 
                    }
                    else
                    {
                        MessageBox.Show("ไม่มีข้อมูลที่ตรงกับเงื่อนไข");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            if (adminflow.Visible == false)
            {
                adminflow.Controls.Clear();
                adminflow.Visible = true; 
                adminflow.BringToFront(); 
                panel1.Visible = false; 
                panel2.Visible = false; 
                showyourlist(); 
            }
            else
            {
                adminflow.Visible = false; 
            }
        }
    }
}