using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ASS8.Klient
{
    /// <summary>
    /// Klasa zawiera dane o pliku
    /// </summary>
    public class pojedynczyPlik
    {
        private string pNazwa;
        private string pHash;
        long pCzas;
        public pojedynczyPlik() { }
        public pojedynczyPlik(long cz,string p, string h)
        {
            data = cz;
            nazwa = p;
            hash = h;
        }
        [XmlAttribute]
        public long data
        {
            get
            {
                return pCzas;
            }
            set
            {
                pCzas = value;
            }
        }

        [XmlAttribute]
        public string nazwa
        {
            get
            {
                return pNazwa;
            }
            set
            {
                pNazwa = value;
            }
        }
        [XmlAttribute]
        public string hash
        {
            get
            {
                return pHash;
            }
            set
            {
                pHash = value;
            }
        }
    }

    /// <summary>
    /// Klasa zawiera liste plikow z ich informacjami
    /// </summary>
    public class pliki
    {
        private List<pojedynczyPlik> pPliki;
        public pliki() {
            plik = new List<pojedynczyPlik>();
        }
        public pliki(List<pojedynczyPlik> p)
        {
            plik = new List<pojedynczyPlik>();
            foreach (pojedynczyPlik tmp in p)
                plik.Add(tmp);
        }
        [XmlElement]
        public List<pojedynczyPlik> plik
        {
            get
            {
                return pPliki;
            }
            set
            {
                pPliki = value;
            }
        }
    }
    /// <summary>
    /// Klasa zawiera ustawienia aplikacji
    /// </summary>
    [XmlRoot]
    public class ustawienia
    {
        private string pIp;
        private int pPort;
        public ustawienia() { }
        public ustawienia(string i, int p)
        {
            ip = i;
            port = p;
        }
        [XmlAttribute]
        public string ip
        {
            get
            {
                return pIp;
            }
            set
            {
                pIp = value;
            }
        }
        [XmlAttribute]
        public int port
        {
            get
            {
                return pPort;
            }
            set
            {
                pPort = value;
            }
        }
    }
    /// <summary>
    /// Klasa zawiera ustawienia globalne
    /// </summary>
    [XmlRoot("ustawienia")]
    public class globalne_ustawienia
    {
        private string pLogin;
        private int pZapamietaj;
        private string pSerwerProxy;
        private int pPortProxy;
        private int pProxy;
        private int pUwierzytelnienieProxy;
        private string pLoginProxy;
        private string pHasloProxy;
        private string pSerwer;
        private int pPort;
        public globalne_ustawienia() { }
        public globalne_ustawienia(string log, int zap, int prox, string serv, int portP, int uProxy, string logProxy, string hasProxy,string s,int p)
        {
            login = log;
            zapamietaj = zap;
            proxy = prox;
            portProxy = portP;
            serwerProxy = serv;
            uwierzytelnienie = uProxy;
            loginProxy = logProxy;
            hasloProxy = hasProxy;
            serwer=s;
            port=p;

        }
        public string serwer
        {
            get
            {
                return pSerwer;
            }
            set
            {
                pSerwer = value;
            }
        }
        public int port
        {
            get
            {
                return pPort;
            }
            set
            {
                pPort = value;
            }
        }
        public int zapamietaj
        {
            get
            {
                return pZapamietaj;
            }
            set
            {
                pZapamietaj = value;
            }
        }
        public string login
        {
            get
            {
                return pLogin;
            }
            set
            {
                pLogin = value;
            }
        }
        public int proxy
        {
            get
            {
                return pProxy;
            }
            set
            {
                pProxy = value;
            }
        }
        public string serwerProxy
        {
            get
            {
                return pSerwerProxy;
            }
            set
            {
                pSerwerProxy = value;
            }
        }
        public int portProxy
        {
            get
            {
                return pPortProxy;
            }
            set
            {
                pPortProxy = value;
            }
        }
        public int uwierzytelnienie
        {
            get
            {
                return pUwierzytelnienieProxy;
            }
            set
            {
                pUwierzytelnienieProxy = value;
            }
        }
        public string loginProxy
        {
            get
            {
                return pLoginProxy;
            }
            set
            {
                pLoginProxy = value;
            }
        }
        public string hasloProxy
        {
            get
            {
                return pHasloProxy;
            }
            set
            {
                pHasloProxy = value;
            }
        }
    }
    /// <summary>
    /// Klasa zawiera ustawienia uzytkownika
    /// </summary>
    [XmlRoot("ustawienia")]
    public class ustawienia_uzytkownika
    {
        private string pSerwerProxy;
        private int pPortProxy;
        private int pProxy;
        private int pUwierzytelnienieProxy;
        private string pLoginProxy;
        private string pHasloProxy;
        string pSciezka;
        int pSekundy;
        int pBledy;
        public ustawienia_uzytkownika() { }
        public ustawienia_uzytkownika(int prox, string serv, int portP, int uProxy, string logProxy, string hasProxy, string sciez, int s, int b)
        {
            proxy = prox;
            portProxy = portP;
            serwerProxy = serv;
            uwierzytelnienie = uProxy;
            loginProxy = logProxy;
            hasloProxy = hasProxy;
            sciezka = sciez;
            sekundy = s;
            bledy = b;
        }
        public string sciezka
        {
            get
            {
                return pSciezka;
            }
            set
            {
                pSciezka = value;
            }
        }
        public int sekundy
        {
            get
            {
                return pSekundy;
            }
            set
            {
                pSekundy = value;
            }
        }
        public int bledy
        {
            get
            {
                return pBledy;
            }
            set
            {
                pBledy = value;
            }
        }
        public int proxy
        {
            get
            {
                return pProxy;
            }
            set
            {
                pProxy = value;
            }
        }
        public string serwerProxy
        {
            get
            {
                return pSerwerProxy;
            }
            set
            {
                pSerwerProxy = value;
            }
        }
        public int portProxy
        {
            get
            {
                return pPortProxy;
            }
            set
            {
                pPortProxy = value;
            }
        }
        public int uwierzytelnienie
        {
            get
            {
                return pUwierzytelnienieProxy;
            }
            set
            {
                pUwierzytelnienieProxy = value;
            }
        }
        public string loginProxy
        {
            get
            {
                return pLoginProxy;
            }
            set
            {
                pLoginProxy = value;
            }
        }
        public string hasloProxy
        {
            get
            {
                return pHasloProxy;
            }
            set
            {
                pHasloProxy = value;
            }
        }
    }
}
