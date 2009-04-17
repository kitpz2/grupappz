from django.conf.urls.defaults import *

# Uncomment the next two lines to enable the admin:
from django.contrib import admin
admin.autodiscover()

urlpatterns = patterns('',
   (r'^admin/(.*)', admin.site.root),
   (r'^accounts/', include('accounts.urls')),
)
