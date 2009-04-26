from django.conf.urls.defaults import *

# Uncomment the next two lines to enable the admin:
from django.contrib import admin
admin.autodiscover()

urlpatterns = patterns('',
    (r'^css/(?P<path>.*)$', 'django.views.static.serve', {'document_root': 'media/css'}),
    (r'^js/(?P<path>.*)$', 'django.views.static.serve', {'document_root': 'media/js'}),
    (r'^$', 'accounts.views.index'),
    (r'^$/', 'accounts.views.index'),
    (r'^admin/(.*)', admin.site.root),
    (r'^accounts/', include('accounts.urls')),
    (r'^friends/',include('friends.urls')),
)
