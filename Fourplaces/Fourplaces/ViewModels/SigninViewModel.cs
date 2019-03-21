﻿using Fourplaces.Models;
using Storm.Mvvm;
using System.Collections.Generic;
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
        private string _firstName;
        private string _lastName;

        public SigninViewModel()
        {
            this.TitleLabel = "Création d'un nouveau compte";
            this.SigninCommand = new Command(SigninClicked);
        }

        private void SigninClicked(object _)
        {
            Signin();
        }

        private async void Signin()
        {
            if (!string.IsNullOrWhiteSpace(Email))
            {
                if (!string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrWhiteSpace(LastName))
                {
                    if (!string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(PasswordTwo))
                    {
                        if((Password == PasswordTwo))
                        {
                            if (await RestService.Rest.SignIn(Email, Password, FirstName, LastName))
                            {
                                await Application.Current.MainPage.DisplayAlert("Inscription", "Votre compte a été crée. Bienvenue sur l'application Fourplaces !", "OK");

                                await NavigationService.PopAsync();
                                await NavigationService.PushAsync<AllPlace>(new Dictionary<string, object>());
                            }
                            else
                            {
                                await Application.Current.MainPage.DisplayAlert("Inscription", "Erreur lors de la tentative de création du compte. Veuillez réessayer.", "OK");
                            }
                        }
                        else
                        { // Password != PasswordTwo
                            await Application.Current.MainPage.DisplayAlert("Inscription", "La vérification du mot de passe à échouée.", "OK");
                        }
                    }
                    else
                    { // Problème password or passwordTwo
                        await Application.Current.MainPage.DisplayAlert("Inscription", "Votre mot de passe ...", "OK"); // TODO
                    }
                }
                else
                { // Problème firstName ou lastName
                    await Application.Current.MainPage.DisplayAlert("Inscription", "Votre nom et prénom ...", "OK"); // TODO
                }
            }
            else
            { // Problème email
                await Application.Current.MainPage.DisplayAlert("Inscription", "Votre adresse email ...", "OK"); // TODO
            }
            Password = "";
            PasswordTwo = "";
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

        public string FirstName
        {
            get => this._firstName;
            set => SetProperty(ref this._firstName, value);
        }

        public string LastName
        {
            get => this._lastName;
            set => SetProperty(ref this._lastName, value);
        }
    }
}
