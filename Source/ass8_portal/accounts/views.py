from django.shortcuts import render_to_response, get_object_or_404
from django.http import Http404, HttpResponseRedirect
from django.contrib.auth.models import User
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

def register(request):
    if request.method =="POST":
        f = RegisterForm(request.POST)
        if not f.is_valid():
            msg = Message(2,"Wypelnij pola oznaczone gwiazdka!")
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
                if "miasto" in request.POST:
                    konto.miasto =request.POST["miasto"]
                if "zainteresowania" in request.POST:
                    konto.zainteresowania =request.POST["zainteresowania"]
                if "imie" in request.POST:
                    konto.user.first_name=request.POST["imie"]
                if "nazwisko" in request.POST:
                    konto.user.last_name=request.POST["nazwisko"]
                konto.save()
                msg = Message(1,"Zakladanie konta zakonczone sukcesem")
                return render_to_response("accounts/detail.html",{"msg":msg, "konto":konto})                
    f = RegisterForm()
    return render_to_response("accounts/register.html", {"form":f})

def profile_view(request, username):
    user = get_object_or_404(User, username=username)
    konto = get_object_or_404(Konto, user=user)
    context = {
        'konto':konto,
    }
    return render_to_response("accounts/detail.html", context, context_instance
            = RequestContext(request))
