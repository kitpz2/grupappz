from django import forms
chmod_choice = ('Moj','Prywatny','Publiczny')

class ChmodForm(forms.Form):
    chmod = forms.ChoiceField(label = "Widocznosc", choices = chmod_choice)


