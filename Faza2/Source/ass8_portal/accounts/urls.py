from django.conf.urls.defaults import *

urlpatterns = patterns('accounts.views',
    url(r'details/(?P<username>[\w-]+)/$','profile_view'),
    url(r'edit/(?P<username>[\w-]+)/$','profile_edit'),
    url(r'save/(?P<username>[\w-]+)/$','profile_save'),
    url(r'delete/(?P<username>[\w-]+)/$','profile_delete'),
    url(r'register/$','register'),
    url(r'login/$','user_login'),
    url(r'logout/$','user_logout'),
    url(r'latest/$','latest_users'),
    url(r'search/$','search'),
)
