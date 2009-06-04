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
    /// <summary>
    /// Klasa obsługująca zmiane serwera proxy
    /// </summary>
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        ASS8___Logowanie log;
        /// <summary>
        /// Konstruktor klasy
        /// </summary>
        /// <param name="form">Formularz wywołujący</param>
        public Form2(ASS8___Logowanie form)
        {
            log = form;
            InitializeComponent();
            chbProxy.Checked = form.proxy == 1;
            if (chbProxy.Checked)
            {
                txtSerwer.Text = form.serwer;
                txtPort.Text = form.port.ToString();
                chbUwierzytelnienie.Checked = form.uwierzytelnienie == 1;
                if (chbUwierzytelnienie.Checked)
                {
                    txtLogin.Text = form.login;
                    txtHaslo.Text = form.haslo;
                }
                WebProxy wp = new WebProxy(txtSerwer.Text + ":" + txtPort.Text, chbUwierzytelnienie.Checked);
                if (chbUwierzytelnienie.Checked)
                    wp.Credentials = new NetworkCredential(txtLogin.Text, txtHaslo.Text);
                WebRequest.DefaultWebProxy = wp;
            }
        }
        /// <summary>
        /// Zapisuje i ustawia dane
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            log.zmianaProxy(chbProxy.Checked ? 1 : 0, txtSerwer.Text, Int32.Parse(txtPort.Text), chbUwierzytelnienie.Checked ? 1 : 0, txtLogin.Text, txtHaslo.Text);
            this.Close();
        }

        /// <summary>
        /// Sprawdza czy został kliknięty CheckBox, jeżeli tak to są zmieniane pola do edycji w zależności od ustawień
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chbProxy_CheckedChanged(object sender, EventArgs e)
        {
            txtSerwer.Enabled = chbProxy.Checked;
            txtPort.Enabled = chbProxy.Checked;
            chbUwierzytelnienie.Enabled = chbProxy.Checked;
        }
        /// <summary>
        /// Sprawdza czy został kliknięty CheckBox, jeżeli tak to są zmieniane pola do edycji w zależności od ustawień
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chbUwierzytelnienie_CheckedChanged(object sender, EventArgs e)
        {
            txtHaslo.Enabled = chbUwierzytelnienie.Checked;
            txtLogin.Enabled = chbUwierzytelnienie.Checked;
        }
        /// <summary>
        /// Sprawdza czy został kliknięty CheckBox, jeżeli tak to są zmieniane pola do edycji w zależności od ustawień
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chbUwierzytelnienie_CheckedChanged_1(object sender, EventArgs e)
        {
            txtHaslo.Enabled = chbUwierzytelnienie.Checked;
            txtLogin.Enabled = chbUwierzytelnienie.Checked;
        }
        /// <summary>
        /// Zamyka formularz
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnuluj_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
