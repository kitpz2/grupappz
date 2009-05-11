using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace ASS8.Klient
{
    public class komunikacja
    {
        private enum operacje
        {
            lista = 100, pobieranie, wysylanie, usuwanie
        }
        private const string endl = "\r\n\r\n";
        private const string endlOdp = "\n";
        private string pobierz()
        {
            NetworkStream stream = serwer.GetStream();
            int dlugoscOdp = 0;
            StringBuilder strRead = new StringBuilder("");
            try
            {
                do
                {
                    Byte[] bytes = new Byte[serwer.Client.ReceiveBufferSize];
                    int i = stream.Read(bytes, 0, serwer.Client.ReceiveBufferSize);
                    dlugoscOdp += i;
                    strRead.Append(Encoding.ASCII.GetString(bytes).Substring(0,i));
                    MessageBox.Show(Encoding.ASCII.GetString(bytes));
                    if (dlugoscOdp == 0) throw new Exception();
                    if (dlugoscOdp >= 2)
                        if (strRead.ToString().Substring(strRead.Length - endlOdp.Length, endlOdp.Length).CompareTo(endlOdp)==0) break;
                } while (true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nie mozna ustanowic polaczenia z serwerem: "+ex.ToString());
                return "";
            }
            return strRead.ToString().Substring(0, dlugoscOdp - endlOdp.Length);
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
        private TcpClient serwer;
        private string log;
        private string haslo;
        private void pobierzUstawienia()
        {
            XmlSerializer xml = new XmlSerializer(typeof(ustawienia));
            if (File.Exists("ustawienia.ini"))
            {
                try
                {
                    TextReader tr = new StreamReader("ustawienia.ini");
                    ustawienia set = new ustawienia();
                    set = (ustawienia)xml.Deserialize(tr);
                    serverIP = set.ip;
                    serverPort = set.port;
                    tr.Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("Blad podczas odczytu adresu serwera, upewnij sie, ze jest on poprawny!");
                    return;
                }
            }
            else
            {
                try
                {
                    FileStream fs = new FileStream("ustawienia.ini", FileMode.Create, FileAccess.Write);
                    ustawienia set = new ustawienia("127.0.0.1", 2000);
                    xml.Serialize(fs, set, names);
                    fs.Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("Blad podczas odczytu pliku konfiguracyjnego");
                }
            }
        }
        public komunikacja()
        {
            names = new XmlSerializerNamespaces();
            names.Add("", "");
            log = "";
            haslo = "";
            pobierzUstawienia();
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
                serwer = new TcpClient(serverIP, serverPort);
                NetworkStream stream = serwer.GetStream();
            }
            catch (Exception)
            {
                MessageBox.Show("Nie moge polaczyc z serwerem");
                return -5;
            }
            /*int dlug = 0;
            StringBuilder strR = new StringBuilder("");
            try
            {
                do
                {
                    Byte[] bytes = new Byte[serwer.Client.ReceiveBufferSize];
                    int i = stream.Read(bytes, 0, serwer.Client.ReceiveBufferSize);
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
            if (!serwer.Connected) return -1;
            NetworkStream networkStream = serwer.GetStream();
            try
            {
                StringWriter stringWriter = new StringWriter();
                klientLogowanie logowanie = new klientLogowanie(log, haslo, ASS8___Logowanie.wersja);
                XmlSerializer xml = new XmlSerializer(typeof(klientLogowanie));
                xml.Serialize(stringWriter, logowanie, names);
                string strToWrite = stringWriter.ToString() + endl;
                networkStream.Write(Encoding.ASCII.GetBytes(strToWrite.ToCharArray()), 0, strToWrite.Length);
            }

            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("Nie udalo sie zalogowac");
                return -1;
            }
            string str = pobierz();
            serwerLogowanie odpSerwera = new serwerLogowanie();
            //MessageBox.Show("23333333");
            try
            {
                XmlSerializer xml = new XmlSerializer(typeof(serwerLogowanie));
                StringReader stringReader = new StringReader(str);
                odpSerwera = (serwerLogowanie)xml.Deserialize(stringReader);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nie udalo sie odebrac odpowiedzi od serwera: " + ex.ToString());
                return -5;
            }
            //MessageBox.Show("ddddddddddd");
            if (odpSerwera.odpowiedz == 0)
                sessionID = odpSerwera.sesja;

            return odpSerwera.odpowiedz;
        }

        public int downloadFiles(string[] nazwyPlikow, string uzytkownik)
        {
            if (uzytkownik.Length == 0) uzytkownik = ".";
            if (!serwer.Connected) return -1;
            NetworkStream stream = serwer.GetStream();
            try
            {
                downloadPliku download = new downloadPliku(sessionID, (int)operacje.pobieranie, uzytkownik, nazwyPlikow);
                XmlSerializer xml = new XmlSerializer(typeof(downloadPliku));
                StringWriter stringWriter = new StringWriter();
                xml.Serialize(stringWriter, download, names);
                string strToWrite = stringWriter.ToString() + endl;
                stream.Write(Encoding.ASCII.GetBytes(strToWrite.ToCharArray()), 0, strToWrite.Length);
            }
            catch (Exception)
            {
                MessageBox.Show("Nie mozna wyslac zapytania o pliki");
                return -5;
            }
            serwerPliki plikiNaSerwerze = new serwerPliki();
            try
            {
                XmlSerializer xml = new XmlSerializer(typeof(serwerPliki));
                string str = pobierz();
                StringReader stringReader = new StringReader(str);
                plikiNaSerwerze = (serwerPliki)xml.Deserialize(stringReader);
            }
            catch (Exception)
            {
                MessageBox.Show("Odebrano bledne dane od serwera");
                return -5;
            }
            if (plikiNaSerwerze.operacja != (int)operacje.pobieranie) return -3;
            if (plikiNaSerwerze.odp == (int)odpowiedzi.wszystko_ok)
            {
                List<plikInfo> pliki = plikiNaSerwerze.plik;
                foreach (plikInfo p in pliki)
                    if (p.rozmiar >= 0)
                    {
                        StringWriter stringWriter = new StringWriter();
                        klientOdpDownload kod = new klientOdpDownload(sessionID, (int)operacje.pobieranie, "ok");
                        XmlSerializer xml = new XmlSerializer(typeof(klientOdpDownload));
                        xml.Serialize(stringWriter, kod, names);
                        string str = stringWriter.ToString() + endl;
                        stream.Write(Encoding.ASCII.GetBytes(str.ToCharArray()), 0, str.Length);
                        FileStream streamWriter = new FileStream(folder + "\\" + p.nazwa, FileMode.OpenOrCreate, FileAccess.Write);
                        int rozmiar = p.rozmiar;
                        int tempRozmiar = rozmiar;
                        while (tempRozmiar > 0)
                        {
                            byte[] bytes = new byte[256];
                            int readSize = bytes.Length;
                            if (tempRozmiar < readSize)
                            {
                                readSize = tempRozmiar;
                            }
                            stream.Read(bytes, 0, readSize);
                            tempRozmiar -= readSize;
                            //MessageBox.Show(tempRozmiar.ToString() + " " + readSize.ToString());
                            streamWriter.Write(bytes, 0, readSize);
                            streamWriter.Close();
                        }
                        streamWriter.Close();
                        try
                        {
                            XmlSerializer xmlHash = new XmlSerializer(typeof(serwerHash));
                            serwerHash shash = new serwerHash();
                            string str2=pobierz();
                            shash = (serwerHash)xmlHash.Deserialize(new StringReader(str2));
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Blad podczas parsowania hasha z pliku");
                            return -5;
                        }
                    }
            }
            return (int)plikiNaSerwerze.odp;
        }

        public int uploadFile(string nazwa, DateTime data, int rozmiar)
        {
            if (!File.Exists(folder + "\\" + nazwa)) return -4;
            if (!serwer.Connected) return -1;
            NetworkStream networkStream = serwer.GetStream();
            try
            {
                StringWriter stringWriter = new StringWriter();
                XmlSerializer xml = new XmlSerializer(typeof(klientUpload));
                klientUpload upload = new klientUpload(sessionID, (int)operacje.wysylanie, nazwa, data, rozmiar, 1);
                xml.Serialize(stringWriter, upload, names);
                string str = stringWriter.ToString() + endl;
                networkStream.Write(Encoding.ASCII.GetBytes(str.ToCharArray()), 0, str.Length);
            }
            catch (Exception)
            {
                MessageBox.Show("Nie mozna wyslac pliku");
                return -5;
            }
            serwerBase odpSerwera = new serwerBase();
            try
            {
                StringReader stringReader = new StringReader("");
                string str = pobierz();
                XmlSerializer xml = new XmlSerializer(typeof(serwerBase));
                odpSerwera = (serwerBase)xml.Deserialize(new StringReader(str));
            }
            catch (Exception)
            {
                MessageBox.Show("Blad podczas odbierania danych z serwera");
                return -5;
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
                StringWriter stringWriter = new StringWriter();
                XmlSerializer xml = new XmlSerializer(typeof(klientOdpDownload));
                klientOdpDownload kod = new klientOdpDownload(sessionID, (int)operacje.wysylanie, odp);
                xml.Serialize(stringWriter, kod, names);
                string str = stringWriter.ToString() + endl;
                networkStream.Write(Encoding.ASCII.GetBytes(str.ToCharArray()), 0, str.Length);
                try
                {
                    StringReader stringReader = new StringReader("");
                    string str2 = pobierz();
                    XmlSerializer x = new XmlSerializer(typeof(serwerBase));
                    odpSerwera = (serwerBase)x.Deserialize(new StreamReader(str2));
                }
                catch (Exception)
                {
                    MessageBox.Show("Blad podczas odbierania danych z serwera");
                    return -5;
                }
            }
            if (odpSerwera.operacja != (int)operacje.wysylanie) return -3;
            if (odpSerwera.odp == (int)odpowiedzi.wszystko_ok || odpSerwera.odp == (int)odpowiedzi.plik_nie_istnieje)
            {
                int rozmiarUp = 10;
                int rozmiarTmp = rozmiar;
                
                FileStream fileStream = new FileStream(folder + "//" + nazwa, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fileStream);
                while (rozmiarTmp != 0)
                {
                   // StringReader stringReader = new StringReader("");
                    if (rozmiarTmp < rozmiarUp)
                        rozmiarUp = rozmiarTmp;
                    byte[] bytes = new byte[rozmiarUp];
                    fileStream.Read(bytes, 0, rozmiarUp);
                    //bytes=br.ReadBytes(rozmiarUp);
                    MessageBox.Show(Encoding.ASCII.GetString(bytes));
                    networkStream.Write(bytes, 0, rozmiarUp);
                    rozmiarTmp -= rozmiarUp;
                }
                networkStream.Write(Encoding.ASCII.GetBytes((endl).ToCharArray()), 0, endl.Length);
                fileStream.Close();
                try
                {
                    XmlSerializer xml = new XmlSerializer(typeof(klientHash));
                    klientHash khash = new klientHash(sessionID, (int)operacje.wysylanie, "-1");
                    StringWriter sw = new StringWriter();
                    xml.Serialize(sw, khash, names);
                    string str = sw.ToString() + endl;
                    networkStream.Write(Encoding.ASCII.GetBytes(str.ToCharArray()), 0, str.Length);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Blad podczas wysylania hasha: "+ex.ToString());
                    return -5;
                }
            }
            return odpSerwera.odp;
        }

        public List<plikInfo> downloadListy(string uzytkownik)
        {
            if (!serwer.Connected) return null;
            NetworkStream networkStream = serwer.GetStream();
            if (uzytkownik.Length == 0) uzytkownik = ".";
            try
            {
                listaPlikow lista = new listaPlikow(sessionID, (int)operacje.lista, uzytkownik);
                XmlSerializer xml = new XmlSerializer(typeof(listaPlikow));
                StringWriter stringWriter = new StringWriter();
                xml.Serialize(stringWriter, lista, names);
                string str = stringWriter.ToString() + endl;
                networkStream.Write(Encoding.ASCII.GetBytes(str), 0, str.Length);
            }
            catch (Exception)
            {
                MessageBox.Show("Nie mozna pobrac listy plikow");
                return null;
            }
            serwerPliki pliki = new serwerPliki();
            try
            {
                XmlSerializer xml = new XmlSerializer(typeof(serwerPliki));
                StringReader stringReader = new StringReader("");
                string str = pobierz();
                pliki = (serwerPliki)xml.Deserialize(new StringReader(str));
            }
            catch (Exception)
            {
                MessageBox.Show("Blad podczas odbierania listy plikow");
                return null;
            }
            if (pliki.operacja != (int)operacje.lista) return null;
            if (pliki.odp != (int)odpowiedzi.wszystko_ok) return null;
            return pliki.plik;
        }
        public int usunPliki(List<pojedynczyPlik> plikiDoUsuniecia)
        {
            if (!serwer.Connected) return -1;
            NetworkStream stream = serwer.GetStream();
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
                stream.Write(Encoding.ASCII.GetBytes(strToWrite.ToCharArray()), 0, strToWrite.Length);
            }
            catch (Exception)
            {
                MessageBox.Show("Nie mozna wyslac zapytania o pliki"); return -5;
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
                catch (Exception)
                {
                    MessageBox.Show("Blad podczas usuwania");
                    return -5;
                }
                if (odpSerw.odp == (int)odpowiedzi.blad_serwera || odpSerw.odp == (int)odpowiedzi.bledny_numer_sesji) return odpSerw.odp;
            }
            return 1;
        }

        public string Login
        {
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
