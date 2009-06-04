using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ASS8.Klient
{
    static class Program
    {
        /// <summary>
        /// Początek programu
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ASS8___Logowanie());
        }
    }
}
