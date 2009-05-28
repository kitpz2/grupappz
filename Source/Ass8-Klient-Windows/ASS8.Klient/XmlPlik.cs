using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ASS8.Klient
{
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
}
