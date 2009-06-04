using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Threading;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
namespace ASS8.Klient
{
    /// <summary>
    /// Kalsa odpowiedzialna za wyswietlenie okna i zalogowanie uzytkownika
    /// </summary>
    public partial class ASS8___Logowanie : Form
    {
        XmlSerializerNamespaces names;
        public int proxy;
        public int uwierzytelnienie;
        /// <summary>
        /// Zmiena przechowuje serwer proxy
        /// </summary>
        public string serwerProxy;
        /// <summary>
        /// Zmienna przechowuje login do serwera proxy
        /// </summary>
        public string loginProxy;
        /// <summary>
        /// Zmienna przechowuje hasło do serwera proxy
        /// </summary>
        public string hasloProxy;
        /// <summary>
        /// Zmienna przechowuje port do serwera proxy
        /// </summary>
        public int portProxy;
        public int zapamietaj;
        /// <summary>
        /// Zmienna przechowuje adres serwera
        /// </summary>
        public string serwer;
        public int port;
        private komunikacja k;
        public static string wersja = "0.6.9";
        /// <summary>
        /// Konstruktor klasy, wczytuje ustawienia z pliku i inicjalizuje formularz
        /// </summary>
        public ASS8___Logowanie()
        {
            names = new XmlSerializerNamespaces();
            names.Add("", "");
            InitializeComponent();
            k = new komunikacja();
            if (!File.Exists("ustawienia.ini"))
            {
                serwer = "127.0.0.1";
                port = 8080;
                return;
            }
            globalne_ustawienia set = new globalne_ustawienia();
            try
            {
                XmlSerializer xml = new XmlSerializer(typeof(globalne_ustawienia));
                TextReader tr = new StreamReader("ustawienia.ini");
                set = (globalne_ustawienia)xml.Deserialize(tr);
                tr.Close();
            }
            catch
            {
                zapamietaj=0;
                set.serwer="127.0.0.1";
                set.port=8080;
                return;
            }
            if (set.zapamietaj == 1)
            {
                zapamietaj = 1;
                txtLogin.Text = set.login;
                proxy = set.proxy;
                uwierzytelnienie = set.uwierzytelnienie;
                serwerProxy = set.serwerProxy;
                hasloProxy = set.hasloProxy;
                portProxy = set.port;
            }
            else
            {
                zapamietaj = 0;
            }
            cbZapamietaj.Checked = zapamietaj == 1;
            serwer = set.serwer;
            port = set.port;

        }
        /// <summary>
        /// Funkcja haszująca ciag znakow do MD5
        /// </summary>
        /// <param name="s">Ciąg do zaszyfrowania</param>
        /// <returns>Ciąg znaków zaszyfrowany</returns>
        private string hashuj(string s)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            StringBuilder sb = new StringBuilder();
            foreach (byte b in md5.ComputeHash(Encoding.ASCII.GetBytes(s)))
                sb.Append(b.ToString("x2"));
            return sb.ToString().ToLower();
        }
        /// <summary>
        /// Obsługa przycisku Loguj
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoguj_Click(object sender, EventArgs e)
        {
            k.Haslo = haslo;
            k.Login = login;
            k.ustawUstawienia(serwer,port);
                try
                {
                    XmlSerializer xml = new XmlSerializer(typeof(globalne_ustawienia));
                    globalne_ustawienia gu = new globalne_ustawienia(login, cbZapamietaj.Checked ? 1 : 0, proxy, serwerProxy, portProxy, uwierzytelnienie, loginProxy, hasloProxy, serwer, port);
                    
                    TextWriter tw = new StreamWriter("ustawienia.ini");
                    xml.Serialize(tw, gu, names);
                    
                    tw.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Nie mozna bylo zapisac ustawien" + ex.ToString());
                }
            
            try
            {
                k.login();
            }
            catch (Wyjatki.BladNieokreslony bn)
            {
                MessageBox.Show(bn.ToString());
                return;
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

        }
        /// <summary>
        /// Glowna zmienna obsługująca komunikację
        /// </summary>
        public komunikacja KomClass
        {
            get
            {
                return k;
            }
        }
        /// <summary>
        /// Właściwość zwraca wpisany login
        /// </summary>
        public string login
        {
            get
            {
                if (txtLogin.Text.Length == 0) return "";
                return txtLogin.Text;
            }
        }
        /// <summary>
        /// Właściwość zwraca wpisane hasło
        /// </summary>
        public string haslo
        {
            get
            {
                if (txtHasło.Text.Length == 0) return "";
                return hashuj(txtHasło.Text);
            }
        }

        /// <summary>
        /// Obsługa przycisku do ustawiania proxy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            Form2 fproxy;
            if (zapamietaj != 1)
                fproxy = new Form2();
            else
                fproxy = new Form2(this);
            fproxy.ShowDialog();
        }
        /// <summary>
        /// Obsługa przycisku do ustawiania serwera
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            Form4 frm = new Form4(this);
            frm.ShowDialog();
        }
        /// <summary>
        /// Zmienia serwer oraz port
        /// </summary>
        /// <param name="serw">Nowy adres serwera</param>
        /// <param name="p">Nowy port serwera</param>
        public void zmianaSerwera(string serw, int p)
        {
            serwer = serw;
            port = p;
        }
        /// <summary>
        /// Zmienia dane do proxy
        /// </summary>
        /// <param name="pr">Czy ma być ustawione proxy</param>
        /// <param name="serw">Adres proxy</param>
        /// <param name="p">Port proxy</param>
        /// <param name="u">Czy jest włączone uwierzytelnienie</param>
        /// <param name="l">Login do uwierzytelnienia</param>
        /// <param name="h">Hasło do uwierzytelnienia</param>
        public void zmianaProxy(int pr, string serw, int p, int u, string l, string h)
        {
            proxy = pr;
            serwerProxy = serw;
            portProxy = p;
            uwierzytelnienie = u;
            loginProxy = l;
            hasloProxy = h;
        }
    }
}
