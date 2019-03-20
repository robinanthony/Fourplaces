using Fourplaces.Models;
using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Fourplaces.ViewModels
{
    class LoginViewModel : ViewModelBase
    {
        public ICommand LoginCommand{ get; set; }
        public ICommand SigninCommand { get; set; }
        public string TitleLabel { get; set; }

        private string _email;
        private string _password;

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

        public LoginViewModel()
        {
            LoginCommand = new Command(LoginClicked);
            SigninCommand = new Command(SigninClicked);
            TitleLabel = "Veuillez vous connecter";
        }

        private void LoginClicked(object _)
        {
            OpenFocusPlace();
        }

        private async void OpenFocusPlace()
        {
            await RestService.Rest.LogIn(Email, Password);

            if (Token.IsInit())
            {
                await NavigationService.PushAsync<AllPlace>(new Dictionary<string, object>());
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Identifiants éronnés", "Vos identifiants sont invalides. Veuillez réiterer votre demande.", "OK");
            }
            // Dans tous les cas je vide le champ password.
            Password = "";
        }

        private void SigninClicked(object _)
        {
            OpenSignin();
        }

        private async void OpenSignin()
        {
            await NavigationService.PushAsync<Signin>(new Dictionary<string, object>());
        }

        public override async Task OnResume()
        {
            await base.OnResume();
            Token.Destroy();
        }
    }
}
