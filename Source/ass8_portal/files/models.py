from django.db import models
from accounts.models import Konto

from datetime import datetime

class Plik(models.Model):
    konto = models.ForeignKey(Konto)
    sciezka = models.CharField(max_length=255)
    dataDodania = models.DateTimeField(default = datetime.now())
    prawaDostepu = models.IntegerField()
    wielkosc = models.IntegerField()
    hashValue = models.CharField(max_length=50)

    def __unicode__(self):
        return self.sciezka.split('/')[-1]
