# Fourplaces
A student's project with Xamarin technology.

# TODO List
    - Mettre à jour l'ecran d'ajout d'une nouvelle place avec toutes les infos liées

    - Ajouter un ecran pour voir son profil
    - Ajouter un ecran permettant de modifier son profil
    - Ajouter un ecran permettant de modifier son mdp

    - Revoir la mise en page XAML de TOUTES les pages (pour qu'elles soient bien en format portrait COMME paysage)

    - Token.cs => Voir comment regler le TODO : déconnecter l'utilisateur si le RefreshToken ne marche pas ...

# Revoir la propreté du code :
    - Vérifier dans toutes les classes les privates / this
    - Ordonner proprement les fonctions
    - Commenter les fonctions si necessaire
    - Retirer tous les using inutiles
    - Vérifier que tout le code est uniforme (TOUT le code en anglais !)
    - Dans le RestService, changer les creations de chaines de caractères pas propres en
      quelque chose de mieux ... String.Format("The current price is {0} per ounce.", pricePerOunce);
