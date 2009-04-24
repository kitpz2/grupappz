/***************************************************************
 * Name:      wx_pch.h
 * Purpose:   Header to create Pre-Compiled Header (PCH)
 * Author:    Paweł Zembrzuski ()
 * Created:   2009-04-17
 * Copyright: Paweł Zembrzuski ()
 * License:   
 **************************************************************/

#ifndef WX_PCH_H_INCLUDED
#define WX_PCH_H_INCLUDED

// basic wxWidgets headers
#include <wx/wxprec.h>

#ifdef __BORLANDC__
    #pragma hdrstop
#endif

#ifndef WX_PRECOMP
    #include <wx/wx.h>
#endif

#ifdef WX_PRECOMP
    // put here all your rarely-changing header files
#endif // WX_PRECOMP

#endif // WX_PCH_H_INCLUDED
