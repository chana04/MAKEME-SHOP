using QRCodeGenerator;
using System;
using System.Windows.Forms;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.text;
using MySql.Data.MySqlClient;
using System.Net;
using System.Net.Mail;
using Aspose.Email.Clients.Graph;
using System.Collections.Generic;
using System.Data;


namespace second_hand_shops
{

    public partial class userdone : Form
    {
        private string _username;
        private byte[] imageBytes;
        private int totalItems = 0;
        private double totalPrice = 0.0;

        public userdone(string username)
        {
            InitializeComponent();
            _username = username;
            name.Text = _username;

        }

        private MySqlConnection databaseConnection()
        {
            string connectionstring = "datasource=127.0.0.1;port=3306;username=root;password=;database=shop;";
            MySqlConnection conn = new MySqlConnection(connectionstring);
            return conn;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainForm qr = new MainForm(_username);
            qr.Show();
            this.Hide();
        }
        private void showcart(PdfPTable t)
        {
            try
            {
                using (MySqlConnection conn = databaseConnection())
                {
                    conn.Open();

                    string cartQuery = "SELECT itemid, amount FROM cart WHERE owner = @username AND status = 'B'";
                    MySqlCommand cartCommand = new MySqlCommand(cartQuery, conn);
                    cartCommand.Parameters.AddWithValue("@username", _username);

                    using (MySqlDataReader cartReader = cartCommand.ExecuteReader())
                    {
                        while (cartReader.Read())
                        {
                            int itemid = Convert.ToInt32(cartReader["itemid"]); 
                            int amount = Convert.ToInt32(cartReader["amount"]); 

                            using (MySqlConnection conn2 = databaseConnection())
                            {
                                conn2.Open();
                                string itemQuery = "SELECT name, price FROM item WHERE itemid = @itemid";
                                MySqlCommand itemCommand = new MySqlCommand(itemQuery, conn2);
                                itemCommand.Parameters.AddWithValue("@itemid", itemid);

 
                                using (MySqlDataReader itemReader = itemCommand.ExecuteReader())
                                {
                                    if (itemReader.Read())
                                    {
                                        string itemName = itemReader["name"].ToString(); 
                                        double itemPrice = Convert.ToDouble(itemReader["price"]); 

                                        // เพิ่มข้อมูลลงในตาราง PDF
                                        t.AddCell(itemid.ToString()); 
                                        t.AddCell(itemName); 
                                        t.AddCell(amount.ToString()); 
                                        t.AddCell(itemPrice.ToString()); 
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

        private void showtotal()
        {
            try
            {
                using (MySqlConnection conn = databaseConnection())
                {
            
                    conn.Open();

                    string countQuery = "SELECT IFNULL(SUM(c.amount), 0) AS totalItems FROM cart c WHERE c.owner = @username AND c.status = 'B'";
                    MySqlCommand countCommand = new MySqlCommand(countQuery, conn);
                    countCommand.Parameters.AddWithValue("@username", _username);
       
                    totalItems = Convert.ToInt32(countCommand.ExecuteScalar());

               
                    string priceQuery = "SELECT IFNULL(SUM(c.amount * i.price), 0) AS totalPrice FROM cart c INNER JOIN item i ON c.itemid = i.itemid WHERE c.owner = @username AND c.status = 'B'";
                    MySqlCommand priceCommand = new MySqlCommand(priceQuery, conn);
                    priceCommand.Parameters.AddWithValue("@username", _username);
         
                    object totalPriceObj = priceCommand.ExecuteScalar();
                    totalPrice = Convert.ToDouble(totalPriceObj);
                }
            }
            catch (MySqlException ex)
            {
           
                MessageBox.Show("An error occurred while fetching total data: " + ex.Message);
            }
        }

        private void recieptnow()
        {
      
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string loadFolderPath = Path.Combine(desktopPath, "load");

     
            if (!Directory.Exists(loadFolderPath))
            {
                Directory.CreateDirectory(loadFolderPath);
            }

            string outputFile = Path.Combine(loadFolderPath, "reciept.pdf");
            int fileCount = 1;

            while (File.Exists(outputFile))
            {
                outputFile = Path.Combine(loadFolderPath, $"reciept {fileCount}.pdf");
                fileCount++;
            }

            showtotal();

            double vatRate = 0.07; 
            double transportationFee = 45.0; 
            double vatAmount = totalPrice * vatRate; 
            double finalPrice = totalPrice + vatAmount + transportationFee; 

            using (FileStream fs = new FileStream(outputFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (Document doc = new Document(PageSize.A4)) 
                {
                    using (PdfWriter w = PdfWriter.GetInstance(doc, fs))
                    {
                        doc.Open();

                        string imagePath = @"C:\work\second hand shops\pic\seesee.PNG";
                        Image logo = Image.GetInstance(imagePath); 
                        logo.ScaleToFit(150f, 150f); 
                        logo.Alignment = Element.ALIGN_CENTER; 
                        doc.Add(logo); 

                        doc.Add(new Paragraph(" ")); 
                        doc.Add(new Paragraph(" ")); 

                        PdfPTable t = new PdfPTable(4);
                        t.WidthPercentage = 100; 

                        // เพิ่มหัวตาราง
                        t.AddCell(CreateCell("Item ID", true));
                        t.AddCell(CreateCell("Item Name", true));
                        t.AddCell(CreateCell("Item Amount", true));
                        t.AddCell(CreateCell("Item Price", true));

                        // เรียกฟังก์ชัน showcart เพื่อแสดงข้อมูลในตาราง
                        showcart(t);

                        doc.Add(t); 

                        doc.Add(new Paragraph(" ")); 

                        doc.Add(new Paragraph($"Total Items: {totalItems}"));
                        doc.Add(new Paragraph($"Total Price: {totalPrice:C}"));
                        doc.Add(new Paragraph($"VAT (7%): {vatAmount:C}"));
                        doc.Add(new Paragraph($"Transportation Fee: {transportationFee:C}"));
                        doc.Add(new Paragraph($"Final Price: {finalPrice:C}"));

                        doc.Add(new Paragraph(" "));
                        doc.Add(new Paragraph(" "));
                        doc.Add(new Paragraph(" "));
                        doc.Add(new Paragraph(" "));
                        doc.Add(new Paragraph(" "));
                        doc.Add(new Paragraph(" "));
                        doc.Add(new Paragraph(" "));
                        doc.Add(new Paragraph(" "));
                        doc.Add(new Paragraph(" "));
                        doc.Add(new Paragraph(" "));
                        doc.Add(new Paragraph(" "));
                        doc.Add(new Paragraph(" "));
                        doc.Add(new Paragraph(" "));

                        // แสดงที่อยู่และข้อมูลการติดต่อ
                        Paragraph address = new Paragraph("123 16, Mittraphap Road, Nai Mueang Sub-district\n" +
                            " Mueang District, Khon Kaen Province, 40002\n" +
                            "Phone: 043-009700, 043-002539\n" +
                            "Fax: 043-202216\n" +
                            "Email: info@kku.ac.th");
                        address.Alignment = Element.ALIGN_RIGHT; // จัดชิดขวาที่อยู่
                        doc.Add(address); // เพิ่มที่อยู่ในเอกสาร

                        doc.Close(); // ปิดเอกสาร
                    }
                }
            }

            // ส่งอีเมลพร้อมไฟล์แนบใบเสร็จ
            string attachmentPath = outputFile;
            SendEmailWithAttachment(attachmentPath);
        }

        private PdfPCell CreateCell(string text, bool isHeader)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text));
            cell.BorderWidth = 1f;
            cell.Padding = 5f;

            if (isHeader)
            {
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.BorderWidthBottom = 2f;
            }
            else
            {
                cell.BorderWidthBottom = 1f;
            }

            return cell;
        }

        private bool insertOrderToDatabase()
        {
            showtotal();
            string newOrderId = clearcart();

            if (string.IsNullOrEmpty(newOrderId))
            {
                return false;
            }

            try
            {
                if (string.IsNullOrWhiteSpace(textname.Text) ||
                    string.IsNullOrWhiteSpace(texthouse.Text) ||
                    string.IsNullOrWhiteSpace(textdistrict.Text) ||
                    string.IsNullOrWhiteSpace(textprovincenew.Text) ||
                    string.IsNullOrWhiteSpace(textcodenew.Text) ||
                    string.IsNullOrWhiteSpace(textroad.Text) ||
                    string.IsNullOrWhiteSpace(textcrash.Text) ||
                    string.IsNullOrWhiteSpace(textsoi.Text) ||
                    !int.TryParse(texthouse.Text, out int houseNum) || 
                    !int.TryParse(textcodenew.Text, out int code)) 
                {
                    MessageBox.Show("Please fill in all fields correctly.");
                    return false;
                }

                double vatRate = 0.07;
                double transportationFee = 45.0;
                double vatAmount = totalPrice * vatRate;
                double finalPrice = totalPrice + vatAmount + transportationFee;

                using (MySqlConnection conn = databaseConnection())
                {
                    conn.Open(); 

                    string insertQuery = @"
                INSERT INTO userinfo (username, name, housenum, district, province, code, date, ETC, pic, orderid, totalprice,status,road,crash,soi)
                VALUES (@username, @name, @housenum, @district, @province, @code, @date, @etc, @pic, @orderid, @totalprice,@s,@road,@crash,@soi)";

                    MySqlCommand cmd = new MySqlCommand(insertQuery, conn);

                    cmd.Parameters.AddWithValue("@username", _username);
                    cmd.Parameters.AddWithValue("@name", textname.Text);
                    cmd.Parameters.AddWithValue("@housenum", houseNum);
                    cmd.Parameters.AddWithValue("@district", textdistrict.Text);
                    cmd.Parameters.AddWithValue("@province", textprovincenew.Text);
                    cmd.Parameters.AddWithValue("@code", code);
                    cmd.Parameters.AddWithValue("@date", DateTime.Now); 
                    cmd.Parameters.AddWithValue("@etc", textETC.Text);
                    cmd.Parameters.AddWithValue("@pic", imageBytes);
                    cmd.Parameters.AddWithValue("@orderid", newOrderId);
                    cmd.Parameters.AddWithValue("@totalprice", finalPrice);
                    cmd.Parameters.AddWithValue("@s", "D");
                    cmd.Parameters.AddWithValue("@road", textroad.Text);
                    cmd.Parameters.AddWithValue("@crash", textcrash.Text);
                    cmd.Parameters.AddWithValue("@soi", textsoi.Text);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Order details have been successfully inserted into the database.");
                    return true;
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("An error occurred while inserting order data: " + ex.Message);
                return false;
            }
            catch (FormatException ex)
            {
               
                MessageBox.Show("Please ensure all inputs are in the correct format: " + ex.Message);
                return false;
            }
        }



        private string clearcart()  //ตรวจสอบสลีป
        {
            if (imageBytes == null || imageBytes.Length == 0)
            {
                MessageBox.Show("Payment slip not found.");
                return null; 
            }

            try
            {
                
                using (MySqlConnection conn = databaseConnection())
                {
                    conn.Open(); 

                    using (MySqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            string timestamp = DateTime.Now.ToString("mmssfff");
                            string newOrderId = $"{_username}_{timestamp}";

                            string cartQuery = "SELECT itemid FROM cart WHERE owner = @username AND status = @status";

                            List<int> itemIds = new List<int>(); 

                            using (MySqlCommand cartCommand = new MySqlCommand(cartQuery, conn, transaction))
                            {
                                cartCommand.Parameters.AddWithValue("@username", _username);
                                cartCommand.Parameters.AddWithValue("@status", "B"); 

                                using (MySqlDataReader cartReader = cartCommand.ExecuteReader())
                                {
                                    while (cartReader.Read())
                                    {
                                        int itemid = Convert.ToInt32(cartReader["itemid"]); 
                                        itemIds.Add(itemid); 
                                    }
                                }
                            }

                            foreach (int itemid in itemIds)
                            {
                                string timestamp1 = Guid.NewGuid().ToString("N").Substring(0, 8);
                                string newShipStatus = $"{timestamp1}{_username}";

                                string updateCartQuery = "UPDATE cart SET orderid = @newOrderId, shipstatus = @newShipStatus, time = @currentTime WHERE owner = @username AND itemid = @itemid AND status = @status";
                                using (MySqlCommand updateCartCommand = new MySqlCommand(updateCartQuery, conn, transaction))
                                {
                                    updateCartCommand.Parameters.AddWithValue("@newOrderId", newOrderId);
                                    updateCartCommand.Parameters.AddWithValue("@newShipStatus", newShipStatus);
                                    updateCartCommand.Parameters.AddWithValue("@currentTime", DateTime.Now.ToString("HH:mm:ss")); // เวลาปัจจุบัน
                                    updateCartCommand.Parameters.AddWithValue("@username", _username);
                                    updateCartCommand.Parameters.AddWithValue("@itemid", itemid);
                                    updateCartCommand.Parameters.AddWithValue("@status", "B");

                                    updateCartCommand.ExecuteNonQuery();
                                }
                            }

                            transaction.Commit();
                            MessageBox.Show("Cart status and ship status updated successfully.");
                            return newOrderId; 
                            //รีเทินไปเพิ่มข้อมูล
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show("An error occurred while clearing the cart: " + ex.Message);
                            return null; 
                        }
                    }
                }
            }
            catch (MySqlException mysqlEx)
            {
                MessageBox.Show("A database error occurred while clearing the cart: " + mysqlEx.Message);
                return null; 
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.ImageLocation = openFileDialog1.FileName;

                using (FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        imageBytes = br.ReadBytes((int)fs.Length);
                    }
                }
            }
        }

        private void SendEmailWithAttachment(string attachmentPath)
        {
            string fromAddress = "iceza55yoo@gmail.com"; 
            string recipientEmail = ""; 

  
            using (MySqlConnection connmail = databaseConnection())
            {
                connmail.Open(); 
      
                MySqlCommand commandmail = new MySqlCommand("SELECT email FROM user WHERE username = @seller", connmail);
                commandmail.Parameters.AddWithValue("@seller", name.Text);

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

            string toAddress = recipientEmail; 
            string subject = "Receipt Notification"; 
            string body = "Thank you for your purchase! Attached is your receipt for the transaction."; 

            using (MailMessage mail = new MailMessage(fromAddress, toAddress, subject, body))
            {
                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com"))
                {
                    smtpClient.Port = 587; 
                    smtpClient.Credentials = new NetworkCredential("iceza55yoo@gmail.com", "qffn xbcg zzor qmaw"); 
                    smtpClient.EnableSsl = true; 

                    Attachment attachment = new Attachment(attachmentPath); 
                    mail.Attachments.Add(attachment); 

                    try
                    {
                        smtpClient.Send(mail); 
                        MessageBox.Show("Email sent successfully.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed to send email: " + ex.Message);
                    }
                }
            }
        }

        private void UpdateCartAndItems()   //ตรวจสอบข้อมูลสินค้า ละเอียด
        {
            try
            {
                using (MySqlConnection conn = databaseConnection()) 
                {
                    conn.Open();
                    using (MySqlTransaction transaction = conn.BeginTransaction()) 
                    {
                        try
                        {
                            string checkQuery = "SELECT c.itemid, c.amount AS cartAmount, i.amount AS itemAmount " +
                                                "FROM cart c " +
                                                "INNER JOIN item i ON c.itemid = i.itemid " +
                                                "WHERE c.owner = @username AND c.status = @B";
                            MySqlCommand checkCommand = new MySqlCommand(checkQuery, conn, transaction);
                            checkCommand.Parameters.AddWithValue("@username", _username); 
                            checkCommand.Parameters.AddWithValue("@B", "B"); 

                            List<int> itemsToUpdateStatus = new List<int>(); 
                            Dictionary<int, int> itemAmounts = new Dictionary<int, int>(); 

                            using (MySqlDataReader checkReader = checkCommand.ExecuteReader()) 
                            {
                                while (checkReader.Read())
                                {
                                    int itemId = checkReader.GetInt32("itemid");
                                    int cartAmount = checkReader.GetInt32("cartAmount");
                                    int itemAmount = checkReader.GetInt32("itemAmount");

                                    if (cartAmount <= itemAmount)
                                    {
                                        itemsToUpdateStatus.Add(itemId); 
                                        itemAmounts[itemId] = cartAmount; 
                                    }
                                }
                            }

                            foreach (var item in itemAmounts)
                            {
                                int itemId = item.Key;
                                int cartAmount = item.Value;

                                string updateQuantityQuery = "UPDATE item SET amount = amount - @cartAmount WHERE itemid = @itemid";
                                MySqlCommand updateQuantityCommand = new MySqlCommand(updateQuantityQuery, conn, transaction);
                                updateQuantityCommand.Parameters.AddWithValue("@cartAmount", cartAmount);
                                updateQuantityCommand.Parameters.AddWithValue("@itemid", itemId);
                                updateQuantityCommand.ExecuteNonQuery();


                                string checkAmountQuery = "SELECT amount FROM item WHERE itemid = @itemid";
                                MySqlCommand checkAmountCommand = new MySqlCommand(checkAmountQuery, conn, transaction);
                                checkAmountCommand.Parameters.AddWithValue("@itemid", itemId);
                                int remainingAmount = Convert.ToInt32(checkAmountCommand.ExecuteScalar()); 


                                //จำนวนคงเหลือ
                                if (remainingAmount == 0)
                                {
                                    string updateStatusQuery = "UPDATE item SET status = 'OUT' WHERE itemid = @itemid";
                                    MySqlCommand updateStatusCommand = new MySqlCommand(updateStatusQuery, conn, transaction);
                                    updateStatusCommand.Parameters.AddWithValue("@itemid", itemId);
                                    updateStatusCommand.ExecuteNonQuery();
                                }
                            }

                            transaction.Commit(); 
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback(); 
                            MessageBox.Show("An error occurred while updating cart status: " + ex.Message);
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("A database error occurred: " + ex.Message);
            }
        }



        private void guna2Button1_Click(object sender, EventArgs e)
        {
            MainForm qr = new MainForm(_username);
            qr.Show();
            this.Hide();
        }


        private void UpdateCartStatus()
        {
            try
            {
                using (MySqlConnection conn = databaseConnection()) 
                {
                    conn.Open();

                    using (MySqlTransaction transaction = conn.BeginTransaction()) 
                    {
                        try
                        {
                            string updateQuery = "UPDATE cart SET status = 'รอการตรวจสอบ' WHERE owner = @username AND status = 'B'";

                            using (MySqlCommand updateCommand = new MySqlCommand(updateQuery, conn, transaction))
                            {
                                updateCommand.Parameters.AddWithValue("@username", _username); 
                                int rowsAffected = updateCommand.ExecuteNonQuery(); 

                                if (rowsAffected > 0) 
                                {
                                    userhome form4 = new userhome(_username); 
                                    form4.Show(); 
                                    this.Hide(); 
                                }
                                else
                                {
                                    MessageBox.Show("No items found to update."); 
                                }
                            }

                            transaction.Commit(); 
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback(); 
                            MessageBox.Show("An error occurred while updating cart status: " + ex.Message);
                        }
                    }
                }
            }
            catch (MySqlException mysqlEx)
            {
                MessageBox.Show("A database error occurred: " + mysqlEx.Message);
            }
        }


        private void guna2Button2_Click(object sender, EventArgs e)
        {
            bool orderInserted = insertOrderToDatabase(); 
            if (!orderInserted)
            {
                return; 
            }

            UpdateCartAndItems(); 
            recieptnow(); 
            UpdateCartStatus(); 
        }


        private void textcode_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void LoadUserInfoIfExists()
        {
            try
            {
                using (MySqlConnection conn = databaseConnection()) 
                {
                    conn.Open();

                    string query = "SELECT * FROM userinfo WHERE username = @username";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    adapter.SelectCommand.Parameters.AddWithValue("@username", name.Text); 
                    DataTable userInfoTable = new DataTable();

                    adapter.Fill(userInfoTable);

                    if (userInfoTable.Rows.Count > 0)
                    {
                        DataRow row = userInfoTable.Rows[0]; 

                        textname.Text = row["name"].ToString();
                        texthouse.Text = row["housenum"].ToString();
                        textdistrict.Text = row["district"].ToString();
                        textprovincenew.Text = row["province"].ToString();
                        textcodenew.Text = row["code"].ToString();
                        textsoi.Text = row["soi"].ToString();
                        textcrash.Text = row["crash"].ToString();
                        textroad.Text = row["road"].ToString();
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("An error occurred while loading user information: " + ex.Message);
            }
        }


        private void guna2Button3_Click(object sender, EventArgs e)
        {
            LoadUserInfoIfExists(); 
        }

    }
}