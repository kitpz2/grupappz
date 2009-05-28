using System;
using System.Collections.Generic;
using System.Text;

namespace ASS8.Klient
{
    class Wyjatki
    {
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
