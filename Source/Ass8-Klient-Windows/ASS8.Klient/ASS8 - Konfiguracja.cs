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
using System.Xml;
using System.Xml.Serialization;
namespace ASS8.Klient
{
    /// <summary>
    /// Klasa wyświetlająca okno do zmiany konfiguracji
    /// </summary>
    public partial class Konfiguracja : Form
    {
        XmlSerializerNamespaces names;
        komunikacja k;
        private bool close = false;
        Form loginForm;
        zarzadca z;
        public int sekundy;
        int bledyKontrola;
        string fol;
        /// <summary>
        /// Wysyołuje automatyczne sprawdzanie katalogu co określona ilość czasu
        /// </summary>
        /// <param name="o">Lista zmienny niezbędnych do utworzenia połączenia (utworzenia klasy komunikacja)</param>
        private void automatyczneSpr(object o)
        {
            List<object> obj=(List<object>)o;
            NotifyIcon ni=(NotifyIcon)obj[0];
            int bledy=(int)obj[1];
            string folder=(string)obj[2];
            komunikacja kom = new komunikacja();
            kom.folder = k.folder;
            kom.Login = k.Login;
            kom.Haslo = k.Haslo;
            kom.ustawUstawienia(k.Serwer, k.Port);
            kom.login();
            while (true)
            {
                zarzadca zz = new zarzadca(kom, ni, bledy, folder);
                zz.zmianaKontroli(bledyKontrola);
                zz.folder = fol;
                zz.szukajZmian();
                Thread.Sleep(sekundy * 1000);
            }
        }
        /// <summary>
        /// Konstruktor klasy, inicjalizuje wszystkie pola w formularzu
        /// </summary>
        /// <param name="log">Formularz logowania</param>
        /// <param name="kom">Zmienna do komunikacji sieciowej</param>
        public Konfiguracja(Form log, komunikacja kom)
        {
            names = new XmlSerializerNamespaces();
            names.Add("", "");
            InitializeComponent();
            loginForm = log;
            KomClass = kom;
            ustawienia_uzytkownika set = new ustawienia_uzytkownika();
            try
            {
                XmlSerializer xml = new XmlSerializer(typeof(ustawienia_uzytkownika));
                TextReader tr = new StreamReader(KomClass.Login + "/ustawienia.ini");
                set = (ustawienia_uzytkownika)xml.Deserialize(tr);
                tr.Close();
                if (set.sekundy < 10)
                    set.sekundy = 30;
            }
            catch
            {
                set.sciezka = KomClass.Login + "/pliki";
                set.sekundy = 30;
                set.bledy = 0;
                set.proxy = 0;
            }
            try
            {
                if (!Directory.Exists(set.sciezka))
                    Directory.CreateDirectory(set.sciezka);
            }
            catch(Exception ex)
            {
                set.sciezka = KomClass.Login + "/pliki";
                if (!Directory.Exists(set.sciezka))
                    Directory.CreateDirectory(set.sciezka);
            }
            txtSciezka.Text = set.sciezka;
            fol = set.sciezka;
            txtSekundy.Text = set.sekundy.ToString();
            radioTray.Checked = set.bledy == 0;
            radioPopup.Checked = set.bledy == 1;
            chbProxy.Checked = set.proxy == 1;
            if (chbProxy.Checked)
            {
                txtSerwer.Text = set.serwerProxy;
                txtPort.Text = set.portProxy.ToString();
                chbUwierzytelnienie.Checked = set.uwierzytelnienie == 1;
                if (chbUwierzytelnienie.Checked)
                {
                    txtLogin.Text = set.loginProxy;
                    txtHaslo.Text = set.hasloProxy;
                }
                WebProxy wp = new WebProxy(txtSerwer.Text + ":" + txtPort.Text, chbUwierzytelnienie.Checked);
                if (chbUwierzytelnienie.Checked)
                    wp.Credentials = new NetworkCredential(txtLogin.Text, txtHaslo.Text);
                WebRequest.DefaultWebProxy = wp;
            }
            sekundy = set.sekundy;
            z = new zarzadca(KomClass, niIkona, set.bledy,set.sciezka);
            bledyKontrola = set.bledy;
            Thread th = new Thread(new ParameterizedThreadStart(automatyczneSpr));
            Thread zmianaF = new Thread(new ParameterizedThreadStart(zmianaFolderu));
            th.IsBackground = true;
            List<object> obj = new List<object>();
            obj.Add(niIkona);
            obj.Add(set.bledy);
            obj.Add(set.sciezka);
            th.Start(obj);
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
        /// <summary>
        /// Wyświetla okno do wyboru nowego folderu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSciezka_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtSciezka.Text = fbd.SelectedPath;
            }
        }
        /// <summary>
        /// Obsługa zamykania formularza
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ASS8___Konfiguracja_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!close)
            {
                this.Visible = false;
                e.Cancel = true;
            }
        }
        /// <summary>
        /// Obsługa opcji w trayu wyświetlającej okno konfiguracji
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tcmsKonfiguracja_Click(object sender, EventArgs e)
        {
            this.Visible = true;
        }
        /// <summary>
        /// Obsługa opcji w trayu wyświetlającej okno znajomych
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tcmsZnajomi_Click(object sender, EventArgs e)
        {
            Form1 frm = new Form1();
            frm.k = k;
            frm.Show();
        }
        /// <summary>
        /// Obsługa opcji w trayu wylogowującej użytkownika
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmsWyloguj_Click(object sender, EventArgs e)
        {
            close = true;
            
            loginForm.Show();
            this.Close();
        }
        /// <summary>
        /// Obsługa opcji w trayu zamykającej aplikację
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tcmsZakoncz_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        /// <summary>
        /// Obsługa opcji w trayu stopującej automatyczną aktualizację
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tcmsStop_Click(object sender, EventArgs e)
        {
            tcmsStart.Enabled = true;
            z.mutex.WaitOne();
            tcmsStop.Enabled = false;
        }
        /// <summary>
        /// Obsługa opcji w trayu startującą automatyczną aktualizację
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tcmsStart_Click(object sender, EventArgs e)
        {
            tcmsStart.Enabled = false;
            z.mutex.ReleaseMutex();
            tcmsStop.Enabled = true;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Funkcja wywoływana podczas zmiany checkboxa i wywołująca odpowiednie zmiany
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            lblSerwer.Enabled = chbProxy.Checked;
            lbPort.Enabled = chbProxy.Checked;
            txtLogin.Enabled = chbProxy.Checked;
            txtHaslo.Enabled = chbProxy.Checked;
            chbUwierzytelnienie.Enabled = chbProxy.Checked;

        }
        /// <summary>
        /// Funkcja wywoływana podczas zmiany checkboxa i wywołująca odpowiednie zmiany
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chbUwierzytelnienie_CheckedChanged(object sender, EventArgs e)
        {
            lblLogin.Enabled = chbProxy.Checked && chbUwierzytelnienie.Checked;
            lblHaslo.Enabled = chbProxy.Checked && chbUwierzytelnienie.Checked;
            txtLogin.Enabled = chbProxy.Checked && chbUwierzytelnienie.Checked;
            txtHaslo.Enabled = chbProxy.Checked && chbUwierzytelnienie.Checked;
        }
        /// <summary>
        /// Obsluga przycisku zapisującego ustawienia. Ustawienia są zapisywane do pliku.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            try
            {
                XmlSerializer xml = new XmlSerializer(typeof(ustawienia_uzytkownika));
                ustawienia_uzytkownika gu = new ustawienia_uzytkownika(chbProxy.Checked ? 1 : 0, txtSerwer.Text, txtPort.Text==""?-1:Int32.Parse(txtPort.Text), chbUwierzytelnienie.Checked ? 1 : 0, txtLogin.Text, txtHaslo.Text, txtSciezka.Text, sekundy, radioLog.Checked ? 1 : radioPopup.Checked ? 2 : radioTray.Checked ? 0 : -1);
                TextWriter tw = new StreamWriter(KomClass.Login + "/ustawienia.ini");
                xml.Serialize(tw, gu, names);
                tw.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nie mozna bylo zapisac ustawien"+ex.ToString());
            }
            z.zmianaKontroli(radioLog.Checked ? 1 : radioPopup.Checked ? 2 : radioTray.Checked ? 0 : -1);
            Thread zmianaF = new Thread(new ParameterizedThreadStart(zmianaFolderu));
            bledyKontrola = radioLog.Checked ? 1 : radioPopup.Checked ? 2 : radioTray.Checked ? 0 : -1;
            if (fol != txtSciezka.Text)
            {
                pliki p = new pliki();
                TextWriter sw = new StreamWriter(KomClass.Login + "/pliki.xml", false);
                XmlSerializer xml = new XmlSerializer(typeof(pliki));
                xml.Serialize(sw, p, names);
                sw.Close();
            }
            fol = txtSciezka.Text;
            zmianaF.Start(txtSciezka.Text);
        }
        /// <summary>
        /// Funkcja odpowiedzialna za zmiane folderu aktualizacji
        /// </summary>
        /// <param name="o"></param>
        private void zmianaFolderu(object o)
        {
            z.folderMutex.WaitOne();
            z.folder = (string)o;
            k.folder = (string)o;
            z.folderMutex.ReleaseMutex();
        }
        /// <summary>
        /// Obsługa przycisku Zamknij
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnuluj_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
