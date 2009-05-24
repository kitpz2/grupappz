#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
Plik odpowiedzialny za zarządzanie projektem. Udostępnia podstawową funkcjonalność związaną z
    - generowaniem zapytan SQL na podstawie modeli w bazie
    - uruchamianie prostego servera na potrzeby testów projektu
    - generowanie nowych aplikacji w projekcie, oraz nowego projektu.
"""
from django.core.management import execute_manager
try:
    import settings # Assumed to be in the same directory.
except ImportError:
    import sys
    sys.stderr.write("Error: Can't find the file 'settings.py' in the directory containing %r. It appears you've customized things.\nYou'll have to run django-admin.py, passing it your settings module.\n(If the file settings.py does indeed exist, it's causing an ImportError somehow.)\n" % __file__)
    sys.exit(1)

if __name__ == "__main__":
    execute_manager(settings)
