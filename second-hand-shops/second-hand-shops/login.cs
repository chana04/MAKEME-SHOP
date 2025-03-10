using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace second_hand_shops
{
    public partial class login : Form
    {
        private MySqlConnection databaseConnection()
        {
            string connectionstring = "datasource=127.0.0.1;port=3306;username=root;password=;database=shop;"; 
            MySqlConnection conn = new MySqlConnection(connectionstring); 
            return conn; 
        }

        public login()
        {
            InitializeComponent(); 
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked) 
            {
                lpassword.UseSystemPasswordChar = true; 
            }
            else 
            {
                lpassword.UseSystemPasswordChar = false; 
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
            string username = lusername.Text; 
            string password = lpassword.Text; 

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password)) 
            {
                MessageBox.Show("Please fill in both username and password."); 
                return; 
            }

            if (username == "admin666" && password == "2544")
            {
                MessageBox.Show("Admin mode activated."); 
                Form8 form8 = new Form8(); 
                form8.Show(); 
                this.Hide(); 
                return; 
            }

            using (MySqlConnection conn = databaseConnection())
            {
                conn.Open(); 

                string query = "SELECT COUNT(*) FROM user WHERE username = @Username AND password = @Password";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username); 
                    cmd.Parameters.AddWithValue("@Password", password); 

                    int count = Convert.ToInt32(cmd.ExecuteScalar()); 

                    if (count > 0) 
                    {
                        MessageBox.Show("Login successful!"); 

                        userhome form4 = new userhome(username); 
                        form4.Show(); 
                        this.Hide(); 
                    }
                    else 
                    {
                        MessageBox.Show("Invalid username or password."); 
                    }
                }
            }
        }
    }
}
