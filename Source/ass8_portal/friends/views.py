from django.shortcuts import render_to_response, get_object_or_404
from django.http import Http404, HttpResponseRedirect
from django.contrib.auth.models import User
from django.template import RequestContext

from friends.helpers import *
from friends.models import UserLink
from accounts.models import Konto
from accounts.view import Message

FRIEND_FUNCTION_MAP = {
    'followers':get_my_followers,
    'following':get_my_following,
    'mutual':get_mutual,
}

def friends_manage(request, username):
    requestKonto = Konto.objects.get(user = request.user)
    viewUser = get_object_or_404(User, username = username)
    viewKonto = Konto.objects.get(user = viewUser)
    followers = FRIEND_FUNCTION_MAP['followers'](viewKonto)
    following = FRIEND_FUNCTION_MAP['following'](viewKonto)
    mutual = FRIEND_FUNCTION_MAP['mutual'](viewKonto)
    context = {
            'followers':followers[:10],
            'following':following[:10],
            'mutual':mutual[:10],
            'requestKonto':requestKonto,
            'viewKonto':viewKonto
    }
    return render_to_response("friends/index.html", context)

def friend_list(request, list_type, username):
    requestKonto = Konto.objects.get(user = request.user)
    viewUser = get_object_or_404(User, username = username)
    viewKonto = Konto.objects.get(user=u)
    context = {
        'list_type':list_type,
        'friends':FRIEND_FUNCTION_MAP[list_type](konto),
        'requestKonto' : requestKonto,
        'viewKonto' : viewKonto,
    }
    return render_to_response("friends/list.html", context)

def add_friend(request, username):
    requestKonto = Konto.objects.get(user = request.user)
    addUser = get_object_or_404(User, username = username)
    addKonto = Konto.objects.get(user = addUser)
    lista = get_my_followers(requestUser)
    if addKonto in lista:
        msg = Message(2,"Użytkownik jest już dodany do Twoich znajomych")
        return render_to_response("accounts/detail.html", {"requestKonto":requestKonto, "viewKonto":addKonto, "msg":msg })
    else:
        link = UserLink(from_user=requestKontoto_user=addKonto)
        link.save()
        msg = Message(1,"Użytkownik pomyślnie dodany do znajomych")
        return render_to_response("accounts/detail.html", {"requestKonto":requestKonto, "viewKonto":addKonto, "msg":msg })

def del_friend(request):
    pass
