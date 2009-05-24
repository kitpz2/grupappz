#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
Plik zawiera opis modeli dla bazy danych dla aplikacji 'files'.
"""
from django.db import models

from accounts.models import Konto
from datetime import datetime

class UserLink(models.Model):
    """
    Model opisujący znajomość między użytkonikami. Opisuje kto(from_user) komu(to_user) 
    udostępnia pliki.
    """
    from_user = models.ForeignKey(Konto, related_name='following_set')
    to_user = models.ForeignKey(Konto, related_name='follower_set')
    date_added = models.DateTimeField(default=datetime.now)

    def __unicode__(self):
        """
        Metoda odpowiedzialna za wyświetlenie znajomości w panelu administracyjnym.
        """
        return "%s udostepnia pliki dla %s" %(self.from_user,
                self.to_user)

    def save(self, *args, **kwargs):
        """
        Metoda zapisywanie modelu do bazy danych. Sprawdzane jest czy użytkownik nie próbuje dodać
        siebie do znajomych.
        """
        if self.from_user== self.to_user:
            raise ValueError("Nie mozesz dodac siebie do znajomych")
        super(UserLink, self).save(**kwargs)

    class Meta:
        """
        Klasa definiująca dodatkowe informacje o modelu.
        """
        unique_together = (('to_user', 'from_user'),)

