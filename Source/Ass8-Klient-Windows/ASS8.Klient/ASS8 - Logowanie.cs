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
        public static string wersja = "0.4.1";
        public ASS8___Logowanie()
        {
            InitializeComponent();
            k = new komunikacja();
        }
        private string hashuj(string s)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            StringBuilder sb = new StringBuilder();
            foreach (byte b in md5.ComputeHash(Encoding.ASCII.GetBytes(s)))
                sb.Append(b.ToString("x2"));
            return sb.ToString().ToLower();
        }
        private void btnLoguj_Click(object sender, EventArgs e)
        {
            k.Haslo = haslo;
            k.Login = login;
            k.pobierzUstawienia();
            try
            {
                k.login();
            }
            catch (Wyjatki.BladNieokreslony bn)
            {
                MessageBox.Show(bn.ToString());
            }
            catch (Wyjatki.BladPolaczenia)
            {
                MessageBox.Show("Nie mozna zalogowac do serwera");
                return;
            }
            catch (Exception)
            {
                MessageBox.Show("Wystapil blad aplikacji");
                return;
            }
            Konfiguracja konfiguracja = new Konfiguracja(this, KomClass);
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
                return hashuj(txtHasło.Text);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 proxy = new Form2();
            proxy.ShowDialog();
        }
    }
}
