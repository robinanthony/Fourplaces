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
        public string TitleLabel { get; set; }


        public string _email;
        public string _password;

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
            TitleLabel = "Veuillez vous connecter";
        }

        private void LoginClicked(object _)
        {
            OpenFocusPlace();
        }

        public async void OpenFocusPlace()
        {
            Console.WriteLine(Email);
            Console.WriteLine(Password);
            await RestService.Rest.LogIn(Email, Password);

            if (Token.IsInit())
            {
                Password = "";
                await NavigationService.PushAsync<AllPlace>(new Dictionary<string, object>());
            }
            else
            {
                // TODO : Qu'est-ce que je fais ici ?
            }
        }

        public override async Task OnResume()
        {
            await base.OnResume();
            Token.Destroy();
        }
    }
}
