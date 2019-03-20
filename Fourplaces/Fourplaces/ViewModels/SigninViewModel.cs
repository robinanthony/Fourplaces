using Storm.Mvvm;
using System.Windows.Input;
using Xamarin.Forms;

namespace Fourplaces.ViewModels
{
    class SigninViewModel : ViewModelBase
    {
        public ICommand SigninCommand { get; set; }
        public string TitleLabel { get; set; }

        private string _email;
        private string _password;
        private string _passwordTwo;
        private string _prenom;
        private string _nom;

        public SigninViewModel()
        {
            this.TitleLabel = "Création d'un nouveau compte";
            this.SigninCommand = new Command(SigninClicked);
        }

        private void SigninClicked(object _)
        {
            // TODO -> Vérifier que les champs sont remplis et faire l'inscription !
            Signin();
        }

        private async void Signin()
        {
            if (!string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(PasswordTwo) && !string.IsNullOrWhiteSpace(Prenom) && !string.IsNullOrWhiteSpace(Nom) && (Password == PasswordTwo))
            {
                await Application.Current.MainPage.DisplayAlert("Test d'inscription", "Vos identifiants sont valides.", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Test d'inscription", "Vos identifiants sont invalides.", "OK");
            }
        }

        public string Email
        {
            get => this._email;
            set => SetProperty(ref this._email, value);
        }

        public string Password
        {
            get => this._password;
            set => SetProperty(ref this._password, value);
        }

        public string PasswordTwo
        {
            get => this._passwordTwo;
            set => SetProperty(ref this._passwordTwo, value);
        }

        public string Prenom
        {
            get => this._prenom;
            set => SetProperty(ref this._prenom, value);
        }

        public string Nom
        {
            get => this._nom;
            set => SetProperty(ref this._nom, value);
        }
    }
}
