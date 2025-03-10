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
    public partial class page1 : Form
    {
        public page1()
        {
            InitializeComponent();
        }
        

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            login form2 = new login();
            form2.Show();
            this.Hide();

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            signin form3 = new signin();
            form3.Show();
            this.Hide();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            se form2 = new se();
            form2.Show();
            this.Hide();
        }
    }
}
