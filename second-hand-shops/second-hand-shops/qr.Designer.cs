namespace QRCodeGenerator
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.labelTotalItems = new System.Windows.Forms.Label();
            this.labelTotalPrice = new System.Windows.Forms.Label();
            this.labelhead = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.labelVAT = new System.Windows.Forms.Label();
            this.labelShipping = new System.Windows.Forms.Label();
            this.price = new System.Windows.Forms.Label();
            this.qrcodehere = new System.Windows.Forms.Label();
            this.guna2Button5 = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Button4 = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Button3 = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            this.qrCodePictureBox = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.qrCodePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.White;
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(396, 494);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(2, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 3;
            // 
            // labelTotalItems
            // 
            this.labelTotalItems.AutoSize = true;
            this.labelTotalItems.Font = new System.Drawing.Font("Bahnschrift", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTotalItems.ForeColor = System.Drawing.Color.Black;
            this.labelTotalItems.Location = new System.Drawing.Point(409, 44);
            this.labelTotalItems.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelTotalItems.Name = "labelTotalItems";
            this.labelTotalItems.Size = new System.Drawing.Size(83, 25);
            this.labelTotalItems.TabIndex = 4;
            this.labelTotalItems.Text = "amount";
            // 
            // labelTotalPrice
            // 
            this.labelTotalPrice.AutoSize = true;
            this.labelTotalPrice.Font = new System.Drawing.Font("Bahnschrift", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTotalPrice.ForeColor = System.Drawing.Color.Black;
            this.labelTotalPrice.Location = new System.Drawing.Point(413, 181);
            this.labelTotalPrice.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelTotalPrice.Name = "labelTotalPrice";
            this.labelTotalPrice.Size = new System.Drawing.Size(54, 25);
            this.labelTotalPrice.TabIndex = 5;
            this.labelTotalPrice.Text = "total";
            this.labelTotalPrice.Click += new System.EventHandler(this.labelTotalPrice_Click);
            // 
            // labelhead
            // 
            this.labelhead.AutoSize = true;
            this.labelhead.Font = new System.Drawing.Font("Bahnschrift", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelhead.ForeColor = System.Drawing.Color.Black;
            this.labelhead.Location = new System.Drawing.Point(409, 16);
            this.labelhead.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelhead.Name = "labelhead";
            this.labelhead.Size = new System.Drawing.Size(71, 25);
            this.labelhead.TabIndex = 9;
            this.labelhead.Text = "status";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // labelVAT
            // 
            this.labelVAT.AutoSize = true;
            this.labelVAT.Font = new System.Drawing.Font("Bahnschrift", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVAT.ForeColor = System.Drawing.Color.Black;
            this.labelVAT.Location = new System.Drawing.Point(409, 110);
            this.labelVAT.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelVAT.Name = "labelVAT";
            this.labelVAT.Size = new System.Drawing.Size(41, 25);
            this.labelVAT.TabIndex = 12;
            this.labelVAT.Text = "vat";
            this.labelVAT.Click += new System.EventHandler(this.labelVAT_Click);
            // 
            // labelShipping
            // 
            this.labelShipping.AutoSize = true;
            this.labelShipping.Font = new System.Drawing.Font("Bahnschrift", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelShipping.ForeColor = System.Drawing.Color.Black;
            this.labelShipping.Location = new System.Drawing.Point(409, 145);
            this.labelShipping.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelShipping.Name = "labelShipping";
            this.labelShipping.Size = new System.Drawing.Size(90, 25);
            this.labelShipping.TabIndex = 13;
            this.labelShipping.Text = "shipping";
            // 
            // price
            // 
            this.price.AutoSize = true;
            this.price.Font = new System.Drawing.Font("Bahnschrift", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.price.ForeColor = System.Drawing.Color.Black;
            this.price.Location = new System.Drawing.Point(409, 74);
            this.price.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.price.Name = "price";
            this.price.Size = new System.Drawing.Size(58, 25);
            this.price.TabIndex = 14;
            this.price.Text = "price";
            this.price.Click += new System.EventHandler(this.price_Click);
            // 
            // qrcodehere
            // 
            this.qrcodehere.AutoSize = true;
            this.qrcodehere.Font = new System.Drawing.Font("Bahnschrift", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.qrcodehere.ForeColor = System.Drawing.Color.Black;
            this.qrcodehere.Location = new System.Drawing.Point(475, 496);
            this.qrcodehere.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.qrcodehere.Name = "qrcodehere";
            this.qrcodehere.Size = new System.Drawing.Size(96, 16);
            this.qrcodehere.TabIndex = 11;
            this.qrcodehere.Text = "QR CODE HERE";
            this.qrcodehere.Visible = false;
            // 
            // guna2Button5
            // 
            this.guna2Button5.BackColor = System.Drawing.Color.Transparent;
            this.guna2Button5.BackgroundImage = global::second_hand_shops.Properties.Resources.next;
            this.guna2Button5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.guna2Button5.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button5.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button5.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2Button5.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2Button5.FillColor = System.Drawing.Color.Transparent;
            this.guna2Button5.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2Button5.ForeColor = System.Drawing.Color.White;
            this.guna2Button5.Location = new System.Drawing.Point(719, 507);
            this.guna2Button5.Name = "guna2Button5";
            this.guna2Button5.Size = new System.Drawing.Size(48, 45);
            this.guna2Button5.TabIndex = 19;
            this.guna2Button5.Click += new System.EventHandler(this.guna2Button5_Click);
            // 
            // guna2Button4
            // 
            this.guna2Button4.BackColor = System.Drawing.Color.White;
            this.guna2Button4.BackgroundImage = global::second_hand_shops.Properties.Resources.pay;
            this.guna2Button4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.guna2Button4.BorderThickness = 1;
            this.guna2Button4.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button4.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button4.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2Button4.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2Button4.FillColor = System.Drawing.Color.Transparent;
            this.guna2Button4.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2Button4.ForeColor = System.Drawing.Color.White;
            this.guna2Button4.Location = new System.Drawing.Point(689, 265);
            this.guna2Button4.Name = "guna2Button4";
            this.guna2Button4.Size = new System.Drawing.Size(78, 45);
            this.guna2Button4.TabIndex = 18;
            this.guna2Button4.Click += new System.EventHandler(this.guna2Button4_Click);
            // 
            // guna2Button3
            // 
            this.guna2Button3.BackgroundImage = global::second_hand_shops.Properties.Resources.back;
            this.guna2Button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.guna2Button3.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button3.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button3.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2Button3.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2Button3.FillColor = System.Drawing.Color.Transparent;
            this.guna2Button3.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2Button3.ForeColor = System.Drawing.Color.White;
            this.guna2Button3.Location = new System.Drawing.Point(5, 507);
            this.guna2Button3.Name = "guna2Button3";
            this.guna2Button3.Size = new System.Drawing.Size(78, 45);
            this.guna2Button3.TabIndex = 17;
            this.guna2Button3.Click += new System.EventHandler(this.guna2Button3_Click);
            // 
            // guna2Button1
            // 
            this.guna2Button1.BackColor = System.Drawing.Color.White;
            this.guna2Button1.BackgroundImage = global::second_hand_shops.Properties.Resources.cart;
            this.guna2Button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.guna2Button1.BorderThickness = 1;
            this.guna2Button1.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2Button1.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2Button1.FillColor = System.Drawing.Color.Transparent;
            this.guna2Button1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2Button1.ForeColor = System.Drawing.Color.White;
            this.guna2Button1.Location = new System.Drawing.Point(689, 433);
            this.guna2Button1.Name = "guna2Button1";
            this.guna2Button1.Size = new System.Drawing.Size(78, 45);
            this.guna2Button1.TabIndex = 15;
            this.guna2Button1.Click += new System.EventHandler(this.guna2Button1_Click);
            // 
            // qrCodePictureBox
            // 
            this.qrCodePictureBox.BackColor = System.Drawing.Color.Transparent;
            this.qrCodePictureBox.Location = new System.Drawing.Point(412, 250);
            this.qrCodePictureBox.Margin = new System.Windows.Forms.Padding(2);
            this.qrCodePictureBox.Name = "qrCodePictureBox";
            this.qrCodePictureBox.Size = new System.Drawing.Size(225, 244);
            this.qrCodePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.qrCodePictureBox.TabIndex = 0;
            this.qrCodePictureBox.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoEllipsis = true;
            this.label2.Font = new System.Drawing.Font("Bahnschrift", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(685, 313);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 42);
            this.label2.TabIndex = 20;
            this.label2.Text = "กดเพื่อสร้าง QR CODE";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label2.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoEllipsis = true;
            this.label3.Font = new System.Drawing.Font("Bahnschrift", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(684, 388);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 42);
            this.label3.TabIndex = 21;
            this.label3.Text = "กดเพื่อ RESET ตะกร้าสินค้า";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label3.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(796, 564);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.guna2Button4);
            this.Controls.Add(this.guna2Button5);
            this.Controls.Add(this.guna2Button1);
            this.Controls.Add(this.guna2Button3);
            this.Controls.Add(this.price);
            this.Controls.Add(this.labelShipping);
            this.Controls.Add(this.labelVAT);
            this.Controls.Add(this.qrcodehere);
            this.Controls.Add(this.labelhead);
            this.Controls.Add(this.labelTotalPrice);
            this.Controls.Add(this.qrCodePictureBox);
            this.Controls.Add(this.labelTotalItems);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainForm";
            this.Text = "qrcode";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.qrCodePictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox qrCodePictureBox;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelTotalItems;
        private System.Windows.Forms.Label labelTotalPrice;
        private System.Windows.Forms.Label labelhead;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label labelVAT;
        private System.Windows.Forms.Label labelShipping;
        private System.Windows.Forms.Label price;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
        private Guna.UI2.WinForms.Guna2Button guna2Button3;
        private Guna.UI2.WinForms.Guna2Button guna2Button4;
        private Guna.UI2.WinForms.Guna2Button guna2Button5;
        private System.Windows.Forms.Label qrcodehere;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}