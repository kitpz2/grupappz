#ifndef PARSER_HPP
#define PARSER_HPP

#include <boost/asio.hpp>
#include <boost/filesystem/operations.hpp>
//#include <boost/filesystem/exception.hpp>


#include <string>

#include <fstream>

#include <libxml++/libxml++.h>
#include <libxml++/parsers/textreader.h>
#include <ctime>
#include "include/md5/md5wrapper.h"
#ifdef HAVE_CONFIG_H
#include <config.h>
#endif

#include "baza.hpp"
#include "debug.hpp"

#define BUFSIZE 1024
#define BUFFER 128
#define BUFSIZE2 1024*2
using boost::asio::ip::tcp;
class parser
{
private:
    ///Stream uzywany do odbierania i wysyłania informacji
    tcp::iostream & stream;
    ///Zmienna przechowująca login uzytkownika
    std::string login;
    ///Zmienna przechowująca haslo (w przyszlosci hash hasla) uzytkownika
    std::string haslo;

    //std::string uzytkownik;
    ///Aktualny ID sesji potrzebny pryz kazdym polaczneiu
    int id_sesji;
    ///Bufor danych
    char bufor[BUFSIZE];
    ///Clasa obsługująca bazę danych
    Baza baza;

    ///Parsuje dane pobrane od klienta
    bool parsuj(std::string &do_parsowania);
    ///Loguje klienta po przetworzeniu xmla odebranego od niego i sparsownaiu go w void parsuj()
    bool logowanie(std::string login, std::string haslo);
    ///Wysyła odpowiedź do logowania na podstawie podaneg i gdzie
    ///0 - Logowanie przebieglo prawidlowo; 1 - błęde hasło lub login; 2 - nieoczekiwany błąd serwera; 3 - błędne zapytanie
    void odpowiedz_login(int i);
    /**Wysyła odpowiedź jednolinijkową zależnie od podanego i
    \param nr_odpowiedzi Numer odpowiedzi
    400 - bledne zapytanie
    401 - bledny numer sesji
    402 - podany plik istnieje (w przypadku wysylania pliku)
    403 - wewnetrzny blad serwera
    404 - podany plik nie istnieje
    405 - błąd odbierania plikow
    406 - wszystko OK
    \param numer_operacji Numer operacji do której odnosi się podany nuemr odpowiedzi
    */
    void Odpowiedz(int nr_odpowiedzi, int numer_operacji=-1);
    ///Wysyła odpowiedź wielolinijkową zależnie od podanego i gdzie i jak w void Odpowiedz(int i, int numer_operacji);
    void Odpowiedz(int nr_odpowiedzi,int nr_operacji,std::string odp);

    ///Wysyła dane podane w stringu w dodatkowo wysyłając znak końca linii
    void wyslij(std::string w);
    ///Wysyła listę plików użytkownika 'uzytkownik'
    void lista_plikow(std::string uzytkownik);
    ///Odbiera plik od użytkownika i umieszcza na serwerze
    void odbieranie_plikow(xmlpp::TextReader &reader, std::string uzytkownik);
    ///Odbiera od klienta informację jakie on chce pobrać pliki i przekazuje kazdy plik pojedynczo do wyslij_plik()
    void wysylanie_plikow(xmlpp::TextReader &reader, std::string uzytkownik,char uprawnienia);
    ///Usuwa plik z serwera (jeszcze nie zaimplementowane)
    void usun_pliki(xmlpp::TextReader &reader,std::string uzytkownik);
    ///wysyła plik podany w argumencie
    void wyslij_plik(std::string plik,std::string uzytkownik, char uprawnienia);
    ///Przygotowuje listę plikow do wysłania do klienta
    std::vector <std::string> pobieranie_listy_plikow(xmlpp::TextReader &reader);
    std::string czytanie_z_socketa();
public:
    parser(tcp::iostream &stream, const char* server, const char* user, const char *pass,const char *db):stream(stream)
    {
        info("Parser Konstruktor Początek");
        login.clear();
        haslo.clear();
        id_sesji=0;
        baza.connect(server,user,pass,db);
        info("Parser Konstruktor Koniec");
    };
    void start();

};

//void odbieracz (tcp::iostream &stream);
void eat_zombie();
#endif//PARSER_HPP
