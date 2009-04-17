from django.conf.urls.defaults import *

urlpatterns = patterns('konto.views',
        (r'^/(?P<username>[a-zA-Z0-9_]+)/','detail'),
)
