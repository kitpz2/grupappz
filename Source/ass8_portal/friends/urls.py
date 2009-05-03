from django.conf.urls.defaults import *

urlpatterns = patterns('friends.views',
    url(r'index/(?P<username>[a-zA-Z0-9_]+)/$','friends_manage'),
    url(r'followers/(?P<username>[a-zA-Z0-9_]+)/$','friend_list',{'list_type':'followers'}),
    url(r'following/(?P<username>[a-zA-Z0-9_]+)/$','friend_list',{'list_type':'following'}),
    url(r'mutual/(?P<username>[a-zA-Z0-9_]+)/$','friend_list',{'list_type':'mutual'}),
    url(r'add/(?P<username>[a-zA-Z0-9_]+)/$','add_friend'),
    url(r'delete/(?P<username>[a-zA-Z0-9_]+)/$','del_friend'),
)
