#include <iostream>
#include <string>
#include <cstdlib>
#include <cstdio>
#include <vector>

#include <sys/types.h>
#include <unistd.h>
#include <sys/wait.h>

#include "version.h"
#include "parser.hpp"
#include "xml.hpp"
#include "debug.hpp"

bool parser::parsuj(std::string &do_parsowania)
{
    line;
#ifdef LIBXMLCPP_EXCEPTIONS_ENABLED
    try
    {
#endif //LIBXMLCPP_EXCEPTIONS_ENABLED
        info2("Do parsowania:",do_parsowania.c_str());
        xmlpp::TextReader reader((unsigned char*)do_parsowania.c_str(),do_parsowania.size());
        while (reader.read())
        {
            info("reader.read()");
            info2("reader.get_name()",reader.get_name().c_str());
            if (reader.get_name().compare("logowanie")==0) ///Logowanie - Trzeba pamiętać że parser umieszcza dodatkowo spację na końcu sparsowanych stringów
            {
                info("Logowanie...");
                if (reader.has_attributes()) ///Sprawdzanie ilosci atrybutów
                {
                    info("reader.has_attributes==true");
                    if (reader.move_to_attribute("login")) ///Przesun sie do atrybutu "login"
                    {
                        login=reader.get_value(); ///Zapisz login
                        info2("login",login.c_str());
                    }
                    else ///Gdy brak takiego atrybutu to konczymy rozmowe
                    {
                        Eline("Brak loginu");
                        odpowiedz_login(3);
                    }
                    if (reader.move_to_attribute("haslo")) ///Przesun sie do atrybutu "haslo"
                    {
                        haslo=reader.get_value();
                        info2("haslo",haslo.c_str());
                    }
                    else ///Gdy brak takiego atrybutu to konczymy rozmowe
                    {
                        Eline("Brak hasla");
                        odpowiedz_login(3);
                    }

                    if (logowanie(login,haslo)==true)///Sprawdzamy czy login i haslo pasują
                    {
                        info("Logowanie - OK");
                        odpowiedz_login(0);///Jeżeli tak to dajemy klientowi ID sesji
                    }
                    else
                    {
                        info("login i haslo nie pasuja");
                        odpowiedz_login(1);///Jeżeli nie to informujemy go o tym i kończymy
                    }
                }
                else ///Brak loginu i hasła
                {
                    info("Brak loginu i hasła");
                    odpowiedz_login(3);
                }
            }
            else if (reader.get_name().compare("klient")==0)///Jeżeli polecenie od klienta
            {
                int operacja; //Czy to jest uzywane?
                std::string t_uzytkownik; //Czy to jest uzywane?
                if (reader.has_attributes()) ///Sprawdzanie ilosci atrybutów
                {
                    info("reader.has_attributes==true");
                    if (reader.move_to_attribute("idsesji")) ///Przesun sie do atrybutu "idsesji"
                    {
#ifdef DEBUG
                        ///Do debugowania
                        char temp[128];
                        std::string uzytkownik;
                        sprintf(temp,"%u",id_sesji);
                        info("Sprawdzanie id_sesji");
                        info2(temp,reader.get_value().c_str());
#endif
                        if (id_sesji==atoi(reader.get_value().c_str())) ///Sprawdzamy czy zgadza się id_sesji
                        {
                            info("id_sesji POPRAWNE");
                            if (reader.move_to_attribute("operacja"))///Sprawdzamy czy istnieje argument "operacja"
                            {
                                info2("operacja",reader.get_value().c_str());
                                operacja=atoi(reader.get_value().c_str());
                            }
                            else if (reader.move_to_attribute("action"))
                            {
                                if (reader.get_value().compare("ok")==0)
                                {
                                    info("action=ok");
                                    return true;
                                }
                                else
                                {
                                    info("action=abort");
                                    return false;
                                }
                            }
                            else
                            {
                                info("BŁĘDNE ZAPYTANIE");
                                Odpowiedz(400);///Błędne zapytanie
                                return false;
                            }
                            if (operacja==100 || operacja==101)
                            {
                                if (reader.move_to_attribute("uzytkownik"))
                                {
                                    info2("uzytkowik",reader.get_value().c_str());
                                    uzytkownik=reader.get_value();
                                }
                                else
                                {
                                    info("BŁĘDNE ZAPYTANIE");
                                    Odpowiedz(400);///Błędne zapytanie
                                    return false;
                                }
                            }
                            switch (operacja)
                            {
                            case 100:
                                info("OPERACJA 100");
                                lista_plikow(uzytkownik);
                                break;
                            case 101:
                                wysylanie_plikow(reader,uzytkownik);
                                info("OPERACJA 101");
                                break;
                            case 102:
                                info("OPERACJA 102");
                                odbieranie_plikow(reader,login);
                                break;
                            case 103:
                                info("OPERACJA 103");
                                usun_pliki(reader,login);
                            default:
                                info("Błędny kod operacji");
                                Odpowiedz(400);
                                break;

                            }
                        }
                        else ///Niezgodne id_sesji
                        {
                            info("ID SESJI JEST NIEZGODNE");
                            Odpowiedz(401);
                            return false;
                        }
                    }
                    else ///Brak id_sesji
                    {
                        info("BRAK ID_SESJI");
                        Odpowiedz(401);
                        return false;
                    }
                }
                else///Brak atrybutów
                {
                    info("brak atrybutów");
                    Odpowiedz(400);
                    return false;
                }
            }
            else//Jeżeli polecenie nie zaczyna się od <klient
            {
                info("nierozpoznano");
                Odpowiedz(400);
                return false;
            }
#ifdef LIBXMLCPP_EXCEPTIONS_ENABLED
        }
    }
    catch (const std::exception& e)///Jeżeli wyjątki w LIBXMLCPP są włącozne i wystąpi błąd parsowania
    {
        info2("BŁAD PARSOWANIA ",e.what());
        wyslij("<?xml version=\"1.0\"?>\
        <serwer xml_stream_error=\"true\"/>"); ///To wysyłana jest o tym informacja
        //exit(0);///I zamykamy połączenie
        return false;
    }
#endif //LIBXMLCPP_EXCEPTIONS_ENABLED

    return true;
}
void parser::wyslij(std::string w)///Funkcja wysyłająca dane przez SOCKET
{
    //w.append(std::endl);
    info2("Wysylam",w.c_str());
    stream<<w;//<<std::endl;

}
bool parser::logowanie(std::string login, std::string haslo)///Funkcja sprawdzająca czy dla podanego loginu hasło jest prawidłowe, jeżeli tak to generuje id_sesji
{
    info("Logowanie()");
    info2(baza.get_passwd(login).c_str(),haslo.c_str());
    //if (baza.get_passwd(login).compare(0,haslo.size(),haslo)==0)///Trzeba pamiętać że zmienna haslo tak naprawde zawiera haslo i na końcu spację.
    if (baza.get_passwd(login).compare(haslo)==0)///Trzeba pamiętać że zmienna haslo tak naprawde zawiera haslo i na końcu spację.
    {
        info("Generowanie ID sesji");
        srand(time(NULL));
        id_sesji=(unsigned int)rand();
        return true;
    }
    else
    {
        md5wrapper md5;
        info2("Haslo z bazy zhaszowane",md5.getHashFromString(baza.get_passwd(login)).c_str());
        if (md5.getHashFromString(baza.get_passwd(login)).compare(haslo)==0)
        {
            info("Generowanie ID sesji");
            srand(time(NULL));
            id_sesji=(unsigned int)rand();
            return true;
        }
        else
        {
            Eline("Logowanie(): niepoprawne haslo");
            return false;
        }
    }
}
///Krotka odpowiedz gdzie i - kod odpowiedzi
void parser::Odpowiedz(int nr_odpowiedzi,int nr_operacji)
{
    sprintf(bufor,"<?xml version=\"1.0\"?>\r\n\
    <serwer operacja=\"%d\" odp=\"%d\"/>",nr_operacji,nr_odpowiedzi);
    wyslij(bufor);
}
///Odpowiedź z dodatkowymi informacjami  gdzie i - kod odpowiedzi
void parser::Odpowiedz(int nr_odpowiedzi,int nr_operacji,std::string odp)
{
    std::string temp;
    sprintf(bufor,"<?xml version=\"1.0\"?>\r\n\
    <serwer operacja=\"%d\" odp=\"%d\">",nr_operacji,nr_odpowiedzi);
    temp.append(bufor);
    if (!odp.empty())
        temp.append(odp);
    else
        temp.append("\r\n");
    temp.append("</serwer>");
    wyslij(temp);
}
void parser::odpowiedz_login(int i)///Funkcja wysyłająca odpowiedź po prośbie o zalogowanie
{
    sprintf(bufor,"<?xml version=\"1.0\"?>\r\n\
        <serwer odpowiedz=\"%d\" sesja=\"%u\" wersja=\"%s %s\"/>",i,id_sesji,"-1","aa");//AutoVersion::FULLVERSION_STRING,AutoVersion::STATUS_SHORT);
    wyslij(bufor);
    if (i!=0)
    {
        stream.close();
        exit(0);
    }
}
void parser::start () ///Funkcja wywoływana tylko raz, rozpoczyna pobieranie informacji z SOCKETA i przekazuje je parserowi
{
    //stream<<"ASS8 Server v numer_wersji"/*<<AutoVersion::FULLVERSION_STRING*/<<"\r\n\r\n";
    std::string a;

    while (true)
    {
        info("PETLA ZBIERAJACA");
        line;
        a=czytanie_z_socketa();
        line;
        //zabezp=0;
        info("ROZPOCZYNAM PARSOWANIE");
        line;
        //std::cout<<a<<std::endl;
        if (!parsuj(a))
            info("Wystąpił błąd przy parsowaniu");

    }
    line;
    stream.close();
    exit(0);
}

std::string parser::czytanie_z_socketa()
{
    std::string a;
    std::string temp;
    a="";
    temp="";
    int zabezp=0, zabezp2=0;
    do
    {
        line;
        boost::system::error_code error;
        line;
        if (error == boost::asio::error::eof)
        {
            line;
            info("Klient disconnected");
            exit(0);
        }

        std::getline(stream,temp);
        line;

        if (temp.find_first_of("<")<=temp.size())
        {
            zabezp=0;
        }
        info2("Otrzymalem: ",temp.c_str());
        if (zabezp>3 || zabezp2>100)
            exit(0);
        ++zabezp;
        ++zabezp2;
        char x[256];
        sprintf(x,"zabezp= %d zabezp2 = %d",zabezp, zabezp2);
        info(x);

        a.append(temp+"\r\n");
        line;
    }
    while (!(temp[0]==13 && temp[1]==0));
    line;
    return a;
}

void parser::lista_plikow(std::string uzytkownik)
{
    info("Lista_plikow");
    /**Jeżeli użytkownik to "." to znaczy ze żadanie jest
    o liste plików użytkownika zalogowanego*/
    if (uzytkownik.compare(".")==0)
        uzytkownik=login;
    /**Prośba o listę plików użytkownika uzytkownik*/
    mysqlpp::StoreQueryResult res=baza.getFilesList(uzytkownik);
    if (res.num_rows()<1)
    {
        info("res.num_rows()<1");
        std::string odp="";
        Odpowiedz(406,100,odp);
        return;
    }
    else
    {
        info("Tworzenie listy");
        std::string odp="";
        std::string temp;
        //char temp[1024];
        for (unsigned int i=0;i<res.num_rows();++i)
        {
            line;
            if (res[i]["hashValue"].compare("-1")==0)
            {
                info2("plik usuniety, pomijam",res[i]["sciezka"].c_str());
                continue;
            }
            char a[1024];
            sprintf(a,"<plik nazwa=\"%s\" data=\"%d\" rozmiar=\"%d\"\
                dostep=\"%d\" hash=\"%s\"/>\r\n",res[i]["sciezka"].c_str(),
                    atoi(res[i]["dataDodania"].c_str()),atoi(res[i]["wielkosc"].c_str()),
                    atoi(res[i]["prawaDostepu"].c_str()),res[i]["hash"].c_str());
            info(a);
            odp.append(a);
        }
        info("Lista utworzona");
        //Odpowiedz(406 - Wszystko ok, 100 - operacja "lista wszystkich plików", xml z listą plików)
        Odpowiedz(406,100,odp);
    }
}

std::vector <std::string> parser::pobieranie_listy_plikow(xmlpp::TextReader &reader)
{
    info("Odbieranie listy plikow");
    std::vector <std::string> nazwy_plikow;
    while (reader.read())
    {

        if (reader.get_name().compare("plik")==0)//czy mamy plik?
        {
            info("mamy plik ");
            if (reader.has_attributes())//Czy mamy atrybuty
            {
                info("mamy atrybuty");
                if (reader.move_to_attribute("nazwa"))//Przejdź do atrybutu "nazwa"
                {
                    info("mamy atrybut \"nazwa\"");
                    nazwy_plikow.push_back(reader.get_value());
                }
                else//Brak atrybutu "nazwa"
                {
                    Eline("Brak Atrybutu 'nazwa'");
                    //Odpowiedz(400);

                }

            }
            else//Brak atrybutów
            {
                Eline("Brak Atrybutów");
                //Odpowiedz(400);
                //return;
            }
        }
        else//nieoczekiwane dane
        {
            Eline2("Nieoczekiwane dane: ",reader.get_name().c_str());
            //Odpowiedz(400);
            //return;
        }
    }
    info("pobieranie/parsowanie listy plików zakonczone");
    return nazwy_plikow;
}

void parser::odbieranie_plikow(xmlpp::TextReader &reader, std::string uzytkownik)
{
    info("Odbieranie plikow");
    reader.read();
    reader.read();
    info2("plik",reader.get_name().c_str());
    if (reader.get_name().compare("plik")==0)
    {
        info("plik jest ok");

        if (reader.has_attributes())
        {
            info("ma atrybuty");

            if (reader.move_to_attribute("nazwa"))
            {
                std::string nazwa = reader.get_value();
                info2("jest nazwa",nazwa.c_str());

                if (reader.move_to_attribute("rozmiar"))
                {
                    int rozmiar=atoi(reader.get_value().c_str());
                    info("ma rozmiar");

                    if (reader.move_to_attribute("hash"))
                    {
                        std::string hash = reader.get_value();
                        info2("jest hash",hash.c_str());

                        std::string sciezka=login+"/";
                        sciezka+=nazwa;
                        line;
                        FILE *plik=fopen(sciezka.c_str(),"r");
                        line;
                        info("Sprawdzanie czy plik istnieje");
                        if (plik!=NULL)
                        {
                            info("Plik istnieje");
                            fclose(plik);
                            mysqlpp::StoreQueryResult wynik= baza.getFileInfo(nazwa,login);
                            if (wynik)
                            {
                                if (wynik.num_rows()<1)
                                {
                                    info("Plik jest na dysku ale nie ma w bazie, zgadzam sie na zastąpienie");
                                    Odpowiedz(404,102);//Brak pliku, mozna wysylac

                                }
                                else
                                {
                                    for (unsigned int i=0;i<wynik.num_rows();++i)
                                    {
                                        info("Sprawdzanie czy jest w bazie");
                                        if (wynik[i]["hash"].compare(hash)==0)
                                        {
                                            line;
                                            info("Plik juz istnieje, co robic?");
                                            line;
                                            Odpowiedz(402,102);
                                            line;
                                            std::string a=czytanie_z_socketa();
                                            line;
                                            if (!parsuj(a))//Zgadza sie klient na zastapienie?
                                            {
                                                info("Klient nie zastepuje pliku");
                                                Odpowiedz(406,102);//Nie zgadza, kończymy
                                                return;
                                            }
                                            else
                                            {
                                                info("Plik zastepuje plik");
                                                Odpowiedz(406,102);//Zgadza, kontynuujemy
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            info("Plik jest na dysku ale nie ma w bazie, zgadzam sie na zastąpienie");
                                            Odpowiedz(404,102);//Brak pliku, mozna wysylac
                                            break;
                                        }
                                        info("To nie powinno się nigdy wywołać");
                                        Odpowiedz(404,103);//Brak pliku, mozna wysylac
                                    }

                                }
                            }
                            else
                            {
                                info("Plik jest na dysku ale nie ma w bazie, zgadzam sie na zastąpienie");
                                Odpowiedz(404,102);//Brak pliku, mozna wysylac

                            }
                        }
                        else
                        {
                            info("Brak pliku, wysyłam odpowiedź klientowi i przystępuje do odbioru pliku");
                            Odpowiedz(404,102);

                        }

                        char temp_sciezka[256];
                        sprintf(temp_sciezka,"%s/temp.%d",login.c_str(),id_sesji);
                        info2("Tworze plik tymczasowy",temp_sciezka);
                        plik=fopen(temp_sciezka,"w+");
                        if (!plik)
                        {
                            Eline("PLIK SIE NIE OTWORZYL!!!!");
                            Odpowiedz(403,102);
                            return;
                        }
                        std::string temp="";
                        info("plik otwarty do zapisu");
                        int size=0;

                        info("zapis pliku");
                        int ile_czytac=128;
                        if (rozmiar<128)
                            ile_czytac=1;

                        char a[129]={0};
                        char t[256];
                        //for (int i=0;i<rozmiar;i+=ile_czytac)
                        int i=0;
                        while (size<rozmiar)
                        {
                            if (i>32)
                            {
                                info("Blad przy odbieraniu pliku");
                                Odpowiedz(403,102);
                                return;
                            }
                            if (rozmiar-size<32)
                                ile_czytac=1;
                            stream.read(a,ile_czytac);
                            std::string tmpp=a;
                            //a[ile_czytac]=0;
                            if (tmpp.size()==0)
                            {
                                ile_czytac=1;
                                info("odebrano 0!");
                                ++i;
                                continue;
                            }
                            info2("odebrano",tmpp.c_str());
                            fprintf(plik,"%s",tmpp.c_str());
                            size+=tmpp.size();
                            sprintf(t,"Odczytano %d bajtów z %d bajtów, pozostalo do odczytania %d bajtów\nteraz bede czytal %d bajtow",size,rozmiar,rozmiar-size,ile_czytac);
                            info(t)
#ifdef DEBUG

                            sprintf(t,"%s - %d",a,tmpp.size());
                            info(t);
#endif


                        }
                        //char *temp2=new char(rozmiar);
                        //stream.read(temp2,rozmiar);
                        //info2("odebrano",temp2);
                        //fprintf(plik,"%s",temp2);
                        md5wrapper md5;
                        std::string temp_hash=md5.getHashFromFile(std::string(temp_sciezka));
                        if (temp_hash.compare(hash)!=0) //sprawdzic jak sie wywoluje
                        {
#ifdef DEBUG
                            char debug[256];
                            sprintf(debug,"Hash %s nie zgadza się z %s",hash.c_str(),temp_hash.c_str());
                            info(debug);
#endif
                            Odpowiedz(405,102);
                            return;
                        }
                        info("plik odebrany prawidlowo");
                        char tmp[128];
                        sprintf(tmp,"Pobrano %d z %d",size,rozmiar);
                        info(tmp);
                        fclose(plik);
                        //FILE *plik2=fopen(sciezka.c_str(),"w+");
                        //std::copy(plik,plik2); --zrobic kopiowanie
                        //fclose(plik2);
                        boost::filesystem::copy_file(temp_sciezka,sciezka);

                        info("Odebrano koncowego xmla");
                        baza.addFile(nazwa,uzytkownik,rozmiar,hash,-1,time(NULL));
                    }
                    else
                    {
                        info("brak hasha");
                        Odpowiedz(400,102);
                    }
                }
                else
                {
                    info("brak rozmiaru pliku");
                    Odpowiedz(400,102);
                }
            }
            else
            {
                Eline("brak nazwy");
                Odpowiedz(400,102);
            }
        }
        else
        {
            Eline("brak atrybutów");
            Odpowiedz(400,102);
        }

    }
    else
    {
        Eline("niepoprawnie wyslany xml");
        Odpowiedz(400,102);
        return;
    }

}
void parser::wysylanie_plikow(xmlpp::TextReader &reader, std::string uzytkownik)
{
    info("Wysylanie plikow");
    std::vector <std::string> pliki=pobieranie_listy_plikow(reader);
    std::vector <std::string>::iterator it;
    for (it=pliki.begin();it<pliki.end();it++)
    {
        wyslij_plik(*it,uzytkownik);
    }
    info("Wyslano pliki");
}

void parser::wyslij_plik(std::string plik,std::string uzytkownik)
{
    info("Wyslij plik");
    if (uzytkownik.compare(".")==0)
        uzytkownik=login;
    mysqlpp::StoreQueryResult res=baza.getFileInfo(plik,uzytkownik);
    if (res.num_rows()<1)
    {
        info("res.num_rows()<1");
        std::string odp="<plik nazwa=\""+plik;
        odp+="\" data=\"-1\" rozmiar=\"-1\" dostep=\"-1\"/>\r\n";

        info("res.num_rows()<1");
        Odpowiedz(101,404,odp);
        return;
    }
    else
    {
        if (res[0]["hashValue"].compare("-1")==0)
        {
            info2("Plik usuniety, pomijam",res[0]["sciezka"].c_str());
            Odpowiedz(101,404);
            return;
        }
        info("wysylanie info o pliku")
        std::string odp="<plik nazwa=\""+res[0]["sciezka"];
        odp+="\" data=\""+res[0]["dataDodania"];
        odp+="\" rozmiar=\""+res[0]["dataDodania"];
        odp+="\" dostep=\""+res[0]["prawaDostepu"];
        odp+="\" hash=\""+res[0]["hashValue"];
        odp+="\"/>\r\n";
        Odpowiedz(101,406,odp);
        info2("odp",odp.c_str())
        info("Info o pliku wyslane");
    }
    info("Czekamy na zgode klienta na wysyl pliku");
    /*     std::string od_klienta,temp;
     *     do
     *     {
     *         info("OCZEKIWANIE NA PUSTĄ LINIE");
     *         std::getline(stream,temp);
     *         //std::cout<<(int)temp[0]<<" "<<(int)temp[1]<<std::endl;
     *         od_klienta.append(temp+"\r\n");
     *     }
     *     while (!(temp[0]==13 && temp[1]==0));
     *     info("sprawdzamy co wyslal klient");
     *     xmlpp::TextReader reader_temp((unsigned char*)od_klienta.c_str(),od_klienta.size());
     *     reader_temp.read();
     *     if (reader_temp.get_name().compare("klient")==0)//Sprawdzamy czy odpowiedź nazleży do klienta
     *     {
     *         info("wyslal to napewno klient");
     *         if (reader_temp.has_attributes())//Sprawdzamy czy odpowiedz ma atrybuty
     *         {
     *             info("Są atrybuty");
     *             if (reader_temp.move_to_attribute("action"))//Przechodzimy do atrybutu "action"
     *             {
     *                 info("mamy atrybut action");
     *                 if (reader_temp.get_value().compare("ok")==0)//Jeżeli odpowiedz jest ok to wysylamy plik
     *                 {
     *                     info("action=ok");
     *                     std::string sciezka=login+"/";
     */
    std::string a=czytanie_z_socketa();
    if (!parsuj(a))
    {
        info("błąd parsowania");
        return;
    }
    std::string sciezka;
    sciezka+=plik;
    info("Otwieram plik");
    FILE *fplik=std::fopen(sciezka.c_str(),"r");
    if (fplik==NULL)
    {
        Eline2("NIE UDALO SIE OTWORZYC PLIKU: ",sciezka.c_str());
        Odpowiedz(402);
        return;
    }
    info("no to wysyłamy");
    char temp[BUFSIZE2];
    while ( (fread ( &temp, 1,BUFSIZE2,fplik ) )> 0 )
    {
        info("Trwa wysyłanie pliku, proszę czekać...");
        stream<<temp;
    }
    stream<<std::endl;
    info("Plik został wysłany");
    //dodac odbior potwierdzenia odbioru
    a=czytanie_z_socketa();
}

void parser::usun_pliki(xmlpp::TextReader &reader,std::string uzytkownik)
{
    info("Usuwanie plików");
    while (reader.read())
    {
        info2("plik",reader.get_name().c_str());
        if (reader.get_name().compare("plik")==0)
        {
            info("Usuam Kolejny plik");
            info("plik jest ok");

            if (reader.has_attributes())
            {
                info("ma atrybuty");

                if (reader.move_to_attribute("nazwa"))
                {
                    std::string nazwa = reader.get_value();
                    info2("jest nazwa",nazwa.c_str());

                    if (reader.move_to_attribute("hash"))
                    {
                        std::string hash=reader.get_value().c_str();
                        info("ma hash");

                        if (baza.rmFile(nazwa,uzytkownik,hash))
                        {
#ifdef DEBUG
                            char debug[256];
                            sprintf(debug,"Plik %s o hashu %s usunięty prawidłowo",nazwa.c_str(),hash.c_str());
                            info(debug)
#endif
                            Odpowiedz(406,103);
                        }
                        {
#ifdef DEBUG
                            char debug[256];
                            sprintf(debug,"Plik %s o hashu %s NIE usunięty",nazwa.c_str(),hash.c_str());
                            info(debug)
#endif
                            Odpowiedz(403,103);
                        }
                    }
                }
            }
        }


    }

}

void eat_zombie()
{
    while (true)
    {
        if (wait(NULL)==-1)
        {
            sleep(1);
        }
    }
    //waitpid ( -1, NULL,WNOHANG);
}

