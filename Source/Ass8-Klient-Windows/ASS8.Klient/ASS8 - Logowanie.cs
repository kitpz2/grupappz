using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Threading;

namespace ASS8.Klient
{
    public partial class ASS8___Logowanie : Form
    {
        private komunikacja k;
        public static string wersja = "0.3.9";
        public ASS8___Logowanie()
        {
            InitializeComponent();
            k = new komunikacja();
        }

        private void btnLoguj_Click(object sender, EventArgs e)
        {
            k.Haslo = haslo;
            k.Login = login;
            k.login();
            ASS8___Konfiguracja konfiguracja = new ASS8___Konfiguracja(this, KomClass);
            this.Hide();
            //k.downloadFiles(new string[] { "plik1", "plik2" }, "ja");
            //MessageBox.Show("Aa");

        }
        public komunikacja KomClass
        {
            get
            {
                return k;
            }
        }
        public string login
        {
            get
            {
                if (txtLogin.Text.Length == 0) return "";
                return txtLogin.Text;
            }
        }
        public string haslo
        {
            get
            {
                if (txtHasło.Text.Length == 0) return "";
                return "-1";
                /*MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                Byte[] haslo;
                Byte[] hash;
                haslo = ASCIIEncoding.Default.GetBytes(txtHasło.Text);
                hash = md5.ComputeHash(haslo);
                return ASCIIEncoding.Default.GetString(hash);*/
            }
        }
    }
}
