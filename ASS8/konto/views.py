from django.shortcuts import render_to_response
from konto.models import Konto, Plik
from django.contrib.auth.models import User

def detail(request, username):
    user = User.objects.filter(username=username)
    konto = Konto.objects.get(user = user)
    return render_to_response("konto/detail.html",{"konto":konto})
