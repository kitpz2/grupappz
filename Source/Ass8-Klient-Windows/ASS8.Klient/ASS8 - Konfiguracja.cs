using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;

namespace ASS8.Klient
{
    public partial class ASS8___Konfiguracja : Form
    {
        private bool close=false;
        Form loginForm;
        public ASS8___Konfiguracja(Form log)
        {
            InitializeComponent();
            loginForm = log;
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
            MessageBox.Show(":)");
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
            tcmsStop.Enabled = false;
        }

        private void tcmsStart_Click(object sender, EventArgs e)
        {
            tcmsStart.Enabled = false;
            tcmsStop.Enabled = true;
        }
    }
}
