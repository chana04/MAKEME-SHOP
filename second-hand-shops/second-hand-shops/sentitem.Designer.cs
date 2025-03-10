namespace second_hand_shops
{
    partial class sentitem
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.sowner = new System.Windows.Forms.Label();
            this.samount = new System.Windows.Forms.Label();
            this.sitemprice = new System.Windows.Forms.Label();
            this.sitemid = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.icon = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.icon)).BeginInit();
            this.SuspendLayout();
            // 
            // sowner
            // 
            this.sowner.Font = new System.Drawing.Font("Arial Rounded MT Bold", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sowner.Location = new System.Drawing.Point(270, 97);
            this.sowner.Name = "sowner";
            this.sowner.Size = new System.Drawing.Size(324, 20);
            this.sowner.TabIndex = 17;
            this.sowner.Text = "text";
            // 
            // samount
            // 
            this.samount.Font = new System.Drawing.Font("Arial Rounded MT Bold", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.samount.Location = new System.Drawing.Point(237, 69);
            this.samount.Name = "samount";
            this.samount.Size = new System.Drawing.Size(357, 20);
            this.samount.TabIndex = 16;
            this.samount.Text = "text";
            // 
            // sitemprice
            // 
            this.sitemprice.Font = new System.Drawing.Font("Arial Rounded MT Bold", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sitemprice.Location = new System.Drawing.Point(224, 38);
            this.sitemprice.Name = "sitemprice";
            this.sitemprice.Size = new System.Drawing.Size(370, 20);
            this.sitemprice.TabIndex = 15;
            this.sitemprice.Text = "text";
            // 
            // sitemid
            // 
            this.sitemid.Font = new System.Drawing.Font("Arial Rounded MT Bold", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sitemid.Location = new System.Drawing.Point(224, 10);
            this.sitemid.Name = "sitemid";
            this.sitemid.Size = new System.Drawing.Size(370, 20);
            this.sitemid.TabIndex = 14;
            this.sitemid.Text = "text";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial Rounded MT Bold", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(155, 97);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 20);
            this.label4.TabIndex = 13;
            this.label4.Text = "Cart owner :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial Rounded MT Bold", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(155, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 20);
            this.label3.TabIndex = 12;
            this.label3.Text = "Amount :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial Rounded MT Bold", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(155, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 20);
            this.label2.TabIndex = 11;
            this.label2.Text = "Price :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(155, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 20);
            this.label1.TabIndex = 10;
            this.label1.Text = "Itemid :";
            // 
            // icon
            // 
            this.icon.Location = new System.Drawing.Point(0, 0);
            this.icon.Name = "icon";
            this.icon.Size = new System.Drawing.Size(149, 150);
            this.icon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.icon.TabIndex = 9;
            this.icon.TabStop = false;
            // 
            // sentitem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.sowner);
            this.Controls.Add(this.samount);
            this.Controls.Add(this.sitemprice);
            this.Controls.Add(this.sitemid);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.icon);
            this.Name = "sentitem";
            this.Size = new System.Drawing.Size(614, 150);
            this.DoubleClick += new System.EventHandler(this.ddb);
            this.MouseEnter += new System.EventHandler(this.adenter);
            this.MouseLeave += new System.EventHandler(this.adleave);
            ((System.ComponentModel.ISupportInitialize)(this.icon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label sowner;
        private System.Windows.Forms.Label samount;
        private System.Windows.Forms.Label sitemprice;
        private System.Windows.Forms.Label sitemid;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox icon;
    }
}
