#include "baza.hpp"

void Baza::connect(const char *server, const char *login, const char *pass, const char *db)
{
    //const char db[]="ass8";
    info("Łączenie z Bazą...");
    if (conn.connect(db, server, login, pass))
    {
        info("Połączono");
        return;
    }
    else
    {
        Eline2("BŁĄÐ POŁĄCZENIA Z BAZĄ!",conn.error());
        exit(1);
    }
}

std::string Baza::get_passwd(std::string login)
{
    info("pobieranie hasla uzytkownika")
    std::string zapytanie="select password from auth_user where username='";
    zapytanie.append(login);
    zapytanie.append("'");
    mysqlpp::Query query = conn.query(zapytanie);
    mysqlpp::StoreQueryResult res = query.store();
    if (res.num_rows()>=1)
    {
        info("Zapytanie poprawne");
        return std::string(res[0]["password"]);
    }
    else
    {
        Eline2("Błąd zapytania do bazy danych: ",query.error());
        return std::string("ERROR");
    }
}
///Zapytanie o listę plikow uzytkownika po id uzytkownika z bazy accounts_konto
mysqlpp::StoreQueryResult Baza::getFilesList(int user_id)
{
    info("pobieranie listy plikow usera o ID = 'id'");
    char a[1024];
    sprintf(a,"select * from files_plik where konto_id='%d'",user_id);
    //std::string zapytanie="select * from files_plik where konto_id='";
    //zapytanie+=user_id;
    //zapytanie.append("'");
    info(a);
    mysqlpp::Query query = conn.query(a);
    mysqlpp::StoreQueryResult res = query.store();
    if (res)
    {
        info("Zapytanie Poprawne");
        return res;
    }
    else
    {
        Eline2("Błąd zapytania do bazy danych: ",query.error());
        mysqlpp::StoreQueryResult res;
        return res;
    }

}
///Zapytanie o listę plików uzytkownika o nazwie podanej w zmiennej user
mysqlpp::StoreQueryResult Baza::getFilesList(std::string user)
{
    info("pobieranie listy plikow usera");
    int id=getUserId(user);
    if (id>=0)
    {
        info("id poprawne");
        return getFilesList(id);
    }
    else
    {
        info("ID NIEPOPRAWNE!");
        mysqlpp::StoreQueryResult res;
        return res;
    }
}

///Zapytanie o ID uzytkownika o loginie 'user' ale nie o id z auth_user tylko o id z accounts_konto
int Baza::getUserId(std::string user)
{
    info("pobranie id usera");
    //Najlpierw zapytanie o ID usera z tabeli auth_user
    std::string zapytanie="select id from auth_user where username='";
    zapytanie.append(user);
    zapytanie.append("'");
    info2("zaytanie: ",zapytanie.c_str());
    mysqlpp::Query query = conn.query(zapytanie);//Wyslanie zapytania do bazy
    mysqlpp::StoreQueryResult res = query.store();//umieszczenie wynikow zapytania w zmiennej res
    //A potem zapytanie o id z tabeli accounts_konto
    if (res)
    {
        zapytanie="select id from accounts_konto where user_id='";
        zapytanie.append(res[0]["id"]);
        zapytanie.append("'");
        info(zapytanie.c_str());
        mysqlpp::Query query2 = conn.query(zapytanie);
        mysqlpp::StoreQueryResult res2 = query2.store();
        if (res)
        {
            info("Zapytanie Poprawne");
            return atoi(res2[0]["id"]);
        }
        else
        {
            Eline2("Błąd zapytania do bazy danych: ",query.error());
            return -1;
        }
    }
    else
    {
        Eline2("Błąd zapytania do bazy danych: ",query.error());
        return -1;
    }
}

mysqlpp::StoreQueryResult Baza::getFileInfo(std::string file, std::string user)
{
    info("pobranie info o pliku usera");
    int id=getUserId(user);
    if (id>=0)
    {
        info("id poprawne");
        return getFileInfo(file,id);
    }
    else
    {
        info("ID NIEPOPRAWNE!");
        mysqlpp::StoreQueryResult res;
        return res;
    }
}
mysqlpp::StoreQueryResult Baza::getFileInfo(std::string file, int user_id)
{
    info("pobranie info o pliku usera o ID = 'id'");
    std::string zapytanie="select * from files_plik where konto_id='";
    zapytanie+=user_id;
    zapytanie.append("' AND sciezka='");
    zapytanie+=file;
    zapytanie.append("'");
    mysqlpp::Query query = conn.query(zapytanie);
    mysqlpp::StoreQueryResult res = query.store();
    if (res)
    {
        info("Zapytanie Poprawne");
        return res;
    }
    else
    {
        Eline2("Błąd zapytania do bazy danych: ",query.error());
        mysqlpp::StoreQueryResult res;
        return res;
    }

}

void Baza::addFile(std::string nazwa, std::string konto, int wielkosc, int hash, int prawa, int data)
{
    info("dodanie pliku do bazy");
    std::string zapytanie="SELECT * from files_plik";
    int ilosc;
    info("pobieranie id");
    int id=getUserId(konto);
    info("id pobrane")
    char a[256];
    sprintf(a,"user %s o id %d umieszcza plik %s",konto.c_str(),id,nazwa.c_str());
    info(a);
    mysqlpp::Query query = conn.query(zapytanie);
    mysqlpp::StoreQueryResult res = query.store();
    if (res)
    {
        //ilosc=res[0]["count(*)"];
        ilosc=res.num_rows();
    }
    else
        info("!res");
    info("po");
    query = conn.query();
    //SELECT count(*) from files_plik;
    char b[1024];
    sprintf(b,"INSERT INTO files_plik VALUES(%d,%d,'%s',CURRENT_DATE,%d,%d,%d)",ilosc+1,id,nazwa.c_str(),prawa,wielkosc,hash);
    query<<b;
    query.execute();
    /*zapytanie="INSERT INTO files_plik VALUES('"+ilosc;
    zapytanie+="','"+id;
    zapytanie+="','"+nazwa;
    zapytanie+="',CURRENT_DATE";
    zapytanie+="','"+prawa;
    zapytanie+="','"+wielkosc;
    zapytanie+="','"+hash;
    zapytanie.append("')");*/
    info2("zapytanie",b);
    //conn.query(b);
    info("po2");
    //res = query.store();
    //if(res)
    //    info("plik dodany do bazy")
    //else
    //    info("plik nie dodany");

}
