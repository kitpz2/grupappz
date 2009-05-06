from django.conf.urls.defaults import *

urlpatterns = patterns('friends.views',
    url(r'index/(?P<username>[\w-]+)/$','friends_manage'),
    url(r'followers/(?P<username>[\w-]+)/$','friend_list',{'list_type':'followers'}),
    url(r'following/(?P<username>[\w-]+)/$','friend_list',{'list_type':'following'}),
    url(r'mutual/(?P<username>[\w-]+)/$','friend_list',{'list_type':'mutual'}),
    url(r'add/(?P<username>[\w-]+)/$','add_friend'),
    url(r'delete/(?P<username>[\w-]+)/$','del_friend'),
)
