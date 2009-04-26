from django.conf.urls.defaults import *

urlpatterns = patterns('friends.views',
    url(r'(?P<username>[a-zA-Z0-9_]+)/$','friends_manage'),
    url(r'(?P<username>[a-zA-Z0-9_]+)/add/(?P<friend>[a-zA-Z0-9_]+)/$','add_friend'),
    url(r'(?P<username>[a-zA-Z0-9_]+)/delete/(?P<friend>[a-zA-Z0-9_]+)/$','del_friend'),
)
