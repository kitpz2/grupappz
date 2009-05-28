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
        private enum operacje
        {
            lista = 100, pobieranie, wysylanie, usuwanie
        }
        private const string endl = "\r\n\r\n";
        private const string endlOdp = ">";
        //NetworkStream stream;
        Socket socket;
        public void wyslij(byte[] wiadomosc, int dlugosc)
        {

            socket.Send(wiadomosc);
            MessageBox.Show(ASCIIEncoding.ASCII.GetString(wiadomosc));
        }
        private byte[] pobierz(int dlugosc)
        {
            byte[] bytes = new byte[dlugosc];
            socket.Receive(bytes, dlugosc, SocketFlags.None);
            return bytes;
        }
        private string pobierz()
        {/*
            int dlugoscOdp = 0;
            NetworkStream streamS = new NetworkStream();
            StreamReader s = new StreamReader(streamS);
            StringBuilder strRead = new StringBuilder("");
            string str;
            try
            {
                str = s.ReadLine();
                MessageBox.Show(str);
     //           MessageBox.Show(str+"|");
               /* do
                {
                    Byte[] bytes = new Byte[256];
                    int i = stream.Read(bytes, 0, 256);
                    dlugoscOdp += i;

                    strRead.Append(Encoding.ASCII.GetString(bytes).Substring(0, i));
                    FileStream fs = new FileStream("plik.txt", FileMode.Append, FileAccess.Write);
                    fs.Write(Encoding.ASCII.GetBytes(strRead.ToString() + "\r\n"), 0, (strRead.ToString() + "\r\n").Length);
                    fs.Close();
                    if (dlugoscOdp == 0) throw new Exception();
                    if (dlugoscOdp >= 2)
                        if (strRead.ToString().Substring(strRead.Length - endlOdp.Length, endlOdp.Length).CompareTo(endlOdp) == 0) break;
                } while (true);

            }
            catch (Exception)
            {
                throw new Wyjatki.BladOdbierania("Błąd podczas odbierania wiadomosci z serwera");
            }
            finally
            {
                //s.Close();
            }
            //s.Close();*/
            int dlugoscOdp=0;
            StringBuilder strRead=new StringBuilder();
            while (true)
            {
                byte[] buffer = new byte[1024];
                int rx = socket.Receive(buffer);
                dlugoscOdp += rx;
                strRead.Append(Encoding.ASCII.GetString(buffer).Substring(0, rx));
                if (dlugoscOdp >= 2)
                    if (strRead.ToString().Substring(strRead.Length - endlOdp.Length, endlOdp.Length).CompareTo(endlOdp) == 0) break;
            }

            return strRead.ToString();
        }
        private enum odpowiedzi
        {
            bledne_zapytanie = 400, bledny_numer_sesji, plik_istnieje, blad_serwera, plik_nie_istnieje, blad_odbierania_plikow, wszystko_ok
        }
        string folder = "pliki";
        XmlSerializerNamespaces names;
        private int sessionID;//83.24.57.228
        private String serverIP;
        private int serverPort;
        //private TcpClient serwer;
        private string log;
        private string haslo;
        public void pobierzUstawienia()
        {
            XmlSerializer xml = new XmlSerializer(typeof(ustawienia));
            if (File.Exists(log+@"/"+"ustawienia.ini"))
            {
                try
                {
                    
                    TextReader tr = new StreamReader(log+"//ustawienia.ini");
                    ustawienia set = new ustawienia();
                    set = (ustawienia)xml.Deserialize(tr);
                    serverIP = set.ip;
                    serverPort = set.port;
                    tr.Close();
                }
                catch (Exception)
                {
                    throw new Wyjatki.BladParsowania("Blad podczas odczytywania adresu serwera. Sprawdz czy jest on poprawny");
                }
            }
            else
            {
                try
                {
                    if (!Directory.Exists(log)) 
                        Directory.CreateDirectory(log);
                    FileStream fs = new FileStream(log+"//ustawienia.ini", FileMode.Create, FileAccess.Write);
                    ustawienia set = new ustawienia("127.0.0.1", 2000);
                    xml.Serialize(fs, set, names);
                    fs.Close();
                }
                catch (Exception)
                {
                    throw new Wyjatki.BladParsowania("Blad podczas tworzenia pliku ustawien. Sprawdz czy istnieje i czy nie jest otwarty.");
                }
            }
        }
        public komunikacja()
        {
            names = new XmlSerializerNamespaces();
            names.Add("", "");
            log = "";
            haslo = "";
        }
        public int login()
        {
            if (log == "" || haslo == "") return -2;
            /* IPAddress[] serv = Dns.GetHostAddresses(serverIP);
             StringBuilder strB=new StringBuilder("");
             foreach (IPAddress ip in serv)
                 strB.Append(ip.ToString());
             MessageBox.Show(strB.ToString());*/
            try
            {
                //serwer = new TcpClient(serverIP, serverPort);
               // stream = serwer.GetStream();
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                IPAddress remoteIPAddress = System.Net.IPAddress.Parse(serverIP);
                IPEndPoint remoteEndPoint = new System.Net.IPEndPoint(remoteIPAddress, serverPort);
                socket.Connect(remoteEndPoint);
            }
            catch (Exception)
            {
                throw new Wyjatki.BladPolaczenia("Blad podczas laczenia do serwera. Sprawdz adres oraz port");
            }

            /*int dlug = 0;
            StringBuilder strR = new StringBuilder("");
            try
            {
                do
                {
                    Byte[] bytes = new Byte[256];
                    int i = stream.Read(bytes, 0, 256);
                    dlug += i;
                    strR.Append(Encoding.ASCII.GetString(bytes));
                    if (dlug >= 2)
                        if (strR[dlug - 1] == '\n' && strR[dlug - 2] == '\r') break;
                    MessageBox.Show(dlug.ToString() + strR.ToString().Substring(0, dlug));
                } while (true);
            }
            catch (Exception)
            {
                MessageBox.Show("Nie mozna ustanowic polaczenia z serwerem");
            }
            MessageBox.Show("Aaaa"+dlug.ToString()+strR.ToString().Substring(0,dlug));*/
            //if (!serwer.Connected) return -1;
            try
            {
                StringWriter stringWriter = new StringWriter();
                klientLogowanie logowanie = new klientLogowanie(log, haslo, ASS8___Logowanie.wersja);
                XmlSerializer xml = new XmlSerializer(typeof(klientLogowanie));
                xml.Serialize(stringWriter, logowanie, names);
                string stR = stringWriter.ToString() + endl;
                wyslij(ASCIIEncoding.ASCII.GetBytes(stR), stR.Length);
                /*string strToWrite = stringWriter.ToString() + endl;
                stream.Write(Encoding.ASCII.GetBytes(strToWrite.ToCharArray()), 0, strToWrite.Length);*/
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

        public int downloadFiles(string[] nazwyPlikow, string uzytkownik,GLItem gli)
        {
            if (uzytkownik.Length == 0) uzytkownik = ".";
            //if (!serwer.Connected) return -1;
            try
            {
                downloadPliku download = new downloadPliku(sessionID, (int)operacje.pobieranie, uzytkownik, nazwyPlikow);
                XmlSerializer xml = new XmlSerializer(typeof(downloadPliku));
                StringWriter stringWriter = new StringWriter();
                xml.Serialize(stringWriter, download, names);
                string str = stringWriter.ToString() + endl;
                wyslij(ASCIIEncoding.ASCII.GetBytes(str), str.Length);
                /*string strToWrite = stringWriter.ToString() + endl;
                stream.Write(Encoding.ASCII.GetBytes(strToWrite.ToCharArray()), 0, strToWrite.Length);*/
            }
            catch (Exception)
            {
                throw new Wyjatki.BladWysylania("Blad podczas wysylania danych na serwer. Sprawdz polaczenie z internetem, oraz ewentualnie ustaw proxy -- zapytanie o sciagniecie plikow");
            }
            serwerPliki plikiNaSerwerze = new serwerPliki();
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
                List<plikInfo> pliki = plikiNaSerwerze.plik;
                foreach (plikInfo p in pliki)
                    if (p.rozmiar >= 0)
                    {
                        FileStream streamWriter = new FileStream(folder + "//" + p.nazwa, FileMode.OpenOrCreate, FileAccess.Write);
                        try
                        {
                            StringWriter stringWriter = new StringWriter();
                            klientOdpDownload kod = new klientOdpDownload(sessionID, (int)operacje.pobieranie, "ok");
                            XmlSerializer xml = new XmlSerializer(typeof(klientOdpDownload));
                            xml.Serialize(stringWriter, kod, names);
                            string str = stringWriter.ToString() + endl;
                            wyslij(ASCIIEncoding.ASCII.GetBytes(str), str.Length);
                            /*string str = stringWriter.ToString() + endl;
                            stream.Write(Encoding.ASCII.GetBytes(str.ToCharArray()), 0, str.Length);*/
                            int rozmiar = p.rozmiar;
                            int tempRozmiar = rozmiar;
                            //NetworkStream stream = serwer.GetStream();
                            while (tempRozmiar > 0)
                            {

                                int readSize = 1024;
                                if (tempRozmiar < readSize)
                                {
                                    readSize = tempRozmiar;
                                }
                                byte[] bytes = pobierz(readSize);
                                //stream.Read(bytes, 0, readSize);
                                tempRozmiar -= readSize;
                                streamWriter.Write(bytes, 0, readSize);
                                if (gli != null)
                                {
                                    ProgressBar pb = (ProgressBar)gli.SubItems[1].Control;
                                    pb.PerformStep();
                                }
                            }
                            streamWriter.Close();
                        }
                        catch (Exception)
                        {
                            throw new Wyjatki.BladOdbierania("Wystapil Blad podczas odbierania pliku z serwera -- odbieranie pliku i zapisywanie");
                        }
                        finally
                        {
                            streamWriter.Close();
                        }
                        try
                        {
                            XmlSerializer xmlHash = new XmlSerializer(typeof(serwerHash));
                            serwerHash shash = new serwerHash();
                            string str2=pobierz();
                            shash = (serwerHash)xmlHash.Deserialize(new StringReader(str2));
                        }
                        catch (Wyjatki.BladOdbierania bo)
                        {
                            bo.add("-- odbieranie hasha pliku");
                            throw bo;
                        }
                        catch (Exception)
                        {
                            throw new Wyjatki.BladParsowania("Dostano bledne dane od serwera lub nastapil blad programu -- odbieranie hasha pliku");
                        }
                    }
            }
            return (int)plikiNaSerwerze.odp;
        }

        public int uploadFile(string nazwa, DateTime data, int rozmiar)
        {
            string katalog=((folder[folder.Length-1]=='/')?folder:folder+"//");
            if (!File.Exists(katalog + nazwa)) return -4;
            //if (!serwer.Connected) return -1;
            try
            {
                StringWriter stringWriter = new StringWriter();
                XmlSerializer xml = new XmlSerializer(typeof(klientUpload));
                klientUpload upload = new klientUpload(sessionID, (int)operacje.wysylanie, nazwa, (long)(data-new DateTime(1970,1,1,0,0,0)).TotalSeconds, rozmiar, 1,hashPliku(katalog+nazwa));
                xml.Serialize(stringWriter, upload, names);
                string str = stringWriter.ToString() + endl;
                wyslij(ASCIIEncoding.ASCII.GetBytes(str), str.Length);
                /*string str = stringWriter.ToString() + endl;
                stream.Write(Encoding.ASCII.GetBytes(str.ToCharArray()), 0, str.Length);*/
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
                    /*string str = stringWriter.ToString() + endl;
                    stream.Write(Encoding.ASCII.GetBytes(str.ToCharArray()), 0, str.Length);*/
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
                    odpSerwera = (serwerBase)x.Deserialize(new StreamReader(str2));
                }
                catch (Wyjatki.BladOdbierania bo)
                {
                    bo.add("-- odpowiedz na zapytanie czy zastapic");
                    throw bo;
                }
                catch (Exception)
                {
                    throw new Wyjatki.BladParsowania("Dostano bledne dane od serwera lub nastapil blad programu -- odpowiedz na zapytanie czy zastapic");
                }
            }
            if (odpSerwera.operacja != (int)operacje.wysylanie) return -3;
            if (odpSerwera.odp == (int)odpowiedzi.wszystko_ok || odpSerwera.odp == (int)odpowiedzi.plik_nie_istnieje)
            {
                int rozmiarUp = 10240;
                int rozmiarTmp = rozmiar;
                
                FileStream fileStream = new FileStream(katalog + nazwa, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fileStream);
                try
                {
                    while (rozmiarTmp != 0)
                    {
                        if (rozmiarTmp < rozmiarUp)
                            rozmiarUp = rozmiarTmp;
                        byte[] bytes = new byte[rozmiarUp];
                        fileStream.Read(bytes, 0, rozmiarUp);
                        wyslij(bytes,rozmiarUp);
                        rozmiarTmp -= rozmiarUp;
                    }
                    wyslij(ASCIIEncoding.ASCII.GetBytes(endl), endl.Length);
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
                    klientHash khash = new klientHash(sessionID, (int)operacje.wysylanie, hashPliku(katalog+nazwa));
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
        private string hashPliku(string plik)
        {
            if (!File.Exists(plik)) throw new Wyjatki.BrakPliku("Brak pliku "+plik);
            StringBuilder sb = new StringBuilder();
            FileStream fs = new FileStream(plik, FileMode.Open);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hash = md5.ComputeHash(fs);
            fs.Close();
            foreach (byte hex in hash)
                sb.Append(hex.ToString("x2"));
            return sb.ToString();
        }

        public List<plikInfo> downloadListy(string uzytkownik)
        {
            //if (!serwer.Connected) return null;
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
                throw new Wyjatki.BladWysylania("Blad podczas pobierania listy plikow -- zapytanie o liste");
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
            return pliki.plik.FindAll(delegate(plikInfo p){return p.hash!="-1";});
        }
        public int usunPliki(List<pojedynczyPlik> plikiDoUsuniecia)
        {
            //if (!serwer.Connected) return -1;
            List<string> plikiDoUs = new List<string>();
            foreach (pojedynczyPlik p in plikiDoUsuniecia)
                plikiDoUs.Add(p.nazwa);
            try
            {
                klientUsun usun = new klientUsun(sessionID, (int)operacje.usuwanie, plikiDoUs.ToArray());
                XmlSerializer xml = new XmlSerializer(typeof(downloadPliku));
                StringWriter stringWriter = new StringWriter();
                xml.Serialize(stringWriter, usun, names);
                string strToWrite = stringWriter.ToString() + endl;
                wyslij(ASCIIEncoding.ASCII.GetBytes(strToWrite), strToWrite.Length);
            }
            catch (Exception)
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
        public string Haslo
        {
            set
            {
                haslo = value;
            }
        }

    }
}
