#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
Plik zawiera globalne ustawienia całego projektu. Plik nie powinien być udostępniany gdyż zwiera 
między innymi niezaszyfrowane hasło do bazy danych. Z używanych w ASS.8 ustawień warto wymienić:
    - DATABASE_ENGINE - nazwa slinika na którym działa baza
    - DATABASE_NAME - nazwa bazy
    - DATABASE_USER - login użytkownika z uprawnieniami do modyfikacj bazy
    - DATABASE_PASSWORD - hasło użytkownika z uprawnieniami do modyfikacj bazy
    - DATABASE_HOST - host na którym jest baze
    - DATABASE_PORT - port hosta na którym jest baza
    - INSTALLED_APPS - lista zainstalowanych aplikacji w naszym projekcie
"""

DEBUG = False
TEMPLATE_DEBUG = DEBUG

import os.path
PROJECT_DIR=os.path.dirname(__file__)

ADMINS = (
    ('Tomek Wojcik', 'wojcikt3@wit.edu.pl'),
)

MANAGERS = ADMINS

DATABASE_ENGINE = 'mysql'                       # 'postgresql_psycopg2', 'postgresql', 'mysql', 'sqlite3' or 'oracle'.
DATABASE_OPTIONS = {
    'use_unicode' :True, 
    'charset': 'utf8',
    'read_default_file':'/etc/my.cnf'
    }
DATABASE_NAME = 'ass_base'                      # Or path to database file if using sqlite3.
DATABASE_USER = 'ass8'                          # Not used with sqlite3.
DATABASE_PASSWORD = 'grupa_ppz'                 # Not used with sqlite3.
DATABASE_HOST = ''                              # Set to empty string for localhost. Not used with sqlite3.
DATABASE_PORT = ''                              # Set to empty string for default. Not used with sqlite3.

# Local time zone for this installation. Choices can be found here:
# http://en.wikipedia.org/wiki/List_of_tz_zones_by_name
# although not all choices may be available on all operating systems.
# If running in a Windows environment this must be set to the same as your
# system time zone.
TIME_ZONE = 'Europe/Warsaw'

# Language code for this installation. All choices can be found here:
# http://www.i18nguy.com/unicode/language-identifiers.html
LANGUAGE_CODE = 'pl'

SITE_ID = 1

# If you set this to False, Django will make some optimizations so as not
# to load the internationalization machinery.
USE_I18N = True

# Absolute path to the directory that holds media.
# Example: "/home/media/media.lawrence.com/"
MEDIA_ROOT = os.path.join(PROJECT_DIR,'media')

# URL that handles the media served from MEDIA_ROOT. Make sure to use a
# trailing slash if there is a path component (optional in other cases).
# Examples: "http://media.lawrence.com", "http://example.com/media/"
MEDIA_URL = ''

# URL prefix for admin media -- CSS, JavaScript and images. Make sure to use a
# trailing slash.
# Examples: "http://foo.com/media/", "/media/".
ADMIN_MEDIA_PREFIX = '/media/admin/'

# Make this unique, and don't share it with anybody.
SECRET_KEY = '==8@n_sijz1m*v-3)cbchod(k3j5-hrhqw1^jx0=s99a_^&@^2'

# List of callables that know how to import templates from various sources.
TEMPLATE_LOADERS = (
    'django.template.loaders.filesystem.load_template_source',
    'django.template.loaders.app_directories.load_template_source',
#     'django.template.loaders.eggs.load_template_source',
)

MIDDLEWARE_CLASSES = (
    'django.middleware.common.CommonMiddleware',
    'django.contrib.sessions.middleware.SessionMiddleware',
    'django.contrib.auth.middleware.AuthenticationMiddleware',
)

ROOT_URLCONF = 'ass8_portal.urls'

TEMPLATE_DIRS = (
    # Put strings here, like "/home/html/django_templates" or "C:/www/django/templates".
    # Always use forward slashes, even on Windows.
    # Don't forget to use absolute paths, not relative paths.
    os.path.join(PROJECT_DIR, "templates")
)

INSTALLED_APPS = (
    'django.contrib.auth',
    'django.contrib.contenttypes',
    'django.contrib.sessions',
    'django.contrib.sites',
    'django.contrib.admin',
    'accounts',
    'friends',
    'files',
)
