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
        private enum odpowiedzi
        {
            bledne_zapytanie = 400, bledny_numer_sesji, plik_istnieje, blad_serwera, plik_nie_istnieje, blad_odbierania_plikow, wszystko_ok
        }
        string folder = "pliki";
        XmlSerializerNamespaces names;
        private int sessionID;
        private String serverIP = "127.0.0.1";
        private int serverPort = 2000;
        private TcpClient serwer;
        private string log;
        private string haslo;
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
            serwer = new TcpClient(serverIP, serverPort);
            if (!serwer.Connected) return -1;
            NetworkStream networkStream = serwer.GetStream();
            try
            {
                StringWriter stringWriter = new StringWriter();
                klientLogowanie logowanie = new klientLogowanie(log, haslo, ASS8___Logowanie.wersja);
                XmlSerializer xml = new XmlSerializer(typeof(klientLogowanie));
                xml.Serialize(stringWriter, logowanie, names);
                string strToWrite = stringWriter.ToString() + "\r\n";
                networkStream.Write(Encoding.ASCII.GetBytes(strToWrite.ToCharArray()), 0, strToWrite.Length);
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("Nie udalo sie zalogowac");
                return -1;
            }
            Byte[] buffor = new Byte[serwer.Client.ReceiveBufferSize];
            int dlugoscOdp = 0;
            StringBuilder strRead = new StringBuilder("");
            do
            {
                int i = networkStream.Read(buffor, 0, serwer.Client.ReceiveBufferSize);
                dlugoscOdp += i;
                strRead.Append(Encoding.ASCII.GetString(buffor));
                if (dlugoscOdp >= 2)
                    if (strRead[dlugoscOdp - 1] == '\n' && strRead[dlugoscOdp - 2] == '\r') break;
            } while (true);
            string str = strRead.ToString().Substring(0, dlugoscOdp - 2);
            serwerLogowanie odpSerwera = new serwerLogowanie();
            try
            {
                XmlSerializer xml = new XmlSerializer(typeof(serwerLogowanie));
                StringReader stringReader = new StringReader(str);
                odpSerwera = (serwerLogowanie)xml.Deserialize(stringReader);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nie udalo sie odebrac odpowiedzi od serwera: " + ex.ToString());
            }
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
                string strToWrite = stringWriter.ToString() + "\r\n";
                stream.Write(Encoding.ASCII.GetBytes(strToWrite.ToCharArray()), 0, strToWrite.Length);
            }
            catch (Exception)
            {
                MessageBox.Show("Nie mozna wyslac zapytania o pliki");
            }
            int dlugoscOdp = 0;
            StringBuilder strRead = new StringBuilder("");
            try
            {
                do
                {
                    Byte[] bytes = new Byte[serwer.Client.ReceiveBufferSize];
                    int i = stream.Read(bytes, 0, serwer.Client.ReceiveBufferSize);
                    dlugoscOdp += i;
                    strRead.Append(Encoding.ASCII.GetString(bytes));
                    if (dlugoscOdp >= 2)
                        if (strRead[dlugoscOdp - 1] == '\n' && strRead[dlugoscOdp - 2] == '\r') break;
                } while (true);
            }
            catch (Exception)
            {
                MessageBox.Show("Nie mozna ustanowic polaczenia z serwerem");
            }
            serwerPliki plikiNaSerwerze = new serwerPliki();
            try
            {
                XmlSerializer xml = new XmlSerializer(typeof(serwerPliki));
                StringReader stringReader = new StringReader(strRead.ToString().Substring(0, dlugoscOdp));
                plikiNaSerwerze = (serwerPliki)xml.Deserialize(stringReader);
            }
            catch (Exception)
            {
                MessageBox.Show("Odebrano bledne dane od serwera");
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
                        string str = stringWriter.ToString() + "\r\n";
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
                            MessageBox.Show(tempRozmiar.ToString() + " " + readSize.ToString());
                            streamWriter.Write(bytes, 0, readSize);
                            streamWriter.Close();
                        }
                        streamWriter.Close();
                        strRead.Remove(0, dlugoscOdp);
                        dlugoscOdp = 0;
                        try
                        {
                            do
                            {
                                byte[] bytes = new byte[serwer.Client.ReceiveBufferSize];
                                int i = stream.Read(bytes, 0, serwer.Client.ReceiveBufferSize);
                                dlugoscOdp += i;
                                strRead.Append(Encoding.ASCII.GetString(bytes));
                                if (dlugoscOdp >= 2)
                                    if (strRead[dlugoscOdp - 1] == '\n' && strRead[dlugoscOdp - 2] == '\r') break;
                            } while (true);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Blad podczas odbierania hasha pliku");
                        }
                        try
                        {
                            XmlSerializer xmlHash = new XmlSerializer(typeof(serwerHash));
                            serwerHash shash = new serwerHash();
                            shash = (serwerHash)xmlHash.Deserialize(new StringReader(strRead.ToString().Substring(0, dlugoscOdp - 2)));
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Blad podczas parsowania hasha z pliku");
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
                string str = stringWriter.ToString() + "\r\n";
                MessageBox.Show(str);
                networkStream.Write(Encoding.ASCII.GetBytes(str.ToCharArray()), 0, str.Length);
            }
            catch (Exception)
            {
                MessageBox.Show("Nie mozna wyslac pliku");
            }
            int dlugoscOdp = 0;
            serwerBase odpSerwera = new serwerBase();
            try
            {
                StringReader stringReader = new StringReader("");
                StringBuilder stringBuilder = new StringBuilder("");
                while (true)
                {
                    Byte[] bytes = new Byte[serwer.Client.ReceiveBufferSize];
                    int i = networkStream.Read(bytes, 0, serwer.Client.ReceiveBufferSize);
                    dlugoscOdp += i;
                    stringBuilder.Append(Encoding.ASCII.GetString(bytes));
                    if (dlugoscOdp >= 2)
                        if (stringBuilder[dlugoscOdp - 1] == '\n' && stringBuilder[dlugoscOdp - 2] == '\r') break;
                }
                XmlSerializer xml = new XmlSerializer(typeof(serwerBase));
                odpSerwera = (serwerBase)xml.Deserialize(new StringReader(stringBuilder.ToString().Substring(0, dlugoscOdp - 2)));
            }
            catch (Exception)
            {
                MessageBox.Show("Blad podczas odbierania danych z serwera");
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
                string str = stringWriter.ToString() + "\r\n";
                networkStream.Write(Encoding.ASCII.GetBytes(str.ToCharArray()), 0, str.Length);
                try
                {
                    StringReader stringReader = new StringReader("");
                    StringBuilder stringBuilder = new StringBuilder("");
                    while (true)
                    {
                        Byte[] bytes = new Byte[serwer.Client.ReceiveBufferSize];
                        int i = networkStream.Read(bytes, 0, serwer.Client.ReceiveBufferSize);
                        dlugoscOdp += i;
                        stringBuilder.Append(Encoding.ASCII.GetString(bytes));
                        if (dlugoscOdp >= 2)
                            if (stringBuilder[dlugoscOdp - 1] == '\n' && stringBuilder[dlugoscOdp - 2] == '\r') break;
                    }
                    XmlSerializer x = new XmlSerializer(typeof(serwerBase));
                    odpSerwera = (serwerBase)x.Deserialize(new StreamReader(stringBuilder.ToString().Substring(0, dlugoscOdp - 2)));
                }
                catch (Exception)
                {
                    MessageBox.Show("Blad podczas odbierania danych z serwera");
                }
            }
            if (odpSerwera.operacja != (int)operacje.wysylanie) return -3;
            if (odpSerwera.odp == (int)odpowiedzi.wszystko_ok || odpSerwera.odp == (int)odpowiedzi.plik_nie_istnieje)
            {
                int rozmiarUp = 256;
                int rozmiarTmp = rozmiar;
                FileStream fileStream = new FileStream(folder + "//" + nazwa, FileMode.Open, FileAccess.Read);
                while (rozmiarTmp != 0)
                {
                    StringReader stringReader = new StringReader("");
                    if (rozmiarTmp < rozmiarUp)
                        rozmiarUp = rozmiarTmp;
                    byte[] bytes = new byte[rozmiarUp];
                    fileStream.Read(bytes, 0, rozmiarUp);
                    MessageBox.Show(Encoding.ASCII.GetString(bytes));
                    networkStream.Write(bytes, 0, rozmiarUp);
                    rozmiarTmp -= rozmiarUp;
                }
                fileStream.Close();
                try
                {
                    XmlSerializer xml = new XmlSerializer(typeof(klientHash));
                    klientHash khash = new klientHash(sessionID, (int)operacje.wysylanie, "-1");
                    StringWriter sw = new StringWriter();
                    xml.Serialize(sw, khash, names);
                    string str = sw.ToString() + "\r\n";
                    networkStream.Write(Encoding.ASCII.GetBytes(str.ToCharArray()), 0, str.Length);
                }
                catch (Exception)
                {
                    MessageBox.Show("Blad podczas wysylania hasha");
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
                string str = stringWriter.ToString() + "\r\n";
                networkStream.Write(Encoding.ASCII.GetBytes(str), 0, str.Length);
            }
            catch (Exception)
            {
                MessageBox.Show("Nie mozna pobrac listy plikow");
            }
            serwerPliki pliki = new serwerPliki();
            try
            {
                XmlSerializer xml = new XmlSerializer(typeof(serwerPliki));
                StringReader stringReader = new StringReader("");
                int dlugoscOdp = 0;
                StringBuilder stringBuilder = new StringBuilder("");
                while (true)
                {
                    byte[] bytes = new byte[serwer.Client.ReceiveBufferSize];
                    int i = networkStream.Read(bytes, 0, serwer.Client.ReceiveBufferSize);
                    dlugoscOdp += i;
                    stringBuilder.Append(Encoding.ASCII.GetString(bytes));
                    if (dlugoscOdp > 2)
                        if (stringBuilder[dlugoscOdp - 2] == '\r' && stringBuilder[dlugoscOdp - 1] == '\n') break;
                }
                string str = stringBuilder.ToString().Substring(0, dlugoscOdp - 2);
                pliki = (serwerPliki)xml.Deserialize(new StringReader(str));
            }
            catch (Exception)
            {
                MessageBox.Show("Blad podczas odbierania listy plikow");
            }
            if (pliki.operacja != (int)operacje.lista) return null;
            if (pliki.odp != (int)odpowiedzi.wszystko_ok) return null;
            return pliki.plik;
        }
        public int usunPliki(List<pojedynczyPlik> plikiDoUsuniecia)
        {
            if (!serwer.Connected) return -1;
            NetworkStream stream = serwer.GetStream();
            List<string> plikiDoUs=new List<string>();
            foreach(pojedynczyPlik p in plikiDoUsuniecia)
                plikiDoUs.Add(p.nazwa);
            try
            {
                klientUsun usun = new klientUsun(sessionID, (int)operacje.usuwanie, plikiDoUs.ToArray());
                XmlSerializer xml = new XmlSerializer(typeof(downloadPliku));
                StringWriter stringWriter = new StringWriter();
                xml.Serialize(stringWriter, usun, names);
                string strToWrite = stringWriter.ToString() + "\r\n";
                stream.Write(Encoding.ASCII.GetBytes(strToWrite.ToCharArray()), 0, strToWrite.Length);
            }
            catch (Exception)
            {
                MessageBox.Show("Nie mozna wyslac zapytania o pliki");
            }
            for (int j = 0; j < plikiDoUsuniecia.Count; j++)
            {
                int dlugoscOdp = 0;
                StringBuilder strRead = new StringBuilder("");
                try
                {
                    do
                    {
                        Byte[] bytes = new Byte[serwer.Client.ReceiveBufferSize];
                        int i = stream.Read(bytes, 0, serwer.Client.ReceiveBufferSize);
                        dlugoscOdp += i;
                        strRead.Append(Encoding.ASCII.GetString(bytes));
                        if (dlugoscOdp >= 2)
                            if (strRead[dlugoscOdp - 1] == '\n' && strRead[dlugoscOdp - 2] == '\r') break;
                    } while (true);
                }
                catch (Exception)
                {
                    MessageBox.Show("Nie mozna ustanowic polaczenia z serwerem");
                }
                serwerBase odpSerw = new serwerBase();
                try
                {
                    XmlSerializer xml = new XmlSerializer(typeof(serwerBase));
                    string str = strRead.ToString().Substring(0, dlugoscOdp - 2);
                    odpSerw = (serwerBase)xml.Deserialize(new StringReader(str));
                }
                catch (Exception)
                {
                    MessageBox.Show("Blad podczas usuwania");
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
