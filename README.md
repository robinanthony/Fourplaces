# Fourplaces
A student's project with Xamarin technology.

# Appareil utilisé lors des tests :
    - Wiko Pulp 4G, Android version 5.1.1 (API 22)

# Fonctionnalités :
    - Se connecter
    - Créer un compte
    - Voir la liste des places existantes
    - Ajouter une place
    - Voir les informations liées à une place
    - Voir les commentaires liés à une place
    - Ajouter un commentaire
    - Voir les informations liés à mon compte
    - Modifier le mot de passe lié à mon compte
    - Modifier les informations liés à mon compte
    - Toute l'application est normalement accessible en format Paysage et
      Portrait.

# Fonctionnalités non opérationnelles :

# Problèmes connues :
    - Si je fais défiler rapidement la page présentant toutes les places
      connues, une erreur "OutOfMemoryError" apparait. Je pense que cela
      vient du fait que trop d'image doivent être chargées en même temps
      et que mon téléphone n'en a pas les capacités. Je ne sais comment
      résoudre ce problème.
    - Lors de la création d'une place, si j'utilise l'option "Prendre une photo"
      afin de donner une image à la place, une latence apparait. Hors, il n'y
      a pas de problème avec l'option "Choisir une image existante". Le même
      problème apparait lors de la mise à jour des informations de l'utilisateur.
      Je pense que le problème vient plus de mon appareil que de l'application
      mais je n'ai pas la possibilité de vérifier.
    - Lors de chaque requête REST, un refresh du token est effectué si cela
      s'avère nécessaire (càd qu'il reste moins de 10 minutes de validité).
      Hors, si le token est déjà expiré, il faudrait déconnecter
      l'utilisateur. Cette effet est manquant, ne sachant pas où mettre l'appel
      du popAsync().

# Revoir la propreté du code :
    - Vérifier dans toutes les classes les privates / this
	  - Vérifier que toutes les commandes sont "public Command MyCommand { get; private set; }"
    - Ordonner proprement les fonctions
    - Commenter les fonctions si necessaire
    - Retirer tous les using inutiles
