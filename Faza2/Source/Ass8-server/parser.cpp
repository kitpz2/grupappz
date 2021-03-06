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
#endif //LIBXMLCPP_EXCEPTIONS_ENABLED
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
                    info("brak atrybutów");
                    Odpowiedz(400);
                }
            }
            else//Jeżeli polecenie nie zaczyna się od <klient
            {
                info("nierozpoznano");
                Odpowiedz(400);
                return;
            }
#ifdef LIBXMLCPP_EXCEPTIONS_ENABLED
        }
    }
    catch (const std::exception& e)///Jeżeli wyjątki w LIBXMLCPP są włącozne i wystąpi błąd parsowania
    {
        info2("BŁAD PARSOWANIA ",e.what());
        wyslij("<?xml version=\"1.0\"?>\
        <serwer xml_stream_error=\"true\"/>"); ///To wysyłana jest o tym informacja
        exit(0);///I zamykamy połączenie
    }
#endif //LIBXMLCPP_EXCEPTIONS_ENABLED


}
void parser::wyslij(std::string w)///Funkcja wysyłająca dane przez SOCKET
{
    stream<<w<<std::endl;
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
        Eline("Logowanie(): niepoprawne haslo");
        return false;
    }
}
///Krotka odpowiedz gdzie i - kod odpowiedzi
void parser::Odpowiedz(int i,int nr_operacji)
{
    sprintf(bufor,"<?xml version=\"1.0\"?>\r\n\
    <serwer operacja=\"%d\" odp=\"%d\"/>",nr_operacji,i);
    wyslij(bufor);
}
///Odpowiedź z dodatkowymi informacjami  gdzie i - kod odpowiedzi
void parser::Odpowiedz(int i,int nr_operacji,std::string odp)
{
    std::string temp;
    sprintf(bufor,"<?xml version=\"1.0\"?>\r\n\
    <serwer operacja=\"%d\" odp=\"%d\">",nr_operacji,i);
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
    std::string temp;
    while (true)
    {
        a="";
        temp="";
        int zabezp=0, zabezp2=0;
        info("PETLA ZBIERAJACA");

        do
        {
            boost::system::error_code error;
            if (error == boost::asio::error::eof)
            {
                info("Klient disconnected");
                exit(0);
            }
            //info("OCZEKIWANIE NA PUSTĄ LINIE");
            std::getline(stream,temp);
            /*if(zabezp>25)
            {
                stream.close();
            }*/

            /*if(temp.empty() || temp.compare(" ")==0)
            {
                info("linia pusta");
                //stream.clear();
            }*/
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

            //std::cout<<(int)temp[0]<<" "<<(int)temp[1]<<std::endl;
            a.append(temp+"\r\n");
        }
        while (!(temp[0]==13 && temp[1]==0));
        //zabezp=0;
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
    if (uzytkownik.compare(".")==0)
        uzytkownik=login;
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
            //sprintf(temp,"<plik nazwa=\"%s\" data=\"%d\" rozmiar=\"%d\" dostep=\"%d\"/>\n",res[i]["sciezka"],res[i]["dataDodania"],res[i]["wielkosc"],res[i]["prawaDostepu"]);
            char a[1024];
            sprintf(a,"<plik nazwa=\"%s\" data=\"%d\" rozmiar=\"%d\" dostep=\"%d\"/>\r\n",res[i]["sciezka"].c_str(),atoi(res[i]["dataDodania"].c_str()),atoi(res[i]["wielkosc"].c_str()),atoi(res[i]["prawaDostepu"].c_str()));
            /*temp="<plik nazwa=\""+res[i]["sciezka"];
            line;
            temp+="\" data=\""+res[i]["dataDodania"];
            line;
            temp+="\" rozmiar=\""+res[i]["wielkosc"];
            line;
            temp+="\" dostep=\""+res[i]["prawaDostepu"];
            line;
            temp+="\r\n";*/
            info(a);
            odp.append(a);
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

        if (reader.get_name().compare("plik")==0)//czy mamy plik?
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
                    /*mysqlpp::StoreQueryResult res=baza.getFileInfo(reader.get_value(),login);
                    if(res.num_rows()<1)
                    {*/

                    std::string odp="<?xml version=\"1.0\"?>\
                    <serwer operacja=\"102\" odp=\"404\"/>\r\n";
                    wyslij(odp);
                    std::string sciezka=login+"/";
                    sciezka+=nazwa;
                    FILE *plik=fopen(sciezka.c_str(),"w+");
                    if (!plik)
                    {
                        Eline("PLIK SIE NIE OTWORZYL!!!!");
                        return;
                    }
                    std::string temp="";
                    info("plik otwarty do zapisu");
                    int size=0;
                    int size_old=0;
                    /*do
                    {
                        size_old=size;
                        info("zapis fragmentu");
                        std::getline(stream,temp);
                        fprintf(plik,"%s",temp.c_str());
                        size+=temp.size();
                        info2("odebrano",temp.c_str());
                        if(size_old==size)
                            break;
                    }while (size<rozmiar);*/
                    info("zapis pliku");
                    //char *a=new char(4);
                    int ile_czytac=32;
                    char a[33]={0};
                    char t[256];
                    for (int i=0;i<rozmiar;i+=32)
                    {
                        if(rozmiar-size<32)
                            ile_czytac=rozmiar-size;
                        stream.read(a,ile_czytac);
                        a[ile_czytac]=0;
                        info2("odebrano",a);
                        fprintf(plik,"%s",a);
                        size+=ile_czytac;
                        sprintf(t,"Odczytano %d bajtów z %d bajtów, pozostalo do odczytania %d bajtów\nteraz bede czytal %d bajtow",size,rozmiar,rozmiar-size,ile_czytac);
                        info(t);
                    }
                    //char *temp2=new char(rozmiar);
                    //stream.read(temp2,rozmiar);
                    //info2("odebrano",temp2);
                    //fprintf(plik,"%s",temp2);
                    fclose(plik);
                    info("plik odebrany");
                    char tmp[128];
                    sprintf(tmp,"Pobrano %d z %d",size,rozmiar);
                    info(tmp);
                    //na razie nie sprawdzam hasha itd
                    //std::getline(stream,temp);
                    //std::getline(stream,temp);
                    //std::getline(stream,temp);
                    info("Odebrano koncowego xmla");
                    baza.addFile(nazwa,uzytkownik,rozmiar,-1,-1,-1);
                    //}
                }
                else
                {
                    info("brak rozmiaru pliku");
                    Odpowiedz(400,102);
                }
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
        odp+="\"/>\r\n";
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
        od_klienta.append(temp+"\r\n");
    }
    while (!(temp[0]==13 && temp[1]==0));
    info("sprawdzamy co wyslal klient");
    xmlpp::TextReader reader_temp((unsigned char*)od_klienta.c_str(),od_klienta.size());
    reader_temp.read();
    if (reader_temp.get_name().compare("klient")==0)//Sprawdzamy czy odpowiedź nazleży do klienta
    {
        info("wyslal to napewno klient");
        if (reader_temp.has_attributes())//Sprawdzamy czy odpowiedz ma atrybuty
        {
            info("Są atrybuty");
            if (reader_temp.move_to_attribute("action"))//Przechodzimy do atrybutu "action"
            {
                info("mamy atrybut action");
                if (reader_temp.get_value().compare("ok")==0)//Jeżeli odpowiedz jest ok to wysylamy plik
                {
                    info("action=ok");
                    std::string sciezka=login+"/";
                    sciezka+=plik;
                    info("Otwieram plik");
                    FILE *plik=std::fopen(sciezka.c_str(),"r");
                    if (plik==NULL)
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
                    Eline("Compare != OK");
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

