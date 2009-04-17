/***************************************************************
 * Name:      ASS8_Klient_v1Main.h
 * Purpose:   Defines Application Frame
 * Author:    Paweł Zembrzuski ()
 * Created:   2009-04-17
 * Copyright: Paweł Zembrzuski ()
 * License:
 **************************************************************/

#ifndef ASS8_KLIENT_V1MAIN_H
#define ASS8_KLIENT_V1MAIN_H

//(*Headers(ASS8_Klient_v1Frame)
#include <wx/sizer.h>
#include <wx/menu.h>
#include <wx/textctrl.h>
#include <wx/panel.h>
#include <wx/button.h>
#include <wx/frame.h>
#include <wx/statusbr.h>
//*)

class ASS8_Klient_v1Frame: public wxFrame
{
    public:

        ASS8_Klient_v1Frame(wxWindow* parent,wxWindowID id = -1);
        virtual ~ASS8_Klient_v1Frame();

    private:

        //(*Handlers(ASS8_Klient_v1Frame)
        void OnQuit(wxCommandEvent& event);
        void OnAbout(wxCommandEvent& event);
        //*)

        //(*Identifiers(ASS8_Klient_v1Frame)
        static const long ID_TEXTCTRL1;
        static const long ID_TEXTCTRL2;
        static const long ID_BUTTON1;
        static const long ID_PANEL1;
        static const long idMenuQuit;
        static const long idMenuAbout;
        static const long ID_STATUSBAR1;
        //*)

        //(*Declarations(ASS8_Klient_v1Frame)
        wxButton* Button1;
        wxPanel* Panel1;
        wxStatusBar* StatusBar1;
        wxTextCtrl* TextCtrl2;
        wxTextCtrl* TextCtrl1;
        //*)

        DECLARE_EVENT_TABLE()
};

#endif // ASS8_KLIENT_V1MAIN_H
