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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void btnZapisz_Click(object sender, EventArgs e)
        {
           if(!chbProxy.Checked)
               WebRequest.DefaultWebProxy = null;
            int tmp;
            if (chbProxy.Checked && (txtSerwer.Text == "" || txtPort.Text == "" || !Int32.TryParse(txtPort.Text, out tmp)))
            {
                MessageBox.Show("Ustaw poprawnie proxy");
                return;
            }
            if (chbProxy.Checked && chbUwierzytelnienie.Checked && (txtLogin.Text == "" || txtHaslo.Text == ""))
            {
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
                WebRequest.DefaultWebProxy = null;
            }
        }


        private void chbProxy_CheckedChanged(object sender, EventArgs e)
        {
            txtSerwer.Enabled = chbProxy.Checked;
            txtPort.Enabled = chbProxy.Checked;
            chbUwierzytelnienie.Enabled = chbProxy.Checked;
        }

        private void chbUwierzytelnienie_CheckedChanged(object sender, EventArgs e)
        {
            txtHaslo.Enabled = chbUwierzytelnienie.Checked;
            txtLogin.Enabled = chbUwierzytelnienie.Checked;
        }

        private void chbUwierzytelnienie_CheckedChanged_1(object sender, EventArgs e)
        {
            txtHaslo.Enabled = chbUwierzytelnienie.Checked;
            txtLogin.Enabled = chbUwierzytelnienie.Checked;
        }

        private void btnAnuluj_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
