namespace ASS8.Klient
{
    partial class Form2
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chbProxy = new System.Windows.Forms.CheckBox();
            this.lbPort = new System.Windows.Forms.Label();
            this.lblSerwer = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtSerwer = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblLogin = new System.Windows.Forms.Label();
            this.txtHaslo = new System.Windows.Forms.TextBox();
            this.chbUwierzytelnienie = new System.Windows.Forms.CheckBox();
            this.txtLogin = new System.Windows.Forms.TextBox();
            this.lblHaslo = new System.Windows.Forms.Label();
            this.btnZapisz = new System.Windows.Forms.Button();
            this.btnAnuluj = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chbProxy);
            this.groupBox1.Controls.Add(this.lbPort);
            this.groupBox1.Controls.Add(this.lblSerwer);
            this.groupBox1.Controls.Add(this.txtPort);
            this.groupBox1.Controls.Add(this.txtSerwer);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(234, 82);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Serwer";
            // 
            // chbProxy
            // 
            this.chbProxy.AutoSize = true;
            this.chbProxy.Location = new System.Drawing.Point(6, 12);
            this.chbProxy.Name = "chbProxy";
            this.chbProxy.Size = new System.Drawing.Size(74, 17);
            this.chbProxy.TabIndex = 19;
            this.chbProxy.Text = "Użyj proxy";
            this.chbProxy.UseVisualStyleBackColor = true;
            this.chbProxy.CheckedChanged += new System.EventHandler(this.chbProxy_CheckedChanged);
            // 
            // lbPort
            // 
            this.lbPort.AutoSize = true;
            this.lbPort.Location = new System.Drawing.Point(175, 33);
            this.lbPort.Name = "lbPort";
            this.lbPort.Size = new System.Drawing.Size(26, 13);
            this.lbPort.TabIndex = 13;
            this.lbPort.Text = "Port";
            // 
            // lblSerwer
            // 
            this.lblSerwer.AutoSize = true;
            this.lblSerwer.Location = new System.Drawing.Point(3, 33);
            this.lblSerwer.Name = "lblSerwer";
            this.lblSerwer.Size = new System.Drawing.Size(40, 13);
            this.lblSerwer.TabIndex = 12;
            this.lblSerwer.Text = "Serwer";
            // 
            // txtPort
            // 
            this.txtPort.Enabled = false;
            this.txtPort.Location = new System.Drawing.Point(178, 49);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(46, 20);
            this.txtPort.TabIndex = 11;
            // 
            // txtSerwer
            // 
            this.txtSerwer.Enabled = false;
            this.txtSerwer.Location = new System.Drawing.Point(6, 49);
            this.txtSerwer.Name = "txtSerwer";
            this.txtSerwer.Size = new System.Drawing.Size(156, 20);
            this.txtSerwer.TabIndex = 10;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblLogin);
            this.groupBox2.Controls.Add(this.txtHaslo);
            this.groupBox2.Controls.Add(this.chbUwierzytelnienie);
            this.groupBox2.Controls.Add(this.txtLogin);
            this.groupBox2.Controls.Add(this.lblHaslo);
            this.groupBox2.Location = new System.Drawing.Point(12, 100);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(234, 93);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Uwierzytelnienie";
            // 
            // lblLogin
            // 
            this.lblLogin.AutoSize = true;
            this.lblLogin.Location = new System.Drawing.Point(3, 39);
            this.lblLogin.Name = "lblLogin";
            this.lblLogin.Size = new System.Drawing.Size(33, 13);
            this.lblLogin.TabIndex = 15;
            this.lblLogin.Text = "Login";
            // 
            // txtHaslo
            // 
            this.txtHaslo.Enabled = false;
            this.txtHaslo.Location = new System.Drawing.Point(124, 56);
            this.txtHaslo.Name = "txtHaslo";
            this.txtHaslo.PasswordChar = '*';
            this.txtHaslo.Size = new System.Drawing.Size(100, 20);
            this.txtHaslo.TabIndex = 18;
            // 
            // chbUwierzytelnienie
            // 
            this.chbUwierzytelnienie.AutoSize = true;
            this.chbUwierzytelnienie.Enabled = false;
            this.chbUwierzytelnienie.Location = new System.Drawing.Point(6, 19);
            this.chbUwierzytelnienie.Name = "chbUwierzytelnienie";
            this.chbUwierzytelnienie.Size = new System.Drawing.Size(123, 17);
            this.chbUwierzytelnienie.TabIndex = 14;
            this.chbUwierzytelnienie.Text = "Użyj uwierzytelnienia";
            this.chbUwierzytelnienie.UseVisualStyleBackColor = true;
            this.chbUwierzytelnienie.CheckedChanged += new System.EventHandler(this.chbUwierzytelnienie_CheckedChanged_1);
            // 
            // txtLogin
            // 
            this.txtLogin.Enabled = false;
            this.txtLogin.Location = new System.Drawing.Point(6, 56);
            this.txtLogin.Name = "txtLogin";
            this.txtLogin.Size = new System.Drawing.Size(100, 20);
            this.txtLogin.TabIndex = 17;
            // 
            // lblHaslo
            // 
            this.lblHaslo.AutoSize = true;
            this.lblHaslo.Location = new System.Drawing.Point(121, 39);
            this.lblHaslo.Name = "lblHaslo";
            this.lblHaslo.Size = new System.Drawing.Size(34, 13);
            this.lblHaslo.TabIndex = 16;
            this.lblHaslo.Text = "Haslo";
            // 
            // btnZapisz
            // 
            this.btnZapisz.Location = new System.Drawing.Point(43, 209);
            this.btnZapisz.Name = "btnZapisz";
            this.btnZapisz.Size = new System.Drawing.Size(75, 23);
            this.btnZapisz.TabIndex = 2;
            this.btnZapisz.Text = "Zapisz";
            this.btnZapisz.UseVisualStyleBackColor = true;
            this.btnZapisz.Click += new System.EventHandler(this.btnZapisz_Click);
            // 
            // btnAnuluj
            // 
            this.btnAnuluj.Location = new System.Drawing.Point(138, 209);
            this.btnAnuluj.Name = "btnAnuluj";
            this.btnAnuluj.Size = new System.Drawing.Size(75, 23);
            this.btnAnuluj.TabIndex = 3;
            this.btnAnuluj.Text = "Anuluj";
            this.btnAnuluj.UseVisualStyleBackColor = true;
            this.btnAnuluj.Click += new System.EventHandler(this.btnAnuluj_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(258, 247);
            this.Controls.Add(this.btnAnuluj);
            this.Controls.Add(this.btnZapisz);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form2";
            this.Text = "Proxy";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lbPort;
        private System.Windows.Forms.Label lblSerwer;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtSerwer;
        private System.Windows.Forms.Label lblLogin;
        private System.Windows.Forms.TextBox txtHaslo;
        private System.Windows.Forms.CheckBox chbUwierzytelnienie;
        private System.Windows.Forms.TextBox txtLogin;
        private System.Windows.Forms.Label lblHaslo;
        private System.Windows.Forms.Button btnZapisz;
        private System.Windows.Forms.Button btnAnuluj;
        private System.Windows.Forms.CheckBox chbProxy;
    }
}