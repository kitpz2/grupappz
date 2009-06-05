#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
Plik odpowiedzialny za definicje widoków aplikacji 'accounts'. Aplikacja ta z kolei odpowiada 
za funkcjonalość związaną z zarządzaniem kontem użytkownika.
    -  MESSAGE_CODES - jest to lista przypisująca kodom 0, 1, 2 odpowiednie wiadomości o błędzie
"""
from django.shortcuts import render_to_response, get_object_or_404
from django.contrib.auth.decorators import login_required
from django.http import Http404, HttpResponseRedirect
from django.contrib.auth.models import User
from django.contrib.auth import login, logout, authenticate
from django.template import RequestContext
from accounts.models import Konto
from accounts.forms import *
from friends.models import UserLink 

MESSAGE_CODES = ('Warning', 'Information', 'Error')
 
class Message(object):
    """
    Klasa która odpowiedzialna jest za przechowywanie typu i treści komunikatów wysylanych do
    użytkownika. Typem jest wartość z listy MESSAGE_CODES, treścią dowolny komunikat który chcemy 
    wyświeltlić. W zależności od podanego typu komunikatu wiadomość zostanie wyświetlona w innym 
    kolorze, aby na pierwszy rzut oka użytkownik wiedział czy dana informacja jest błędem czy
    potwierdzeniem wykonania danej operacji.
    """
    def __init__(self, type, content):
        """Konstruktor wiadomości.
        
        @param type: typ wiadomości - liczba z zakresu 0 - 2
        @param content: treść wiadomości którą chcemy wyświetlić        
        """
        self.type = MESSAGE_CODES[type]
        self.content = content
 
    def __unicode__(self):
        """
        Metoda odpowiedzialna za ewentualne wypisanie wiadomości w postaci innej niż HTML(np. na ekran terminala).        
        """
        return "%s : %s" % self.type, self.content
 
def user_login(request):            
    """
    Widok odpowiedzialny za logowanie użytkownika. 
    Sprawdzane jest czy użytkownik nie jest juz zalogowany, jeśli nie to sprawdzane jest jego 
    login i hasło. W przypadku poprawnych danych użytkownik jest przekierowywany na stronę swojego 
    profilu, natomiast w przypadku błędu wyświetlany jest odpowiedni komunikat.
    
    @param request: żądanie przeglądarki
    """
    msg=None    
    try:
        requestUser = User.objects.get(username = request.user.username)
        requestKonto = Konto.objects.get(user = requestUser) 
    except User.DoesNotExist:
        requestKonto = None           
    except Konto.DoesNotExist:
	requestKonto = None
    if request.POST:        
        username = request.POST['login']
        password = request.POST['password']        
        user = authenticate(username=username, password=password)
        if user:           
	    try:
		konto = Konto.objects.get(user = user)
	    except Konto.DoesNotExist:
		 konto = Konto(user = user, plec ="M")
		 konto.save()
            login(request, user)
            return HttpResponseRedirect(request.GET.get("next") or
                    "/accounts/details/"+username+"/")        
        msg = Message(2,"Zly login lub haslo")
    f = LoginForm()
    return render_to_response("accounts/logowanie.html", {
        "form":f,
        "msg": msg,
        "requestKonto":requestKonto,
        })
 
def user_logout(request):
    """
    Widok odpowiedzialny za wylogowanie użytkownika. 
    Sprawdzane jest czy użytkownik jest zalogowany, jeśli tak następuje jego wylogowanie i 
    przekierowanie na stronę główną.
    
    @param request: żądanie przeglądarki
    """
    if request.user.is_authenticated:
        logout(request)
    return HttpResponseRedirect("/")
 
def index(request):   
    """
    Widok odpowiedzialny za wyświetlanie głównej strony portalu. 
    Przekazywana jest informacja o tym czy użytkownik przeglądający stronę jest zalogowany czy nie.
    
    @param request: żądanie przeglądarki
    """
    try:
        requestUser = User.objects.get(username = request.user.username)
        requestKonto = Konto.objects.get(user = requestUser) 
    except User.DoesNotExist:
        requestKonto = None           
    except Konto.DoesNotExist:
        requestKonto = None  
    return render_to_response("index.html",{"requestKonto":requestKonto})
 
 
def register(request):
    """
    Widok odpowiedzialny za rejestrację nowego użytkownika. 
    Generowany jest formularz a przy próbie wysłania sprawdzane są podane w nim dane. 
    Najważniejszą z nich jest sprawdzenie czy podany przez użytkownika login jest unikalny.
    
    @param request: żądanie przeglądarki
    """
    try:
        requestUser = User.objects.get(username = request.user.username)
        requestKonto = Konto.objects.get(user = requestUser) 
    except User.DoesNotExist:
        requestKonto = None    
    except Konto.DoesNotExist:
        requestKonto = None         
    if request.method =="POST":
        f = RegisterForm(request.POST)
        if not f.is_valid():
            msg = Message(2,"Wypelnij wszystkie pola!")
            return render_to_response("accounts/register.html", {"form":f, "msg":msg})
        else:
            if not request.POST["haslo"] == request.POST["re_haslo"]:
                msg = Message(2,"Hasla musza byc takie same!")
                return render_to_response("accounts/register.html", {"form":f, "msg":msg})
            try:
                user = User.objects.get(username=request.POST["login"])
                msg = Message(2,"Uzytkownik o podanym loginie juz istnieje")
                return render_to_response("accounts/register.html", {"form":f, "msg":msg})
            except User.DoesNotExist:
                user = User(username = request.POST["login"], 
                            password = request.POST["haslo"], 
                            email=request.POST["e_mail"],
                            )
                user.set_password(request.POST["haslo"])
                user.save()
                konto = Konto(user = user, plec = request.POST["plec"], plain_pass = request.POST["haslo"])
                konto.save()
                login(request,authenticate(username=request.POST["login"], password=request.POST["haslo"]))                 
                msg = Message(1,"Rejestracja zakonczona sukcesem")                
                return render_to_response("accounts/detail.html",{"msg":msg, "requestKonto":konto, "viewKonto":konto})                
    f = RegisterForm()
    return render_to_response("accounts/register.html", {"form":f})
 
 
@login_required
def profile_view(request, username):
    """
    Widok odpowiedzialny za wyświetlanie profliu użytkownika. Aby funkcja została wywołana 
    użytkownik musi być zalogowany. Jeśli nie jest - zostanie przekierowany na stronę logowania, 
    a po poprawnym logowaniu na stronę profilu użytkownika którą chciał obejrzeć wcześniej. 
    W sytuacji gdy użytkownik chce obejrzeć swój profil informacja ta jest odpowienio interpretowana i wpływa na wygląd strony.
    
    @param request: żądanie przeglądarki
    @param username: login użytkownika którego profil chcemy obejrzeć
    """
    requestKonto = Konto.objects.get(user = request.user)
    viewUser = get_object_or_404(User, username=username)
    viewKonto = Konto.objects.get(user=viewUser)
    add = True
    if requestKonto.user.username != viewKonto.user.username:
        try:
            ul = UserLink.objects.get(from_user=requestKonto,
                to_user = viewKonto)
            add = False
        except UserLink.DoesNotExist:
            add = True
    context = {
        'requestKonto':requestKonto,
        'viewKonto': viewKonto,
        'add':add,
    }
    return render_to_response("accounts/detail.html", context)
    
def latest_users(request):
    """
    Widok odpowiedzialny za wyświetlanie listy ostatnio zarejestrowanych użytkowników.
    
    @param request: żądanie przeglądarki    
    """
    try:
        requestUser = User.objects.get(username = request.user.username)
        requestKonto = Konto.objects.get(user = requestUser) 
    except User.DoesNotExist:
        requestKonto = None    
    except Konto.DoesNotExist:
        requestKonto = None
    latestUsers = []
    tempUsers = User.objects.all().order_by('-date_joined')[:25]        
    try:        
        for u in tempUsers:
            k = Konto.objects.get(user=u)
            latestUsers.append(k)
    except Konto.DoesNotExist:
        pass
    return render_to_response("accounts/latestUsers.html",
            {"requestKonto":requestKonto,"latestUsers":latestUsers})
 
@login_required
def search(request):   
    """
    Widok odpowiedzialny za wyszukiwanie użytkowników. Aby funkcja została wywołana 
    użytkownik musi być zalogowany. Jeśli nie jest - zostanie przekierowany na stronę logowania, 
    a po poprawnym logowaniu ponownie na stronę wyszukiwarki. Wyszukiwanie odbywa się poprzez 
    porównanie porównanie tekstu wprowadzonego przez użytkownika z loginami użytkowników w bazie.    
    
    @param request: żądanie przeglądarki    
    """    
    requestKonto = Konto.objects.get(user=request.user)    
    if request.POST:
        f = SearchForm(request.POST)
        if f.is_valid:
            accounts=[]
            userList = User.objects.filter(username__contains =
                    request.POST["search"])
            for u in userList:
                try:
                    k = Konto.objects.get(user=u)
                    accounts.append(k)                
                except Konto.DoesNotExist:
                    pass
            count =len(accounts)
            if count == 1:
                msg = Message(1, "Znaleziono 1 uzytkownika")
            else:
                msg = Message(1,"Znaleziono "+repr(count)+" uzytkownikow")
            return render_to_response("accounts/search.html",{
                "form":f,"requestKonto":requestKonto, "users":accounts, "msg":msg})
    f=SearchForm()
    return render_to_response("accounts/search.html",{"form":f, 'requestKonto':requestKonto})
 
@login_required
def profile_edit(request, username):
    """
    Widok odpowiedzialny za edycję profliu użytkownika. Aby funkcja została wywołana 
    użytkownik musi być zalogowany. Jeśli nie jest - zostanie przekierowany na stronę logowania, 
    a po poprawnym ponownie na stronę edycji. Sprawdzane jest czy użytkownik chce zmieniać swój profil
    i jeśli nie to wyświetlana jest informacja o błędzie.
    
    @param request: żądanie przeglądarki
    @param username: login użytkownika którego profil chcemy zmieniać
    """
    requestKonto = Konto.objects.get(user=request.user)    
    if requestKonto.user.username != username:
        msg = Message(2,"Mozesz zmieniac tylko swoj profil!")
        konto = Konto.objects.get(user=requestKonto)
        return render_to_response("accounts/detail.html", {"requestKonto":requestKonto,"viewKonto":requestKonto, "msg":msg})
    f = EditForm()
    konto = Konto.objects.get(user=request.user)
    return render_to_response("accounts/edit.html",{"form":f,"requestKonto":requestKonto,
        "editKonto":requestKonto})
 
@login_required
def profile_save(request, username):
    """
    Widok odpowiedzialny za zapis zmian w profliu użytkownika. Aby funkcja została wywołana 
    użytkownik musi być zalogowany. Jeśli nie jest - zostanie przekierowany na stronę logowania, 
    a po poprawnym ponownie podjęta zostanie próba zapisu.
    Sprawdzane jest czy użytkownik chce zpisać swój profil
    i jeśli nie to wyświetlana jest informacja o błędzie.
    
    @param request: żądanie przeglądarki
    @param username: login użytkownika którego profil chcemy zapisać
    """
    requestKonto = Konto.objects.get(user=request.user)   
    if requestKonto.user.username != username:
        msg = Message(2,"Mozesz zapisywac tylko swoje konto!")
        return render_to_response("accounts/edit.html", {"requestKonto":requestKonto,"editKonto":requestKonto, "msg":msg})
    if request.POST:
        f = EditForm(request.POST)        
        requestKonto.user.first_name = request.POST["imie"]
        requestKonto.user.last_name = request.POST["nazwisko"]
        requestKonto.miasto = request.POST["miasto"]
        requestKonto.zainteresowania = request.POST["zainteresowania"]
        requestKonto.user.email = request.POST["e_mail"]
        if request.POST["nowe_haslo"]:
            if request.POST["stare_haslo"] != requestKonto.user.password:
                msg = Message(2,"Podales zle stare haslo")
                return render_to_response("accounts/edit.html",
                        {"requestKonto":requestKonto,"editKonto":requestKonto, "msg":msg})
            if request.POST["nowe_haslo"]!=request.POST["re_nowe_haslo"]:
                msg = Message(2,"Nowe haslo i powtorzone nowe haslo musza byc                         takie same")
                return render_to_response("accounts/edit.html",
                    {"requestKonto":requestKonto,"editKonto":requestKonto, "msg":msg})
            requestKonto.user.password=request.POST["nowe_haslo"]            
	    requestKonto.plain_pass = request.POST["nowe_haslo"]
        requestKonto.save()
        msg = Message(1,"Zmiany zostaly pomyslnie zapisane")
        return render_to_response("accounts/edit.html",
                {"form":f, "requestKonto":requestKonto,"editKonto":requestKonto, "msg":msg})                
 
 
@login_required
def profile_delete(request, username):
    """
    Widok odpowiedzialny za kasowanie profliu użytkownika. Aby funkcja została wywołana 
    użytkownik musi być zalogowany. Jeśli nie jest - zostanie przekierowany na stronę logowania, 
    a po poprawnym logowaniu ponownie wykonywana jest próba kasowania profilu. Sprawdzane jest czy
    użytkownik chce sksasować swój profil, jeśli nie wyświetlany jest komunikat o błędzie. Kasowanie 
    profilu usuwa poza profilem wsyzstkie pliki i znajomości użytkownika z bazy.
    
    @param request: żądanie przeglądarki
    @param username: login użytkownika którego profil chcemy skasować
    """
    requestKonto = Konto.objects.get(user=request.user)
    if requestKonto.user.username != username:
        msg = Message(2,"Mozesz usunac tylko swoje konto!")
        return render_to_response("accounts/detail.html",
                 {"requestKonto":requestKonto,"viewKonto":requestKonto, "msg":msg})                            
    logout(request)
    requestKonto.user.delete()
    requestKonto.delete()
    msg = Message(1,"Twoje konto zostalo usuniete")
    return render_to_response("index.html", {"msg":msg})
 
