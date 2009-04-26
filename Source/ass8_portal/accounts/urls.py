from django.conf.urls.defaults import *

urlpatterns = patterns('accounts.views',
    url(r'details/(?P<username>[a-zA-Z0-9_]+)/$','profile_view'),
    url(r'edit/(?P<username>[a-zA-Z0-9_]+)/$','profile_edit'),
    url(r'save/(?P<username>[a-zA-z0-9_]+)/$','profile_save'),
    url(r'delete/(?P<username>[a-zA-z0-9_]+)/$','profile_delete'),
    url(r'register/$','register'),
    url(r'login/$','user_login'),
    url(r'logout/$','user_logout'),
    url(r'latest/$','latest_users'),
    url(r'search/$','search'),
)
