#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
Plik zawiera ustawienia adresów URL dla aplikacji 'firends', 
które użytkownik może wpisywać i skojarzone z nimi widoki które zostaną wywołane przy 
danym adresie URL. Zmienna urlpatterns opisuje 
adresy i widoki w postaci: 
    wyrażenie regularne spełniane przez URL, widok, dodatkowe argumenty.
"""
from django.conf.urls.defaults import *

urlpatterns = patterns('friends.views',
    url(r'index/(?P<username>[\w-]+)/$','friends_manage'),
    url(r'followers/(?P<username>[\w-]+)/$','friend_list',{'list_type':'followers'}),
    url(r'following/(?P<username>[\w-]+)/$','friend_list',{'list_type':'following'}),
    url(r'mutual/(?P<username>[\w-]+)/$','friend_list',{'list_type':'mutual'}),
    url(r'add/(?P<username>[\w-]+)/$','add_friend'),
    url(r'delete/(?P<username>[\w-]+)/$','del_friend'),
)
