namespace ASS8.Klient
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.txtUzytkownik = new System.Windows.Forms.TextBox();
            this.grpUzytkownik = new System.Windows.Forms.GroupBox();
            this.btnListuj = new System.Windows.Forms.Button();
            this.grpPliki = new System.Windows.Forms.GroupBox();
            this.lvPliki = new System.Windows.Forms.ListView();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnPobierane = new System.Windows.Forms.Button();
            this.grpUzytkownik.SuspendLayout();
            this.grpPliki.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtUzytkownik
            // 
            this.txtUzytkownik.Location = new System.Drawing.Point(6, 19);
            this.txtUzytkownik.Name = "txtUzytkownik";
            this.txtUzytkownik.Size = new System.Drawing.Size(144, 20);
            this.txtUzytkownik.TabIndex = 0;
            // 
            // grpUzytkownik
            // 
            this.grpUzytkownik.Controls.Add(this.txtUzytkownik);
            this.grpUzytkownik.Controls.Add(this.btnListuj);
            this.grpUzytkownik.Location = new System.Drawing.Point(12, 12);
            this.grpUzytkownik.Name = "grpUzytkownik";
            this.grpUzytkownik.Size = new System.Drawing.Size(264, 53);
            this.grpUzytkownik.TabIndex = 1;
            this.grpUzytkownik.TabStop = false;
            // 
            // btnListuj
            // 
            this.btnListuj.Location = new System.Drawing.Point(183, 16);
            this.btnListuj.Name = "btnListuj";
            this.btnListuj.Size = new System.Drawing.Size(75, 23);
            this.btnListuj.TabIndex = 1;
            this.btnListuj.Text = "Listuj";
            this.btnListuj.UseVisualStyleBackColor = true;
            this.btnListuj.Click += new System.EventHandler(this.btnListuj_Click);
            // 
            // grpPliki
            // 
            this.grpPliki.Controls.Add(this.lvPliki);
            this.grpPliki.Location = new System.Drawing.Point(12, 75);
            this.grpPliki.Name = "grpPliki";
            this.grpPliki.Size = new System.Drawing.Size(264, 174);
            this.grpPliki.TabIndex = 2;
            this.grpPliki.TabStop = false;
            // 
            // lvPliki
            // 
            this.lvPliki.FullRowSelect = true;
            this.lvPliki.GridLines = true;
            this.lvPliki.Location = new System.Drawing.Point(6, 14);
            this.lvPliki.Name = "lvPliki";
            this.lvPliki.Size = new System.Drawing.Size(252, 154);
            this.lvPliki.TabIndex = 0;
            this.lvPliki.UseCompatibleStateImageBehavior = false;
            this.lvPliki.SelectedIndexChanged += new System.EventHandler(this.lvPliki_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(18, 264);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Ściagaj";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(195, 264);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "Zamknij";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnPobierane
            // 
            this.btnPobierane.Location = new System.Drawing.Point(108, 264);
            this.btnPobierane.Name = "btnPobierane";
            this.btnPobierane.Size = new System.Drawing.Size(75, 23);
            this.btnPobierane.TabIndex = 5;
            this.btnPobierane.Text = "Pobierane";
            this.btnPobierane.UseVisualStyleBackColor = true;
            this.btnPobierane.Click += new System.EventHandler(this.btnPobierane_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(288, 299);
            this.Controls.Add(this.btnPobierane);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.grpPliki);
            this.Controls.Add(this.grpUzytkownik);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Pliki znajomych";
            this.grpUzytkownik.ResumeLayout(false);
            this.grpUzytkownik.PerformLayout();
            this.grpPliki.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtUzytkownik;
        private System.Windows.Forms.GroupBox grpUzytkownik;
        private System.Windows.Forms.Button btnListuj;
        private System.Windows.Forms.GroupBox grpPliki;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ListView lvPliki;
        private System.Windows.Forms.Button btnPobierane;
    }
}