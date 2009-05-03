from django.shortcuts import render_to_response, get_object_or_404
from django.contrib.auth.decorators import login_required
from django.http import Http404, HttpResponseRedirect
from django.contrib.auth.models import User
from django.contrib.auth import login, logout, authenticate
from django.template import RequestContext
from accounts.models import Konto
from accounts.forms import *
 
MESSAGE_CODES = ('Warning', 'Information', 'Error')
 
class Message(object):
    def __init__(self, type, content):
        self.type = MESSAGE_CODES[type]
        self.content = content
 
    def __unicode__(self):
        return "%s : %s" % self.type, self.content
 
def user_login(request):        
    requestKonto = None
    if request.user in User.objects.all():    
        requestUser = User.objects.get(username = request.user.username)
        requestKonto = Konto.objects.get(user = requestUser) 
    msg=None
    if request.POST:
        username = request.POST['login']
        password = request.POST['password']
        user = authenticate(username=username, password=password)
        if user:
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
    if request.user.is_authenticated:
        logout(request)
    return HttpResponseRedirect("/")
 
def index(request):   
    requestKonto = None
    if request.user in User.objects.all():    
        requestUser = User.objects.get(username = request.user.username)
        requestKonto = Konto.objects.get(user = requestUser)        
    return render_to_response("index.html",{"requestKonto":requestKonto})
 
 
def register(request):
    requestKonto = None
    if request.user in User.objects.all():    
        requestUser = User.objects.get(username = request.user.username)
        requestKonto = Konto.objects.get(user = requestUser)         
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
                konto = Konto(user = user, plec = request.POST["plec"])
                konto.save()
                msg = Message(1,"Rejestracja zakonczona sukcesem")
                login(request,user)
                return render_to_response("accounts/detail.html",{"msg":msg, "requestKonto":konto, "viewKonto":konto})                
    f = RegisterForm()
    return render_to_response("accounts/register.html", {"form":f})
 
 
@login_required
def profile_view(request, username):    
    requestKonto = Konto.objects.get(user = request.user)
    viewUser = get_object_or_404(User, username=username)
    viewKonto = Konto.objects.get(user=viewUser)
    context = {
        'requestKonto':requestKonto,
        'viewKonto': viewKonto,
    }
    return render_to_response("accounts/detail.html", context)
    
def latest_users(request):
    requestKonto = Konto.objects.get(user=request.user)    
    tempUsers = User.objects.all().order_by('-date_joined')[:25]
    latestUsers = []
    for u in tempUsers:
        k = Konto.objects.get(user=u)
        latestUsers.append(k)
    return render_to_response("accounts/latestUsers.html",
            {"requestKonto":requestKonto,"latestUsers":latestUsers})
 
@login_required
def search(request):
    requestKonto = Konto.objects.get(user=request.user)    
    if request.POST:
        f = SearchForm(request.POST)
        if f.is_valid:
            accounts=[]
            userList = User.objects.filter(username__contains =
                    request.POST["search"])
            for user in userList:
                k = Konto.objects.get(user=user)
                accounts.append(k)                
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
        requestKonto.save()
        msg = Message(1,"Zmiany zostaly pomyslnie zapisane")
        return render_to_response("accounts/edit.html",
                {"form":f, "requestKonto":requestKonto,"editKonto":requestKonto, "msg":msg})                
 
 
@login_required
def profile_delete(request, username):
    requestKonto = Konto.objects.get(user=request.user)
    if requestKonto.user.username != username:
        msg = Message(2,"Mozesz usunac tylko swoje konto!")
        return render_to_response("accounts/detail.html",
                 {"requestKonto":requestKonto,"viewKonto":requestKonto, "msg":msg})        
    logout(request)
    requestKonto.user.delete()
    konto.delete()
    msg = Message(1,"Twoje konto zostalo usuniete")
    return render_to_response("index.html", {"msg":msg})
 