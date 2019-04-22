using Fourplaces.Models;
using Storm.Mvvm;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Fourplaces.ViewModels
{
    class LoginViewModel : ViewModelBase
    {
//==============================================================================
//================================= ATTRIBUTS ==================================
//==============================================================================
        public ICommand LoginCommand{ get; private set; }
        public ICommand SigninCommand { get; private set; }

        public string _titleLabel;
        public string TitleLabel
        {
            get => this._titleLabel;
            set => SetProperty(ref this._titleLabel, value);
        }

        private string _email;
        public string Email
        {
            get => this._email;
            set => SetProperty(ref this._email, value);
        }

        private string _password;
        public string Password
        {
            get => this._password;
            set => SetProperty(ref this._password, value);
        }

//==============================================================================
//============================== FCT PRINCIPALES ===============================
//==============================================================================
        public LoginViewModel()
        {
            this.LoginCommand = new Command(this.LoginClicked);
            this.SigninCommand = new Command(this.SigninClicked);
            this.TitleLabel = "Veuillez vous connecter";
        }

        public override async Task OnResume()
        {
            await base.OnResume();
            Token.Destroy();
        }

//==============================================================================
//============================== FCT SECONDAIRES ===============================
//==============================================================================
        private void LoginClicked()
        {
            OpenFocusPlace();
        }

        private async void OpenFocusPlace()
        {
            await RestService.Rest.LogIn(this.Email, this.Password);

            if (Token.IsInit())
            {
                await NavigationService.PushAsync<AllPlace>(new Dictionary<string, object>());
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Identifiants éronnés", "Vos identifiants sont invalides. Veuillez réiterer votre demande.", "OK");
            }
            // Dans tous les cas je vide le champ password.
            this.Password = "";
        }

        private void SigninClicked()
        {
            OpenSignin();
        }

        private async void OpenSignin()
        {
            await NavigationService.PushAsync<Signin>(new Dictionary<string, object>());
        }

    }
}
