from django.conf.urls.defaults import *

urlpatterns = patterns('accounts.views',
    url(r'^(?P<username>[a-zA-Z0-9_]+)/$','profile_view'),
    url(r'^(?P<username>[a-zA-Z0-9_]+)/followers/$','friend_list',
        {'list_type':'followers'}),
    url(r'^(?P<username>[a-zA-Z0-9_]+)/following/$','friend_list',
        {'list_type':'following'}),
    url(r'^(?P<username>[a-zA-Z0-9_]+)/mutual/$','friend_list',
        {'list_type':'mutual'}),
)
