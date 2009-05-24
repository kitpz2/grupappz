#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
Plik zawiera globalne ustawienia adresów URL, które użytkownik może wpisywać i skojarzone z nimi
widoki które zostaną wywołane przy danym adresie URL. Zmienna urlpatterns opisuje 
adresy i widoki w postaci: 
    wyrażenie regularne spełniane przez URL, widok, dodatkowe argumenty.
"""

from django.conf.urls.defaults import *
from django.contrib import admin
admin.autodiscover()

urlpatterns = patterns('',
    (r'^css/(?P<path>.*)$', 'django.views.static.serve', {'document_root': 'media/css'}),
    (r'^js/(?P<path>.*)$', 'django.views.static.serve', {'document_root': 'media/js'}),
    (r'^gfx/(?P<path>.*)$','django.views.static.serve', {'document_root': 'media/gfx'}),
    (r'^$', 'accounts.views.index'),
    (r'^$/', 'accounts.views.index'),
    (r'^admin/(.*)', admin.site.root),
    (r'^accounts/', include('accounts.urls')),
    (r'^friends/',include('friends.urls')),
    (r'^files/',include('files.urls')),
)