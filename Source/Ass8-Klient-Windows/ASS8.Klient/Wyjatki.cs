using System;
using System.Collections.Generic;
using System.Text;

namespace ASS8.Klient
{
    class Wyjatki
    {
        /// <summary>
        /// Klasa reprezentuje wyjątek rzucany podczas błędu połączenia
        /// </summary>
        public class BladPolaczenia : Exception
        {
            string blad;
            public BladPolaczenia(string s)
            {
                blad = s;
            }
            public override string ToString()
            {
                return blad;
            }
        }
        /// <summary>
        /// Klasa reprezentuje wyjątek rzucany podczas błędu parsowania odpowiedzi od serwera
        /// </summary>
        public class BladParsowania : Exception
        {
            string blad;
            public BladParsowania(string s)
            {
                blad = s;
            }
            public override string ToString()
            {
                return blad;
            }
        }
        /// <summary>
        /// Klasa reprezentuje wyjątek rzucany podczas braku pliku
        /// </summary>
        public class BrakPliku : Exception
        {
            string blad;
            public BrakPliku(string s)
            {
                blad = s;
            }
            public override string ToString()
            {
                return blad;
            }
        }
        /// <summary>
        /// Klasa reprezentuje wyjątek rzucany podczas błędu odbierania danych od serwera
        /// </summary>
        public class BladOdbierania : Exception
        {
            string blad;
            public BladOdbierania(string s)
            {
                blad = s;
            }
            public override string ToString()
            {
                return blad;
            }
            public void add(string s)
            {
                blad += " " + s;
            }
        }
        /// <summary>
        /// Klasa reprezentuje wyjątek rzucany podczas błędu wysyłania danych do serwera
        /// </summary>
        public class BladWysylania : Exception
        {
            string blad;
            public BladWysylania(string s)
            {
                blad = s;
            }
            public override string ToString()
            {
                return blad;
            }
        }
        /// <summary>
        /// Klasa reprezentuje wyjątek rzucany podczas nieokreślonego błędu
        /// </summary>
        public class BladNieokreslony : Exception
        {
            string blad;
            public BladNieokreslony(string s)
            {
                blad = s;
            }
            public override string ToString()
            {
                return blad;
            }

        }
    }
}
