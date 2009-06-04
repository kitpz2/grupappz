using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Security.Cryptography;
using GlacialComponents.Controls;
namespace ASS8.Klient
{
    public class komunikacja
    {
        /// <summary>
        /// Typ numeryczny reprezentuje wszystkie akcje dostepne w komunikacji z serwerem
        /// </summary>
        private enum operacje
        {
            lista = 100, pobieranie, wysylanie, usuwanie
        }
        private const string endl = "\r\n\r\n";
        private const string endlOdp = "#";
        Socket socket;
        /// <summary>
        /// Wysyła wiadomość do serwera
        /// </summary>
        /// <param name="wiadomosc">Wiadomość do wysłania</param>
        /// <param name="dlugosc">Długość wiadomości</param>
        public void wyslij(byte[] wiadomosc, int dlugosc)
        {

            try
            {
                socket.Send(wiadomosc);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        /// <summary>
        /// Pobiera wiadomość od serwera
        /// </summary>
        /// <param name="dlugosc">Długość do pobrania</param>
        /// <param name="bytes">Tablica do zapisania odebranych danych</param>
        /// <returns>Długość faktycznie pobranych danych</returns>
        private int pobierz(int dlugosc, out byte[] bytes)
        {
            bytes = new byte[dlugosc];
            int i = 0;
            try
            {
                i = socket.Receive(bytes, 0, dlugosc, SocketFlags.None);
            }
            catch (Exception)
            {
                throw new Wyjatki.BladWysylania("Blad podczas odbierania");
            }

            return i;
        }
        /// <summary>
        /// Pobiera dane z serwera
        /// </summary>
        /// <returns>Wiadomość pobrana</returns>
        private string pobierz()
        {
            int dlugoscOdp = 0;
            StringBuilder strRead = new StringBuilder();
            while (true)
            {
                byte[] buffer = new byte[1024];
                int rx = socket.Receive(buffer, 0, 1, SocketFlags.None);
                dlugoscOdp += rx;
                strRead.Append(Encoding.ASCII.GetString(buffer).Substring(0, rx));
                if (dlugoscOdp >= 2)
                    if (strRead.ToString().Substring(strRead.Length - endlOdp.Length, endlOdp.Length).CompareTo(endlOdp) == 0) break;
            }
            return strRead.ToString().Substring(0, strRead.Length - endlOdp.Length);
        }
        /// <summary>
        /// Typ reprezentuje możliwe odpowiedzi serwera
        /// </summary>
        private enum odpowiedzi
        {
            bledne_zapytanie = 400, bledny_numer_sesji, plik_istnieje, blad_serwera, plik_nie_istnieje, blad_odbierania_plikow, wszystko_ok
        }
        public string folder;
        XmlSerializerNamespaces names;
        private int sessionID;
        private String serverIP;
        private int serverPort;
        private string log;
        private string haslo;
        /// <summary>
        /// Ustawia serwer i port do połączenia
        /// </summary>
        /// <param name="s">Adres serwera</param>
        /// <param name="p">Port serwera</param>
        public void ustawUstawienia(string s, int p)
        {
            serverIP = s;
            serverPort = p;
        }
        /// <summary>
        /// Kontruktor klasy
        /// </summary>
        public komunikacja()
        {
            names = new XmlSerializerNamespaces();
            names.Add("", "");
            log = "";
            haslo = "";
        }
        /// <summary>
        /// Loguje do serwera
        /// </summary>
        /// <returns>Zwraca odpowiedz od serwera</returns>
        public int login()
        {
            if (log == "" || haslo == "") return -2;
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                IPAddress remoteIPAddress = System.Net.IPAddress.Parse(serverIP);
                IPEndPoint remoteEndPoint = new System.Net.IPEndPoint(remoteIPAddress, serverPort);
                socket.Connect(remoteEndPoint);
            }
            catch (Exception)
            {
                throw new Wyjatki.BladPolaczenia("Blad podczas laczenia do serwera. Sprawdz adres oraz port");
            }
            try
            {
                StringWriter stringWriter = new StringWriter();
                klientLogowanie logowanie = new klientLogowanie(log, haslo, ASS8___Logowanie.wersja);
                XmlSerializer xml = new XmlSerializer(typeof(klientLogowanie));
                xml.Serialize(stringWriter, logowanie, names);
                string stR = stringWriter.ToString() + endl;
                wyslij(ASCIIEncoding.ASCII.GetBytes(stR), stR.Length);
            }

            catch (Exception)
            {
                throw new Wyjatki.BladWysylania("Blad podczas wysylania danych na serwer. Sprawdz polaczenie z internetem, oraz ewentualnie ustaw proxy -- zapytanie o logowanie");
            }
            string str;
            try
            {
                str = pobierz();
            }
            catch (Wyjatki.BladOdbierania bo)
            {
                bo.add("-- odpowiedz logowania");
                throw bo;
            }
            catch (Exception)
            {
                throw new Wyjatki.BladNieokreslony("Nieokreslony blad programu -- odpowiedz logowania");
            }
            serwerLogowanie odpSerwera = new serwerLogowanie();
            try
            {
                XmlSerializer xml = new XmlSerializer(typeof(serwerLogowanie));
                StringReader stringReader = new StringReader(str);
                odpSerwera = (serwerLogowanie)xml.Deserialize(stringReader);
            }
            catch (Exception)
            {
                throw new Wyjatki.BladParsowania("Dostano bledne dane od serwera lub nastapil blad programu -- deserializacja odpowiedzi o logowanie");
            }
            if (odpSerwera.odpowiedz == 1)
            {
                throw new Wyjatki.BladNieokreslony("Bledny login lub haslo");
            }
            if (odpSerwera.odpowiedz == 0)
                sessionID = odpSerwera.sesja;

            return odpSerwera.odpowiedz;
        }
        /// <summary>
        /// Ściąga pliki z serwera
        /// </summary>
        /// <param name="nazwyPlikow">Lista plików do ściagnięcia</param>
        /// <param name="uzytkownik">Użytkownik od którego mają być ściągane pliki</param>
        /// <param name="gli">Zmienna reprezentuje pasek postępu</param>
        /// <param name="path">Zmienna reprezentuje folder do zapisu pliku</param>
        /// <returns>Odpowiedź od serwera lub 1 w przypadku gdy wszystko poszło dobrze</returns>
        public int downloadFiles(string[] nazwyPlikow, string uzytkownik, GLItem gli, string path)
        {
            if (uzytkownik.Length == 0) uzytkownik = ".";
            try
            {
                downloadPliku download = new downloadPliku(sessionID, (int)operacje.pobieranie, uzytkownik, nazwyPlikow);
                XmlSerializer xml = new XmlSerializer(typeof(downloadPliku));
                StringWriter stringWriter = new StringWriter();
                xml.Serialize(stringWriter, download, names);
                string str = stringWriter.ToString() + endl;
                wyslij(ASCIIEncoding.ASCII.GetBytes(str), str.Length);
            }
            catch (Exception)
            {
                throw new Wyjatki.BladWysylania("Blad podczas wysylania danych na serwer. Sprawdz polaczenie z internetem, oraz ewentualnie ustaw proxy -- zapytanie o sciagniecie plikow");
            }
            foreach (string s in nazwyPlikow)
            {
                serwerPliki plikiNaSerwerze = new serwerPliki();
                string[] tab = s.Split("/".ToCharArray());
                string tmp = (folder[folder.Length-1]=='/'?folder:folder+"/");
                for (int i = 0; i < tab.Length - 1; i++)
                {
                    tmp += tab[i] + "/";
                    if (!Directory.Exists(tmp))
                        Directory.CreateDirectory(tmp);
                }
                try
                {
                    XmlSerializer xml = new XmlSerializer(typeof(serwerPliki));
                    string str = pobierz();

                    StringReader stringReader = new StringReader(str);
                    plikiNaSerwerze = (serwerPliki)xml.Deserialize(stringReader);
                }
                catch (Wyjatki.BladOdbierania bo)
                {
                    bo.add("-- odpowiedz sciaganie plikow");
                    throw bo;
                }
                catch (Exception)
                {
                    throw new Wyjatki.BladParsowania("Dostano bledne dane od serwera lub nastapil blad programu -- odpowiedz sciaganie plikow");
                }
                if (plikiNaSerwerze.operacja != (int)operacje.pobieranie) return -3;
                if (plikiNaSerwerze.odp == (int)odpowiedzi.wszystko_ok)
                {
                    if (plikiNaSerwerze.plik.Count != 1)
                    { 
                        StringWriter stringWriter = new StringWriter();
                        klientOdpDownload kod = new klientOdpDownload(sessionID, (int)operacje.pobieranie, "ok");
                        XmlSerializer xml = new XmlSerializer(typeof(klientOdpDownload));
                        xml.Serialize(stringWriter, kod, names);
                        string str = stringWriter.ToString() + endl;
                        wyslij(ASCIIEncoding.ASCII.GetBytes(str), str.Length);
                        StringReader stringReader = new StringReader("");
                        str = pobierz();
                        XmlSerializer xmlO = new XmlSerializer(typeof(serwerBase));
                        serwerBase odpSerwera = (serwerBase)xmlO.Deserialize(new StringReader(str));
                        continue;
                    }
                    plikInfo p = plikiNaSerwerze.plik[0];
                    if (p.rozmiar >= 0)
                    {
                        FileStream streamWriter = null;
                        if (path == null)
                            streamWriter = new FileStream(folder + "/" + p.nazwa, FileMode.OpenOrCreate, FileAccess.Write);
                        else
                        {
                            if (p.rozmiar == 0)
                                ((ProgressBar)gli.SubItems[1].Control).PerformStep();
                            streamWriter = new FileStream(path + "/" + p.nazwa, FileMode.OpenOrCreate, FileAccess.Write);
                        }
                        try
                        {
                            StringWriter stringWriter = new StringWriter();
                            klientOdpDownload kod = new klientOdpDownload(sessionID, (int)operacje.pobieranie, "ok");
                            XmlSerializer xml = new XmlSerializer(typeof(klientOdpDownload));
                            xml.Serialize(stringWriter, kod, names);
                            string str = stringWriter.ToString() + endl;
                            wyslij(ASCIIEncoding.ASCII.GetBytes(str), str.Length);
                            int rozmiar = p.rozmiar;
                            int tempRozmiar = rozmiar;
                            while (tempRozmiar > 0)
                            {

                                int readSize = 102400;
                                if (tempRozmiar < readSize)
                                {
                                    readSize = tempRozmiar;
                                }
                                byte[] bytes;
                                int odebrane = pobierz(readSize, out bytes);
                                tempRozmiar -= odebrane;
                                streamWriter.Write(bytes, 0, odebrane);
                                if (gli != null)
                                {
                                    ProgressBar pb = (ProgressBar)gli.SubItems[1].Control;
                                    object[] obj = new object[] { pb, odebrane };
                                    postep(pb, odebrane);
                                }
                            }
                            streamWriter.Close();
                        }
                        catch (Exception ex)
                        {
                            throw new Wyjatki.BladOdbierania("Wystapil Blad podczas odbierania pliku z serwera -- odbieranie pliku i zapisywanie");
                        }
                        finally
                        {
                            streamWriter.Close();
                        }
                    }
                }
            }
            return 1;
        }
        private delegate void ProgressBarHandler(ProgressBar bar, int progressValue);
        /// <summary>
        /// Aktualizuje postęp pliku w pasku postępu
        /// </summary>
        /// <param name="bar">Pasek postępu</param>
        /// <param name="progressValue">Wartość do zaktualizowania</param>
        private void postep(ProgressBar bar, int progressValue)
        {
            /*if (bar.InvokeRequired)
            {
                bar.Invoke(new ProgressBarHandler(postep), new object[] { progressValue });
            }
            else
            {
                bar.Value = progressValue;
            }*/
        }
        /// <summary>
        /// Wysyła plik na serwer
        /// </summary>
        /// <param name="nazwa">Nazwa pliku</param>
        /// <param name="data">Data pliku</param>
        /// <param name="rozmiar">Rozmiar pliku</param>
        /// <returns>Odpowiedź od serwera</returns>
        public int uploadFile(string nazwa, DateTime data, int rozmiar)
        {
            string katalog = ((folder[folder.Length - 1] == '/') ? folder : folder + "/");
            if (!File.Exists(katalog + nazwa)) return -4;
            try
            {
                StringWriter stringWriter = new StringWriter();
                XmlSerializer xml = new XmlSerializer(typeof(klientUpload));
                klientUpload upload = new klientUpload(sessionID, (int)operacje.wysylanie, nazwa, (long)(data - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds, rozmiar, 0, hashPliku(katalog + nazwa));
                xml.Serialize(stringWriter, upload, names);
                string str = stringWriter.ToString() + endl;
                wyslij(ASCIIEncoding.ASCII.GetBytes(str), str.Length);
            }
            catch (Exception)
            {
                throw new Wyjatki.BladWysylania("Blad podczas wysylania danych na serwer. Sprawdz polaczenie z internetem, oraz ewentualnie ustaw proxy -- wysylanie zapytanie");
            }
            serwerBase odpSerwera = new serwerBase();
            try
            {
                StringReader stringReader = new StringReader("");
                string str = pobierz();
                XmlSerializer xml = new XmlSerializer(typeof(serwerBase));
                odpSerwera = (serwerBase)xml.Deserialize(new StringReader(str));
            }
            catch (Wyjatki.BladOdbierania bo)
            {
                bo.add("-- odpowiedz wgrywanie plikow");
                throw bo;
            }
            catch (Exception)
            {
                throw new Wyjatki.BladParsowania("Dostano bledne dane od serwera lub nastapil blad programu -- odpowiedz wgrywanie plikow");
            }
            if (odpSerwera.operacja != (int)operacje.wysylanie) return -3;
            if (odpSerwera.odp == (int)odpowiedzi.plik_istnieje)
            {
                string odp;
                if (MessageBox.Show("Plik " + nazwa + " istnieje na serwerze, czy zastapic go?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    odp = "ok";
                }
                else
                {
                    odp = "abort";
                }
                try
                {
                    StringWriter stringWriter = new StringWriter();
                    XmlSerializer xml = new XmlSerializer(typeof(klientOdpDownload));
                    klientOdpDownload kod = new klientOdpDownload(sessionID, (int)operacje.wysylanie, odp);
                    xml.Serialize(stringWriter, kod, names);
                    string str = stringWriter.ToString() + endl;
                    wyslij(ASCIIEncoding.ASCII.GetBytes(str), str.Length);
                }
                catch (Exception)
                {
                    throw new Wyjatki.BladWysylania("Blad podczas wysylania danych na serwer. Sprawdz polaczenie z internetem, oraz ewentualnie ustaw proxy -- odpowiedz czy plik zastapic");
                }
                try
                {
                    StringReader stringReader = new StringReader("");
                    string str2 = pobierz();
                    XmlSerializer x = new XmlSerializer(typeof(serwerBase));
                    odpSerwera = (serwerBase)x.Deserialize(new StringReader(str2));
                }
                catch (Wyjatki.BladOdbierania bo)
                {
                    bo.add("-- odpowiedz na zapytanie czy zastapic");
                    throw bo;
                }
                catch (Exception ex)
                {
                    throw new Wyjatki.BladParsowania("Dostano bledne dane od serwera lub nastapil blad programu -- odpowiedz na zapytanie czy zastapic" + ex.ToString());
                }
            }
            if (odpSerwera.operacja != (int)operacje.wysylanie) return -3;
            if (odpSerwera.odp == (int)odpowiedzi.wszystko_ok || odpSerwera.odp == (int)odpowiedzi.plik_nie_istnieje)
            {
                int rozmiarUp = 102400;
                int rozmiarTmp = rozmiar;
                FileStream fileStream = new FileStream(katalog + nazwa, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fileStream);
                int c = 0;
                try
                {
                    while (rozmiarTmp != 0)
                    {
                        c++;
                        if (rozmiarTmp < rozmiarUp)
                            rozmiarUp = rozmiarTmp;
                        byte[] bytes = br.ReadBytes(rozmiarUp);
                        wyslij(bytes, rozmiarUp);
                        rozmiarTmp -= rozmiarUp;
                        System.Threading.Thread.Sleep(100);
                    }
                }
                catch (Exception)
                {
                    throw new Wyjatki.BladWysylania("Blad podczas wysylania pliku na serwer -- wysylanie pliku");
                }
                finally
                {
                    fileStream.Close();
                }
                fileStream.Close();
                try
                {
                    XmlSerializer xml = new XmlSerializer(typeof(klientHash));
                    klientHash khash = new klientHash(sessionID, (int)operacje.wysylanie, hashPliku(katalog + nazwa));
                    StringWriter sw = new StringWriter();
                    xml.Serialize(sw, khash, names);
                    string str = sw.ToString() + endl;
                    wyslij(ASCIIEncoding.ASCII.GetBytes(str), str.Length);
                }
                catch (Exception)
                {
                    throw new Wyjatki.BladWysylania("Blad podczas wysylania pliku na serwer -- wysylanie hasha");
                }
            }
            return odpSerwera.odp;
        }
        /// <summary>
        /// Oblicza hash pliku algorytmem MD5
        /// </summary>
        /// <param name="plik">Ścieża do pliku</param>
        /// <returns>Hash pliku</returns>
        private string hashPliku(string plik)
        {
            if (!File.Exists(plik)) throw new Wyjatki.BrakPliku("Brak pliku " + plik);
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
        /// Ściąga listę plików użytkwonika
        /// </summary>
        /// <param name="uzytkownik">Użytkownika którego listę plików chcemy ściągnać</param>
        /// <returns>Lista plików użytkownika</returns>
        public List<plikInfo> downloadListy(string uzytkownik)
        {
            if (uzytkownik.Length == 0) uzytkownik = ".";
            try
            {
                listaPlikow lista = new listaPlikow(sessionID, (int)operacje.lista, uzytkownik);
                XmlSerializer xml = new XmlSerializer(typeof(listaPlikow));
                StringWriter stringWriter = new StringWriter();
                xml.Serialize(stringWriter, lista, names);
                string str = stringWriter.ToString() + endl;
                wyslij(ASCIIEncoding.ASCII.GetBytes(str), str.Length);
            }
            catch (Exception)
            {
                throw new Wyjatki.BladWysylania("Blad podczas pobierania listy plikow -- zapytanie o download");
            }
            serwerPliki pliki = new serwerPliki();
            try
            {
                XmlSerializer xml = new XmlSerializer(typeof(serwerPliki));
                StringReader stringReader = new StringReader("");
                string str = pobierz();
                pliki = (serwerPliki)xml.Deserialize(new StringReader(str));
            }
            catch (Wyjatki.BladOdbierania bo)
            {
                bo.add("-- odpowiedz na pobranie listy");
                throw bo;
            }
            catch (Exception)
            {
                throw new Wyjatki.BladParsowania("Dostano bledne dane od serwera lub nastapil blad programu -- odpowiedz na pobranie listy");
            }
            if (pliki.operacja != (int)operacje.lista) return null;
            if (pliki.odp != (int)odpowiedzi.wszystko_ok) return null;
            return pliki.plik;
        }
        /// <summary>
        /// Usuwa pliki z serwer
        /// </summary>
        /// <param name="plikiDoUsuniecia">Lista plików do usunięcia</param>
        /// <returns>Odpowiedź od serwera</returns>
        public int usunPliki(List<pojedynczyPlik> plikiDoUsuniecia)
        {
            List<plikInfo> plikiDoUs = new List<plikInfo>();
            foreach (pojedynczyPlik p in plikiDoUsuniecia)
                plikiDoUs.Add(new plikInfo(p.nazwa, -1, -1, -1, p.hash));
            try
            {
                klientUsun usun = new klientUsun(sessionID, (int)operacje.usuwanie, plikiDoUs);
                XmlSerializer xml = new XmlSerializer(typeof(klientUsun));
                StringWriter stringWriter = new StringWriter();
                xml.Serialize(stringWriter, usun, names);
                string strToWrite = stringWriter.ToString() + endl;
                wyslij(ASCIIEncoding.ASCII.GetBytes(strToWrite), strToWrite.Length);
            }
            catch (Exception ex)
            {
                throw new Wyjatki.BladWysylania("Blad podczas usuwania plików z serwera -- usuwanie zapytanie");
            }
            for (int j = 0; j < plikiDoUsuniecia.Count; j++)
            {
                serwerBase odpSerw = new serwerBase();
                try
                {
                    XmlSerializer xml = new XmlSerializer(typeof(serwerBase));
                    string str = pobierz();
                    odpSerw = (serwerBase)xml.Deserialize(new StringReader(str));
                }
                catch (Wyjatki.BladOdbierania bo)
                {
                    bo.add("-- usuwanie odpowiedz");
                    throw bo;
                }
                catch (Exception)
                {
                    throw new Wyjatki.BladParsowania("Dostano bledne dane od serwera lub nastapil blad programu -- usuwanie odpowiedz");
                }
                if (odpSerw.odp == (int)odpowiedzi.blad_serwera || odpSerw.odp == (int)odpowiedzi.bledny_numer_sesji) return odpSerw.odp;
            }
            return 1;
        }
        /// <summary>
        /// Właściwość ustawia lub zwraca login do połączenia z serwerem
        /// </summary>
        public string Login
        {
            get
            {
                return log;
            }
            set
            {
                log = value;
            }
        }
        /// <summary>
        /// Właściwość ustawia lub zwraca adres do połączenia z serwerem
        /// </summary>
        public string Serwer
        {
            get
            {
                return serverIP;
            }
        }
        /// <summary>
        /// Właściwość ustawia lub zwraca port do połączenia z serwerem
        /// </summary>
        public int Port
        {
            get
            {
                return serverPort;
            }
        }
        /// <summary>
        /// Właściwość ustawia lub zwraca hasło do połączenia z serwerem
        /// </summary>
        public string Haslo
        {
            set
            {
                haslo = value;
            }
            get
            {
                return haslo;
            }
        }

    }
}
