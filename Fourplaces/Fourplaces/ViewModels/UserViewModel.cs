using Fourplaces.Models;
using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Fourplaces.ViewModels
{
    public class UserViewModel : ViewModelBase
    {
        public Command TakeAPhoto { get; private set; }
        public Command TakeAnImage { get; private set; }
        public Command PatchMe { get; private set; }
        public Command PatchPassword { get; private set; }

        private string _oldPassword;
        public string OldPassword
        {
            get
            {
                return this._oldPassword;
            }
            set
            {
                SetProperty(ref this._oldPassword, value);
            }
        }

        private string _newPassword;
        public string NewPassword
        {
            get
            {
                return this._newPassword;
            }
            set
            {
                SetProperty(ref this._newPassword, value);
            }
        }

        private string _newPasswordBis;
        public string NewPasswordBis
        {
            get
            {
                return this._newPasswordBis;
            }
            set
            {
                SetProperty(ref this._newPasswordBis, value);
            }
        }

        private UserData _myUser;
        public UserData MyUser
        {
            get
            {
                return this._myUser;
            }
            set
            {
                SetProperty(ref this._myUser, value);
            }
        }

        public override async Task OnResume()
        {
            await base.OnResume();
            (Boolean test, UserData data) = await RestService.Rest.GetUserData();

            if (test)
            {
                MyUser = data;
            }
            else
            {
                // deconnexion ?
            }
        }

        public UserViewModel()
        {
            this.PatchPassword = new Command(PacthPasswordClicked);
        }

        private void PacthPasswordClicked()
        {
            PatchPwd();
        }

        private async void PatchPwd()
        {
            if (!string.IsNullOrWhiteSpace(NewPassword) && !string.IsNullOrWhiteSpace(NewPasswordBis) && !string.IsNullOrWhiteSpace(OldPassword))
            {
                if (NewPassword == NewPasswordBis)
                {
                    (Boolean test, string message) = await RestService.Rest.PatchPassword(OldPassword, NewPassword);

                    if (test)
                    {
                        await Application.Current.MainPage.DisplayAlert("Modification du mot de passe", message, "OK");
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Modification du mot de passe", message, "OK");
                    }
                }
                else
                { // NewPassword != NewPasswordBis
                    await Application.Current.MainPage.DisplayAlert("Modification du mot de passe", "Vous devez copier identiquement le nouveau mot de passe deux fois.", "OK");
                }
            }
            else
            { // Problème taille passwords
                await Application.Current.MainPage.DisplayAlert("Modification du mot de passe", "Votre mot de passe ne peut être vide.", "OK");
            }

            OldPassword = "";
            NewPassword = "";
            NewPasswordBis = "";
        }
    }
}
