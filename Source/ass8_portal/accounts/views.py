from django.shortcuts import render_to_response, get_object_or_404
from django.contrib.auth.decorators import login_required
from django.http import Http404, HttpResponseRedirect
from django.contrib.auth.models import User
from django.contrib.auth import login, logout, authenticate
from django.template import RequestContext
from accounts.helpers import *
from accounts.models import Konto
from accounts.forms import *

FRIEND_FUNCTION_MAP = {
    'followers':get_followers,
    'following':get_following,
    'mutual':get_mutual,
}

MESSAGE_CODES = ('Warning', 'Information', 'Error')

class Message(object):
    def __init__(self, type, content):
        self.type = MESSAGE_CODES[type]
        self.content = content

    def __unicode__(self):
        return "%s : %s" % self.type, self.content

def user_login(request):
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
        })

def user_logout(request):
    if request.user.is_authenticated:
        logout(request)
    return HttpResponseRedirect("/")

def index(request):
    user = None
    if request.user.is_authenticated:
        user = request.user
    return render_to_response("index.html",{"user":user})


def register(request):
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
                user.save()
                konto = Konto(user = user, plec = request.POST["plec"])
                konto.save()
                msg = Message(1,"Rejestracja zakonczona sukcesem")
                return render_to_response("accounts/detail.html",{"msg":msg, "konto":konto})                
    f = RegisterForm()
    return render_to_response("accounts/register.html", {"form":f})


@login_required
def profile_view(request, username):
    user = request.user
    u = get_object_or_404(User, username=username)
    konto = get_object_or_404(Konto, user=u)
    context = {
        'konto':konto,
        'user': user,
    }
    return render_to_response("accounts/detail.html", context, context_instance
            = RequestContext(request))


def latest_users(request):
    user = None
    if request.user.is_authenticated:
        user = request.user
    tempUsers = User.objects.all().order_by('-date_joined')[:25]
    latestUsers = []
    for u in tempUsers:
        k = Konto.objects.get(user=u)
        latestUsers.append(k)
    return render_to_response("accounts/latestUsers.html",
            {"user":user,"latestUsers":latestUsers})

@login_required
def search(request):
    user = request.user
    if request.POST:
        f = SearchForm(request.POST)
        if f.is_valid:
            accounts=[]
            users = User.objects.filter(username__contains =
                    request.POST["search"])
            for user in users:
                k = Konto.objects.get(user=user)
                accounts.append(k)                
            count =len(accounts)
            if count == 1:
                msg = Message(1, "Znaleziono 1 uzytkownika")
            else:
                msg = Message(1,"Znaleziono "+repr(count)+" uzytkownikow")
            return render_to_response("accounts/search.html",{
                "form":f,"user":user, "users":accounts, "msg":msg})
    f=SearchForm()
    return render_to_response("accounts/search.html",{"form":f, 'user':user})
