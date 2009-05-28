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
    public partial class Pobierane : Form
    {
        public Pobierane()
        {
            InitializeComponent();
            glPliki.Columns.Add(new GLColumn("Nazwa"));
            glPliki.Columns.Add(new GLColumn("Postęp"));
            glPliki.Columns.Add(new GLColumn("Czas"));
            
        }

        private void btnZamknij_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
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
            //gli.SubItems[2].Control = (Control)timer;
            return gli;
        }

        private void btnCzysc_Click(object sender, EventArgs e)
        {
            glPliki.Items.Clear();
        }
    }
}
