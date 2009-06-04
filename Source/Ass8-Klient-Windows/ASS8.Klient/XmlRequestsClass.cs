using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ASS8.Klient
{
    /// <summary>
    /// Klasa zawiera dane do serializacji zapytania do serwera.
    /// </summary>
    [XmlRoot("klient")]
    public class klientBase
    {
        private int idSesji;
        private int op;
        public klientBase() { }
        public klientBase(int id, int oper)
        {
            idsesji = id;
            operacja = oper;
        }
        [XmlAttribute]
        public int idsesji
        {
            get
            {
                return idSesji;
            }
            set
            {
                idSesji = value;
            }
        }
        [XmlAttribute]
        public int operacja
        {
            get
            {
                return op;
            }
            set
            {
                op = value;
            }
        }
    }
    /// <summary>
    /// Klasa zawiera dane do serializacji informacji o pliku
    /// </summary>
    [XmlRoot("plik")]
    public class plikBase
    {
        private string pNazwa;
        public plikBase() { }
        public plikBase(string n)
        {
            nazwa = n;
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
    }
    /// <summary>
    /// Klasa zawiera dane do serializacji rozszerzonych informacji o pliku
    /// </summary>
    [XmlRoot("plik")]
    public class plikInfo : plikBase
    {
        private long pData;
        private int pRozmiar;
        private string pHash;
        private int pDostep;
        public plikInfo() { }
        public plikInfo(string n, long d, int r, int dos,string h)
            : base(n)
        {
            pData = d;
            rozmiar = r;
            pDostep = dos;
            pHash = h;
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
        [XmlAttribute]
        public long data
        {
            get
            {
                return pData;
            }
            set
            {
                pData = value;
            }
        }
        [XmlAttribute]
        public int rozmiar
        {
            get
            {
                return pRozmiar;
            }
            set
            {
                pRozmiar = value;
            }
        }
        [XmlAttribute]
        public int dostep
        {
            get
            {
                return pDostep;
            }
            set
            {
                pDostep = value;
            }
        }
    }
    /// <summary>
    /// Klasa zawiera dane do serializacji zapytania o logowanie
    /// </summary>
    [XmlRoot("logowanie")]
    public class klientLogowanie
    {
        public klientLogowanie() { }
        public klientLogowanie(string l, string h, string v)
        {
            Login = l;
            Haslo = h;
            wersjaKlienta = v;
        }
        [XmlAttribute("login")]
        public string Login
        {
            get
            {
                return login;
            }
            set
            {
                login = value;
            }
        }
        [XmlAttribute("haslo")]
        public string Haslo
        {
            get
            {
                return haslo;
            }
            set
            {
                haslo = value;
            }
        }
        [XmlAttribute("wersja")]
        public string wersjaKlienta
        {
            get
            {
                return vKlienta;
            }
            set
            {
                vKlienta = value;
            }
        }

        private string login;
        private string haslo;
        private string vKlienta;
    }
    /// <summary>
    /// Klasa zawiera dane do serializacji listy plikow uzytkownika
    /// </summary>
    [XmlRoot("klient")]
    public class listaPlikow : klientBase
    {
        private string uzyt;
        public listaPlikow() { }
        public listaPlikow(int id, int oper, string u)
            : base(id, oper)
        {
            uzytkownik = u;
        }
        [XmlAttribute]
        public string uzytkownik
        {
            get
            {
                return uzyt;
            }
            set
            {
                uzyt = value;
            }
        }
    }
    /// <summary>
    /// Klasa zawiera dane do serializacji zapytania o sciagniecie pliku
    /// </summary>
    [XmlRoot("klient")]
    public class downloadPliku : listaPlikow
    {
        private List<plikBase> pPlik;
        public downloadPliku() { }
        public downloadPliku(int id, int oper, string u, string[] nazwa)
            : base(id, oper, u)
        {
            plik = new List<plikBase>();
            foreach (string s in nazwa)
                plik.Add(new plikBase(s));
        }
        [XmlElement("plik")]
        public List<plikBase> plik
        {
            get
            {
                return pPlik;
            }
            set
            {
                pPlik = value;
            }
        }
    }
    /// <summary>
    /// Klasa zawiera dane do serializacji odpowiedzi o ściągnięcie pliku
    /// </summary>
    [XmlRoot("klient")]
    public class klientOdpDownload : klientBase
    {
        private string pAction;
        public klientOdpDownload() { }
        public klientOdpDownload(int id, int oper, string act)
            : base(id, oper)
        {
            action = act;
        }
        [XmlAttribute]
        public string action
        {
            get
            {
                return pAction;
            }
            set
            {
                pAction = value;
            }
        }

    }
    /// <summary>
    /// Klasa zawiera dane do serializacji zapytania o wysyłanie pliku
    /// </summary>
    [XmlRoot("klient")]
    public class klientUpload : klientBase
    {
        plikInfo pPlik;
        public klientUpload()
        {
        }
        public klientUpload(int id, int oper, string n, long d, int r, int dos,string h)
            : base(id, oper)
        {
            plik = new plikInfo(n, d, r, dos,h);
        }
        [XmlElement]
        public plikInfo plik
        {
            get
            {
                return pPlik;
            }
            set
            {
                pPlik = value;
            }
        }
    }
    /// <summary>
    /// Klasa zawiera dane do serializacji wysyłania hasha pliku
    /// </summary>
    [XmlRoot("klient")]
    public class klientHash : klientBase
    {
        private string pHash;
        public klientHash() { }
        public klientHash(int id, int oper, string h)
            : base(id, oper)
        {
            hash = h;
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
    /// Klasa zawiera dane do serializacji zapytania o usunięcie pliku
    /// </summary>
    [XmlRoot("klient")]
    public class klientUsun : klientBase
    {
        List<plikInfo> pPlik;
        public klientUsun() { }
        public klientUsun(int id, int oper, List<plikInfo> pi)
            : base(id, oper)
        {
            plik = pi;
        }
        [XmlElement]
        public List<plikInfo> plik
        {
            get
            {
                return pPlik;
            }
            set
            {
                pPlik = value;
            }
        }
    }



    /// <summary>
    /// Klasa zawiera dane do serializacji odpowiedzi serwera
    /// </summary>
    [XmlRoot("serwer")]
    public class serwerBase
    {
        private int pOdpowiedz;
        private int pOp;
        public serwerBase() { }
        public serwerBase(int o, int op)
        {
            odp = o;
            operacja = op;
        }
        [XmlAttribute]
        public int odp
        {
            get
            {
                return pOdpowiedz;
            }
            set
            {
                pOdpowiedz = value;
            }
        }
        [XmlAttribute]
        public int operacja
        {
            get
            {
                return pOp;
            }
            set
            {
                pOp = value;
            }
        }
    }
    /// <summary>
    /// Klasa zawiera dane do serializacji odpowiedzi serwera na zapytanie o logowanie
    /// </summary>
    [XmlRoot("serwer")]
    public class serwerLogowanie
    {
        private int pOdpowiedz;
        private int pSesja;
        private string pWersja;
        public serwerLogowanie() { }
        public serwerLogowanie(int odp, int ses, string wer)
        {
            odpowiedz = odp;
            sesja = ses;
            wersja = wer;
        }
        [XmlAttribute]
        public int odpowiedz
        {
            get
            {
                return pOdpowiedz;
            }
            set
            {
                pOdpowiedz = value;
            }
        }
        [XmlAttribute]
        public int sesja
        {
            get
            {
                return pSesja;
            }
            set
            {
                pSesja = value;
            }
        }
        [XmlAttribute]
        public string wersja
        {
            get
            {
                return pWersja;
            }
            set
            {
                pWersja = value;
            }
        }
    }
    /// <summary>
    /// Klasa zawiera dane do serializacji odpowiedzi serwera na zapytanie o liste plikow
    /// </summary>
    [XmlRoot("serwer")]
    public class serwerPliki : serwerBase
    {
        private List<plikInfo> pPlik;
        public serwerPliki() { }
        [XmlElement]
        public List<plikInfo> plik
        {
            get
            {
                return pPlik;
            }
            set
            {
                pPlik = value;
            }
        }
    }
    /// <summary>
    /// Klasa zawiera dane do serializacji odpowiedzi serwera na zapytanie o hash pliku
    /// </summary>
    [XmlRoot("serwer")]
    public class serwerHash : serwerBase
    {
        private string pHash;
        public serwerHash() { }
        public serwerHash(int o, int op, string h)
            : base(o, op)
        {
            hash = h;
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

}