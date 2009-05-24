#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
ASS.8 - Portal to aplikacja internetowa oparta o framework django. Framework ten spełnia nieco 
zmodyfikowany paradygmat tworzenia aplikacji MVC Model View Controler. W przypadku django możemy 
mówić o MTV - Model Template View. Gdzie:
    - Model - to część aplikacji odpowiedzilna za modelowanie danych w bazie. Praca z modelami
    nie wymaga znajomości SQL, a jedynie wewnętrznych funkcji frameworka.
    - Template - to szablon strony którą widzi użytkowniki. System szablonów poza znacznikami HTML
    udostępnia też wewnętrzne znaczniki która udostępniają prostą nie liniowość w postacji warunków czy pętli.
    - View - to ta część aplikacji która odpowiada za operacje na bazie danych, ich modyfikacje i
    przekazanie w odpowieniej formie do szablonu.
    
ASS.8 składa się z djangowego projektu który scala w sobie 3 współdziałające aplikacje:
    - accounts - aplikacja odpowiedzialna za zarządzanie kontami użytkownika
    - files - aplikacja odpowiedzialna za zarządzanie plikami użytkownika
    - friends - aplikacja odpowiedzialna za zarządzanie znajomościami użytkownika
"""