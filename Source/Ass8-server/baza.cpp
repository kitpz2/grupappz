#include "baza.hpp"

void Baza::connect(const char *server, const char *login, const char *pass, const char *db)
{
    //const char db[]="ass8";
    try
    {
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
    catch (const std::exception& e)//jezeli wystapil wyjatek w SQLu
    {
        info2("BŁAD PARSOWANIA SQL w connect : ",e.what());
        exit(0);///I zamykamy połączenie

    }
}

std::string Baza::get_passwd(std::string login)
{
    try
    {
        info("pobieranie hasla uzytkownika")
        char zapytanie[256];
        sprintf(zapytanie,"select plain_pass from accounts_konto where user_id='%d'",getUserIdLogin(login));
        //std::string zapytanie="select plain_pass from accounts_konto where userid='";
        //zapytanie.append(login);
        //zapytanie.append("'");
        info2("Zapytanie do bazy",zapytanie);
        mysqlpp::Query query = conn.query(zapytanie);
        mysqlpp::StoreQueryResult res = query.store();
        if (res.num_rows()>=1)
        {
            info("Zapytanie poprawne");
            info2("haslo",res[0]["plain_pass"].c_str());
            return std::string(res[0]["plain_pass"]);
        }
        else
        {
            Eline2("Błąd zapytania do bazy danych: ",query.error());
            return std::string("ERROR");
        }
    }
    catch (const std::exception& e)//jezeli wystapil wyjatek w SQLu
    {
        info2("BŁAD PARSOWANIA SQL w get_passwd : ",e.what());
        return "-1";
    }
}
///Zapytanie o listę plikow uzytkownika po id uzytkownika z bazy accounts_konto
mysqlpp::StoreQueryResult Baza::getFilesList(int user_id, char uprawnienia)
{
    try
    {
#ifdef DEBUG
        char debug[1024];
        sprintf(debug,"pobranie listy plikow usera o ID = '%d'",user_id);
        info(debug);
        sprintf(debug,"Uprawnienia usera: %d",(unsigned int)(uprawnienia));
        info(debug);
#endif//DEBUG
        char a[1024];
        sprintf(a,"select * from files_plik where konto_id='%d' and prawaDostepu>='%d'",user_id,(unsigned int)(uprawnienia));
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
    catch (const std::exception& e)//jezeli wystapil wyjatek w SQLu
    {
        info2("BŁAD PARSOWANIA SQL w getFilesList : ",e.what());
        return (mysqlpp::StoreQueryResult());
    }

}
///Zapytanie o listę plików uzytkownika o nazwie podanej w zmiennej user
mysqlpp::StoreQueryResult Baza::getFilesList(std::string user, char uprawnienia)
{
    try
    {
        info("pobieranie listy plikow usera");
        int id=getUserId(user);
        if (id>=0)
        {
            info("id poprawne");
            return getFilesList(id,uprawnienia);
        }
        else
        {
            info("ID NIEPOPRAWNE!");
            mysqlpp::StoreQueryResult res;
            return res;
        }
    }
    catch (const std::exception& e)//jezeli wystapil wyjatek w SQLu
    {
        info2("BŁAD PARSOWANIA SQL w getFilesList II : ",e.what());
        return (mysqlpp::StoreQueryResult());
    }
}

int Baza::getUserIdLogin(std::string user)
{
    try
    {
        info("pobranie id usera");
        char zapytanie[256];
        sprintf(zapytanie,"select id from auth_user where username='%s'",user.c_str());
        //Najlpierw zapytanie o ID usera z tabeli auth_user
        //std::string zapytanie="select id from auth_user where username='";
        //zapytanie.append(user);
        //zapytanie.append("'");
        info2("zaytanie: ",zapytanie);
        mysqlpp::Query query = conn.query(zapytanie);//Wyslanie zapytania do bazy
        mysqlpp::StoreQueryResult res = query.store();//umieszczenie wynikow zapytania w zmiennej res
        //A potem zapytanie o id z tabeli accounts_konto
        if (res)
        {
            info(res[0]["id"].c_str());
            return atoi(res[0]["id"].c_str());
        }
        else
        {
            Eline2("Błąd zapytania do bazy danych: ",query.error());
            return -1;
        }
    }
    catch (const std::exception& e)//jezeli wystapil wyjatek w SQLu
    {
        info2("BŁAD PARSOWANIA SQL w getUserId : ",e.what());
        return -1;
    }
}
///Zapytanie o ID uzytkownika o loginie 'user' ale nie o id z auth_user tylko o id z accounts_konto
int Baza::getUserId(std::string user)
{
    try
    {
        info("pobranie id usera");
        char zapytanie[256];
        sprintf(zapytanie,"select id from auth_user where username='%s'",user.c_str());
        //Najlpierw zapytanie o ID usera z tabeli auth_user
        //std::string zapytanie="select id from auth_user where username='";
        //zapytanie.append(user);
        //zapytanie.append("'");
        info2("zaytanie: ",zapytanie);
        mysqlpp::Query query = conn.query(zapytanie);//Wyslanie zapytania do bazy
        info("tick");
        mysqlpp::StoreQueryResult res = query.store();//umieszczenie wynikow zapytania w zmiennej res
        info("tick2");
        //A potem zapytanie o id z tabeli accounts_konto
        if (res)
        {
            info("tick3");
            char zapytanie[256];
            info("tick4");
            sprintf(zapytanie,"select id from accounts_konto where user_id='%s'",res[0]["id"].c_str());
            //zapytanie="select id from accounts_konto where user_id='";
            //zapytanie.append(res[0]["id"]);
            //zapytanie.append("'");
            info(zapytanie);
            mysqlpp::Query query2 = conn.query(zapytanie);
            mysqlpp::StoreQueryResult res2 = query2.store();
            if (res2)
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
    catch (const std::exception& e)//jezeli wystapil wyjatek w SQLu
    {
        info2("BŁAD PARSOWANIA SQL w getUserId : ",e.what());
        return -1;
    }
}

mysqlpp::StoreQueryResult Baza::getFileInfo(std::string file, std::string user, char uprawnienia)
{
    try
    {
        info("pobranie info o pliku usera");
        int id=getUserId(user);
        if (id>=0)
        {
            info("id poprawne");
            return getFileInfo(file,id,uprawnienia);
        }
        else
        {
            info("ID NIEPOPRAWNE!");
            mysqlpp::StoreQueryResult res;
            return res;
        }
    }
    catch (const std::exception& e)//jezeli wystapil wyjatek w SQLu
    {
        info2("BŁAD PARSOWANIA SQL w getFileInfo : ",e.what());

        return (mysqlpp::StoreQueryResult());
    }
}
mysqlpp::StoreQueryResult Baza::getFileInfo(std::string file, int user_id,char uprawnienia)
{
    try
    {
#ifdef DEBUG
        char debug[1024];
        sprintf(debug,"pobranie info o pliku usera o ID = '%d'",user_id);
        info(debug);
        sprintf(debug,"Uprawnienia usera: %d",(unsigned int)(uprawnienia));
        info(debug);
#endif//DEBUG

        char zapytanie[256];
        sprintf(zapytanie,"select * from files_plik where konto_id='%d' AND sciezka='%s' AND prawaDostepu>='%d'",user_id,file.c_str(),(unsigned int)(uprawnienia));
        info2("zapytanie",zapytanie);
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
    catch (const std::exception& e)//jezeli wystapil wyjatek w SQLu
    {
        info2("BŁAD PARSOWANIA SQL w getFileInfo II : ",e.what());

        return (mysqlpp::StoreQueryResult());
    }

}

bool Baza::addFile(std::string nazwa, std::string konto, int wielkosc, std::string hash, int prawa, int data)
{
    try
    {
        info("dodanie pliku do bazy");
        //std::string zapytanie="SELECT * from files_plik";
        //int ilosc;
        info("pobieranie id");
        int id=getUserId(konto);
        info("id pobrane")
        #ifdef DEBUG
        char a[256];
        sprintf(a,"user %s o id %d umieszcza plik %s",konto.c_str(),id,nazwa.c_str());
        info(a);
        #endif//DEBUG
        /*mysqlpp::Query query = conn.query(zapytanie);
        mysqlpp::StoreQueryResult res = query.store();
        if (res)
        {
            ilosc=res.num_rows();
        }
        else
        {
            info("!res");
            return false;
        }
        info("po");*/
        mysqlpp::Query query = conn.query();
        char b[1024];
        sprintf(b,"INSERT INTO files_plik\
         (konto_id,sciezka,dataDodania,prawaDostepu,wielkosc,hashValue)\
         VALUES(%d,'%s',%d,%d,%d,'%s')",\
         id,nazwa.c_str(),data,prawa,wielkosc,hash.c_str());
        query<<b;
        query.execute();

        info2("zapytanie",b);
        info("po2");
        return true;
    }
    catch (const std::exception& e)//jezeli wystapil wyjatek w SQLu
    {
        info2("BŁAD PARSOWANIA SQL w addFile : ",e.what());
        return false;
    }

}

bool Baza::rmFile(std::string nazwa, std::string konto, std::string hash)
{
    try
    {
        info("usuwanie pliku z bazy");

        info("pobieranie id");
        int id=getUserId(konto);
        info("id pobrane")
        char a[256];
        sprintf(a,"user %s o id %d usuwa plik %s - hash %s",konto.c_str(),id,nazwa.c_str(),hash.c_str());
        info(a);

        mysqlpp::Query  query = conn.query();
        char b[1024];
        sprintf(b,"DELETE from files_plik WHERE hashValue='%s' AND konto_id='%d'",hash.c_str(),id);
        info(b);
        query<<b;
        query.execute();
        info2("wykonano",b);
        return true;
    }
    catch (const std::exception& e)//jezeli wystapil wyjatek w SQLu
    {
        info2("BŁAD PARSOWANIA SQL w addFile : ",e.what());
        return false;
    }
}

char Baza::friends(std::string user1, std::string user2)
{
    if (user1.compare(user2)==0)
        return 0;
    char a[256];
    int id1=getUserId(user1);
    int id2=getUserId(user2);
#ifdef DEBUG
    sprintf(a,"sprawdzam czy %d może oglądać pliki %d",id1,id2);
    info(a);
#endif
    sprintf(a,"select id from friends_userlink where from_user_id='%d' and to_user_id='%d'",id2,id2);
    info(a);
    mysqlpp::Query query = conn.query(a);
    mysqlpp::StoreQueryResult res = query.store();
    if (res)
    {
        if (res.num_rows()==1)
        {
            return 1;
        }
        else
            return 2;

    }
    else
    {
        info("!res");
        return 2;
    }
    return 2;
}
