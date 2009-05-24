#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
Plik rejestruje model Plik w panelu administracyjnym. Operacja ta jest wymagana, aby 
można było zmieniać dane Pliku z poziomu administratora.
"""
from django.contrib import admin
from files.models import Plik

admin.site.register(Plik)
