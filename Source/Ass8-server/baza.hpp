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
        mysqlpp::Connection conn;
    public:
        std::string get_passwd(std::string login);
        Baza(){};
        void connect(const char *server, const char *login, const char *pass);
        mysqlpp::StoreQueryResult getFilesList(int user_id);
        mysqlpp::StoreQueryResult getFilesList(std::string user);
        mysqlpp::StoreQueryResult getFileInfo(std::string file, std::string user);
        mysqlpp::StoreQueryResult getFileInfo(std::string file, int user_id);
        int getUserId(std::string user);
        void addFile(std::string nazwa, std::string konto, int wielkosc, int hash=-1, int prawa=-1, int data=-1);

};

#endif//BAZA_HPP
