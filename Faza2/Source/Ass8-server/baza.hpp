#ifndef BAZA_HPP
#define BAZA_HPP
#include <string>
#include <mysql++.h>
#include <iostream>
#include <iomanip>
#include "debug.hpp"
class Baza
{
    private:
        ///Klasa do łączenia się z bazą danych
        mysqlpp::Connection conn;
    public:
        ///Pobiera hasło uzytkownika z bazy
        std::string get_passwd(std::string login);
        ///Konstruktor pusty
        Baza(){};
        ///Łaczy się z bazą damych
        void connect(const char *server, const char *login, const char *pass, const char *db);
        ///Pobiera listę plików z bazy na podstawie ID uzytkownika
        mysqlpp::StoreQueryResult getFilesList(int user_id);
        ///Najpierw wywołuje getUserId() potem z id otrzymanym z tamtąd wywołuje getFilesList(int user_id);
        mysqlpp::StoreQueryResult getFilesList(std::string user);
        ///Podobnie jak getFilesList tylko ze pobiera informację o jednym pliku
        mysqlpp::StoreQueryResult getFileInfo(std::string file, std::string user);
        ///
        mysqlpp::StoreQueryResult getFileInfo(std::string file, int user_id);
        ///Pobiera id uzytkownika 'user'
        int getUserId(std::string user);
        ///Dodaje plik do bazy danych
        void addFile(std::string nazwa, std::string konto, int wielkosc, int hash=-1, int prawa=-1, int data=-1);

};

#endif//BAZA_HPP
