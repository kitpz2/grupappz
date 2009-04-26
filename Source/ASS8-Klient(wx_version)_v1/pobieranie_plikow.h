#ifndef POBIERANIE_PLIKOW_H
#define POBIERANIE_PLIKOW_H

#ifndef WX_PRECOMP
	//(*HeadersPCH(pobieranie_plikow)
	#include <wx/sizer.h>
	#include <wx/textctrl.h>
	#include <wx/panel.h>
	#include <wx/frame.h>
	//*)
#endif
//(*Headers(pobieranie_plikow)
#include <wx/treectrl.h>
//*)

class pobieranie_plikow: public wxFrame
{
	public:

		pobieranie_plikow(wxWindow* parent=0,wxWindowID id=wxID_ANY,const wxPoint& pos=wxDefaultPosition,const wxSize& size=wxDefaultSize);
		virtual ~pobieranie_plikow();

		//(*Declarations(pobieranie_plikow)
		wxPanel* Panel1;
		wxTreeCtrl* TreeCtrl1;
		wxTextCtrl* TextCtrl1;
		//*)

	protected:

		//(*Identifiers(pobieranie_plikow)
		static const long ID_TEXTCTRL1;
		static const long ID_TREECTRL1;
		static const long ID_PANEL1;
		//*)

	private:

		//(*Handlers(pobieranie_plikow)
		//*)

		DECLARE_EVENT_TABLE()
};

#endif
