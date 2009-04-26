from django.shortcuts import render_to_response, get_object_or_404
from django.http import Http404, HttpResponseRedirect
from django.contrib.auth.models import User
from django.template import RequestContext

from friends.helpers import *
from friends.models import UserLink
from accounts.models import Konto

FRIEND_FUNCTION_MAP = {
    'followers':get_followers,
    'following':get_following,
    'mutual':get_mutual,
}

def friends_manage(request, username):
    u = get_object_or_404(User, username=username)
    konto = Konto.objects.get(user=u)
    context = {        
        'followers':FRIEND_FUNCTION_MAP[followers](user),
        'following':FRIEND_FUNCTION_MAP[following](user),
        'mutual':FRIEND_FUNCTION_MAP[mutual](user),
    }
    return render_to_response("friends/index.html",
            context, context_instance = RequestContext(request))

def add_friend(request):
    pass
    
def del_friend(request):
    pass