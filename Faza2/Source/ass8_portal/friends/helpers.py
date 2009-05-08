from accounts.models import Konto
from friends.models import UserLink

#znajomosc: ja udostepniam komus
def get_my_followers(user):
    ul = UserLink.objects.filter(from_user =
            user).values('to_user').order_by('-date_added')
    return Konto.objects.filter(id__in=[i['to_user'] for i in ul])


#znajomosc: ktos udostepnia mi
def get_my_following(user):
    ul = UserLink.objects.filter(to_user =
            user).values('from_user').order_by('-date_added')
    return Konto.objects.filter(id__in=[i['from_user'] for i in ul])

#znajomosc w dwie strony
def get_mutual(user):
    followers = UserLink.objects.filter(from_user =
            user).values('to_user').order_by('-date_added')
    following = UserLink.objects.filter(to_user =
            user).values('from_user').order_by('-date_added')
    followers_set = set([i['to_user'] for i  in followers])
    following_set = set([i['from_user'] for i in following])
    return Konto.objects.filter(id__in=followers_set.intersection(following_set))
