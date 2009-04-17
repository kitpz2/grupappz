from django.shortcuts import render_to_response, get_object_or_404
from django.http import Http404, HttpResponseRedirect
from django.contrib.auth.models import User
from django.template import RequestContext
from accounts.helpers import *
from accounts.models import Konto

FRIEND_FUNCTION_MAP = {
    'followers':get_followers,
    'following':get_following,
    'mutual':get_mutual,
}

def friend_list(request, list_type, username):
    user = get_object_or_404(User, username=username)
    konto = get_object_or_404(Konto, user=user)
    context = {
        'list_type':list_type,
        'friends':FRIEND_FUNCTION_MAP[list_type](konto),
    }
    return render_to_response("accounts/friend_list.html",
        context, context_instance = RequestContext(request))

def profile_view(request, username):
    user = get_object_or_404(User, username=username)
#    konto = get_object_or_404(Konto, user=user)
    context = {
        'konto':user,
    }
    return render_to_response("accounts/detail.html", context, context_instance
            = RequestContext(request))
