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
    public partial class cartitem : UserControl
    {
        public cartitem()
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
            set { _itemID = value; citemid.Text = value; }
        }

        [Category("Custom Props")]
        public string Itemamount
        {
            get { return _itemamount; }
            set { _itemamount = value; camount.Text = value; }
        }

        [Category("Custom Props")]
        public string Itemprice
        {
            get { return _itemprice; }
            set { _itemprice = value; citemprice.Text = value; }
        }


        [Category("Custom Props")]
        public string Itemuser
        {
            get { return _user; }
            set { _user = value; cowner.Text = value; }
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
            this.BackColor = Color.Silver;
        }

        private void ddb(object sender, EventArgs e)
        {
            try
            {
                string ID = ItemID;
                string usn = Itemuser;

                userdel form10 = new userdel(ID, usn);
                form10.Show();

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
