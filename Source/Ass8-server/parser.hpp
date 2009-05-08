#ifndef PARSER_HPP
#define PARSER_HPP

#include <boost/asio.hpp>

#include <string>

#include <fstream>

#include <libxml++/libxml++.h>
#include <libxml++/parsers/textreader.h>
#ifdef HAVE_CONFIG_H
#include <config.h>
#endif

#include "baza.hpp"
#include "debug.hpp"

#define BUFSIZE 1024
#define BUFSIZE2 1024*2
using boost::asio::ip::tcp;
class parser
{
private:
    tcp::iostream & stream;
    std::string login;
    std::string haslo;
    //std::string uzytkownik;
    int id_sesji;
    char bufor[BUFSIZE];
    Baza baza;

    void parsuj(std::string do_parsowania);
    bool logowanie(std::string login, std::string haslo);
    void odpowiedz_login(int i);
    void Odpowiedz(int i, int numer_operacji=-1);
    void Odpowiedz(int i,int nr_operacji,std::string odp);

    void wyslij(std::string w);
    void lista_plikow(std::string uzytkownik);
    void odbieranie_plikow(xmlpp::TextReader &reader, std::string uzytkownik);
    void wysylanie_plikow(xmlpp::TextReader &reader, std::string uzytkownil);
    void usun_pliki(xmlpp::TextReader &reader,std::string uzytkownik);
    void wyslij_plik(std::string plik,std::string uzytkownik);
    std::vector <std::string> pobieranie_listy_plikow(xmlpp::TextReader &reader);
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
