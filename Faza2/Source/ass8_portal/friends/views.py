from django.shortcuts import render_to_response, get_object_or_404
from django.http import Http404, HttpResponseRedirect
from django.contrib.auth.models import User
from django.contrib.auth.decorators import login_required
from django.template import RequestContext

from friends.helpers import *
from friends.models import UserLink
from accounts.models import Konto
from accounts.views import Message

FRIEND_FUNCTION_MAP = {
    'followers':get_my_followers,
    'following':get_my_following,
    'mutual':get_mutual,
}

@login_required
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
@login_required
def friend_list(request, list_type, username):
    requestKonto = Konto.objects.get(user = request.user)
    viewUser = get_object_or_404(User, username = username)
    viewKonto = Konto.objects.get(user=viewUser)
    context = {
        'list_type':list_type,
        'friends':FRIEND_FUNCTION_MAP[list_type](viewKonto),
        'requestKonto' : requestKonto,
        'viewKonto' : viewKonto,
    }
    return render_to_response("friends/list.html", context)

@login_required
def add_friend(request, username):
    requestKonto = Konto.objects.get(user = request.user)
    addUser = get_object_or_404(User, username = username)
    addKonto = Konto.objects.get(user = addUser)
    lista = get_my_followers(requestKonto)
    if addKonto in lista:
        msg = Message(2,"Uzytkownik jest juz dodany do Twoich znajomych")
        return render_to_response("accounts/detail.html", {"requestKonto":requestKonto, "viewKonto":addKonto, "msg":msg })
    else:
        link = UserLink(from_user=requestKonto,to_user=addKonto)
        link.save()
        msg = Message(1,"Uzytkownik pomyslnie dodany do znajomych")
        return render_to_response("accounts/detail.html", {
            "requestKonto":requestKonto, 
            "viewKonto":addKonto, 
            "msg":msg })

@login_required
def del_friend(request, username):
    requestKonto = Konto.objects.get(user = request.user)
    delUser = get_object_or_404(User, username = username)
    delKonto = Konto.objects.get(user = delUser)
    try:
        ul = UserLink.objects.get(from_user=requestKonto, 
                to_user=delKonto)
    except UserLink.DoesNotExist:
        msg = Message(1,"Nie mozesz usunac tego uzytkownika.")
        followers = FRIEND_FUNCTION_MAP['followers'](requestKonto)
        following = FRIEND_FUNCTION_MAP['following'](requestKonto)
        mutual = FRIEND_FUNCTION_MAP['mutual'](requestKonto)
        context = {
            'msg':msg,
            'followers':followers[:10],
            'following':following[:10],
            'mutual':mutual[:10],
            'requestKonto':requestKonto,
            'viewKonto':requestKonto
            }
        return render_to_response("friends/index.html", context)
    ul.delete()
    msg = Message(1,"Uzytkownik pomyslnie usuniety ze znajomych")
    return HttpResponseRedirect("/friends/index/"+requestKonto.user.username+"/")
