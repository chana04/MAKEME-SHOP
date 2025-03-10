using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;


namespace second_hand_shops
{
    public partial class userbuy : Form
    {
        private string _itemid;
        private string _username;

        private MySqlConnection databaseConnection()
        {
            string connectionstring = "datasource=127.0.0.1;port=3306;username=root;password=;database=shop;";
            MySqlConnection conn = new MySqlConnection(connectionstring);
            return conn;
        }

        public userbuy(string itemid, string username)
        {
            InitializeComponent();

            _itemid = itemid;
            label0.Text = _itemid;
            this.username.Text = username;
            _username = username;
            aammboxlist.Text = "1";

            using (MySqlConnection conn = databaseConnection())
            {
         
                conn.Open();

                MySqlCommand command = new MySqlCommand("SELECT name, price, info, amount, seller, pic, pic2, pic3, tag FROM item WHERE itemid = @itemid", conn);

                command.Parameters.AddWithValue("@itemid", itemid);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        name.Text = reader.GetString("name"); 
                        price.Text = reader.GetDecimal("price").ToString(); 
                        info.Text = reader.GetString("info"); 
                        amount.Text = reader.GetInt32("amount").ToString(); 
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



        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = databaseConnection())
            {
                string itemId = label0.Text;
                string owner = username.Text;
                string status = "B"; 
                string amountText = aammboxlist.Text; 
                int amount;

                if (!int.TryParse(amountText, out amount))
                {
                    MessageBox.Show("Please enter a valid amount."); 
                    return; 
                }

                try
                {
                    conn.Open(); 

            
                    string checkCartItemQuery = "SELECT amount FROM cart WHERE itemid = @itemId AND owner = @owner AND status = @sta";
                    MySqlCommand checkCartItemCmd = new MySqlCommand(checkCartItemQuery, conn);
                    checkCartItemCmd.Parameters.AddWithValue("@itemId", itemId);
                    checkCartItemCmd.Parameters.AddWithValue("@owner", owner);
                    checkCartItemCmd.Parameters.AddWithValue("@sta", "B");
                    object cartResult = checkCartItemCmd.ExecuteScalar(); 

                    int cartQuantity = cartResult == null ? 0 : Convert.ToInt32(cartResult); 

                    string checkItemQuery = "SELECT amount FROM item WHERE itemid = @itemId";
                    MySqlCommand checkItemCmd = new MySqlCommand(checkItemQuery, conn);
                    checkItemCmd.Parameters.AddWithValue("@itemId", itemId);
                    object itemResult = checkItemCmd.ExecuteScalar();

                    if (itemResult == null)
                    {
                        MessageBox.Show("Item not found.");
                        return;
                    }

                    int availableQuantity = Convert.ToInt32(itemResult); 

                    int totalAmount = cartQuantity + amount;
                    if (totalAmount > availableQuantity)
                    {
                        MessageBox.Show("The total amount exceeds the available quantity.");
                        return;
                    }

                    if (totalAmount <= 0)
                    {
                        MessageBox.Show("The total amount must be greater than zero.");
                        return;
                    }

                    string query;
                    if (cartQuantity > 0)
                    {
                        query = "UPDATE cart SET amount = @Amount WHERE itemid = @ItemId AND owner = @Owner";
                    }
                    else
                    {
                        query = "INSERT INTO cart (itemid, owner, seller, day, price, status, amount) VALUES (@ItemId, @Owner, @Seller, @Day, @Price, @Status, @Amount)";
                    }

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ItemId", itemId);
                        cmd.Parameters.AddWithValue("@Owner", owner);
                        cmd.Parameters.AddWithValue("@Seller", seller.Text);
                        cmd.Parameters.AddWithValue("@Day", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Price", price.Text);
                        cmd.Parameters.AddWithValue("@Status", status);
                        cmd.Parameters.AddWithValue("@Amount", totalAmount);

                        cmd.ExecuteNonQuery(); 
                        MessageBox.Show("Data inserted successfully"); 

                        this.Hide(); 
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error: " + ex.Message); 
                }
                finally
                {
                    conn.Close(); 
                }
            }
        }


        private void guna2Button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void userbuy_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
