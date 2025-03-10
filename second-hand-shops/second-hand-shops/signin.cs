using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace second_hand_shops
{
    public partial class signin : Form
    {
        private MySqlConnection databaseConnection()
        {
            string connectionstring = "datasource=127.0.0.1;port=3306;username=root;password=;database=shop;";
            MySqlConnection conn = new MySqlConnection(connectionstring);
            return conn;
        }

        public signin()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email && email.EndsWith(".com", StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhoneNumber(string phone)  //เช็คเบอร์
        {
            return Regex.IsMatch(phone, @"^\d{10}$");
        }

        private bool IsValidUsername(string username) //เช็ค username 
        {
            return username.Length <= 12 && Regex.IsMatch(username, @"^[A-Za-z0-9]+$");
        }

        private bool IsEmailExists(string email) //เข็คเมลซ้ำ
        {
            using (MySqlConnection conn = databaseConnection())
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM user WHERE email = @Email";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            page1 form1 = new page1();
            form1.Show();
            this.Hide();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            string Email = email.Text;
            string Password = pass.Text;
            string CPassword = cpass.Text;
            string Telephone = tel.Text;
            string Username = username.Text;

            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(CPassword) || string.IsNullOrWhiteSpace(Telephone) ||
                string.IsNullOrWhiteSpace(Username))
            {
                MessageBox.Show("Please fill in all the fields.");
                return;
            }

            if (!IsValidEmail(Email))
            {
                MessageBox.Show("Invalid email format.");
                return;
            }

            if (Password != CPassword)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            if (!IsValidPhoneNumber(Telephone))
            {
                MessageBox.Show("Invalid phone number format. Please enter a 10-digit numeric phone number.");
                return;
            }

            if (!IsValidUsername(Username))
            {
                MessageBox.Show("Invalid username format. Username must be at most 12 characters long and contain only letters and numbers.");
                return;
            }

            if (IsEmailExists(Email))
            {
                MessageBox.Show("Email already exists. Please use a different email address.");
                return;
            }

            using (MySqlConnection conn = databaseConnection())
            {
                string query = "INSERT INTO user (email, username, password, telephone) VALUES (@Email, @Username, @Password, @Telephone)";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", Email);
                    cmd.Parameters.AddWithValue("@Username", Username);
                    cmd.Parameters.AddWithValue("@Password", Password);
                    cmd.Parameters.AddWithValue("@Telephone", Telephone);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Data inserted successfully");
                        page1 form1 = new page1();
                        form1.Show();
                        this.Hide();
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

        private void signin_Load(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void tel_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void username_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void cpass_TextChanged(object sender, EventArgs e)
        {

        }

        private void pass_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void email_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
