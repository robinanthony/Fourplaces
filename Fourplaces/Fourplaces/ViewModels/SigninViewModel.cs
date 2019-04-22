using Fourplaces.Models;
using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace Fourplaces.ViewModels
{
    class SigninViewModel : ViewModelBase
    {
//==============================================================================
//================================= ATTRIBUTS ==================================
//==============================================================================
        public ICommand SigninCommand { get; private set; }

        private string _titleLabel;
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

        private string _passwordTwo;
        public string PasswordTwo
        {
            get => this._passwordTwo;
            set => SetProperty(ref this._passwordTwo, value);
        }

        private string _firstName;
        public string FirstName
        {
            get => this._firstName;
            set => SetProperty(ref this._firstName, value);
        }

        private string _lastName;
        public string LastName
        {
            get => this._lastName;
            set => SetProperty(ref this._lastName, value);
        }

//==============================================================================
//============================== FCT PRINCIPALES ===============================
//==============================================================================
        public SigninViewModel()
        {
            this.TitleLabel = "Création d'un nouveau compte";
            this.SigninCommand = new Command(this.SigninClicked);
        }

//==============================================================================
//============================== FCT SECONDAIRES ===============================
//==============================================================================
        private void SigninClicked()
        {
            Signin();
        }

        private async void Signin()
        {
            if (!string.IsNullOrWhiteSpace(this.Email))
            {
                if (!string.IsNullOrWhiteSpace(this.FirstName) && !string.IsNullOrWhiteSpace(this.LastName))
                {
                    if (!string.IsNullOrWhiteSpace(this.Password) && !string.IsNullOrWhiteSpace(this.PasswordTwo))
                    {
                        if((this.Password == this.PasswordTwo))
                        {
                            (Boolean test, string message) = await RestService.Rest.SignIn(this.Email, this.Password, this.FirstName, this.LastName);
                            if (test)
                            {
                                await Application.Current.MainPage.DisplayAlert("Inscription", message, "OK");

                                await NavigationService.PopAsync();
                                await NavigationService.PushAsync<AllPlace>(new Dictionary<string, object>());
                            }
                            else
                            {
                                await Application.Current.MainPage.DisplayAlert("Inscription", message, "OK");
                            }
                        }
                        else
                        { // Password != PasswordTwo
                            await Application.Current.MainPage.DisplayAlert("Inscription", "La vérification du mot de passe à échouée.", "OK");
                        }
                    }
                    else
                    { // Problème password or passwordTwo
                        await Application.Current.MainPage.DisplayAlert("Inscription", "Votre mot de passe ne peut être vide.", "OK");
                    }
                }
                else
                { // Problème firstName ou lastName
                    await Application.Current.MainPage.DisplayAlert("Inscription", "Votre nom et prénom ne peuvent être vide.", "OK");
                }
            }
            else
            { // Problème email
                await Application.Current.MainPage.DisplayAlert("Inscription", "Votre adresse email ne peut être vide.", "OK");
            }
            this.Password = "";
            this.PasswordTwo = "";
        }

    }
}
