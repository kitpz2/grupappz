from django.db import models
from django.contrib.auth.models import User

# Create your models here.

class Konto(models.Model):
    user = models.OneToOneField(User, unique=True, null=False)
    miasto = models.CharField(max_length=50)
    zainteresowania = models.TextField()
    plec = models.CharField(max_length=1)

    def __unicode__(self):
        return self.user.username
        
    def save(self, *args, **kwargs):
        self.user.save(*args, **kwargs)
        super(Konto,self).save(**kwargs)
        

