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
    /// <summary>
    /// Klasa zarządzająca połączeniem i wszystkimi operacjami z plikami
    /// </summary>
    class zarzadca
    {
        /// <summary>
        /// Zmienna przechowuje klasę do obsługi sieciowej
        /// </summary>
        private komunikacja k;
        /// <summary>
        /// Zmienna przechowuje folder użytkownika
        /// </summary>
        public string folder;
        /// <summary>
        /// Zmienna przechowuje login użytkownika
        /// </summary>
        string login;
        int kontrolaBledow;
        public Mutex folderMutex;
        public Mutex mutex;
        XmlSerializerNamespaces names;
        NotifyIcon notify;
        /// <summary>
        /// Zmienia kontroli błędów (gdzie ma być wypisywany komunikat o bledach)
        /// </summary>
        /// <param name="kontrola">Zmienna określająca typ obsługi błędów</param>
        public void zmianaKontroli(int kontrola)
        {
            kontrolaBledow = kontrola;
        }
        /// <summary>
        /// Konstruktor klasy, inicjalizuje wszystkie zmienne oraz tworzy katalog użytkownika jeżeli nie istnieje
        /// </summary>
        /// <param name="kom">Zmienna do obsługi sieciowej</param>
        /// <param name="ni">Zmienna ikony traya</param>
        /// <param name="b">Zmienna z obsługą błędów</param>
        /// <param name="fol">Folder użytkownika</param>
        public zarzadca(komunikacja kom,NotifyIcon ni,int b,string fol)
        {
            folder = fol;
            k = kom;
            k.folder = folder;
            kontrolaBledow = b;
            notify = ni;
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
        /// <summary>
        /// Wyświetla błąd w trayu
        /// </summary>
        /// <param name="str">Wiadomość do wyświetlenia</param>
        private void wyswietlBlad(string str){
            notify.ShowBalloonTip(1000,"Błąd",str,ToolTipIcon.Error);
        }
        /// <summary>
        /// Zapisuje błąd do pliku
        /// </summary>
        /// <param name="str">Wiadomość do zapisywania</param>
        private void zapiszBlad(string str)
        {
            if (!Directory.Exists(k.Login)) Directory.CreateDirectory(k.Login);
            FileStream fs = new FileStream(k.Login + "/err.log", FileMode.Append, FileAccess.Write);
            fs.Write(Encoding.ASCII.GetBytes(str + "\r\n"), 0, (str + "\r\n").Length);
            fs.Close();
        }
        /// <summary>
        /// Sprawdza które pliki należy ściągnąć z serwera
        /// </summary>
        /// <param name="serwer">Pliki na serwerze</param>
        /// <param name="plik">Pliki zapisane w pliku konfiguracyjnym</param>
        /// <param name="katalog">Pliki w katalogu</param>
        /// <returns>Lista plików do ściągnięcia</returns>
        private List<plikInfo> sprawdzRoznicaDownload(List<plikInfo> serwer, pliki plik, pliki katalog)
        {
            List<plikInfo> lista = new List<plikInfo>();
            foreach (plikInfo pSerwer in serwer)
            {
                pojedynczyPlik pPlik = plik.plik.Find(delegate(pojedynczyPlik p) { return p.nazwa == pSerwer.nazwa; });
                pojedynczyPlik pKatalog = katalog.plik.Find(delegate(pojedynczyPlik p) { return p.nazwa == pSerwer.nazwa; });
                if (pPlik != null || pKatalog != null)
                    continue;
                lista.Add(pSerwer);
            }
            return lista;
        }
        /// <summary>
        /// Sprawdza które pliki należy wysłać na serwer
        /// </summary>
        /// <param name="serwer">Pliki na serwerze</param>
        /// <param name="plik">Pliki zapisane w pliku konfiguracyjnym</param>
        /// <param name="katalog">Pliki w katalogu</param>
        /// <returns>Lista plików wysłania</returns>
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
        /// <summary>
        /// Sprawdza które pliki należy usunąć z dysku
        /// </summary>
        /// <param name="serwer">Pliki na serwerze</param>
        /// <param name="plik">Pliki zapisane w pliku konfiguracyjnym</param>
        /// <param name="katalog">Pliki w katalogu</param>
        /// <returns>Lista plików do usunięcia</returns>
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
        /// <summary>
        /// Sprawdza które pliki należy usunąć z serwera
        /// </summary>
        /// <param name="serwer">Pliki na serwerze</param>
        /// <param name="plik">Pliki zapisane w pliku konfiguracyjnym</param>
        /// <param name="katalog">Pliki w katalogu</param>
        /// <returns>Lista plików do usunięcia</returns>
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
        /// <summary>
        /// Zapisuje do pliku konfiguracyjnego wszystkie pliki znajdujące się w katalogu
        /// </summary>
        public void zapiszInfoPlikow()
        {
            pliki p = new pliki();
            plikiKatalog("", p);
            TextWriter sw = new StreamWriter(login + "/pliki.xml", false);
            XmlSerializer xml = new XmlSerializer(typeof(pliki));
            xml.Serialize(sw, p, names);
            sw.Close();
        }
        /// <summary>
        /// Odbiera pliki z serwera
        /// </summary>
        /// <param name="plikiDoSciagniecia">Lista plików do ściągnięcia</param>
        private void odbierzPliki(List<plikInfo> plikiDoSciagniecia)
        {
            List<string> str = new List<string>();
            foreach (plikInfo pi in plikiDoSciagniecia)
                str.Add(pi.nazwa);
            try
            {
                k.downloadFiles(str.ToArray(), "", null,null);
            }
            catch (Exception ex)
            {
                switch (kontrolaBledow)
                {
                    case 0: wyswietlBlad(ex.ToString()); break;
                    case 1: zapiszBlad(ex.ToString()); break;
                    case 2: MessageBox.Show(ex.ToString()); break;
                }
            }
        }
        /// <summary>
        /// Wysyła pliki na serwer
        /// </summary>
        /// <param name="plikiDoWyslania">Lista plików do wysłania</param>
        private void wyslijPlik(List<pojedynczyPlik> plikiDoWyslania)
        {
            foreach (pojedynczyPlik p in plikiDoWyslania)
            {
                FileInfo fi = new FileInfo(folder + "/" + p.nazwa);
                try
                {
                    k.uploadFile(p.nazwa, fi.LastWriteTime, (int)fi.Length);
                }
                catch (Exception ex)
                {
                    switch (kontrolaBledow)
                    {
                        case 0: wyswietlBlad(ex.ToString()); break;
                        case 1: zapiszBlad(ex.ToString()); break;
                        case 2: MessageBox.Show(ex.ToString()); break;
                    }
                }
            }
        }
        /// <summary>
        /// Pobiera listę plików z pliku konfiguracyjnego
        /// </summary>
        /// <returns>Zmienna przechowująca wszystkie pliki zapisane</returns>
        private pliki plikiZapisane()
        {
            pliki plikiLokalnie = new pliki(new List<pojedynczyPlik>());
            try
            {
                TextReader plikXml = new StreamReader(login + "/pliki.xml");
                XmlSerializer xml = new XmlSerializer(typeof(pliki));
                plikiLokalnie = (pliki)xml.Deserialize(plikXml);
                plikXml.Close();
            }
            catch (Exception)
            {
                string str = "Nie mozna pobrac pliku konfiguracyjnego\r\n";
                switch (kontrolaBledow)
                {
                    case 0: wyswietlBlad(str); break;
                    case 1: zapiszBlad(str); break;
                    case 2: MessageBox.Show(str); break;
                }
            }
            return plikiLokalnie;
        }
        /// <summary>
        /// Oblicza hash pliku w MD5
        /// </summary>
        /// <param name="plik">Nazwa pliku do zakodowania</param>
        /// <returns>Hash pliku</returns>
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
        /// <summary>
        /// Kasuje plik z dysku
        /// </summary>
        /// <param name="pliki">Lista plików do usunięcia</param>
        private void wykasujPlik(List<pojedynczyPlik> pliki)
        {
            foreach (pojedynczyPlik p in pliki)
            {
                try
                {
                    File.Delete(p.nazwa);
                }
                catch (Exception)
                {   string str = "Nie mozna usunac pliku " + p.nazwa + "\r\n";
                    switch (kontrolaBledow)
                    {
                        case 0: wyswietlBlad(str); break;
                        case 1: zapiszBlad(str); break;
                        case 2: MessageBox.Show(str); break;
                    }
                }
            }
        }
        /// <summary>
        /// Zprawdza jakie pliki są w katalogu
        /// </summary>
        /// <param name="katalog">Katalog w którym ma sprawdzać</param>
        /// <param name="lista">Lista plików jakie się znajdują w katalogu</param>
        private void plikiKatalog(string katalog, pliki lista)
        {
            if (lista == null) return;
            try
            {
                string tmpKatalog = folder + ((folder[folder.Length - 1] == '/') ? "" : "/") + katalog;
                DirectoryInfo di = new DirectoryInfo(tmpKatalog);
                FileInfo[] files = di.GetFiles();
                foreach (FileInfo fi in files)
                    lista.plik.Add(new pojedynczyPlik((long)(fi.LastWriteTime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds, katalog + (katalog == "" ? "" : "/") + fi.Name, hashPliku(tmpKatalog + "/" + fi.Name)));
                foreach (DirectoryInfo diTmp in di.GetDirectories())
                    plikiKatalog(katalog + (katalog==""?"":"/") + diTmp.Name, lista);
            }
            catch (Exception ex)
            {
                string str = "Nie mozna pobrac listy plikow z katalogu " + katalog + "\r\n" + ex.ToString();
                switch (kontrolaBledow)
                {
                    case 0: wyswietlBlad(str); break;
                    case 1: zapiszBlad(str); break;
                    case 2: MessageBox.Show(str); break;
                }
            }
        }
        /// <summary>
        /// Funkcja sprawdza jakie zmieny nastąpiły na dysku lub serwerze i w zależności od tego jakie pliki się pojawiły podejmuje odpowiednią akcję
        /// </summary>
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
                switch (kontrolaBledow)
                {
                    case 0: wyswietlBlad(ex.ToString()); break;
                    case 1: zapiszBlad(ex.ToString()); break;
                    case 2: MessageBox.Show(ex.ToString()); break;
                }
                return;
            }
            if (plikiNaSerwerze == null)
                plikiNaSerwerze = new List<plikInfo>();
            if (plikiNaSerwerze == null) return;
            pliki plikiLokalnieZapis = plikiZapisane();
            pliki plikiLokalnieKat = new pliki();
            plikiKatalog("", plikiLokalnieKat);
            if (plikiLokalnieKat == null || plikiLokalnieZapis == null) return;
            List<plikInfo> roznicaSerwerLok = sprawdzRoznicaDownload(plikiNaSerwerze, plikiLokalnieZapis, plikiLokalnieKat);
            List<pojedynczyPlik> roznicaLokSerwer = sprawdzRoznicaUpload(plikiNaSerwerze, plikiLokalnieZapis, plikiLokalnieKat);
            List<pojedynczyPlik> roznicaLokKat = sprawdzRoznicaUsunSerwer(plikiNaSerwerze, plikiLokalnieZapis, plikiLokalnieKat);
            List<pojedynczyPlik> plikiDoWykasowania = sprawdzRoznicaUsunKatalog(plikiNaSerwerze, plikiLokalnieZapis, plikiLokalnieKat);
            List<pojedynczyPlik> plikiDoAktualizacjiS = sprawdzAktualizacje(plikiLokalnieZapis, plikiLokalnieKat, plikiNaSerwerze); 
            List<plikInfo> plikiDoAktualizacjiK = sprawdzAktualizacje(plikiNaSerwerze, plikiLokalnieZapis, plikiLokalnieKat); 
            List<plikInfo> p = new List<plikInfo>();
            if (roznicaSerwerLok.Count != 0)
            {
                odbierzPliki(roznicaSerwerLok);
            }
            if (roznicaLokSerwer.Count != 0)
            {
                try
                {
                    wyslijPlik(roznicaLokSerwer);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            if (roznicaLokKat.Count != 0)
                usunPliki(roznicaLokKat);
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
        /// <summary>
        /// Sprawdza jakie są różnice pomiędzy plikami na serwerze i w katalogu
        /// </summary>
        /// <param name="plik">Lista plikow w pliku konfiguracyjnym</param>
        /// <param name="katalog">Lista plików w katalogu</param>
        /// <param name="serwer">Lista plików na serwerze</param>
        /// <returns>Lista plików do aktualizacji</returns>
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
        /// <summary>
        /// Sprawdza jakie są różnice pomiędzy plikami na serwerze i w katalogu
        /// </summary>
        /// <param name="plik">Lista plikow w pliku konfiguracyjnym</param>
        /// <param name="katalog">Lista plików w katalogu</param>
        /// <param name="serwer">Lista plików na serwerze</param>
        /// <returns>Lista plików do aktualizacji</returns>
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
        /// <summary>
        /// Sprawdza jakie są różnice pomiędzy plikami na serwerze i w katalogu
        /// </summary>
        /// <param name="plik">Lista plikow w pliku konfiguracyjnym</param>
        /// <param name="katalog">Lista plików w katalogu</param>
        /// <param name="serwer">Lista plików na serwerze</param>
        /// <returns>Lista plików do aktualizacji</returns>
        private void usunPliki(List<pojedynczyPlik> pliki)
        {
            try
            {
                if (k.usunPliki(pliki) == 403)
                {   string str = "Nie udalo sie usunac niektorych plikow z serwera, pobierz liste plikow w celu sprawdzenia\r\n";
                switch (kontrolaBledow)
                {
                    case 0: wyswietlBlad(str); break;
                    case 1: zapiszBlad(str); break;
                    case 2: MessageBox.Show(str); break;
                }
                }
            }
            catch (Exception ex)
            {
                switch (kontrolaBledow)
                {
                    case 0: wyswietlBlad(ex.ToString()); break;
                    case 1: zapiszBlad(ex.ToString()); break;
                    case 2: MessageBox.Show(ex.ToString()); break;
                }
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
