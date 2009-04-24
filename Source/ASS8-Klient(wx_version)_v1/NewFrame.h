#ifndef NEWFRAME_H
#define NEWFRAME_H

#ifndef WX_PRECOMP
	//(*HeadersPCH(NewFrame)
	#include <wx/sizer.h>
	#include <wx/stattext.h>
	#include <wx/textctrl.h>
	#include <wx/panel.h>
	#include <wx/bmpbuttn.h>
	#include <wx/dirdlg.h>
	#include <wx/frame.h>
	//*)
#endif
//(*Headers(NewFrame)
#include <wx/spinctrl.h>
//*)

class NewFrame: public wxFrame
{
	public:

		NewFrame(wxWindow* parent=0,wxWindowID id=wxID_ANY,const wxPoint& pos=wxDefaultPosition,const wxSize& size=wxDefaultSize);
		virtual ~NewFrame();

		//(*Declarations(NewFrame)
		wxSpinCtrl* SpinCtrl1;
		wxStaticText* StaticText2;
		wxPanel* Panel1;
		wxStaticText* StaticText1;
		wxStaticText* StaticText3;
		wxBitmapButton* BitmapButton1;
		wxSpinCtrl* SpinCtrl3;
		wxDirDialog* DirDialog1;
		wxSpinCtrl* SpinCtrl2;
		wxTextCtrl* TextCtrl1;
		//*)

	protected:

		//(*Identifiers(NewFrame)
		static const long ID_TEXTCTRL1;
		static const long ID_BITMAPBUTTON1;
		static const long ID_STATICTEXT1;
		static const long ID_SPINCTRL1;
		static const long ID_STATICTEXT2;
		static const long ID_SPINCTRL2;
		static const long ID_STATICTEXT3;
		static const long ID_SPINCTRL3;
		static const long ID_PANEL1;
		//*)

	private:

		//(*Handlers(NewFrame)
		//*)

		DECLARE_EVENT_TABLE()
};

#endif
