#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
Plik zawiera opis modeli dla bazy danych dla aplikacji 'files'.
"""
from django.db import models
from accounts.models import Konto

from datetime import datetime

class Plik(models.Model):
    """
    Klasa opisująca plik w systemie. Model zawiera podstawowe pola takie jak wielkość czy hash pliku,
    z poziomu aplikacji kluczowe jest to że każdy plik musi być skojarzony z jakimś użytkownikiem.
    """
    konto = models.ForeignKey(Konto)
    sciezka = models.CharField(max_length=255)
    dataDodania = models.DateTimeField(default = datetime.now())
    prawaDostepu = models.IntegerField()
    wielkosc = models.IntegerField()
    hashValue = models.CharField(max_length=50)

    def __unicode__(self):
        """
        Metoda odpowiedzialna za wyświetlenie modelu w panelu administracyjnym
        """
        return self.sciezka.split('/')[-1]

    class Meta:
        """
        Klasa definiująca dodatkowe informacje o modelu.
        """
        verbose_name_plural = "Pliki"
        
