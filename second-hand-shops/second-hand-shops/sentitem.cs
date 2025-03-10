using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace second_hand_shops
{
    public partial class sentitem : UserControl
    {
        public sentitem()
        {
            InitializeComponent();
        }

        #region Properties

        private string _itemID;
        private string _itemamount;
        private string _itemprice;
        private string _user;
        private Image _itemImage;





        [Category("Custom Props")]
        public string ItemID
        {
            get { return _itemID; }
            set { _itemID = value; sitemid.Text = value; }
        }

        [Category("Custom Props")]
        public string Itemamount
        {
            get { return _itemamount; }
            set { _itemamount = value; samount.Text = value; }
        }

        [Category("Custom Props")]
        public string Itemprice
        {
            get { return _itemprice; }
            set { _itemprice = value; sitemprice.Text = value; }
        }


        [Category("Custom Props")]
        public string Itemuser
        {
            get { return _user; }
            set { _user = value; sowner.Text = value; }
        }


        [Category("Custom Props")]
        public Image itemImage
        {
            get { return _itemImage; }
            set { _itemImage = value; icon.Image = value; }
        }

        #endregion

        private void adleave(object sender, EventArgs e)
        {
            this.BackColor = Color.White;
        }

        private void adenter(object sender, EventArgs e)
        {
            this.BackColor = Color.OldLace;
        }

        private void ddb(object sender, EventArgs e)
        {
            try
            {
                string ID = ItemID;
                string usn = Itemuser;

                Form1 form12 = new Form1(ID, usn);
                form12.Show();

                Form parentForm = this.FindForm();
                if (parentForm != null)
                {
                    parentForm.Hide();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
    }
}
