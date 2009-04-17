from django import forms

plec_choice = (('K','Kobieta'),('M','Mezczyzna'))

class LoginForm(forms.Form):
    login = forms.CharField(label="Login")
    password = forms.CharField(label="Haslo", widget=forms.PasswordInput)

class RegisterForm(forms.Form):
    #obowiazkowe pola
    login = forms.CharField(label="Login")
    haslo = forms.CharField(label="Haslo", widget=forms.PasswordInput)
    re_haslo = forms.CharField(label="Powtorz haslo", widget=forms.PasswordInput)
    e_mail = forms.CharField(label="e-mail")
    plec = forms.ChoiceField(label="Plec", choices=plec_choice)
    #nieobowiazkowe
    #imie = forms.CharField(label="Imie", required=False)
    #nazwisko = forms.CharField(label="Nazwisko", required=False)
    #miasto = forms.CharField(label="Miasto", required=False)
    #zainteresowania = forms.Textarea(label="Zainteresowania", required=False)
