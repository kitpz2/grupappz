from django import forms

plec_choice = (('K','Kobieta'),('M','Mezczyzna'))

class LoginForm(forms.Form):
    login = forms.CharField(label="Login")
    password = forms.CharField(label="Haslo", widget=forms.PasswordInput)

class RegisterForm(forms.Form):    
    login = forms.CharField(label="Login")
    haslo = forms.CharField(label="Haslo", widget=forms.PasswordInput)
    re_haslo = forms.CharField(label="Powtorz haslo", widget=forms.PasswordInput)
    e_mail = forms.EmailField(label="e-mail")
    plec = forms.ChoiceField(label="Plec", choices=plec_choice)

class SearchForm(forms.Form):
    search = forms.CharField(label="Szukaj")

class EditForm(forms.Form):
    imie = forms.CharField(label="Imie", required=False)
    nazwisko = forms.CharField(label="Nazwisko", required=False)
    miasto = forms.CharField(label="Miasto", required=False)
    zainteresowania = forms.CharField(label="Zainteresowania", widget=forms.Textarea, required=False)
    e_mail = forms.EmailField(label="e-mail", required=False)
    stare_haslo = forms.CharField(label="Stare haslo", widget=forms.PasswordInput, required=False)
    nowe_haslo = forms.CharField(label="Nowe haslo", widget=forms.PasswordInput, required=False)
    re_nowe_haslo = forms.CharField(label="Powtorz haslo", widget=forms.PasswordInput, required=False)
