#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
Plik zawiera opis modeli dla bazy danych dla aplikacji 'accounts'.
"""
from django.db import models
from django.contrib.auth.models import User

class Konto(models.Model):
    """
    Model związany z kontem użytkownika. Korzysta z proponowanego przez django modelu User, jednak
    rozszerza go o dodatkowe pola.
    """
    user = models.ForeignKey(User)
    miasto = models.CharField(max_length=50)
    zainteresowania = models.TextField()
    plec = models.CharField(max_length=1)

    def __unicode__(self):
        """        
        Metoda odpowiedzialna za wyświetlanie modelu w panelu administracyjnym.
        """
        return self.user.username

    def save(self, *args, **kwargs):
        """
        Metoda odpowiedzialna za zapisanie danych do bazy. 
        """
        self.user.save(*args, **kwargs)
        super(Konto,self).save(**kwargs)

    class Meta:
        """
        Klasa definiująca dodatkowe informacje o modelu.
        """
        verbose_name_plural = "Konta"

