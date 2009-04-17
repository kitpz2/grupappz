from django.conf.urls.defaults import *

urlpatterns = patterns('accounts.views',
    url(r'details/(?P<username>[a-zA-Z0-9_]+)/$','profile_view'),
    url(r'register/$','register'),
)
