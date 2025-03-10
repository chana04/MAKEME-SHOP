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
    public partial class se : Form
    {
        public se()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            page1 form1 = new page1();
            form1.Show();
            this.Hide();
        }
    }
}
