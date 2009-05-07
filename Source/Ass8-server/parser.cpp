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

void parser::parsuj(std::string do_parsowania)
{
    line;
#ifdef LIBXMLCPP_EXCEPTIONS_ENABLED
    try
    {
        line;
#endif //LIBXMLCPP_EXCEPTIONS_ENABLED
        xmlpp::TextReader reader((unsigned char*)do_parsowania.c_str(),do_parsowania.size());
        while (reader.read())
        {
            info("reader.read()");
            info2("reader.get_name()",reader.get_name().c_str());
            if (reader.get_name().compare("logowanie ")) ///Logowanie - Trzeba pamiętać że parser umieszcza dodatkowo spację na końcu sparsowanych stringów
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
                        odpowiedz_login(3);
                    if (reader.move_to_attribute("haslo")) ///Przesun sie do atrybutu "haslo"
                    {
                        haslo=reader.get_value();
                        info2("haslo",haslo.c_str());
                    }
                    else ///Gdy brak takiego atrybutu to konczymy rozmowe
                        odpowiedz_login(3);

                    if (logowanie(login,haslo)==true)///Sprawdzamy czy login i haslo pasują
                    {
                        info("Logowanie - OK");
                        odpowiedz_login(0);///Jeżeli tak to dajemy klientowi ID sesji
                    }
                    else
                        odpowiedz_login(1);///Jeżeli nie to informujemy go o tym i kończymy
                }
                else ///Brak loginu i hasła
                    odpowiedz_login(3);
            }
            else if (reader.get_name().compare("klient "))///Jeżeli polecenie od klienta
            {
                int operacja;
                std::string t_uzytkownik;
                if (reader.has_attributes()) ///Sprawdzanie ilosci atrybutów
                {
                    info("reader.has_attributes==true");
                    if (reader.move_to_attribute("idsesji")) ///Przesun sie do atrybutu "idsesji"
                    {
                        ///Do debugowania
                        char temp[128];
                        std::string uzytkownik;
                        sprintf(temp,"%u",id_sesji);
                        info("Sprawdzanie id_sesji");
                        info2(temp,reader.get_value().c_str());
                        if (id_sesji==atoi(reader.get_value().c_str())) ///Sprawdzamy czy zgadza się id_sesji
                        {
                            info("id_sesji POPRAWNE");
                            if (reader.move_to_attribute("operacja"))///Sprawdzamy czy istnieje argument "operacja"
                            {
                                info2("operacja",reader.get_value().c_str());
                                operacja=atoi(reader.get_value().c_str());
                            }
                            else
                            {
                                info("BŁĘDNE ZAPYTANIE");
                                Odpowiedz(400);///Błędne zapytanie
                            }
                            if (reader.move_to_attribute("uzytkownik"))
                            {
                                info2("uzytkowik",reader.get_value().c_str());
                                uzytkownik=reader.get_value();
                            }
                            else
                            {
                                info("BŁĘDNE ZAPYTANIE");
                                Odpowiedz(400);///Błędne zapytanie
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
                                odbieranie_plikow(reader,uzytkownik);
                                break;
                            case 103:
                                info("OPERACJA 103");
                                usun_pliki(reader,uzytkownik);
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
                        }
                    }
                    else ///Brak id_sesji
                    {
                        info("BRAK ID_SESJI");
                        Odpowiedz(401);
                    }
                }
                else///Brak atrybutów
                {
                    Odpowiedz(400);
                }
            }
            else//Jeżeli polecenie nie zaczyna się od <klient
            {
                Odpowiedz(400);
            }
#ifdef LIBXMLCPP_EXCEPTIONS_ENABLED
        }
    }
    catch (const std::exception& e)///Jeżeli wyjątki w LIBXMLCPP są włącozne i wystąpi błąd parsowania
    {
        info("BŁAD PARSOWANIA XML");
        wyslij("<?xml version=\"1.0\"?>\
        <serwer xml_stream_error=\"true\"/>"); ///To wysyłana jest o tym informacja
        exit(0);///I zamykamy połączenie
    }
#endif //LIBXMLCPP_EXCEPTIONS_ENABLED


}
void parser::wyslij(std::string w)///Funkcja wysyłająca dane przez SOCKET
{
    stream<<w<<std::endl<<std::endl;
}
bool parser::logowanie(std::string login, std::string haslo)///Funkcja sprawdzająca czy dla podanego loginu hasło jest prawidłowe, jeżeli tak to generuje id_sesji
{
    info2(baza.get_passwd(login).c_str(),haslo.c_str());
    if (baza.get_passwd(login).compare(0,haslo.size()-1,haslo))///Trzeba pamiętać że zmienna haslo tak naprawde zawiera haslo i na końcu spację.
    {
        srand(time(NULL));
        id_sesji=(unsigned int)rand();
        return true;
    }
    else
        return false;
}
///Krotka odpowiedz gdzie i - kod odpowiedzi
void parser::Odpowiedz(int i,int nr_operacji)
{
    sprintf(bufor,"<?xml version=\"1.0\"?>\
    <serwer operacja=\"%d\" odp=\"%d\"/>",nr_operacji,i);
    wyslij(bufor);
}
///Odpowiedź z dodatkowymi informacjami  gdzie i - kod odpowiedzi
void parser::Odpowiedz(int i,int nr_operacji,std::string odp)
{
    std::string temp;
    sprintf(bufor,"<?xml version=\"1.0\"?>\
    <serwer operacja=\"%d\" odp=\"%d\">",nr_operacji,i);
    temp.append(bufor);
    temp.append(odp);
    temp.append("</serwer>");
    wyslij(temp);
}
void parser::odpowiedz_login(int i)///Funkcja wysyłająca odpowiedź po prośbie o zalogowanie
{
    sprintf(bufor,"<?xml version=\"1.0\"?>\
        <serwer odpowiedz=\"%d\" sesja=\"%u\" wersja=\"%s %s\"/>",i,id_sesji,AutoVersion::FULLVERSION_STRING,AutoVersion::STATUS_SHORT);
    wyslij(bufor);
    if (i!=0)
    {
        stream.close();
        exit(0);
    }
}
void parser::start () ///Funkcja wywoływana tylko raz, rozpoczyna pobieranie informacji z SOCKETA i przekazuje je parserowi
{
    stream<<"ASS8 Server v"<<AutoVersion::FULLVERSION_STRING<<std::endl;
    std::string a;
    std::string temp;
    while (true)
    {
        info("PETLA ZBIERAJACA");
        do
        {
            info("OCZEKIWANIE NA PUSTĄ LINIE");
            std::getline(stream,temp);
            //std::cout<<(int)temp[0]<<" "<<(int)temp[1]<<std::endl;
            a.append(temp+"\n");
        }
        while (!(temp[0]==13 && temp[1]==0));
        info("ROZPOCZYNAM PARSOWANIE");
        //std::cout<<a<<std::endl;
        parsuj(a);
    }
    stream.close();
    exit(0);
}

void parser::lista_plikow(std::string uzytkownik)
{
    info("Lista_plikow");
    if (uzytkownik.compare(". "))
        uzytkownik=login;
    mysqlpp::StoreQueryResult res=baza.getFilesList(uzytkownik);
    if (res.num_rows()<1)
    {
        info("res.num_rows()<1");
        Odpowiedz(400);
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
            //sprintf(temp,"<plik nazwa=\"%s\" data=\"%d\" rozmiar=\"%d\" dostep=\"%d\"/>\n",res[i]["sciezka"],res[i]["dataDodania"],res[i]["wielkosc"],res[i]["prawaDostepu"]);
            temp="<plik nazwa=\""+res[i]["sciezka"];
            temp+="\" data=\""+res[i]["dataDodania"];
            temp+="\" rozmiar=\""+res[i]["wielkosc"];
            temp+="\" dostep=\""+res[i]["prawaDostepu"];
            temp+="\n";
            odp.append(temp);
        }
        info("Lista utworzona");
        Odpowiedz(406,100,odp);
    }
}

std::vector <std::string> parser::pobieranie_listy_plikow(xmlpp::TextReader &reader)
{
    info("Odbieranie listy plikow");
    std::vector <std::string> nazwy_plikow;
    while (reader.read())
    {

        if (reader.get_name().compare("plik "))//czy mamy plik?
        {
            info("mamy <plik ");
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
                    //return;
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

/*void wyslij_plik(xmlpp::TextReader &reader,std::string plik)
{

}*/
void parser::odbieranie_plikow(xmlpp::TextReader &reader, std::string uzytkownik)
{
    info("Odbieranie plikow");
    reader.read();
    if(reader.get_name().compare("plik "))
    {
        if(reader.has_attributes())
        {
            if(reader.move_to_attribute("nazwa"))
            {
                /*mysqlpp::StoreQueryResult res=baza.getFileInfo(reader.get_value(),login);
                if(res.num_rows()<1)
                {*/

                    std::string odp="<?xml version=\"1.0\"?>\
                    <serwer operacja=\"102\" odp=\"404\"/>";
                    wyslij(odp);
                    std::string sciezka=login+"/";
                    sciezka+=reader.get_value().c_str();
                    FILE *plik=fopen(sciezka.c_str(),"w+");
                    if(!plik)
                    {
                        Eline("PLIK SIE NIE OTWORZYL!!!!");
                        return;
                    }
                    std::string temp="";

                    do
                    {
                        fprintf(plik,"%s",temp.c_str());
                        std::getline(stream,temp);
                    }while(!(temp[0]==13 && temp[1]==0));
                    info("plik odebrany");
                    //na razie nie sprawdzam hasha itd
                    std::getline(stream,temp);
                    std::getline(stream,temp);
                    std::getline(stream,temp);
                    info("Odebrano koncowego xmla");

                //}
            }
        }
        else
        {
            Eline("brak atrybutów");
            Odpowiedz(400);
        }

    }
    else
    {
        Eline("niepoprawnie wyslany xml");
        Odpowiedz(400);
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
    if (uzytkownik.compare(". "))
        uzytkownik=login;
    mysqlpp::StoreQueryResult res=baza.getFileInfo(plik,uzytkownik);
    if (res.num_rows()<1)
    {
        info("res.num_rows()<1");
        std::string odp="<plik nazwa=\""+plik;
        odp+="\" data=\"-1\" rozmiar=\"-1\" dostep=\"-1\"/>\n";
        info("res.num_rows()<1");
        Odpowiedz(101,402,odp);
        return;
    }
    else
    {
        info("wysylanie info o pliku")
        std::string odp="<plik nazwa=\""+res[0]["sciezka"];
        odp+="\" data=\""+res[0]["dataDodania"];
        odp+="\" rozmiar=\""+res[0]["dataDodania"];
        odp+="\" dostep=\""+res[0]["prawaDostepu"];
        odp+="\"/>\n";
        Odpowiedz(101,406,odp);
        info("Info o pliku wyslane");
    }
    info("Czekamy na zgode klienta na wysyl pliku");
    std::string od_klienta,temp;
    do
    {
        info("OCZEKIWANIE NA PUSTĄ LINIE");
        std::getline(stream,temp);
        //std::cout<<(int)temp[0]<<" "<<(int)temp[1]<<std::endl;
        od_klienta.append(temp+"\n");
    }
    while (!(temp[0]==13 && temp[1]==0));
    info("sprawdzamy co wyslal klient");
    xmlpp::TextReader reader_temp((unsigned char*)od_klienta.c_str(),od_klienta.size());
    reader_temp.read();
    if (reader_temp.get_name().compare("klient "))//Sprawdzamy czy odpowiedź nazleży do klienta
    {
        info("wyslal to napewno klient");
        if (reader_temp.has_attributes())//Sprawdzamy czy odpowiedz ma atrybuty
        {
            info("Są atrybuty");
            if (reader_temp.move_to_attribute("action"))//Przechodzimy do atrybutu "action"
            {
                info("mamy atrybut action");
                if (reader_temp.get_value().compare("ok "))//Jeżeli odpowiedz jest ok to wysylamy plik
                {
                    info("action=ok");
                    std::string sciezka=login+"/";
                    sciezka+=plik;
                    info("Otwieram plik");
                    FILE *plik=std::fopen(sciezka.c_str(),"r");
                    if(plik==NULL)
                    {
                        Eline2("NIE UDALO SIE OTWORZYC PLIKU: ",sciezka.c_str());
                        Odpowiedz(402);
                        return;
                    }
                    info("no to wysyłamy");
                    char temp[BUFSIZE2];
                    while ( (fread ( &temp, 1,BUFSIZE2,plik ) )> 0 )
                    {
                        info("Trwa wysyłanie pliku, proszę czekać...");
                        stream<<temp;
                    }
                    /*std::fstream plik(plik,"r");
                    if(plik.is_iopen())
                    {*/
                    stream<<std::endl;
                    info("Plik został wysłany");
                    std::string odp="<?xml version=\"1.0\"?>\
                    <serwer operacja=\"101\" odp=\"kod_odp\" hash=\"-1\" />";
                    wyslij(odp);

                }
                else//Klient nie zgadza sie na przeslanie
                {
                    Eline("Compare != OK")
                    return;
                }
            }
            else//Brak atrybutu "action"
            {
                Eline("Brak atrybutu \"action\"");
                Odpowiedz(400);
                return;
            }

        }
        else//Brak atrybutów
        {
            Eline("Brak atrybutów");
            Odpowiedz(400);
            return;
        }
    }
    else//Odpowiedź nie od klienta
    {
        Eline("Odpowiedz nie od klienta");
        Odpowiedz(400);
        return;
    }
}
void parser::usun_pliki(xmlpp::TextReader &reader,std::string uzytkownik)
{
info("NOT IMPLEMENTED YET!!!");

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

