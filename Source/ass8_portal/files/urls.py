from django.conf.urls.defaults import *

urlpatterns = patterns('files.views',
    url(r'details/(?P<username>[\w-]+)/$','view'),
    url(r'edit/(?P<username>[\w-]+)/$','edit'),
    url(r'chmod/(?P<id>[\d]+)/own/$','to_own'),
    url(r'chmod/(?P<id>[\d]+)/private/$','to_private'),
    url(r'chmod/(?P<id>[\d]+)/public/$','to_public'),
    url(r'delete/(?P<id>[\d]+)/$','delete'),
)
