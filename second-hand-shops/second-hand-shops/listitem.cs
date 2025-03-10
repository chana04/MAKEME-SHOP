using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace second_hand_shops
{
    public partial class listitem : UserControl
    {
        private MySqlConnection databaseConnection()
        {
            string connectionstring = "datasource=127.0.0.1;port=3306;username=root;password=;database=shop;";
            MySqlConnection conn = new MySqlConnection(connectionstring);
            return conn;
        }

        public listitem()
        {
            InitializeComponent();
            this.DoubleClick -= doubleclick;
            this.DoubleClick += doubleclick;
        }

        #region Properties
        private string _price;
        private string _name;
        private string _info;
        private string _tag;
        private string _seller;
        private string _amount;
        private string _sc;
        private Image _icon;

        [Category("Custom Props")]
        public string Price
        {
            get { return _price; }
            set { _price = value; text1.Text = value; }
        }

        [Category("Custom Props")]
        public string Name1
        {
            get { return _name; }
            set { _name = value; text2.Text = value; }
        }

        [Category("Custom Props")]
        public string Info
        {
            get { return _info; }
            set { _info = value; text3.Text = value; }
        }

        [Category("Custom Props")]
        public string Tag1
        {
            get { return _tag; }
            set { _tag = value; text4.Text = value; }
        }

        [Category("Custom Props")]
        public string Seller
        {
            get { return _seller; }
            set { _seller = value; text5.Text = value; }
        }

        [Category("Custom Props")]
        public string Amount
        {
            get { return _amount; }
            set { _amount = value; text6.Text = value; }
        }

        [Category("Custom Props")]
        public string Sc
        {
            get { return _sc; }
            set { _sc = value; sctext.Text = value; }
        }

        [Category("Custom Props")]
        public Image Icon
        {
            get { return _icon; }
            set { _icon = value; pictureBox1.Image = value; }
        }
        #endregion

        private void panelleave(object sender, EventArgs e)
        {
            this.BackColor = Color.White;
        }

        private void panelenter(object sender, EventArgs e)
        {
            this.BackColor = Color.Silver;
        }



        private void doubleclick(object sender, EventArgs e)
        {
            using (MySqlConnection conn = databaseConnection())
            {
                conn.Open();

                string seller = Seller;
                string name = Name1;
                string usn = Sc;
                

                MySqlCommand command = new MySqlCommand("SELECT itemid FROM item WHERE seller = @seller AND name = @name", conn);
                command.Parameters.AddWithValue("@seller", seller);
                command.Parameters.AddWithValue("@name", name);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (!reader.IsDBNull(reader.GetOrdinal("itemid")))
                        {
                            int kitemid = reader.GetInt32(reader.GetOrdinal("itemid"));
                            userbuy form6 = new userbuy(kitemid.ToString(), usn);
                            form6.Show();
                            
                        }
                        else
                        {  
                        }
                    }
                }
                conn.Close();
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void listitem_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
