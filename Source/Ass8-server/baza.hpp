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
        mysqlpp::StoreQueryResult getFilesList(int user_id, char uprawnienia);
        ///Najpierw wywołuje getUserId() potem z id otrzymanym z tamtąd wywołuje getFilesList(int user_id);
        mysqlpp::StoreQueryResult getFilesList(std::string user, char uprawnienia);
        ///Podobnie jak getFilesList tylko ze pobiera informację o jednym pliku
        mysqlpp::StoreQueryResult getFileInfo(std::string file, std::string user, char uprawnienia);
        ///
        mysqlpp::StoreQueryResult getFileInfo(std::string file, int user_id, char uprawnienia);
        ///Pobiera id uzytkownika 'user'
        int getUserId(std::string user);
        /**
        Funkcja sprawdzająca czy użytkownicy są znajomymi
        \param user1 Użytkownik pytający
        \param user2 Użytkownik sprawdzany
        \return char przedstawiający informację o rodzaju znajomości:
        1 - użytkownik sprawdzany udostępnia pliku użytkownikowi pytającemu
        2 - użytkownik sprawdzany nie udostępnia plików użytkownikowi pytającemu
        0 - to są pliki tego użytkownika
        */
        char friends(std::string user1, std::string user2);
        ///Dodaje plik do bazy danych
        bool addFile(std::string nazwa, std::string konto, int wielkosc, std::string hash="-1", int prawa=-1, int data=-1);
        /**Usuwa plik z bazy
        \param nazwa nazwa pliku do usuniecia
        \param konto nazwa konta z ktorego sie usuwa
        \param hash hash pliku (dla sprawdzenia czy napewno dobry plik)
        \return czy operacja zakończona powodzeniem*/
        bool rmFile(std::string nazwa, std::string konto, std::string hash);
};

#endif//BAZA_HPP
