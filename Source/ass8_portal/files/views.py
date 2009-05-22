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
    requestKonto = Konto.objects.get(user = request.user)
    viewUser = get_object_or_404(User, username=username)
    viewKonto = Konto.objects.get(user = viewUser)
    if requestKonto.user.username != viewKonto.user.username:
        try:
            ul = UserLink.objects.get(from_user = viewKonto, to_user =
                requestKonto)
            privateFileList = Plik.objects.get(konto = viewUser, prawaDostepu = PRIVATE)
            publicFileList = Plik.objects.get(konto = viewUser, prawaDostepu =
                    PUBLIC)
            return render_to_response('files/view.html',
                    {"requestKonto":requestKonto,
                        "viewKonto":viewKonto, 
                        "private":privateFileList,
                        "public":publicFileList,
                        "chmod":PRIVATE})
        except UserLink.DoesNotExist:
            fileList = Plik.objects.get(konto = viewUser, prawaDostepu = PUBLIC)
            return render_to_response('files/view.html',
                    {"requestKonto":requestKonto,
                        "viewKonto":viewKonto,
                        "public":fileList,
                        "chmod":PUBLIC})
    else:
        privateFileList = Plik.objects.get(konto = viewUser, prawaDostepu =
                PRIVATE)
        publicFileList = Plik.objects.get(konto = viewUser, prawaDostepu =
                PUBLIC)
        ownFileList = Plik.objects.get(konto = viewUser, prawaDostepu = MY)

        return render_to_response('files/view.html',
                    {"requestKonto":requestKonto,
                        "viewKonto":viewKonto,
                        "private":privateFileList,
                        "public":publicFileList,
                        "own":ownFileList,
                        "chmod":MY})

@login_required
def to_public(request, id):
    requestKonto = Konto.objects.get(user = request.user)        
    f = Plik.objects.get(id = id)
    f.prawaDostepu = PUBLIC
    f.save()
    msg = Message(1, "Prawa dostepu zostaly pomyslnie zmienione.")
    return render_to_response("files/view.html", {"requestKonto":
        requestKonto, "viewKonto":requestKonto, "msg":msg})


@login_required
def to_private(request, id):
    requestKonto = Konto.objects.get(user = request.user)            
    f = Plik.objects.get(id = id)
    f.prawaDostepu = PRIVATE
    f.save()
    msg = Message(1, "Prawa dostepu zostaly pomyslnie zmienione.")
    return render_to_response("files/view.html", {"requestKonto":
        requestKonto, "viewKonto":requestKonto, "msg":msg})
        
@login_required
def to_own(request, id):
    requestKonto = Konto.objects.get(user = request.user)    
    msg = Message(2, "Mozesz zmieniac tylko swoje pliki!")
    return render_to_response("files/view.html", {"requestKonto":requestKonto,
        "viewKonto":viewKonto,"msg":msg})            
    f = Plik.objects.get(id = id)
    f.prawaDostepu = MY
    f.save()
    msg = Message(1, "Prawa dostepu zostaly pomyslnie zmienione.")
    return render_to_response("files/view.html", {"requestKonto":
        requestKonto, "viewKonto":requestKonto, "msg":msg})



@login_required
def delete(request, ID):
    requestKonto = Konto.objects.get(user = request.user)            
    f = Plik.objects.get(id = id)
    f.delete()
    msg = Message(1, "Plik został usunięty z bazy.")
    return render_to_response("files/view.html", {"requestKonto":
        requestKonto, "viewKonto":requestKonto, "msg":msg})


@login_required
def latest(request, username):
    requestKonto = Konto.objects.get(user = request.user)
    viewUser = get_object_or_404(User, username=username)
    viewKonto = Konto.objects.get(user = viewUser)
    if requestKonto.user.username != viewKonto.user.username:
        try:
            ul = UserLink.objects.get(from_user = viewKonto, to_user =
                    requestKonto)
            privateFileList = Plik.objects.get(konto = viewUser, prawaDostepu =
                    PRIVATE).order_by('-dataDodania')[:10]
            publicFileList = Plik.objects.get(konto = viewUser, prawaDostepu =
                    PUBLIC).order_by('-dataDodania')[:10]
            return render_to_response('files/latest.html',
                    {"requestKonto":requestKonto,
                        "viewKonto":viewKonto, 
                        "private":privateFileList,
                        "public":publicFileList,
                        "chmod":PRIVATE})
        except UserLink.DoesNotExist:
            fileList = Plik.objects.get(konto = viewUser, prawaDostepu = PUBLIC).order_by('-dataDodania')[:10]
            return render_to_response('files/latest.html',
                    {"requestKonto":requestKonto,
                        "viewKonto":viewKonto,
                        "public":fileList,
                        "chmod":PUBLIC})
    else:
        privateFileList = Plik.objects.get(konto = viewUser, prawaDostepu =
                PRIVATE).order_by('-dataDodania')[:10]
        publicFileList = Plik.objects.get(konto = viewUser, prawaDostepu =
                PUBLIC).order_by('-dataDodania')[:10]
        ownFileList = Plik.objects.get(konto = viewUser, prawaDostepu = MY).order_by('-dataDodania')[:10]

        return render_to_response('files/latest.html',
                    {"requestKonto":requestKonto,
                        "viewKonto":viewKonto,
                        "private":privateFileList,
                        "public":publicFileList,
                        "own":ownFileList,
                        "chmod":MY})

