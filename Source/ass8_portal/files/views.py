#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
Plik odpowiedzialny za definicje widoków aplikacji 'files'.
    -  MY - jest to zmienna opisująca prawa dostępu jako 'osobiste' przypisana jest jej wartość 0
    -  PRIVATE - jest to zmienna opisująca prawa dostępu jako 'prywatne' przypisana jest jej wartość 1
    -  PUBLIC - jest to zmienna opisująca prawa dostępu jako 'publincze' przypisana jest jej wartość 2
"""
from django.shortcuts import render_to_response, get_object_or_404
from django.contrib.auth.decorators import login_required
from django.http import Http404, HttpResponseRedirect
from django.contrib.auth.models import User
from django.contrib.auth import login, logout, authenticate
from django.template import RequestContext
from accounts.models import Konto
from files.models import Plik
from accounts.views import Message, MESSAGE_CODES
from friends.models import UserLink

MY = 0
PRIVATE = 1
PUBLIC = 2

@login_required
def view(request, username):
    """
    Widok odpowiedzialny za wyświetlanie listy plików użytkownika. Aby funkcja została wywołana 
    użytkownik musi być zalogowany. Jeśli nie jest - zostanie przekierowany na stronę logowania, 
    a po poprawnym logowaniu ponownie na stronę z listą. Sprawdzane jest czy istnieje znajomość
    użytkownika który generuje żądanie z użytkownikiem którego żądanie dotyczy. Jeśli znajomość
    istnieje użytkownik może przglądać pliki oznaczone jako prywatne. Jeśli takiej znajomości nie ma
    to może oglądać tylko pliki oznaczone jako 'publiczne'.
    W sytuacji gdy użytkownik chce obejrzeć swój profil może obejrzeć swoje wsyzstkie pliki.
    
    @param request: żądanie przeglądarki
    @param username: login użytkownika którego listę plików chcemy obejrzeć.
    """
    requestKonto = Konto.objects.get(user = request.user)
    viewUser = get_object_or_404(User, username=username)
    viewKonto = Konto.objects.get(user = viewUser)
    privateFileList=None
    publicFileList=None
    ownFileList=None
    if requestKonto.user.username != viewKonto.user.username:
	try:
            ul = UserLink.objects.get(from_user = viewKonto, to_user =
                requestKonto)            
            privateFileList = Plik.objects.filter(konto = viewKonto,
                    prawaDostepu = PRIVATE)            
            publicFileList = Plik.objects.filter(konto = viewKonto, prawaDostepu =
                PUBLIC)
            
            return render_to_response('files/view.html',
                    {"requestKonto":requestKonto,
                        "viewKonto":viewKonto, 
                        "private":privateFileList,
                        "public":publicFileList,
                        "chmod":PRIVATE})
        except UserLink.DoesNotExist:
            fileList = Plik.objects.filter(konto = viewKonto, prawaDostepu = PUBLIC)
            return render_to_response('files/view.html',
                    {"requestKonto":requestKonto,
                        "viewKonto":viewKonto,
                        "public":fileList,
                        "chmod":PUBLIC})
    else:        
        privateFileList = Plik.objects.filter(konto = viewKonto, prawaDostepu =
                PRIVATE)        
        publicFileList = Plik.objects.filter(konto = viewKonto, prawaDostepu =
                PUBLIC)        
        ownFileList = Plik.objects.filter(konto = viewKonto, prawaDostepu = MY)
        
        return render_to_response('files/view.html',
                    {"requestKonto":requestKonto,
                        "viewKonto":viewKonto,
                        "private":privateFileList,
                        "public":publicFileList,
                        "own":ownFileList,
                        "chmod":MY})
                        
@login_required
def edit(request, username):
    """
    Widok odpowiedzialny za edycję listy plików użytkownika. Aby funkcja została wywołana 
    użytkownik musi być zalogowany. Jeśli nie jest - zostanie przekierowany na stronę logowania, 
    a po poprawnym logowaniu ponownie na stronę z listą. Sprawdzane jest czy użytkownik chce zmieniać
    swoją listę plików, jeśli nie wyświetlany jest odpowieni komunikat.
    
    @param request: żądanie przeglądarki
    @param username: login użytkownika którego listę plików chcemy zmienić
    """
    requestKonto = Konto.objects.get(user = request.user)
    viewUser = get_object_or_404(User, username=username)
    viewKonto = Konto.objects.get(user = viewUser)
    if requestKonto.user.username != viewKonto.user.username:
        msg = Message(2, "Mozesz zmieniac tylko swoje pliki!")
        return render_to_response("files/view.html", {"requestKonto":requestKonto,
        "viewKonto":viewKonto,"msg":msg})
    privateFileList = Plik.objects.filter(konto = viewKonto, prawaDostepu =
        PRIVATE)        
    publicFileList = Plik.objects.filter(konto = viewKonto, prawaDostepu =
        PUBLIC)        
    ownFileList = Plik.objects.filter(konto = viewKonto, prawaDostepu = MY)
    return render_to_response('files/edit.html',
            {"requestKonto":requestKonto,
                "viewKonto":viewKonto,
                "private":privateFileList,
                "public":publicFileList,
                "own":ownFileList,
                "chmod":MY})
    
    
   
@login_required
def to_public(request, id):
    """
    Widok odpowiedzialny za zmianę pliku na 'publiczny'. Aby funkcja została wywołana 
    użytkownik musi być zalogowany. Jeśli nie jest - zostanie przekierowany na stronę logowania, 
    a po poprawnym logowaniu ponownie na stronę z listą. Sprawdzane jest czy dany plik istnieje,
    jeśli nie wyświetlany jest odpowieni komunikat.
    
    @param request: żądanie przeglądarki
    @param id: id pliku który chcemy zmienić
    """
    requestKonto = Konto.objects.get(user = request.user)    
    try:
        f = Plik.objects.get(id = id, konto = requestKonto)
        f.prawaDostepu = PUBLIC
        f.save()
        site = "/files/edit/"+requestKonto.user.username
        return HttpResponseRedirect(site)
    except Plik.DoesNotExist:
        msg = Message(2, "Podany plik nie istnieje.")
        privateFileList = Plik.objects.filter(konto = requestKonto, prawaDostepu =
        PRIVATE)        
        publicFileList = Plik.objects.filter(konto = requestKonto, prawaDostepu =
            PUBLIC)        
        ownFileList = Plik.objects.filter(konto = requestKonto, prawaDostepu = MY)
        return render_to_response('files/edit.html',
            {"requestKonto":requestKonto,                
                "private":privateFileList,
                "public":publicFileList,
                "own":ownFileList,
                "chmod":MY,
                "msg":msg})


@login_required
def to_private(request, id):
    """
    Widok odpowiedzialny za zmianę pliku na 'prywatny'. Aby funkcja została wywołana 
    użytkownik musi być zalogowany. Jeśli nie jest - zostanie przekierowany na stronę logowania, 
    a po poprawnym logowaniu ponownie na stronę z listą. Sprawdzane jest czy dany plik istnieje,
    jeśli nie wyświetlany jest odpowieni komunikat.
    
    @param request: żądanie przeglądarki
    @param id: id pliku który chcemy zmienić
    """
    requestKonto = Konto.objects.get(user = request.user)    
    try:
        f = Plik.objects.get(id = id, konto = requestKonto)
        f.prawaDostepu = PRIVATE
        f.save()
        site = "/files/edit/"+requestKonto.user.username
        return HttpResponseRedirect(site)
    except Plik.DoesNotExist:
        msg = Message(2, "Podany plik nie istnieje.")
        privateFileList = Plik.objects.filter(konto = requestKonto, prawaDostepu =
        PRIVATE)        
        publicFileList = Plik.objects.filter(konto = requestKonto, prawaDostepu =
            PUBLIC)        
        ownFileList = Plik.objects.filter(konto = requestKonto, prawaDostepu = MY)
        return render_to_response('files/edit.html',
            {"requestKonto":requestKonto,                
                "private":privateFileList,
                "public":publicFileList,
                "own":ownFileList,
                "chmod":MY,
                "msg":msg})        
        
@login_required
def to_own(request, id):
    """
    Widok odpowiedzialny za zmianę pliku na 'osobisty'. Aby funkcja została wywołana 
    użytkownik musi być zalogowany. Jeśli nie jest - zostanie przekierowany na stronę logowania, 
    a po poprawnym logowaniu ponownie na stronę z listą. Sprawdzane jest czy dany plik istnieje,
    jeśli nie wyświetlany jest odpowieni komunikat.
    
    @param request: żądanie przeglądarki
    @param id: id pliku który chcemy zmienić
    """
    requestKonto = Konto.objects.get(user = request.user)    
    try:
        f = Plik.objects.get(id = id, konto = requestKonto)
        f.prawaDostepu = MY
        f.save()        
        site = "/files/edit/"+requestKonto.user.username
        return HttpResponseRedirect(site)
    except Plik.DoesNotExist:
        msg = Message(2, "Podany plik nie istnieje.")
        privateFileList = Plik.objects.filter(konto = requestKonto, prawaDostepu =
        PRIVATE)        
        publicFileList = Plik.objects.filter(konto = requestKonto, prawaDostepu =
            PUBLIC)        
        ownFileList = Plik.objects.filter(konto = requestKonto, prawaDostepu = MY)
        return render_to_response('files/edit.html',
            {"requestKonto":requestKonto,
                "private":privateFileList,
                "public":publicFileList,
                "own":ownFileList,
                "chmod":MY,
                "msg":msg})



@login_required
def delete(request, id):
    """
    Widok odpowiedzialny za usunięcie danego pliku. Aby funkcja została wywołana 
    użytkownik musi być zalogowany. Jeśli nie jest - zostanie przekierowany na stronę logowania, 
    a po poprawnym logowaniu ponownie na stronę z listą. Sprawdzane jest czy dany plik istnieje,
    jeśli nie wyświetlany jest odpowieni komunikat.
    
    @param request: żądanie przeglądarki
    @param id: id pliku który chcemy zmienić
    """
    requestKonto = Konto.objects.get(user = request.user)        
    try:
        f = Plik.objects.get(id = id, konto = requestKonto)        
        f.delete()
        site = "/files/edit/"+requestKonto.user.username
        return HttpResponseRedirect(site)
    except Plik.DoesNotExist:
        msg = Message(2, "Podany plik nie istnieje.")
        privateFileList = Plik.objects.filter(konto = requestKonto, prawaDostepu =
        PRIVATE)        
        publicFileList = Plik.objects.filter(konto = requestKonto, prawaDostepu =
            PUBLIC)        
        ownFileList = Plik.objects.filter(konto = requestKonto, prawaDostepu = MY)
        return render_to_response('files/edit.html',
            {"requestKonto":requestKonto,                
                "private":privateFileList,
                "public":publicFileList,
                "own":ownFileList,
                "chmod":MY,
                "msg":msg})        
