#include "wx_pch.h"
#include "NewFrame.h"

#ifndef WX_PRECOMP
	//(*InternalHeadersPCH(NewFrame)
	#include <wx/intl.h>
	#include <wx/string.h>
	//*)
#endif
//(*InternalHeaders(NewFrame)
#include <wx/bitmap.h>
#include <wx/button.h>
#include <wx/image.h>
//*)

//(*IdInit(NewFrame)
const long NewFrame::ID_TEXTCTRL1 = wxNewId();
const long NewFrame::ID_BITMAPBUTTON1 = wxNewId();
const long NewFrame::ID_STATICTEXT1 = wxNewId();
const long NewFrame::ID_SPINCTRL1 = wxNewId();
const long NewFrame::ID_STATICTEXT2 = wxNewId();
const long NewFrame::ID_SPINCTRL2 = wxNewId();
const long NewFrame::ID_STATICTEXT3 = wxNewId();
const long NewFrame::ID_SPINCTRL3 = wxNewId();
const long NewFrame::ID_PANEL1 = wxNewId();
//*)

BEGIN_EVENT_TABLE(NewFrame,wxFrame)
	//(*EventTable(NewFrame)
	//*)
END_EVENT_TABLE()

NewFrame::NewFrame(wxWindow* parent,wxWindowID id,const wxPoint& pos,const wxSize& size)
{
	//(*Initialize(NewFrame)
	wxStaticBoxSizer* StaticBoxSizer2;
	wxFlexGridSizer* FlexGridSizer4;
	wxFlexGridSizer* FlexGridSizer3;
	wxFlexGridSizer* FlexGridSizer2;
	wxStaticBoxSizer* StaticBoxSizer3;
	wxBoxSizer* BoxSizer1;
	wxStaticBoxSizer* StaticBoxSizer1;
	wxFlexGridSizer* FlexGridSizer1;
	wxStdDialogButtonSizer* StdDialogButtonSizer1;
	
	Create(parent, id, wxEmptyString, wxDefaultPosition, wxDefaultSize, wxSTAY_ON_TOP|wxCAPTION|wxCLOSE_BOX|wxMINIMIZE_BOX, _T("id"));
	SetClientSize(wxDefaultSize);
	Move(wxDefaultPosition);
	SetMinSize(wxSize(200,300));
	BoxSizer1 = new wxBoxSizer(wxHORIZONTAL);
	Panel1 = new wxPanel(this, ID_PANEL1, wxDefaultPosition, wxDefaultSize, wxTAB_TRAVERSAL, _T("ID_PANEL1"));
	FlexGridSizer1 = new wxFlexGridSizer(0, 1, 0, 0);
	FlexGridSizer1->AddGrowableCol(0);
	FlexGridSizer1->AddGrowableCol(1);
	FlexGridSizer1->AddGrowableCol(2);
	FlexGridSizer1->AddGrowableCol(3);
	StaticBoxSizer1 = new wxStaticBoxSizer(wxHORIZONTAL, Panel1, _("Synchronizowany katalog"));
	FlexGridSizer2 = new wxFlexGridSizer(0, 3, 0, 0);
	FlexGridSizer2->AddGrowableCol(0);
	FlexGridSizer2->AddGrowableCol(1);
	FlexGridSizer2->AddGrowableCol(2);
	FlexGridSizer2->AddGrowableCol(3);
	TextCtrl1 = new wxTextCtrl(Panel1, ID_TEXTCTRL1, wxEmptyString, wxDefaultPosition, wxDefaultSize, 0, wxDefaultValidator, _T("ID_TEXTCTRL1"));
	FlexGridSizer2->Add(TextCtrl1, 1, wxTOP|wxBOTTOM|wxLEFT|wxEXPAND|wxALIGN_CENTER_HORIZONTAL|wxALIGN_CENTER_VERTICAL, 5);
	BitmapButton1 = new wxBitmapButton(Panel1, ID_BITMAPBUTTON1, wxBitmap(wxImage(_T("/home/pawel/Dokumenty/Uczelnia/grupappz/Source/ASS8-Klient_v1/bitmaps/open.bmp"))), wxDefaultPosition, wxSize(35,35), wxBU_AUTODRAW, wxDefaultValidator, _T("ID_BITMAPBUTTON1"));
	BitmapButton1->SetMaxSize(wxSize(35,35));
	FlexGridSizer2->Add(BitmapButton1, 1, wxTOP|wxBOTTOM|wxRIGHT|wxSHAPED|wxALIGN_RIGHT|wxALIGN_CENTER_VERTICAL, 5);
	StaticBoxSizer1->Add(FlexGridSizer2, 1, wxALL|wxEXPAND|wxALIGN_CENTER_HORIZONTAL|wxALIGN_CENTER_VERTICAL, 5);
	FlexGridSizer1->Add(StaticBoxSizer1, 1, wxALL|wxEXPAND|wxALIGN_CENTER_HORIZONTAL|wxALIGN_CENTER_VERTICAL, 5);
	StaticBoxSizer2 = new wxStaticBoxSizer(wxHORIZONTAL, Panel1, _("Ograniczenia Pobierania i wysłania (kB/s)"));
	FlexGridSizer3 = new wxFlexGridSizer(0, 2, 0, 0);
	StaticText1 = new wxStaticText(Panel1, ID_STATICTEXT1, _("Maksymalna prędkość Uploadu (0 - nieogranoiczona)"), wxDefaultPosition, wxDefaultSize, 0, _T("ID_STATICTEXT1"));
	FlexGridSizer3->Add(StaticText1, 1, wxALL|wxALIGN_CENTER_HORIZONTAL|wxALIGN_CENTER_VERTICAL, 5);
	SpinCtrl1 = new wxSpinCtrl(Panel1, ID_SPINCTRL1, _T("0"), wxDefaultPosition, wxDefaultSize, 0, 0, 100, 0, _T("ID_SPINCTRL1"));
	SpinCtrl1->SetValue(_T("0"));
	FlexGridSizer3->Add(SpinCtrl1, 1, wxALL|wxALIGN_CENTER_HORIZONTAL|wxALIGN_CENTER_VERTICAL, 5);
	StaticText2 = new wxStaticText(Panel1, ID_STATICTEXT2, _("Maksymalna prędkość Downloadu (0 - nieograniczona)"), wxDefaultPosition, wxDefaultSize, 0, _T("ID_STATICTEXT2"));
	FlexGridSizer3->Add(StaticText2, 1, wxALL|wxALIGN_CENTER_HORIZONTAL|wxALIGN_CENTER_VERTICAL, 5);
	SpinCtrl2 = new wxSpinCtrl(Panel1, ID_SPINCTRL2, _T("0"), wxDefaultPosition, wxDefaultSize, 0, 0, 100, 0, _T("ID_SPINCTRL2"));
	SpinCtrl2->SetValue(_T("0"));
	FlexGridSizer3->Add(SpinCtrl2, 1, wxALL|wxALIGN_CENTER_HORIZONTAL|wxALIGN_CENTER_VERTICAL, 5);
	StaticBoxSizer2->Add(FlexGridSizer3, 1, wxALL|wxALIGN_CENTER_HORIZONTAL|wxALIGN_CENTER_VERTICAL, 5);
	FlexGridSizer1->Add(StaticBoxSizer2, 1, wxALL|wxALIGN_CENTER_HORIZONTAL|wxALIGN_CENTER_VERTICAL, 5);
	StaticBoxSizer3 = new wxStaticBoxSizer(wxHORIZONTAL, Panel1, _("Częśtotliwość aktualizacji"));
	FlexGridSizer4 = new wxFlexGridSizer(0, 1, 0, 0);
	StaticText3 = new wxStaticText(Panel1, ID_STATICTEXT3, _("Co ile czasu ma być sprawdzana aktualnosć plików na serwerze i lokalnie (s)"), wxDefaultPosition, wxDefaultSize, 0, _T("ID_STATICTEXT3"));
	FlexGridSizer4->Add(StaticText3, 1, wxALL|wxALIGN_CENTER_HORIZONTAL|wxALIGN_CENTER_VERTICAL, 5);
	SpinCtrl3 = new wxSpinCtrl(Panel1, ID_SPINCTRL3, _T("0"), wxDefaultPosition, wxDefaultSize, 0, 0, 100, 0, _T("ID_SPINCTRL3"));
	SpinCtrl3->SetValue(_T("0"));
	FlexGridSizer4->Add(SpinCtrl3, 1, wxALL|wxALIGN_CENTER_HORIZONTAL|wxALIGN_CENTER_VERTICAL, 5);
	StaticBoxSizer3->Add(FlexGridSizer4, 1, wxALL|wxALIGN_CENTER_HORIZONTAL|wxALIGN_CENTER_VERTICAL, 5);
	FlexGridSizer1->Add(StaticBoxSizer3, 1, wxALL|wxALIGN_CENTER_HORIZONTAL|wxALIGN_CENTER_VERTICAL, 5);
	StdDialogButtonSizer1 = new wxStdDialogButtonSizer();
	StdDialogButtonSizer1->AddButton(new wxButton(Panel1, wxID_OK, wxEmptyString));
	StdDialogButtonSizer1->AddButton(new wxButton(Panel1, wxID_CANCEL, wxEmptyString));
	StdDialogButtonSizer1->Realize();
	FlexGridSizer1->Add(StdDialogButtonSizer1, 1, wxALL|wxALIGN_CENTER_HORIZONTAL|wxALIGN_CENTER_VERTICAL, 5);
	Panel1->SetSizer(FlexGridSizer1);
	FlexGridSizer1->Fit(Panel1);
	FlexGridSizer1->SetSizeHints(Panel1);
	BoxSizer1->Add(Panel1, 1, wxALL|wxEXPAND|wxALIGN_CENTER_HORIZONTAL|wxALIGN_CENTER_VERTICAL, 5);
	SetSizer(BoxSizer1);
	DirDialog1 = new wxDirDialog(this, _("Select directory"), wxEmptyString, wxDD_DEFAULT_STYLE, wxDefaultPosition, wxDefaultSize, _T("wxDirDialog"));
	BoxSizer1->Fit(this);
	BoxSizer1->SetSizeHints(this);
	//*)
}

NewFrame::~NewFrame()
{
	//(*Destroy(NewFrame)
	//*)
}

