using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Threading;
namespace ASS8.Klient
{
    class zarzadca
    {
        private komunikacja k;
        public string folder="pliki";
        string login;
        public Mutex folderMutex;
        public Mutex mutex;
        XmlSerializerNamespaces names;
        public zarzadca(komunikacja kom)
        {
            k = kom;
            folderMutex = new Mutex();
            mutex = new Mutex();
            names = new XmlSerializerNamespaces();
            names.Add("", "");
            login = k.Login;
            if (!Directory.Exists(login))
                Directory.CreateDirectory(k.Login);
            if (!File.Exists(login + @"/" + "pliki.xml"))
            {
                TextWriter sw = new StreamWriter(login + @"/" + "pliki.xml", false);
                pliki p = new pliki(new List<pojedynczyPlik>());
                XmlSerializer xml = new XmlSerializer(typeof(pliki));
                xml.Serialize(sw, p, names);
                sw.Close();
            }
        }
        //   List<plikInfo> sprawdzNoweLokalnePliki(plikInfo plik) { }


        private List<plikInfo> sprawdzRoznicaDownload(List<plikInfo> serwer, pliki plik, pliki katalog)
        {
            List<plikInfo> lista = new List<plikInfo>();
            foreach (plikInfo pSerwer in serwer)
            {
                pojedynczyPlik pPlik = plik.plik.Find(delegate(pojedynczyPlik p) { return p.nazwa == pSerwer.nazwa; });
                pojedynczyPlik pKatalog = katalog.plik.Find(delegate(pojedynczyPlik p) { return p.nazwa == pSerwer.nazwa; });
                if (pPlik != null && pKatalog != null)
                    continue;
                lista.Add(pSerwer);
            }
            return lista;
        }

        private List<pojedynczyPlik> sprawdzRoznicaUpload(List<plikInfo> serwer, pliki plik, pliki katalog)
        {
            List<pojedynczyPlik> lista = new List<pojedynczyPlik>();
            foreach (pojedynczyPlik pKatalog in katalog.plik)
            {
                pojedynczyPlik pPlik = plik.plik.Find(delegate(pojedynczyPlik p) { return p.nazwa == pKatalog.nazwa; });
                plikInfo pSerwer = serwer.Find(delegate(plikInfo p) { return p.nazwa == pKatalog.nazwa; });
                if (pPlik != null && pSerwer != null)
                    continue;
                lista.Add(pKatalog);
            }
            return lista;
        }

        private List<pojedynczyPlik> sprawdzRoznicaUsunKatalog(List<plikInfo> serwer, pliki plik, pliki katalog)
        {
            List<pojedynczyPlik> lista = new List<pojedynczyPlik>();
            foreach (pojedynczyPlik pKatalog in katalog.plik)
            {
                pojedynczyPlik pPlik = plik.plik.Find(delegate(pojedynczyPlik p) { return p.nazwa == pKatalog.nazwa; });
                plikInfo pSerwer = serwer.Find(delegate(plikInfo p) { return p.nazwa == pKatalog.nazwa; });
                if (pPlik != null && pSerwer == null)
                    lista.Add(pKatalog);
            }
            return lista;
        }
        private List<pojedynczyPlik> sprawdzRoznicaUsunSerwer(List<plikInfo> serwer, pliki plik, pliki katalog)
        {
            List<pojedynczyPlik> lista = new List<pojedynczyPlik>();
            foreach (plikInfo pSerwer in serwer)
            {
                pojedynczyPlik pPlik = plik.plik.Find(delegate(pojedynczyPlik p) { return p.nazwa == pSerwer.nazwa; });
                pojedynczyPlik pKatalog = katalog.plik.Find(delegate(pojedynczyPlik p) { return p.nazwa == pSerwer.nazwa; });
                if (pPlik != null && pKatalog == null)
                    lista.Add(pPlik);
             
            }
            return lista;
        }
        private void zapiszInfoPlikow()
        {
            pliki p = new pliki();
            plikiKatalog("", p);
            TextWriter sw = new StreamWriter("pliki.xml", false);
            XmlSerializer xml = new XmlSerializer(typeof(pliki));
            xml.Serialize(sw, p, names);
            sw.Close();
        }
        private void odbierzPliki(List<plikInfo> plikiDoSciagniecia)
        {
            List<string> str = new List<string>();
            foreach (plikInfo pi in plikiDoSciagniecia)
                str.Add(pi.nazwa);
            try
            {
                k.downloadFiles(str.ToArray(), "", null);
            }
            catch (Exception ex)
            {
                if (!Directory.Exists(k.Login)) Directory.CreateDirectory(k.Login);
                FileStream fs = new FileStream(k.Login + "//err.log", FileMode.Append, FileAccess.Write);
                fs.Write(Encoding.ASCII.GetBytes(ex.ToString() + "\r\n"), 0, (ex.ToString() + "\r\n").Length);
                fs.Close();
            }
        }
        private void wyslijPlik(List<pojedynczyPlik> plikiDoWyslania)
        {
            foreach (pojedynczyPlik p in plikiDoWyslania)
            {
                FileInfo fi = new FileInfo(folder + "//" + p.nazwa);
                try
                {
                    k.uploadFile(p.nazwa, fi.LastWriteTime, (int)fi.Length);
                }
                catch (Exception ex)
                {
                    if (!Directory.Exists(k.Login)) Directory.CreateDirectory(k.Login);
                    FileStream fs = new FileStream(k.Login + "//err.log", FileMode.Append, FileAccess.Write);
                    fs.Write(Encoding.ASCII.GetBytes(ex.ToString() + "\r\n"),0,(ex.ToString() + "\r\n").Length);
                    fs.Close();
                }
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
                TextReader plikXml = new StreamReader(login + @"/" + "pliki.xml");
                XmlSerializer xml = new XmlSerializer(typeof(pliki));
                plikiLokalnie = (pliki)xml.Deserialize(plikXml);
                plikXml.Close();
            }
            catch (Exception)
            {
                if (!Directory.Exists(k.Login)) Directory.CreateDirectory(k.Login);
                FileStream fs = new FileStream(k.Login + "//err.log", FileMode.Append, FileAccess.Write);
                string str = "Nie mozna pobrac pliku konfiguracyjnego\r\n";
                fs.Write(Encoding.ASCII.GetBytes(str), 0, str.Length);
                fs.Close();
            }
            return plikiLokalnie;
        }
        private string hashPliku(string plik)
        {
            if (!File.Exists(plik)) throw new Exception();
            StringBuilder sb = new StringBuilder();
            FileStream fs = new FileStream(plik, FileMode.Open);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hash = md5.ComputeHash(fs);
            fs.Close();
            foreach (byte hex in hash)
                sb.Append(hex.ToString("x2"));
            return sb.ToString();
        }
        private void wykasujPlik(List<pojedynczyPlik> pliki)
        {
            foreach (pojedynczyPlik p in pliki)
            {
                try
                {
                    File.Delete(p.nazwa);
                }
                catch(Exception)
                {
                    if (!Directory.Exists(k.Login)) Directory.CreateDirectory(k.Login);
                    FileStream fs = new FileStream(k.Login + "//err.log", FileMode.Append, FileAccess.Write);
                    string str = "Nie mozna usunac pliku " + p.nazwa + "\r\n";
                    fs.Write(Encoding.ASCII.GetBytes(str), 0, str.Length);
                    fs.Close();
                }
            }
        }

        private void plikiKatalog(string katalog, pliki lista)//dodac przechodzenie przez kazdy folder w katalogu
        {
            if(lista==null)return;
            try
            {
                string tmpKatalog = folder + ((folder[folder.Length - 1] == '/') ? "" : "//") + katalog;
                DirectoryInfo di = new DirectoryInfo(tmpKatalog);
                FileInfo[] files = di.GetFiles();
                foreach (FileInfo fi in files)
                    lista.plik.Add(new pojedynczyPlik((long)(fi.LastWriteTime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds, katalog + (katalog == "" ? "" : "//") + fi.Name, hashPliku(tmpKatalog + "//" + fi.Name)));
                foreach (DirectoryInfo diTmp in di.GetDirectories())
                    plikiKatalog(katalog + "//" + diTmp.Name, lista);
            }
            catch (Exception ex) {
                if (!Directory.Exists(k.Login)) Directory.CreateDirectory(k.Login);
                FileStream fs = new FileStream(k.Login + "//err.log", FileMode.Append, FileAccess.Write);
                string str = "Nie mozna pobrac listy plikow z katalogu " +katalog+ "\r\n"+ex.ToString();
                fs.Write(Encoding.ASCII.GetBytes(str), 0, str.Length);
                fs.Close();
                lista = null;
            }
        }
        public void szukajZmian()
        {
            mutex.WaitOne();
            folderMutex.WaitOne();
            List<plikInfo> plikiNaSerwerze;
            try
            {
                plikiNaSerwerze = k.downloadListy("");
            }
            catch (Exception ex)
            {
                if (!Directory.Exists(k.Login)) Directory.CreateDirectory(k.Login);
                FileStream fs = new FileStream(k.Login + "//err.log", FileMode.Append, FileAccess.Write);
                fs.Write(Encoding.ASCII.GetBytes(ex.ToString() + "\r\n"), 0, (ex.ToString() + "\r\n").Length);
                fs.Close();
                return;
            }
            if (plikiNaSerwerze == null)
                plikiNaSerwerze = new List<plikInfo>();
            if (plikiNaSerwerze == null) return;
            pliki plikiLokalnieZapis = plikiZapisane();
            pliki plikiLokalnieKat = new pliki();
            plikiKatalog("", plikiLokalnieKat);
            if (plikiLokalnieKat == null)
                MessageBox.Show("aa");
            if (plikiLokalnieKat == null || plikiLokalnieZapis == null) return;
            List<plikInfo> roznicaSerwerLok =sprawdzRoznicaDownload(plikiNaSerwerze, plikiLokalnieZapis,plikiLokalnieKat);
            List<pojedynczyPlik> roznicaLokSerwer = sprawdzRoznicaUpload(plikiNaSerwerze, plikiLokalnieZapis, plikiLokalnieKat);
            List<pojedynczyPlik> roznicaLokKat = sprawdzRoznicaUsunSerwer(plikiNaSerwerze, plikiLokalnieZapis, plikiLokalnieKat);
            List<pojedynczyPlik> plikiDoWykasowania = sprawdzRoznicaUsunKatalog(plikiNaSerwerze, plikiLokalnieZapis, plikiLokalnieKat);
            List<pojedynczyPlik> plikiDoAktualizacjiS = sprawdzAktualizacje(plikiLokalnieZapis, plikiLokalnieKat, plikiNaSerwerze); //jezeli jest nowa wersja na dysku
            List<plikInfo> plikiDoAktualizacjiK = sprawdzAktualizacje(plikiNaSerwerze, plikiLokalnieZapis, plikiLokalnieKat); //jezeli jest nowa wersja na serwerze
            if (roznicaSerwerLok.Count != 0)
            {
                odbierzPliki(roznicaSerwerLok);
            }
            if (roznicaLokSerwer.Count != 0)
            {
                wyslijPlik(roznicaLokSerwer);
            }
            if (roznicaLokKat.Count != 0)
                usunPliki(roznicaLokKat);
               
            //TODO zrobic usuwanie z dysku
            if (plikiDoWykasowania.Count != 0)
                wykasujPlik(plikiDoWykasowania);
            if (plikiDoAktualizacjiK.Count != 0)
                odbierzPliki(plikiDoAktualizacjiK);
            if (plikiDoAktualizacjiS.Count != 0)
                wyslijPlik(plikiDoAktualizacjiS);
            zapiszInfoPlikow();
            folderMutex.ReleaseMutex();
            mutex.ReleaseMutex();
        }
        private List<pojedynczyPlik> sprawdzAktualizacje(pliki plik, pliki katalog, List<plikInfo> serwer)
        {
            List<pojedynczyPlik> tmp = new List<pojedynczyPlik>();
            foreach (pojedynczyPlik pPlik in plik.plik)
            {
                pojedynczyPlik pKatalog = katalog.plik.Find(delegate(pojedynczyPlik p) { return p.nazwa == pPlik.nazwa; });
                plikInfo pSerwer = serwer.Find(delegate(plikInfo p) { return p.nazwa == pPlik.nazwa; });
                if (pKatalog == null || pSerwer == null) continue;
                if (pKatalog.hash == pSerwer.hash && pKatalog.hash == pPlik.hash && pSerwer.hash == pPlik.hash) continue;
                if (pKatalog.hash != pPlik.hash && pKatalog.hash != pSerwer.hash && pSerwer.hash == pPlik.hash)
                    tmp.Add(pKatalog);
                if (pKatalog.hash != pPlik.hash && pKatalog.hash != pSerwer.hash && pSerwer.hash != pPlik.hash)
                    if (pKatalog.data > pSerwer.data)
                        tmp.Add(pKatalog);
            }
            return tmp;
        }
        private List<plikInfo> sprawdzAktualizacje(List<plikInfo> serwer, pliki plik, pliki katalog)
        {
            List<plikInfo> tmp = new List<plikInfo>();
            foreach (pojedynczyPlik pPlik in plik.plik)
            {
                pojedynczyPlik pKatalog = katalog.plik.Find(delegate(pojedynczyPlik p) { return p.nazwa == pPlik.nazwa; });
                plikInfo pSerwer = serwer.Find(delegate(plikInfo p) { return p.nazwa == pPlik.nazwa; });
                if (pKatalog == null || pSerwer == null) continue;
                if (pKatalog.hash == pSerwer.hash && pKatalog.hash == pPlik.hash && pSerwer.hash == pPlik.hash) continue;
                if (pSerwer.hash != pPlik.hash && pSerwer.hash != pKatalog.hash && pKatalog.hash == pPlik.hash)
                    tmp.Add(pSerwer);
                if (pSerwer.hash != pPlik.hash && pSerwer.hash != pKatalog.hash && pKatalog.hash != pPlik.hash)
                    if (pSerwer.data > pKatalog.data)
                        tmp.Add(pSerwer);
            }
            return tmp;
        }

        private void usunPliki(List<pojedynczyPlik> pliki)
        {
            try
            {
                if(k.usunPliki(pliki)==403)
                {
                    if (!Directory.Exists(k.Login)) Directory.CreateDirectory(k.Login);
                    FileStream fs = new FileStream(k.Login + "//err.log", FileMode.Append, FileAccess.Write);
                    string str = "Nie udalo sie usunac niektorych plikow z serwera, pobierz liste plikow w celu sprawdzenia\r\n";
                    fs.Write(Encoding.ASCII.GetBytes(str), 0, str.Length);
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                if (!Directory.Exists(k.Login)) Directory.CreateDirectory(k.Login);
                FileStream fs = new FileStream(k.Login + "//err.log", FileMode.Append, FileAccess.Write);
                fs.Write(Encoding.ASCII.GetBytes(ex.ToString() + "\r\n"), 0, (ex.ToString() + "\r\n").Length);
                fs.Close();
            }
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
