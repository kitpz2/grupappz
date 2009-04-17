from django.db import models
from django.contrib.auth.models import User
from datetime import datetime

class Konto(models.Model):
    user = models.OneToOneField(User)
    miasto = models.CharField(max_length=50)
    zainteresowania = models.TextField()
    plec = models.CharField(max_length=1)
    
    def __unicode__(self):
        return user.username
    
    class Meta:
        pass

class Plik(models.Model):
    user = models.ForeignKey(Konto)
    sciezka = models.CharField(max_length=255)
    dataDodania = models.DateTimeField(default = datetime.now)
    prawaDostepu = models.IntegerField()

    def __unicode__(self):
        temp = sciezka.split('/')
        return temp[-1] 

    class Meta:
        pass
