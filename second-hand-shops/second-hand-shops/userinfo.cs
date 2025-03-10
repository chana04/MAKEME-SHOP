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
    public partial class userinfo : UserControl
    {
        public userinfo()
        {
            InitializeComponent();
        }

        #region Properties
        private string _aname;
        private string _aid;
        private string _ainfo;
        private string _ausername;
        private string _atag;
        private string _aamount;
        private string _astatus;
        private string _aprice;
        private Image _aicon;

        [Category("Custom Props")]
        public string Aname
        {
            get { return _aname; }
            set { _aname = value; adminname.Text = value; }
        }

        [Category("Custom Props")]
        public string Aid
        {
            get { return _aid; }
            set { _aid = value; adminid.Text = value; }
        }

        [Category("Custom Props")]
        public string Ainfo
        {
            get { return _ainfo; }
            set { _ainfo = value; admininfo.Text = value; }
        }

        [Category("Custom Props")]
        public string Ausername
        {
            get { return _ausername; }
            set { _ausername = value; adminusername.Text = value; }
        }

        [Category("Custom Props")]
        public string Atag
        {
            get { return _atag; }
            set { _atag = value; admintag.Text = value; }
        }


        [Category("Custom Props")]
        public string Aamount
        {
            get { return _aamount; }
            set { _aamount = value; adminamount.Text = value; }
        }

        [Category("Custom Props")]
        public string Astatus
        {
            get { return _astatus; }
            set { _astatus = value; adminstatus.Text = value; }
        }

        [Category("Custom Props")]
        public string Aprice
        {
            get { return _aprice; }
            set { _aprice = value; adminprice.Text = value; }
        }


        [Category("Custom Props")]
        public Image Aicon
        {
            get { return _aicon; }
            set { _aicon = value; adminicon.Image = value; }
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

       private void ddclick(object sender, EventArgs e)
        {
            adminupdate form9 = new adminupdate(Aid);
            form9.Show();
            ((Form)this.TopLevelControl).Hide();
        }
    }
}
