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
    public partial class Form1 : Form
    {
        Pobierane pobieranie = null;
        public komunikacja k;
        public Form1()
        {
            InitializeComponent();
            lvPliki.View = View.List;
            lvPliki.FullRowSelect = true;
            lvPliki.GridLines = true;
            lvPliki.Sorting = SortOrder.Ascending;
            lvPliki.Columns.Add("Nazwa", -2, HorizontalAlignment.Left);
            lvPliki.Columns.Add("Rozmiar", -2, HorizontalAlignment.Left);
            lvPliki.Columns.Add("Data", -2, HorizontalAlignment.Left);
            lvPliki.Columns.Add("Hash", -2, HorizontalAlignment.Left);
            lvPliki.Columns.Add("Sciezka", -2, HorizontalAlignment.Left);
        }
        string wyswietlonePliki;
        private void btnListuj_Click(object sender, EventArgs e)
        {
            if (txtUzytkownik.Text == "")
            {
                MessageBox.Show("Wpisz uzytkownika");
                return;
            }
            wyswietlonePliki = txtUzytkownik.Text;
            List<plikInfo> pliki = k.downloadListy(txtUzytkownik.Text);
            foreach (plikInfo p in pliki)
            {
                string[] str = p.nazwa.Split("//".ToCharArray());
                ListViewItem item = new ListViewItem(str[str.Length-1]);
                item.SubItems.Add(p.rozmiar.ToString());
                item.SubItems.Add((new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(p.data)).ToShortDateString());
                item.SubItems.Add(p.hash);
                item.SubItems.Add(p.nazwa.Substring(0, p.nazwa.Length - str[str.Length - 1].Length));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (pobieranie == null)
                pobieranie = new Pobierane();
            foreach (ListViewItem lvi in lvPliki.SelectedItems)
            {
                Thread th = new Thread(new ParameterizedThreadStart(pobieraj));
                plikInfo plik = new plikInfo(lvi.Name, 0, Int32.Parse(lvi.SubItems[0].Text), 0, "");
                List<object> obj = new List<object>();
                obj.Add(lvi.SubItems[3].Name+lvi.Name);
                obj.Add(pobieranie.dodajZadanie(plik));
                obj.Add(wyswietlonePliki);
                th.Start(obj);
            }
        }

        private void pobieraj(object o){
            List<object> obj=(List<object>)o;
            string[] plik = {""};
            plik[0]=(string)obj[0];
            GLItem gli=(GLItem)obj[1];
            string uzytkownik=(string)obj[2];
            k.downloadFiles(plik, uzytkownik, gli);
        }
        private void btnPobierane_Click(object sender, EventArgs e)
        {
            if (pobieranie == null)
                pobieranie = new Pobierane();
            else
                pobieranie.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
