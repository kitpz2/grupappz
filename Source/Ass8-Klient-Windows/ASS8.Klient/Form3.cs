using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GlacialComponents.Controls;
namespace ASS8.Klient
{
    /// <summary>
    /// Klasa wyświetlająca okno z pobieranymi zadaniami
    /// </summary>
    public partial class Pobierane : Form
    {
        /// <summary>
        /// Konstruktor klasy
        /// </summary>
        public Pobierane()
        {
            InitializeComponent();
            glPliki.Columns.Add(new GLColumn("Nazwa"));
            glPliki.Columns.Add(new GLColumn("Postęp"));
            glPliki.Columns.Add(new GLColumn("Czas"));
            
        }
        /// <summary>
        /// Funkcja obsługi przycisku Zamknij
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnZamknij_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        /// <summary>
        /// Usuwa zadanie z listy
        /// </summary>
        /// <param name="g">Zadanie do usuniecia</param>
        public void usunZadanie(GLItem g)
        {
            glPliki.Items.Remove(g);
        }
        /// <summary>
        /// Dodaje zadanie do listy
        /// </summary>
        /// <param name="plik"></param>
        /// <returns></returns>
        public GLItem dodajZadanie(plikInfo plik)
        {
            ProgressBar pb = new ProgressBar();
            pb.Step = 256;
            pb.Maximum = plik.rozmiar;
            pb.Minimum = 0;
            GLItem gli = glPliki.Items.Add(plik.nazwa);
            gli.SubItems[1].Control = pb;
            Timer timer = new Timer();
            timer.Interval = 1000;
            return gli;
        }
        /// <summary>
        /// Czysci wszystkie zadania
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCzysc_Click(object sender, EventArgs e)
        {
            glPliki.Items.Clear();
        }
        /// <summary>
        /// Zamyka okno pobieran
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pobierane_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
        }
    }
}
