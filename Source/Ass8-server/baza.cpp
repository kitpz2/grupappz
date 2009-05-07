#include "baza.hpp"

void Baza::connect(const char *server, const char *login, const char *pass)
{
    const char db[]="ass8";
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
    std::string zapytanie="select password from auth_user where username='";
    zapytanie.append(login);
    zapytanie.append("'");
    mysqlpp::Query query = conn.query(zapytanie);
    mysqlpp::StoreQueryResult res = query.store();
    if (res)
    {
        info("Zapytanie poprawne");
        return std::string(res[0]["password"]);
    }
    else
    {
        Eline2("Błąd zapytania do bazy danych: ",query.error());
        return "ERROR";
    }
}
///Zapytanie o listę plikow uzytkownika po id uzytkownika z bazy accounts_konto
mysqlpp::StoreQueryResult Baza::getFilesList(int user_id)
{
    std::string zapytanie="select * from files_plik where konto_id='";
    zapytanie+=user_id;
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
///Zapytanie o listę plików uzytkownika o nazwie podanej w zmiennej user
mysqlpp::StoreQueryResult Baza::getFilesList(std::string user)
{
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
    //Najlpierw zapytanie o ID usera z tabeli auth_user
    std::string zapytanie="select id from auth_user where username='";
    zapytanie.append(user);
    zapytanie.append("'");
    mysqlpp::Query query = conn.query(zapytanie);//Wyslanie zapytania do bazy
    mysqlpp::StoreQueryResult res = query.store();//umieszczenie wynikow zapytania w zmiennej res
    //A potem zapytanie o id z tabeli accounts_konto
    if (res)
    {
        zapytanie="select id from accounts_konto where user_id='";
        zapytanie.append(res[0]["id"]);
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
    std::string zapytanie="SELECT count(*) from files_plik";
    int ilosc;
    int id=getUserId(konto);
    mysqlpp::Query query = conn.query(zapytanie);
    mysqlpp::StoreQueryResult res = query.store();
    if (res)
    {
        ilosc=res[0]["count(*)"];
    }
    //SELECT count(*) from files_plik;
    zapytanie="INSERT INTO files_plik VALUES('"+ilosc;
    zapytanie+="','"+id;
    zapytanie+="','"+nazwa;
    zapytanie+="',CURRENT_DATE";
    zapytanie+="','"+prawa;
    zapytanie+="','"+wielkosc;
    zapytanie+="','"+hash;
    zapytanie.append("')");
    conn.query(zapytanie);

}
