#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
Plik zawiera ustawienia adresów URL dla aplikacji 'files', 
które użytkownik może wpisywać i skojarzone z nimi widoki które zostaną wywołane przy 
danym adresie URL. Zmienna urlpatterns opisuje 
adresy i widoki w postaci: 
    wyrażenie regularne spełniane przez URL, widok, dodatkowe argumenty.
"""
from django.conf.urls.defaults import *

urlpatterns = patterns('files.views',
    url(r'details/(?P<username>[\w-]+)/$','view'),
    url(r'edit/(?P<username>[\w-]+)/$','edit'),
    url(r'chmod/(?P<id>[\d]+)/own/$','to_own'),
    url(r'chmod/(?P<id>[\d]+)/private/$','to_private'),
    url(r'chmod/(?P<id>[\d]+)/public/$','to_public'),
    url(r'delete/(?P<id>[\d]+)/$','delete'),
)
