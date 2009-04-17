/***************************************************************
 * Name:      ASS8_Klient_v1App.cpp
 * Purpose:   Code for Application Class
 * Author:    Paweł Zembrzuski ()
 * Created:   2009-04-17
 * Copyright: Paweł Zembrzuski ()
 * License:
 **************************************************************/

#include "wx_pch.h"
#include "ASS8_Klient_v1App.h"

//(*AppHeaders
#include "ASS8_Klient_v1Main.h"
#include <wx/image.h>
//*)

IMPLEMENT_APP(ASS8_Klient_v1App);

bool ASS8_Klient_v1App::OnInit()
{
    //(*AppInitialize
    bool wxsOK = true;
    wxInitAllImageHandlers();
    if ( wxsOK )
    {
    	ASS8_Klient_v1Frame* Frame = new ASS8_Klient_v1Frame(0);
    	Frame->Show();
    	SetTopWindow(Frame);
    }
    //*)
    return wxsOK;

}
