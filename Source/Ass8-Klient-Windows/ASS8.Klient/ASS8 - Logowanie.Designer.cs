namespace ASS8.Klient
{
    partial class ASS8___Logowanie
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
            this.lblLogin = new System.Windows.Forms.Label();
            this.lblHaslo = new System.Windows.Forms.Label();
            this.txtLogin = new System.Windows.Forms.TextBox();
            this.txtHasło = new System.Windows.Forms.TextBox();
            this.cbZapamietaj = new System.Windows.Forms.CheckBox();
            this.btnLoguj = new System.Windows.Forms.Button();
            this.grpLogowanie = new System.Windows.Forms.GroupBox();
            this.grpLogowanie.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblLogin
            // 
            this.lblLogin.AutoSize = true;
            this.lblLogin.Location = new System.Drawing.Point(6, 16);
            this.lblLogin.Name = "lblLogin";
            this.lblLogin.Size = new System.Drawing.Size(33, 13);
            this.lblLogin.TabIndex = 0;
            this.lblLogin.Text = "Login";
            // 
            // lblHaslo
            // 
            this.lblHaslo.AutoSize = true;
            this.lblHaslo.Location = new System.Drawing.Point(6, 44);
            this.lblHaslo.Name = "lblHaslo";
            this.lblHaslo.Size = new System.Drawing.Size(36, 13);
            this.lblHaslo.TabIndex = 1;
            this.lblHaslo.Text = "Hasło";
            // 
            // txtLogin
            // 
            this.txtLogin.Location = new System.Drawing.Point(45, 13);
            this.txtLogin.Name = "txtLogin";
            this.txtLogin.Size = new System.Drawing.Size(100, 20);
            this.txtLogin.TabIndex = 2;
            // 
            // txtHasło
            // 
            this.txtHasło.Location = new System.Drawing.Point(45, 41);
            this.txtHasło.Name = "txtHasło";
            this.txtHasło.Size = new System.Drawing.Size(100, 20);
            this.txtHasło.TabIndex = 3;
            // 
            // cbZapamietaj
            // 
            this.cbZapamietaj.AutoSize = true;
            this.cbZapamietaj.Location = new System.Drawing.Point(9, 67);
            this.cbZapamietaj.Name = "cbZapamietaj";
            this.cbZapamietaj.Size = new System.Drawing.Size(78, 17);
            this.cbZapamietaj.TabIndex = 4;
            this.cbZapamietaj.Text = "Zapamiętaj";
            this.cbZapamietaj.UseVisualStyleBackColor = true;
            // 
            // btnLoguj
            // 
            this.btnLoguj.Location = new System.Drawing.Point(45, 90);
            this.btnLoguj.Name = "btnLoguj";
            this.btnLoguj.Size = new System.Drawing.Size(75, 23);
            this.btnLoguj.TabIndex = 5;
            this.btnLoguj.Text = "Zaloguj";
            this.btnLoguj.UseVisualStyleBackColor = true;
            this.btnLoguj.Click += new System.EventHandler(this.btnLoguj_Click);
            // 
            // grpLogowanie
            // 
            this.grpLogowanie.Controls.Add(this.txtLogin);
            this.grpLogowanie.Controls.Add(this.btnLoguj);
            this.grpLogowanie.Controls.Add(this.lblLogin);
            this.grpLogowanie.Controls.Add(this.cbZapamietaj);
            this.grpLogowanie.Controls.Add(this.lblHaslo);
            this.grpLogowanie.Controls.Add(this.txtHasło);
            this.grpLogowanie.Location = new System.Drawing.Point(12, 12);
            this.grpLogowanie.Name = "grpLogowanie";
            this.grpLogowanie.Size = new System.Drawing.Size(163, 129);
            this.grpLogowanie.TabIndex = 6;
            this.grpLogowanie.TabStop = false;
            // 
            // ASS8___Logowanie
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(189, 157);
            this.Controls.Add(this.grpLogowanie);
            this.MaximizeBox = false;
            this.Name = "ASS8___Logowanie";
            this.Text = "ASS.8 - Logowanie";
            this.grpLogowanie.ResumeLayout(false);
            this.grpLogowanie.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblLogin;
        private System.Windows.Forms.Label lblHaslo;
        private System.Windows.Forms.TextBox txtLogin;
        private System.Windows.Forms.TextBox txtHasło;
        private System.Windows.Forms.CheckBox cbZapamietaj;
        private System.Windows.Forms.Button btnLoguj;
        private System.Windows.Forms.GroupBox grpLogowanie;
    }
}

