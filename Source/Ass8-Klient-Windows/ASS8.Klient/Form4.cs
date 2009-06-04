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
    /// Klasa wyświetlająca okno do zmiany serwera
    /// </summary>
    public partial class Form4 : Form
    {
        ASS8___Logowanie log;
        /// <summary>
        /// konstruktor klasy
        /// </summary>
        /// <param name="form">Formularz klasy wywołującej</param>
        public Form4(ASS8___Logowanie form)
        {
            log = form;
            InitializeComponent();
            textBox1.Text = form.serwer;
            textBox2.Text = form.port.ToString();
        }
        /// <summary>
        /// Obsługa przycisku zapisz
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                IPAddress.Parse(textBox1.Text);
            }
            catch
            {
                MessageBox.Show("Wpisz poprawny adres");
                return;
            }
            try
            {
                Int32.Parse(textBox2.Text);
            }
            catch
            {
                MessageBox.Show("Wpisz poprawny port");
            }
            log.zmianaSerwera(textBox1.Text, Int32.Parse(textBox2.Text));
            this.Close();
        }
        /// <summary>
        /// Obsługa przycisku zamknij
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
