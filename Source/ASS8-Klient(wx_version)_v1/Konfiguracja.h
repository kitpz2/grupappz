#ifndef KONFIGURACJA_H
#define KONFIGURACJA_H

#ifndef WX_PRECOMP
	//(*HeadersPCH(Konfiguracja)
	#include <wx/sizer.h>
	#include <wx/textctrl.h>
	#include <wx/panel.h>
	#include <wx/frame.h>
	//*)
#endif
//(*Headers(Konfiguracja)
//*)

class Konfiguracja: public wxFrame
{
	public:

		Konfiguracja(wxWindow* parent=0,wxWindowID id=wxID_ANY,const wxPoint& pos=wxDefaultPosition,const wxSize& size=wxDefaultSize);
		virtual ~Konfiguracja();

		//(*Declarations(Konfiguracja)
		wxPanel* Panel1;
		wxTextCtrl* TextCtrl1;
		//*)

	protected:

		//(*Identifiers(Konfiguracja)
		static const long ID_TEXTCTRL1;
		static const long ID_PANEL1;
		//*)

	private:

		//(*Handlers(Konfiguracja)
		//*)

		DECLARE_EVENT_TABLE()
};

#endif
