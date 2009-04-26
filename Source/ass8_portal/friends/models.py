from django.db import models

from accounts.models import Konto
from datetime import datetime

class UserLink(models.Model):
    from_user = models.ForeignKey(Konto, related_name='following_set')
    to_user = models.ForeignKey(Konto, related_name='follower_set')
    date_added = models.DateTimeField(default=datetime.now)

    def __unciode__(self):
        return "%s udostepnia pliki dla %s" %(self.from_user.user.username,
                selr.to_user.user.username)

    def save(self, *args, **kwargs):
        if self.from_user== self.to_user:
            raise ValueError("Nie mozesz dodac siebie do znajomych")
        super(UserLink, self).save(**kwargs)

    class Meta:
        unique_together = (('to_user', 'from_user'),)

