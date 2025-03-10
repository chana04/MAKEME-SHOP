using MySql.Data.MySqlClient;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace second_hand_shops
{
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
            this.DoubleClick -= ddb;
            this.DoubleClick += ddb;
        }

        #region Properties

        private string _itemName;
        private string _itemID;
        private string _itemamount;
        private string _utext;
        private string _ustatus;
        private Image _itemImage;


        [Category("Custom Props")]
        public string ItemName
        {
            get { return _itemName; }
            set { _itemName = value; name.Text = value; }
        }

        [Category("Custom Props")]
        public string ItemID
        {
            get { return _itemID; }
            set { _itemID = value; id.Text = value; }
        }

        [Category("Custom Props")]
        public string Itemamount
        {
            get { return _itemamount; }
            set { _itemamount = value; amount.Text = value; }
        }

        [Category("Custom Props")]
        public string Utext
        {
            get { return _utext; }
            set { _utext = value; utext.Text = value; }
        }


        [Category("Custom Props")]
        public string Itemstatus
        {
            get { return _ustatus; }
            set { _ustatus = value; sta.Text = value; }
        }


        [Category("Custom Props")]
        public Image itemImage
        {
            get { return _itemImage; }
            set { _itemImage = value; pictureBox1.Image = value; }
        }

        #endregion

        private void pl(object sender, EventArgs e)
        {
            this.BackColor = Color.White;
        }

        private void pe(object sender, EventArgs e)
        {
            this.BackColor = Color.Silver;
        }

        private void ddb(object sender, EventArgs e)
        {
            try
            {
                string ID = ItemID;
                string usn = Utext;

                userupdate form7 = new userupdate(ID, usn);
                form7.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }


    }
}
