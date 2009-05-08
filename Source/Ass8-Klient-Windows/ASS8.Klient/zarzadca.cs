using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Forms;
using System.Security.Cryptography;
namespace ASS8.Klient
{
    class zarzadca
    {
        private komunikacja k;
        string folder = "pliki";
        XmlSerializerNamespaces names;
        public zarzadca()
        {
            names = new XmlSerializerNamespaces();
            names.Add("", "");
            if (!File.Exists("pliki.xml"))
            {
                TextWriter sw = new StreamWriter("pliki.xml", false);
                pliki p = new pliki(new List<pojedynczyPlik>());
                XmlSerializer xml = new XmlSerializer(typeof(pliki));
                xml.Serialize(sw, p, names);
                sw.Close();
            }
        }
        //   List<plikInfo> sprawdzNoweLokalnePliki(plikInfo plik) { }
        private List<plikInfo> sprawdzRoznice(List<plikInfo> plikiDoPorownywania, pliki plikiPorownywane)
        {
            List<plikInfo> ret = new List<plikInfo>();
            foreach (plikInfo p in plikiDoPorownywania)
            {
                bool jest = false;
                foreach (pojedynczyPlik p2 in plikiPorownywane.plik)
                {
                    int comp = p.nazwa.CompareTo(p2.nazwa);
                    if (comp == 0)
                    {
                        jest = true;
                        break;
                    }
                    if (comp < 0) break;
                }
                if (!jest)
                    ret.Add(p);
            }
            return ret;

        }
        private List<pojedynczyPlik> sprawdzRoznice(pliki plikiDoPorownywania, List<plikInfo> plikiPorownywane)
        {
            List<pojedynczyPlik> ret = new List<pojedynczyPlik>();
            foreach (pojedynczyPlik p in plikiDoPorownywania.plik)
            {
                bool jest = false;
                foreach (plikInfo p2 in plikiPorownywane)
                {
                    int comp = p.nazwa.CompareTo(p2.nazwa);
                    if (comp == 0)
                    {
                        jest = true;
                        break;
                    }
                    if (comp < 0) break;
                }
                if (!jest)
                    ret.Add(p);
            }
            return ret;

        }
        private List<pojedynczyPlik> sprawdzRoznice(pliki plikiDoPorownywania, pliki plikiPorownywane)
        {
            List<pojedynczyPlik> ret = new List<pojedynczyPlik>();
            foreach (pojedynczyPlik p in plikiDoPorownywania.plik)
            {
                bool jest = false;
                foreach (pojedynczyPlik p2 in plikiPorownywane.plik)
                {
                    int comp = p.nazwa.CompareTo(p2.nazwa);
                    if (comp == 0)
                    {
                        jest = true;
                        break;
                    }
                    if (comp < 0) break;
                }
                if (!jest)
                    ret.Add(p);
            }
            return ret;

        }
        private void zapiszInfoPlikow()
        {
            DirectoryInfo di = new DirectoryInfo(folder);
            FileInfo[] files = di.GetFiles();
            List<pojedynczyPlik> pp = new List<pojedynczyPlik>();
            foreach (FileInfo fi in files)
            {
                FileStream fs = new FileStream(folder + "//" + fi.Name, FileMode.Open);
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] hash = md5.ComputeHash(fs);
                fs.Close();
                pp.Add(new pojedynczyPlik(fi.Name, ""));//Encoding.ASCII.GetString(hash)));
            }
            TextWriter sw = new StreamWriter("pliki.xml", false);
            pliki p = new pliki(pp);
            XmlSerializer xml = new XmlSerializer(typeof(pliki));
            xml.Serialize(sw, p, names);
            sw.Close();
        }
        private void odbierzPliki(List<plikInfo> plikiDoSciagniecia)
        {
            List<string> str = new List<string>();
            foreach (plikInfo pi in plikiDoSciagniecia)
                str.Add(pi.nazwa);
            k.downloadFiles(str.ToArray(), "");
        }
        private void wyslijPlik(List<pojedynczyPlik> plikiDoWyslania)
        {
            foreach (pojedynczyPlik p in plikiDoWyslania)
            {
                FileInfo fi = new FileInfo(folder + "//" + p.nazwa);
                k.uploadFile(p.nazwa, fi.LastWriteTime, (int)fi.Length);
            }
        }
        /*private void usunPlikiSerwer(List<pojedynczyPlik> plikiDoUsuniecia)
        {
            foreach (pojedynczyPlik p in plikiDoWyslania)
            {
                FileInfo fi = new FileInfo(folder + "//" + p.nazwa);
                k.uploadFile(p.nazwa, fi.LastWriteTime, (int)fi.Length);
            }
        }*/
        private pliki plikiZapisane()
        {
            pliki plikiLokalnie = new pliki(new List<pojedynczyPlik>());
            try
            {
                TextReader plikXml = new StreamReader("pliki.xml");
                XmlSerializer xml = new XmlSerializer(typeof(pliki));
                plikiLokalnie = (pliki)xml.Deserialize(plikXml);
                plikXml.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nie mozna bylo odczytac pliku konfiguracyjnego: " + ex.ToString());
            }
            return plikiLokalnie;
        }
        private pliki plikiKatalog()
        {

            pliki p = new pliki(new List<pojedynczyPlik>());
            try
            {
                DirectoryInfo di = new DirectoryInfo(folder);
                FileInfo[] files = di.GetFiles();
                foreach (FileInfo fi in files)
                    p.plik.Add(new pojedynczyPlik(fi.Name, ""));
            }
            catch (Exception) { MessageBox.Show("Nie mozna pobrac listy plikow"); }
            return p;
        }
        public void szukajZmian()
        {
            List<plikInfo> plikiNaSerwerze = k.downloadListy("");
            if (plikiNaSerwerze == null) return;
            pliki plikiLokalnieZapis = plikiZapisane();
            pliki plikiLokalnieKat = plikiKatalog();
            if (plikiLokalnieKat == null || plikiLokalnieZapis == null) return;
            List<plikInfo> roznicaSerwerLok = sprawdzRoznice(plikiNaSerwerze, plikiLokalnieZapis);
            List<pojedynczyPlik> roznicaLokSerwer = sprawdzRoznice(plikiLokalnieKat, plikiNaSerwerze);
            List<pojedynczyPlik> roznicaLokKat = sprawdzRoznice(plikiLokalnieZapis, plikiLokalnieKat);
            if (roznicaSerwerLok.Count != 0)
            {
                odbierzPliki(roznicaSerwerLok);
            }
            if (roznicaLokSerwer.Count != 0)
            {
                wyslijPlik(roznicaLokSerwer);
            }
            /*if(roznicaLokKat.Count!=0)
                usunPliki(roznicaLokKat);*/
            zapiszInfoPlikow();
        }

        public komunikacja kom
        {
            set
            {
                k = value;
            }
        }
    }
}
