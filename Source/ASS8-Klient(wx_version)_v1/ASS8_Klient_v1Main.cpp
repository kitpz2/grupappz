/***************************************************************
 * Name:      ASS8_Klient_v1Main.cpp
 * Purpose:   Code for Application Frame
 * Author:    Paweł Zembrzuski ()
 * Created:   2009-04-17
 * Copyright: Paweł Zembrzuski ()
 * License:
 **************************************************************/

#include "wx_pch.h"
#include "ASS8_Klient_v1Main.h"
#include <wx/msgdlg.h>

//(*InternalHeaders(ASS8_Klient_v1Frame)
#include <wx/intl.h>
#include <wx/string.h>
//*)

//helper functions
enum wxbuildinfoformat {
    short_f, long_f };

wxString wxbuildinfo(wxbuildinfoformat format)
{
    wxString wxbuild(wxVERSION_STRING);

    if (format == long_f )
    {
#if defined(__WXMSW__)
        wxbuild << _T("-Windows");
#elif defined(__UNIX__)
        wxbuild << _T("-Linux");
#endif

#if wxUSE_UNICODE
        wxbuild << _T("-Unicode build");
#else
        wxbuild << _T("-ANSI build");
#endif // wxUSE_UNICODE
    }

    return wxbuild;
}

//(*IdInit(ASS8_Klient_v1Frame)
const long ASS8_Klient_v1Frame::ID_TEXTCTRL1 = wxNewId();
const long ASS8_Klient_v1Frame::ID_TEXTCTRL2 = wxNewId();
const long ASS8_Klient_v1Frame::ID_BUTTON1 = wxNewId();
const long ASS8_Klient_v1Frame::ID_PANEL1 = wxNewId();
const long ASS8_Klient_v1Frame::idMenuQuit = wxNewId();
const long ASS8_Klient_v1Frame::idMenuAbout = wxNewId();
const long ASS8_Klient_v1Frame::ID_STATUSBAR1 = wxNewId();
//*)

BEGIN_EVENT_TABLE(ASS8_Klient_v1Frame,wxFrame)
    //(*EventTable(ASS8_Klient_v1Frame)
    //*)
END_EVENT_TABLE()

ASS8_Klient_v1Frame::ASS8_Klient_v1Frame(wxWindow* parent,wxWindowID id)
{
    //(*Initialize(ASS8_Klient_v1Frame)
    wxStaticBoxSizer* StaticBoxSizer2;
    wxMenuItem* MenuItem2;
    wxMenuItem* MenuItem1;
    wxMenu* Menu1;
    wxBoxSizer* BoxSizer1;
    wxMenuBar* MenuBar1;
    wxStaticBoxSizer* StaticBoxSizer1;
    wxFlexGridSizer* FlexGridSizer1;
    wxMenu* Menu2;
    
    Create(parent, id, wxEmptyString, wxDefaultPosition, wxDefaultSize, wxSTAY_ON_TOP|wxCAPTION|wxCLOSE_BOX, _T("id"));
    BoxSizer1 = new wxBoxSizer(wxHORIZONTAL);
    Panel1 = new wxPanel(this, ID_PANEL1, wxDefaultPosition, wxDefaultSize, wxTAB_TRAVERSAL, _T("ID_PANEL1"));
    FlexGridSizer1 = new wxFlexGridSizer(0, 1, 0, 0);
    FlexGridSizer1->AddGrowableCol(0);
    FlexGridSizer1->AddGrowableCol(1);
    FlexGridSizer1->AddGrowableCol(2);
    FlexGridSizer1->AddGrowableCol(3);
    FlexGridSizer1->AddGrowableRow(0);
    FlexGridSizer1->AddGrowableRow(1);
    FlexGridSizer1->AddGrowableRow(2);
    FlexGridSizer1->AddGrowableRow(3);
    StaticBoxSizer1 = new wxStaticBoxSizer(wxHORIZONTAL, Panel1, _("Login"));
    TextCtrl1 = new wxTextCtrl(Panel1, ID_TEXTCTRL1, wxEmptyString, wxDefaultPosition, wxDefaultSize, 0, wxDefaultValidator, _T("ID_TEXTCTRL1"));
    StaticBoxSizer1->Add(TextCtrl1, 1, wxALL|wxEXPAND|wxALIGN_CENTER_HORIZONTAL|wxALIGN_CENTER_VERTICAL, 5);
    FlexGridSizer1->Add(StaticBoxSizer1, 1, wxALL|wxEXPAND|wxALIGN_CENTER_HORIZONTAL|wxALIGN_CENTER_VERTICAL, 5);
    StaticBoxSizer2 = new wxStaticBoxSizer(wxHORIZONTAL, Panel1, _("Hasło"));
    TextCtrl2 = new wxTextCtrl(Panel1, ID_TEXTCTRL2, wxEmptyString, wxDefaultPosition, wxDefaultSize, wxTE_PASSWORD, wxDefaultValidator, _T("ID_TEXTCTRL2"));
    StaticBoxSizer2->Add(TextCtrl2, 1, wxALL|wxEXPAND|wxALIGN_CENTER_HORIZONTAL|wxALIGN_CENTER_VERTICAL, 5);
    FlexGridSizer1->Add(StaticBoxSizer2, 1, wxALL|wxEXPAND|wxALIGN_CENTER_HORIZONTAL|wxALIGN_CENTER_VERTICAL, 5);
    Button1 = new wxButton(Panel1, ID_BUTTON1, _("Zaloguj"), wxDefaultPosition, wxDefaultSize, 0, wxDefaultValidator, _T("ID_BUTTON1"));
    FlexGridSizer1->Add(Button1, 1, wxALL|wxALIGN_CENTER_HORIZONTAL|wxALIGN_CENTER_VERTICAL, 5);
    Panel1->SetSizer(FlexGridSizer1);
    FlexGridSizer1->Fit(Panel1);
    FlexGridSizer1->SetSizeHints(Panel1);
    BoxSizer1->Add(Panel1, 1, wxALL|wxEXPAND|wxALIGN_CENTER_HORIZONTAL|wxALIGN_CENTER_VERTICAL, 5);
    SetSizer(BoxSizer1);
    MenuBar1 = new wxMenuBar();
    Menu1 = new wxMenu();
    MenuItem1 = new wxMenuItem(Menu1, idMenuQuit, _("Quit\tAlt-F4"), _("Quit the application"), wxITEM_NORMAL);
    Menu1->Append(MenuItem1);
    MenuBar1->Append(Menu1, _("&File"));
    Menu2 = new wxMenu();
    MenuItem2 = new wxMenuItem(Menu2, idMenuAbout, _("About\tF1"), _("Show info about this application"), wxITEM_NORMAL);
    Menu2->Append(MenuItem2);
    MenuBar1->Append(Menu2, _("Help"));
    SetMenuBar(MenuBar1);
    StatusBar1 = new wxStatusBar(this, ID_STATUSBAR1, 0, _T("ID_STATUSBAR1"));
    int __wxStatusBarWidths_1[1] = { -1 };
    int __wxStatusBarStyles_1[1] = { wxSB_NORMAL };
    StatusBar1->SetFieldsCount(1,__wxStatusBarWidths_1);
    StatusBar1->SetStatusStyles(1,__wxStatusBarStyles_1);
    SetStatusBar(StatusBar1);
    BoxSizer1->Fit(this);
    BoxSizer1->SetSizeHints(this);
    
    Connect(idMenuQuit,wxEVT_COMMAND_MENU_SELECTED,(wxObjectEventFunction)&ASS8_Klient_v1Frame::OnQuit);
    Connect(idMenuAbout,wxEVT_COMMAND_MENU_SELECTED,(wxObjectEventFunction)&ASS8_Klient_v1Frame::OnAbout);
    //*)
}

ASS8_Klient_v1Frame::~ASS8_Klient_v1Frame()
{
    //(*Destroy(ASS8_Klient_v1Frame)
    //*)
}

void ASS8_Klient_v1Frame::OnQuit(wxCommandEvent& event)
{
    Close();
}

void ASS8_Klient_v1Frame::OnAbout(wxCommandEvent& event)
{
    wxString msg = wxbuildinfo(long_f);
    wxMessageBox(msg, _("Welcome to..."));
}
