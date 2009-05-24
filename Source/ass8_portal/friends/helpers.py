#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
Plik zawiera definicję funkcji pomocniczych.
"""

from accounts.models import Konto
from friends.models import UserLink

def get_my_followers(user):
    """
    Funkcja odpowiada za pobranie listy użytkowników którym dany użytkownik udostępnia pliki.
    
    @param user: konto użytkownika dla którego chcemy pobrać listę
    @return lista użytkowników którym dany użytkownik udostępnia pliki
    """
    ul = UserLink.objects.filter(from_user =
            user).values('to_user').order_by('-date_added')
    return Konto.objects.filter(id__in=[i['to_user'] for i in ul])


#znajomosc: ktos udostepnia mi
def get_my_following(user):
    """
    Funkcja odpowiada za pobranie listy użytkowników którzy udostępniają pliki danemu użytkownikowi.
    
    @param user: konto użytkownika dla którego chcemy pobrać listę
    @return lista użytkowników którzy udostępniają pliki danemu użytkownikowi
    """
    ul = UserLink.objects.filter(to_user =
            user).values('from_user').order_by('-date_added')
    return Konto.objects.filter(id__in=[i['from_user'] for i in ul])

#znajomosc w dwie strony
def get_mutual(user):
    """
    Funkcja odpowiada za pobranie listy znajomości w dwie strony dla danego użytkownika. 
    
    
    @param user: konto użytkownika dla którego chcemy pobrać listę
    @return lista znajomości w dwie strony dla danego użytkownika
    """
    followers = UserLink.objects.filter(from_user =
            user).values('to_user').order_by('-date_added')
    following = UserLink.objects.filter(to_user =
            user).values('from_user').order_by('-date_added')
    followers_set = set([i['to_user'] for i  in followers])
    following_set = set([i['from_user'] for i in following])
    return Konto.objects.filter(id__in=followers_set.intersection(following_set))
