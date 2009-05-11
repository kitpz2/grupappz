using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ASS8.Klient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnLoguj_Click(object sender, EventArgs e)
        {
            ASS8___Konfiguracja konfiguracja = new ASS8___Konfiguracja(this);
            this.Hide();
        }
    }
}
