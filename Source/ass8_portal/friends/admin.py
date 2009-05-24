#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
Plik rejestruje model UserLink w panelu administracyjnym. Operacja ta jest wymagana, aby 
można było zmieniać dane znajomości z poziomu administratora.
"""
from django.contrib import admin
from friends.models import UserLink

admin.site.register(UserLink)
