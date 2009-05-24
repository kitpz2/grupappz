#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
Plik rejestruje model Konto w panelu administracyjnym. Operacja ta jest wymagana, aby 
można było zmieniać dane Konta z poziomu administratora.
"""

from django.contrib import admin
from accounts.models import Konto


admin.site.register(Konto)
