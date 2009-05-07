//
// daytime_server.cpp
// ~~~~~~~~~~~~~~~~~~
//
// Copyright (c) 2003-2008 Christopher M. Kohlhoff (chris at kohlhoff dot com)
//
// Distributed under the Boost Software License, Version 1.0. (See accompanying
// file LICENSE_1_0.txt or copy at http://www.boost.org/LICENSE_1_0.txt)
//

#include <iostream>
#include <string>
#include <cstdio>
#include <cstdlib>

#include <sys/types.h>
#include <unistd.h>
#include <sys/wait.h>


#include <boost/asio.hpp>
#include <boost/thread/thread.hpp>
#include <boost/bind.hpp>

#include "version.h"
#include "parser.hpp"

using boost::asio::ip::tcp;

int main(int argc, char *argv[])
{
    ///Zmienna przechowująca port na którym serwer nasłucuje
    unsigned int port;
    ///Zmienne przechowujące parametry podłączenia do bazy danych
    const char *server = 0, *user = 0, *pass = 0, *db=0;
    ///Zmienna przechowująca nazwę bazy w bazie danych
    if (argc<6)///Jeżeli jest mniej niż 5 argumentów
    {
        std::cout<<"Zła liczba argumentów:\n\
        serwer <adres_serwera_mysql> <login> <haslo> <db> <port_nasłuchiwania_aserwera_ass8>"<<std::endl;
        exit(1);///To kończymy program
    }
    else
    {
        server=argv[1];
        user=argv[2];
        pass=argv[3];
        db=argv[4];
        std::sscanf(argv[5],"%u",&port);
    }
    try
    {
        ///Potrzebne do połączenia z klientem
        boost::asio::io_service io_service;
        ///Potrzebne do połączenia z klientem
        tcp::endpoint endpoint(tcp::v4(), port);
        ///Potrzebne do połączenia z klientem
        tcp::acceptor acceptor(io_service, endpoint);
        ///Watek ktory bedzie usuwal skonczone forki
        boost::thread w1(&eat_zombie);
        while (true)///Nieskonczona pętla
        {
            ///Wyjście/Wejście socketa
            tcp::iostream stream;

            acceptor.accept(*stream.rdbuf());
            if (fork()==0)
            {
                ///utworzenie parsera w forku (dla każdego klienta jeden taki jest tworzony);
                parser p(stream,server,user,pass,db);
                p.start();
            }
        }
    }
    catch (std::exception& e)
    {
        std::cerr << e.what() << std::endl;
    }

    return 0;
}

