namespace ASS8.Klient
{
    partial class Konfiguracja
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Konfiguracja));
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioLog = new System.Windows.Forms.RadioButton();
            this.radioPopup = new System.Windows.Forms.RadioButton();
            this.radioTray = new System.Windows.Forms.RadioButton();
            this.tabUstawienia = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.grpProxy = new System.Windows.Forms.GroupBox();
            this.txtHaslo = new System.Windows.Forms.TextBox();
            this.txtLogin = new System.Windows.Forms.TextBox();
            this.lblHaslo = new System.Windows.Forms.Label();
            this.lblLogin = new System.Windows.Forms.Label();
            this.chbUwierzytelnienie = new System.Windows.Forms.CheckBox();
            this.lbPort = new System.Windows.Forms.Label();
            this.lblSerwer = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtSerwer = new System.Windows.Forms.TextBox();
            this.chbProxy = new System.Windows.Forms.CheckBox();
            this.grpSynchronizacja.SuspendLayout();
            this.cmsTray.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabUstawienia.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.grpProxy.SuspendLayout();
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
            this.grpSynchronizacja.Location = new System.Drawing.Point(16, 6);
            this.grpSynchronizacja.Name = "grpSynchronizacja";
            this.grpSynchronizacja.Size = new System.Drawing.Size(247, 96);
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
            this.txtSekundy.Text = "30";
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
            this.btnSciezka.Text = "...";
            this.btnSciezka.UseVisualStyleBackColor = true;
            this.btnSciezka.Click += new System.EventHandler(this.btnSciezka_Click);
            // 
            // txtSciezka
            // 
            this.txtSciezka.Location = new System.Drawing.Point(22, 35);
            this.txtSciezka.Name = "txtSciezka";
            this.txtSciezka.Size = new System.Drawing.Size(175, 20);
            this.txtSciezka.TabIndex = 1;
            this.txtSciezka.Text = "pliki";
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
            this.btnZapisz.Location = new System.Drawing.Point(35, 245);
            this.btnZapisz.Name = "btnZapisz";
            this.btnZapisz.Size = new System.Drawing.Size(75, 23);
            this.btnZapisz.TabIndex = 1;
            this.btnZapisz.Text = "Zapisz";
            this.btnZapisz.UseVisualStyleBackColor = true;
            this.btnZapisz.Click += new System.EventHandler(this.btnZapisz_Click);
            // 
            // btnAnuluj
            // 
            this.btnAnuluj.Location = new System.Drawing.Point(176, 245);
            this.btnAnuluj.Name = "btnAnuluj";
            this.btnAnuluj.Size = new System.Drawing.Size(75, 23);
            this.btnAnuluj.TabIndex = 2;
            this.btnAnuluj.Text = "Zamknij";
            this.btnAnuluj.UseVisualStyleBackColor = true;
            this.btnAnuluj.Click += new System.EventHandler(this.btnAnuluj_Click);
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
            this.cmsTray.Size = new System.Drawing.Size(135, 148);
            // 
            // tcmsStart
            // 
            this.tcmsStart.Enabled = false;
            this.tcmsStart.Name = "tcmsStart";
            this.tcmsStart.Size = new System.Drawing.Size(134, 22);
            this.tcmsStart.Text = "Start";
            this.tcmsStart.Click += new System.EventHandler(this.tcmsStart_Click);
            // 
            // tcmsStop
            // 
            this.tcmsStop.Name = "tcmsStop";
            this.tcmsStop.Size = new System.Drawing.Size(134, 22);
            this.tcmsStop.Text = "Stop";
            this.tcmsStop.Click += new System.EventHandler(this.tcmsStop_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(131, 6);
            // 
            // tcmsKonfiguracja
            // 
            this.tcmsKonfiguracja.Name = "tcmsKonfiguracja";
            this.tcmsKonfiguracja.Size = new System.Drawing.Size(134, 22);
            this.tcmsKonfiguracja.Text = "Konfiguracja";
            this.tcmsKonfiguracja.Click += new System.EventHandler(this.tcmsKonfiguracja_Click);
            // 
            // tcmsZnajomi
            // 
            this.tcmsZnajomi.Name = "tcmsZnajomi";
            this.tcmsZnajomi.Size = new System.Drawing.Size(134, 22);
            this.tcmsZnajomi.Text = "Znajomi";
            this.tcmsZnajomi.Click += new System.EventHandler(this.tcmsZnajomi_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(131, 6);
            // 
            // cmsWyloguj
            // 
            this.cmsWyloguj.Name = "cmsWyloguj";
            this.cmsWyloguj.Size = new System.Drawing.Size(134, 22);
            this.cmsWyloguj.Text = "Wyloguj";
            this.cmsWyloguj.Click += new System.EventHandler(this.cmsWyloguj_Click);
            // 
            // tcmsZakoncz
            // 
            this.tcmsZakoncz.Name = "tcmsZakoncz";
            this.tcmsZakoncz.Size = new System.Drawing.Size(134, 22);
            this.tcmsZakoncz.Text = "Zakończ";
            this.tcmsZakoncz.Click += new System.EventHandler(this.tcmsZakoncz_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioLog);
            this.groupBox1.Controls.Add(this.radioPopup);
            this.groupBox1.Controls.Add(this.radioTray);
            this.groupBox1.Location = new System.Drawing.Point(16, 108);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(247, 92);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Ostrzeżenia";
            // 
            // radioLog
            // 
            this.radioLog.AutoSize = true;
            this.radioLog.Location = new System.Drawing.Point(6, 19);
            this.radioLog.Name = "radioLog";
            this.radioLog.Size = new System.Drawing.Size(177, 17);
            this.radioLog.TabIndex = 2;
            this.radioLog.Text = "Bledy i ostrzezenia loguj do pliku";
            this.radioLog.UseVisualStyleBackColor = true;
            // 
            // radioPopup
            // 
            this.radioPopup.AutoSize = true;
            this.radioPopup.Location = new System.Drawing.Point(6, 65);
            this.radioPopup.Name = "radioPopup";
            this.radioPopup.Size = new System.Drawing.Size(215, 17);
            this.radioPopup.TabIndex = 1;
            this.radioPopup.Text = "Bledy i ostrzezenia wyswietlaj na ekranie";
            this.radioPopup.UseVisualStyleBackColor = true;
            // 
            // radioTray
            // 
            this.radioTray.AutoSize = true;
            this.radioTray.Checked = true;
            this.radioTray.Location = new System.Drawing.Point(6, 42);
            this.radioTray.Name = "radioTray";
            this.radioTray.Size = new System.Drawing.Size(189, 17);
            this.radioTray.TabIndex = 0;
            this.radioTray.TabStop = true;
            this.radioTray.Text = "Bledy i ostrzezenia pokazuj w trayu";
            this.radioTray.UseVisualStyleBackColor = true;
            // 
            // tabUstawienia
            // 
            this.tabUstawienia.Controls.Add(this.tabPage1);
            this.tabUstawienia.Controls.Add(this.tabPage2);
            this.tabUstawienia.Location = new System.Drawing.Point(4, 7);
            this.tabUstawienia.Name = "tabUstawienia";
            this.tabUstawienia.SelectedIndex = 0;
            this.tabUstawienia.Size = new System.Drawing.Size(282, 232);
            this.tabUstawienia.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.grpSynchronizacja);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(274, 206);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Ogólne";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.grpProxy);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(274, 206);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Proxy";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // grpProxy
            // 
            this.grpProxy.Controls.Add(this.txtHaslo);
            this.grpProxy.Controls.Add(this.txtLogin);
            this.grpProxy.Controls.Add(this.lblHaslo);
            this.grpProxy.Controls.Add(this.lblLogin);
            this.grpProxy.Controls.Add(this.chbUwierzytelnienie);
            this.grpProxy.Controls.Add(this.lbPort);
            this.grpProxy.Controls.Add(this.lblSerwer);
            this.grpProxy.Controls.Add(this.txtPort);
            this.grpProxy.Controls.Add(this.txtSerwer);
            this.grpProxy.Controls.Add(this.chbProxy);
            this.grpProxy.Location = new System.Drawing.Point(6, 6);
            this.grpProxy.Name = "grpProxy";
            this.grpProxy.Size = new System.Drawing.Size(262, 171);
            this.grpProxy.TabIndex = 0;
            this.grpProxy.TabStop = false;
            this.grpProxy.Text = "Proxy";
            // 
            // txtHaslo
            // 
            this.txtHaslo.Enabled = false;
            this.txtHaslo.Location = new System.Drawing.Point(143, 131);
            this.txtHaslo.Name = "txtHaslo";
            this.txtHaslo.PasswordChar = '*';
            this.txtHaslo.Size = new System.Drawing.Size(100, 20);
            this.txtHaslo.TabIndex = 9;
            // 
            // txtLogin
            // 
            this.txtLogin.Enabled = false;
            this.txtLogin.Location = new System.Drawing.Point(8, 131);
            this.txtLogin.Name = "txtLogin";
            this.txtLogin.Size = new System.Drawing.Size(100, 20);
            this.txtLogin.TabIndex = 8;
            // 
            // lblHaslo
            // 
            this.lblHaslo.AutoSize = true;
            this.lblHaslo.Location = new System.Drawing.Point(140, 115);
            this.lblHaslo.Name = "lblHaslo";
            this.lblHaslo.Size = new System.Drawing.Size(34, 13);
            this.lblHaslo.TabIndex = 7;
            this.lblHaslo.Text = "Haslo";
            this.lblHaslo.Click += new System.EventHandler(this.label4_Click);
            // 
            // lblLogin
            // 
            this.lblLogin.AutoSize = true;
            this.lblLogin.Location = new System.Drawing.Point(5, 115);
            this.lblLogin.Name = "lblLogin";
            this.lblLogin.Size = new System.Drawing.Size(33, 13);
            this.lblLogin.TabIndex = 6;
            this.lblLogin.Text = "Login";
            // 
            // chbUwierzytelnienie
            // 
            this.chbUwierzytelnienie.AutoSize = true;
            this.chbUwierzytelnienie.Enabled = false;
            this.chbUwierzytelnienie.Location = new System.Drawing.Point(8, 94);
            this.chbUwierzytelnienie.Name = "chbUwierzytelnienie";
            this.chbUwierzytelnienie.Size = new System.Drawing.Size(123, 17);
            this.chbUwierzytelnienie.TabIndex = 5;
            this.chbUwierzytelnienie.Text = "Użyj uwierzytelnienia";
            this.chbUwierzytelnienie.UseVisualStyleBackColor = true;
            this.chbUwierzytelnienie.CheckedChanged += new System.EventHandler(this.chbUwierzytelnienie_CheckedChanged);
            // 
            // lbPort
            // 
            this.lbPort.AutoSize = true;
            this.lbPort.Location = new System.Drawing.Point(177, 44);
            this.lbPort.Name = "lbPort";
            this.lbPort.Size = new System.Drawing.Size(26, 13);
            this.lbPort.TabIndex = 4;
            this.lbPort.Text = "Port";
            // 
            // lblSerwer
            // 
            this.lblSerwer.AutoSize = true;
            this.lblSerwer.Location = new System.Drawing.Point(5, 44);
            this.lblSerwer.Name = "lblSerwer";
            this.lblSerwer.Size = new System.Drawing.Size(40, 13);
            this.lblSerwer.TabIndex = 3;
            this.lblSerwer.Text = "Serwer";
            // 
            // txtPort
            // 
            this.txtPort.Enabled = false;
            this.txtPort.Location = new System.Drawing.Point(180, 60);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(46, 20);
            this.txtPort.TabIndex = 2;
            // 
            // txtSerwer
            // 
            this.txtSerwer.Enabled = false;
            this.txtSerwer.Location = new System.Drawing.Point(8, 60);
            this.txtSerwer.Name = "txtSerwer";
            this.txtSerwer.Size = new System.Drawing.Size(156, 20);
            this.txtSerwer.TabIndex = 1;
            // 
            // chbProxy
            // 
            this.chbProxy.AutoSize = true;
            this.chbProxy.Location = new System.Drawing.Point(8, 19);
            this.chbProxy.Name = "chbProxy";
            this.chbProxy.Size = new System.Drawing.Size(74, 17);
            this.chbProxy.TabIndex = 0;
            this.chbProxy.Text = "Użyj proxy";
            this.chbProxy.UseVisualStyleBackColor = true;
            this.chbProxy.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // Konfiguracja
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(289, 280);
            this.Controls.Add(this.tabUstawienia);
            this.Controls.Add(this.btnAnuluj);
            this.Controls.Add(this.btnZapisz);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Konfiguracja";
            this.ShowInTaskbar = false;
            this.Text = "ASS.8 - Konfiguracja";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ASS8___Konfiguracja_FormClosing);
            this.grpSynchronizacja.ResumeLayout(false);
            this.grpSynchronizacja.PerformLayout();
            this.cmsTray.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabUstawienia.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.grpProxy.ResumeLayout(false);
            this.grpProxy.PerformLayout();
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
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioPopup;
        private System.Windows.Forms.RadioButton radioTray;
        private System.Windows.Forms.TabControl tabUstawienia;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox grpProxy;
        private System.Windows.Forms.TextBox txtLogin;
        private System.Windows.Forms.Label lblHaslo;
        private System.Windows.Forms.Label lblLogin;
        private System.Windows.Forms.CheckBox chbUwierzytelnienie;
        private System.Windows.Forms.Label lbPort;
        private System.Windows.Forms.Label lblSerwer;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtSerwer;
        private System.Windows.Forms.CheckBox chbProxy;
        private System.Windows.Forms.TextBox txtHaslo;
        private System.Windows.Forms.RadioButton radioLog;
    }
}