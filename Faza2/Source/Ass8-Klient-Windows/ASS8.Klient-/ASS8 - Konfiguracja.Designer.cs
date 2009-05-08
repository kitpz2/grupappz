namespace ASS8.Klient
{
    partial class ASS8___Konfiguracja
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ASS8___Konfiguracja));
            this.grpSynchronizacja = new System.Windows.Forms.GroupBox();
            this.lblSekundy = new System.Windows.Forms.Label();
            this.txtSekundy = new System.Windows.Forms.TextBox();
            this.lblCzas = new System.Windows.Forms.Label();
            this.btnSciezka = new System.Windows.Forms.Button();
            this.txtSciezka = new System.Windows.Forms.TextBox();
            this.lblSciezka = new System.Windows.Forms.Label();
            this.btnZapisz = new System.Windows.Forms.Button();
            this.btnAnuluj = new System.Windows.Forms.Button();
            this.niIkona = new System.Windows.Forms.NotifyIcon(this.components);
            this.cmsTray = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tcmsStart = new System.Windows.Forms.ToolStripMenuItem();
            this.tcmsStop = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tcmsKonfiguracja = new System.Windows.Forms.ToolStripMenuItem();
            this.tcmsZnajomi = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsWyloguj = new System.Windows.Forms.ToolStripMenuItem();
            this.tcmsZakoncz = new System.Windows.Forms.ToolStripMenuItem();
            this.grpSynchronizacja.SuspendLayout();
            this.cmsTray.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpSynchronizacja
            // 
            this.grpSynchronizacja.Controls.Add(this.lblSekundy);
            this.grpSynchronizacja.Controls.Add(this.txtSekundy);
            this.grpSynchronizacja.Controls.Add(this.lblCzas);
            this.grpSynchronizacja.Controls.Add(this.btnSciezka);
            this.grpSynchronizacja.Controls.Add(this.txtSciezka);
            this.grpSynchronizacja.Controls.Add(this.lblSciezka);
            this.grpSynchronizacja.Location = new System.Drawing.Point(12, 12);
            this.grpSynchronizacja.Name = "grpSynchronizacja";
            this.grpSynchronizacja.Size = new System.Drawing.Size(247, 101);
            this.grpSynchronizacja.TabIndex = 0;
            this.grpSynchronizacja.TabStop = false;
            this.grpSynchronizacja.Text = " ";
            // 
            // lblSekundy
            // 
            this.lblSekundy.AutoSize = true;
            this.lblSekundy.Location = new System.Drawing.Point(147, 67);
            this.lblSekundy.Name = "lblSekundy";
            this.lblSekundy.Size = new System.Drawing.Size(15, 13);
            this.lblSekundy.TabIndex = 5;
            this.lblSekundy.Text = "s.";
            // 
            // txtSekundy
            // 
            this.txtSekundy.Location = new System.Drawing.Point(104, 64);
            this.txtSekundy.Name = "txtSekundy";
            this.txtSekundy.Size = new System.Drawing.Size(37, 20);
            this.txtSekundy.TabIndex = 4;
            // 
            // lblCzas
            // 
            this.lblCzas.AutoSize = true;
            this.lblCzas.Location = new System.Drawing.Point(19, 67);
            this.lblCzas.Name = "lblCzas";
            this.lblCzas.Size = new System.Drawing.Size(88, 13);
            this.lblCzas.TabIndex = 3;
            this.lblCzas.Text = "Synchronizuj co: ";
            // 
            // btnSciezka
            // 
            this.btnSciezka.Location = new System.Drawing.Point(203, 33);
            this.btnSciezka.Name = "btnSciezka";
            this.btnSciezka.Size = new System.Drawing.Size(24, 22);
            this.btnSciezka.TabIndex = 2;
            this.btnSciezka.UseVisualStyleBackColor = true;
            this.btnSciezka.Click += new System.EventHandler(this.btnSciezka_Click);
            // 
            // txtSciezka
            // 
            this.txtSciezka.Location = new System.Drawing.Point(22, 35);
            this.txtSciezka.Name = "txtSciezka";
            this.txtSciezka.Size = new System.Drawing.Size(175, 20);
            this.txtSciezka.TabIndex = 1;
            // 
            // lblSciezka
            // 
            this.lblSciezka.AutoSize = true;
            this.lblSciezka.Location = new System.Drawing.Point(19, 19);
            this.lblSciezka.Name = "lblSciezka";
            this.lblSciezka.Size = new System.Drawing.Size(104, 13);
            this.lblSciezka.TabIndex = 0;
            this.lblSciezka.Text = "Ścieżka do katalogu";
            // 
            // btnZapisz
            // 
            this.btnZapisz.Location = new System.Drawing.Point(22, 231);
            this.btnZapisz.Name = "btnZapisz";
            this.btnZapisz.Size = new System.Drawing.Size(75, 23);
            this.btnZapisz.TabIndex = 1;
            this.btnZapisz.Text = "Zapisz";
            this.btnZapisz.UseVisualStyleBackColor = true;
            // 
            // btnAnuluj
            // 
            this.btnAnuluj.Location = new System.Drawing.Point(178, 231);
            this.btnAnuluj.Name = "btnAnuluj";
            this.btnAnuluj.Size = new System.Drawing.Size(75, 23);
            this.btnAnuluj.TabIndex = 2;
            this.btnAnuluj.Text = "Anuluj";
            this.btnAnuluj.UseVisualStyleBackColor = true;
            // 
            // niIkona
            // 
            this.niIkona.ContextMenuStrip = this.cmsTray;
            this.niIkona.Icon = ((System.Drawing.Icon)(resources.GetObject("niIkona.Icon")));
            this.niIkona.Visible = true;
            // 
            // cmsTray
            // 
            this.cmsTray.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tcmsStart,
            this.tcmsStop,
            this.toolStripSeparator1,
            this.tcmsKonfiguracja,
            this.tcmsZnajomi,
            this.toolStripSeparator2,
            this.cmsWyloguj,
            this.tcmsZakoncz});
            this.cmsTray.Name = "cmsTray";
            this.cmsTray.Size = new System.Drawing.Size(153, 170);
            // 
            // tcmsStart
            // 
            this.tcmsStart.Enabled = false;
            this.tcmsStart.Name = "tcmsStart";
            this.tcmsStart.Size = new System.Drawing.Size(152, 22);
            this.tcmsStart.Text = "Start";
            this.tcmsStart.Click += new System.EventHandler(this.tcmsStart_Click);
            // 
            // tcmsStop
            // 
            this.tcmsStop.Name = "tcmsStop";
            this.tcmsStop.Size = new System.Drawing.Size(152, 22);
            this.tcmsStop.Text = "Stop";
            this.tcmsStop.Click += new System.EventHandler(this.tcmsStop_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // tcmsKonfiguracja
            // 
            this.tcmsKonfiguracja.Name = "tcmsKonfiguracja";
            this.tcmsKonfiguracja.Size = new System.Drawing.Size(152, 22);
            this.tcmsKonfiguracja.Text = "Konfiguracja";
            this.tcmsKonfiguracja.Click += new System.EventHandler(this.tcmsKonfiguracja_Click);
            // 
            // tcmsZnajomi
            // 
            this.tcmsZnajomi.Name = "tcmsZnajomi";
            this.tcmsZnajomi.Size = new System.Drawing.Size(152, 22);
            this.tcmsZnajomi.Text = "Znajomi";
            this.tcmsZnajomi.Click += new System.EventHandler(this.tcmsZnajomi_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(149, 6);
            // 
            // cmsWyloguj
            // 
            this.cmsWyloguj.Name = "cmsWyloguj";
            this.cmsWyloguj.Size = new System.Drawing.Size(152, 22);
            this.cmsWyloguj.Text = "Wyloguj";
            this.cmsWyloguj.Click += new System.EventHandler(this.cmsWyloguj_Click);
            // 
            // tcmsZakoncz
            // 
            this.tcmsZakoncz.Name = "tcmsZakoncz";
            this.tcmsZakoncz.Size = new System.Drawing.Size(152, 22);
            this.tcmsZakoncz.Text = "Zakończ";
            this.tcmsZakoncz.Click += new System.EventHandler(this.tcmsZakoncz_Click);
            // 
            // ASS8___Konfiguracja
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(273, 266);
            this.Controls.Add(this.btnAnuluj);
            this.Controls.Add(this.btnZapisz);
            this.Controls.Add(this.grpSynchronizacja);
            this.MaximizeBox = false;
            this.Name = "ASS8___Konfiguracja";
            this.Text = "ASS.8 - Konfiguracja";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ASS8___Konfiguracja_FormClosing);
            this.grpSynchronizacja.ResumeLayout(false);
            this.grpSynchronizacja.PerformLayout();
            this.cmsTray.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSynchronizacja;
        private System.Windows.Forms.Button btnSciezka;
        private System.Windows.Forms.TextBox txtSciezka;
        private System.Windows.Forms.Label lblSciezka;
        private System.Windows.Forms.Button btnZapisz;
        private System.Windows.Forms.Button btnAnuluj;
        private System.Windows.Forms.Label lblSekundy;
        private System.Windows.Forms.TextBox txtSekundy;
        private System.Windows.Forms.Label lblCzas;
        private System.Windows.Forms.NotifyIcon niIkona;
        private System.Windows.Forms.ContextMenuStrip cmsTray;
        private System.Windows.Forms.ToolStripMenuItem tcmsKonfiguracja;
        private System.Windows.Forms.ToolStripMenuItem tcmsZnajomi;
        private System.Windows.Forms.ToolStripMenuItem cmsWyloguj;
        private System.Windows.Forms.ToolStripMenuItem tcmsZakoncz;
        private System.Windows.Forms.ToolStripMenuItem tcmsStart;
        private System.Windows.Forms.ToolStripMenuItem tcmsStop;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}