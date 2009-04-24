#include "wx_pch.h"
#include "Konfiguracja.h"

#ifndef WX_PRECOMP
	//(*InternalHeadersPCH(Konfiguracja)
	#include <wx/intl.h>
	#include <wx/string.h>
	//*)
#endif
//(*InternalHeaders(Konfiguracja)
//*)

//(*IdInit(Konfiguracja)
const long Konfiguracja::ID_TEXTCTRL1 = wxNewId();
const long Konfiguracja::ID_PANEL1 = wxNewId();
//*)

BEGIN_EVENT_TABLE(Konfiguracja,wxFrame)
	//(*EventTable(Konfiguracja)
	//*)
END_EVENT_TABLE()

Konfiguracja::Konfiguracja(wxWindow* parent,wxWindowID id,const wxPoint& pos,const wxSize& size)
{
	//(*Initialize(Konfiguracja)
	wxBoxSizer* BoxSizer1;
	wxStaticBoxSizer* StaticBoxSizer1;
	wxFlexGridSizer* FlexGridSizer1;
	
	Create(parent, id, wxEmptyString, wxDefaultPosition, wxDefaultSize, wxDEFAULT_FRAME_STYLE, _T("id"));
	SetClientSize(wxDefaultSize);
	Move(wxDefaultPosition);
	BoxSizer1 = new wxBoxSizer(wxHORIZONTAL);
	Panel1 = new wxPanel(this, ID_PANEL1, wxDefaultPosition, wxDefaultSize, wxTAB_TRAVERSAL, _T("ID_PANEL1"));
	FlexGridSizer1 = new wxFlexGridSizer(0, 3, 0, 0);
	StaticBoxSizer1 = new wxStaticBoxSizer(wxHORIZONTAL, Panel1, _("Label"));
	TextCtrl1 = new wxTextCtrl(Panel1, ID_TEXTCTRL1, _("Text"), wxDefaultPosition, wxDefaultSize, 0, wxDefaultValidator, _T("ID_TEXTCTRL1"));
	StaticBoxSizer1->Add(TextCtrl1, 1, wxALL|wxALIGN_CENTER_HORIZONTAL|wxALIGN_CENTER_VERTICAL, 5);
	FlexGridSizer1->Add(StaticBoxSizer1, 1, wxALL|wxALIGN_CENTER_HORIZONTAL|wxALIGN_CENTER_VERTICAL, 5);
	Panel1->SetSizer(FlexGridSizer1);
	FlexGridSizer1->Fit(Panel1);
	FlexGridSizer1->SetSizeHints(Panel1);
	BoxSizer1->Add(Panel1, 1, wxALL|wxALIGN_CENTER_HORIZONTAL|wxALIGN_CENTER_VERTICAL, 5);
	SetSizer(BoxSizer1);
	BoxSizer1->Fit(this);
	BoxSizer1->SetSizeHints(this);
	//*)
}

Konfiguracja::~Konfiguracja()
{
	//(*Destroy(Konfiguracja)
	//*)
}

