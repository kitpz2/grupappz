from django.db import models
from django.contrib.auth.models import User

# Create your models here.

class Konto(models.Model):
    user = models.ForeignKey(User)
    miasto = models.CharField(max_length=50)
    zainteresowania = models.TextField()
    plec = models.CharField(max_length=1)

    def __unicode__(self):
        return user.username

