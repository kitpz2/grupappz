using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using GlacialComponents.Controls;
namespace ASS8.Klient
{
    /// <summary>
    /// Klasa odpowiedzialna za wyświetlenie okna z wyszukiwaniem plików znajomych
    /// </summary>
    public partial class Form1 : Form
    {
        Pobierane pobieranie = null;
        public komunikacja k;
        /// <summary>
        /// Konstruktor klasy
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            lvPliki.View = View.Details;
            lvPliki.FullRowSelect = true;
            lvPliki.GridLines = true;
            lvPliki.Sorting = SortOrder.Ascending;
            lvPliki.Columns.Add("Nazwa",70, HorizontalAlignment.Left);
            lvPliki.Columns.Add("Sciezka",120 , HorizontalAlignment.Left);            
            lvPliki.Columns.Add("Rozmiar", -2, HorizontalAlignment.Center);
        }
        string wyswietlonePliki;
        /// <summary>
        /// Funkcja odpowiedzialna za listowanie plików wpisanego użytkownika
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnListuj_Click(object sender, EventArgs e)
        {
            if (txtUzytkownik.Text == "")
            {
                MessageBox.Show("Wpisz uzytkownika");
                return;
            }
            wyswietlonePliki = txtUzytkownik.Text;
            komunikacja kk = new komunikacja();
            kk.Login = k.Login;
            kk.Haslo = k.Haslo;
            kk.ustawUstawienia(k.Serwer, k.Port);
            kk.login();
            List<plikInfo> pliki = kk.downloadListy(txtUzytkownik.Text);

            foreach (plikInfo p in pliki)
            {
                string[] str = p.nazwa.Split("/".ToCharArray());
                ListViewItem item = new ListViewItem(str[str.Length-1]);
                item.SubItems.Add(p.nazwa.Substring(0, p.nazwa.Length - str[str.Length - 1].Length));
                item.SubItems.Add(p.rozmiar.ToString());
                lvPliki.Items.Add(item);
            }
        }
        /// <summary>
        /// Funkcja odpowiedzialna za obslugę przycisku pobierającego zaznaczone pliku
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (pobieranie == null)
                pobieranie = new Pobierane();
            foreach (ListViewItem lvi in lvPliki.SelectedItems)
            {
                Thread th = new Thread(new ParameterizedThreadStart(pobieraj));
                plikInfo plik = new plikInfo(lvi.SubItems[0].Text, 0, Int32.Parse(lvi.SubItems[2].Text), 0, "");
                List<object> obj = new List<object>();
                obj.Add(lvi.SubItems[1].Text+lvi.SubItems[0].Text);
                obj.Add(plik);
                obj.Add(wyswietlonePliki);
                th.SetApartmentState(ApartmentState.STA);
                th.IsBackground = true;
                th.Start(obj);
            }
        }
        Mutex mut = new Mutex();
        /// <summary>
        /// Funkcja pobierająca wybrane pliki z serwera
        /// </summary>
        /// <param name="o"></param>
        private void pobieraj(object o){
            List<object> obj = (List<object>)o;
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.Cancel)
            {
                pobieranie.usunZadanie((GLItem)obj[1]);
                return;
            }
            string[] plik = {""};
            plik[0]=(string)obj[0];
            plikInfo p=(plikInfo)obj[1];
            GLItem gli = pobieranie.dodajZadanie(p);
            string uzytkownik=(string)obj[2];
            mut.WaitOne();
            k.downloadFiles(plik, uzytkownik, gli,fbd.SelectedPath);
            mut.ReleaseMutex();
            
        }
        /// <summary>
        /// Przycisk wyswietla pobierane aktualnie pliki (i zakonczone)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPobierane_Click(object sender, EventArgs e)
        {
            if (pobieranie == null)
            {
                pobieranie = new Pobierane();
                pobieranie.Show();
            }
            else
                pobieranie.Show();
        }
        /// <summary>
        /// Funkcja zamyka okno
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void lvPliki_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
