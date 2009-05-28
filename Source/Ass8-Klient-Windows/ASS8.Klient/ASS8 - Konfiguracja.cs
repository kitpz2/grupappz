using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Threading;
using System.Security.Cryptography;
using System.IO;
namespace ASS8.Klient
{
    public partial class Konfiguracja : Form
    {
        komunikacja k;
        private bool close = false;
        Form loginForm;
        zarzadca z;
        public int sekundy;
        private void automatyczneSpr()
        {
            while (true)
            {
                z.szukajZmian();
                Thread.Sleep(sekundy);
            }
        }
        public Konfiguracja(Form log, komunikacja kom)
        {
            InitializeComponent();
            loginForm = log;
            KomClass = kom;
            z = new zarzadca(KomClass);
            Thread th = new Thread(automatyczneSpr);
            th.IsBackground = true;
            th.Start();
        }
        public komunikacja KomClass
        {
            get
            {
                return k;
            }
            set
            {
                k = value;
            }
        }
        private void btnSciezka_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtSciezka.Text = fbd.SelectedPath;
            }
        }

        private void ASS8___Konfiguracja_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!close)
            {
                this.Visible = false;
                e.Cancel = true;
            }
        }

        private void tcmsKonfiguracja_Click(object sender, EventArgs e)
        {
            this.Visible = true;
        }

        private void tcmsZnajomi_Click(object sender, EventArgs e)
        {
            Form1 frm = new Form1();
            frm.k = k;
            frm.Show();
        }

        private void cmsWyloguj_Click(object sender, EventArgs e)
        {
            close = true;
            this.Close();
            loginForm.Show();
        }

        private void tcmsZakoncz_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void tcmsStop_Click(object sender, EventArgs e)
        {
            tcmsStart.Enabled = true;
            z.mutex.WaitOne();
            tcmsStop.Enabled = false;
        }

        private void tcmsStart_Click(object sender, EventArgs e)
        {
            tcmsStart.Enabled = false;
            z.mutex.ReleaseMutex();
            tcmsStop.Enabled = true;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            lblSerwer.Enabled = chbProxy.Checked;
            lbPort.Enabled = chbProxy.Checked;
            txtLogin.Enabled = chbProxy.Checked;
            txtHaslo.Enabled = chbProxy.Checked;
            chbUwierzytelnienie.Enabled = chbProxy.Checked;

        }

        private void chbUwierzytelnienie_CheckedChanged(object sender, EventArgs e)
        {
            lblLogin.Enabled = chbProxy.Checked && chbUwierzytelnienie.Checked;
            lblHaslo.Enabled = chbProxy.Checked && chbUwierzytelnienie.Checked;
            txtLogin.Enabled = chbProxy.Checked && chbUwierzytelnienie.Checked;
            txtHaslo.Enabled = chbProxy.Checked && chbUwierzytelnienie.Checked;
        }

        private void btnZapisz_Click(object sender, EventArgs e)
        {
            if (sekundy < 10)
            {
                MessageBox.Show("Liczba sekund nie moze byc mniejsza niz 10");
                return;
            }
                sekundy = Int32.Parse(txtSekundy.Text);
                if (!Directory.Exists(txtSciezka.Text))
                    Directory.CreateDirectory(txtSciezka.Text);

                Thread th = new Thread(new ParameterizedThreadStart(zmianaFolderu));
            int tmp;
            if(chbProxy.Checked&&(txtSerwer.Text==""||txtPort.Text==""||!Int32.TryParse(txtPort.Text,out tmp))){
                MessageBox.Show("Ustaw poprawnie proxy");
                return;
            }
            if(chbProxy.Checked&&chbUwierzytelnienie.Checked&&(txtLogin.Text==""||txtHaslo.Text=="")){
                MessageBox.Show("Ustaw poprawnie proxy");
                return;
            }

            if (chbProxy.Checked)
            {
                WebProxy wp = new WebProxy(txtSerwer.Text + ":" + txtPort.Text, chbUwierzytelnienie.Checked);
                if (chbUwierzytelnienie.Checked)
                    wp.Credentials = new NetworkCredential(txtLogin.Text, txtHaslo.Text);
                WebRequest.DefaultWebProxy = wp;
            }
            else
            {
                WebRequest.DefaultWebProxy=null;
            }
                th.Start(txtSciezka.Text);

        }
        private void zmianaFolderu(object o)
        {
            z.folderMutex.WaitOne();
            z.folder = (string)o;
            z.folderMutex.ReleaseMutex();
        }

    }
}
