using Fourplaces.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Storm.Mvvm;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Fourplaces.ViewModels
{
    public class UserViewModel : ViewModelBase
    {
//==============================================================================
//================================= ATTRIBUTS ==================================
//==============================================================================
        public ICommand TakeAPhoto { get; private set; }
        public ICommand TakeAnImage { get; private set; }
        public ICommand PatchMe { get; private set; }
        public ICommand PatchPassword { get; private set; }

        private string _titleLabel;
        public string TitleLabel
        {
            get => this._titleLabel;
            set => SetProperty(ref this._titleLabel, value);
        }

        private string _oldPassword;
        public string OldPassword
        {
            get => this._oldPassword;
            set => SetProperty(ref this._oldPassword, value);
        }

        private string _newPassword;
        public string NewPassword
        {
            get => this._newPassword;
            set => SetProperty(ref this._newPassword, value);
        }

        private string _newPasswordBis;
        public string NewPasswordBis
        {
            get => this._newPasswordBis;
            set => SetProperty(ref this._newPasswordBis, value);
        }

        private string _newFirstName;
        public string NewFirstName
        {
            get => this._newFirstName;
            set => SetProperty(ref this._newFirstName, value);
        }

        private string _newLastName;
        public string NewLastName
        {
            get => this._newLastName;
            set => SetProperty(ref this._newLastName, value);
        }

        private Boolean PicNeedPatch;
        private MediaFile _image;
        private ImageSource _newPicture;
        public ImageSource NewPicture
        {
            get => this._newPicture;
            set => SetProperty(ref this._newPicture, value);
        }

        private UserData _myUser;
        public UserData MyUser
        {
            get => this._myUser;
            set
            {
                SetProperty(ref this._myUser, value);
                this.NewFirstName = this.MyUser.FirstName;
                this.NewLastName = this.MyUser.LastName;
                this.NewPicture = this.MyUser.ImageSource;
            }
        }

//==============================================================================
//============================== FCT PRINCIPALES ===============================
//==============================================================================
        public UserViewModel()
        {
            this.PatchPassword = new Command(this.PacthPasswordClicked);
            this.PatchMe = new Command(this.PatchUserData);
            this.TakeAPhoto = new Command(this.PhotoCommand);
            this.TakeAnImage = new Command(this.ImageCommand);
        }

        public override async Task OnResume()
        {
            await base.OnResume();

            this.TitleLabel = "Informations utilisateur";

            (Boolean test, UserData data) = await RestService.Rest.GetUserData();

            if (test)
            {
                this.MyUser = data;
                this.PicNeedPatch = false;
            }
            else
            {
                // deconnexion ?
            }
        }

//==============================================================================
//============================== FCT SECONDAIRES ===============================
//==============================================================================
        private void PacthPasswordClicked()
        {
            PatchPwd();
        }

        private async void PatchPwd()
        {
            if (!string.IsNullOrWhiteSpace(this.NewPassword) && !string.IsNullOrWhiteSpace(this.NewPasswordBis) && !string.IsNullOrWhiteSpace(this.OldPassword))
            {
                if (this.NewPassword == this.NewPasswordBis)
                {
                    (Boolean test, string message) = await RestService.Rest.PatchPassword(this.OldPassword, this.NewPassword);

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

            this.OldPassword = "";
            this.NewPassword = "";
            this.NewPasswordBis = "";
        }

        private async void PatchUserData()
        {
            if ( this.PicNeedPatch)
            { // Si l'image a été modifiée
                MemoryStream memoryStream = new MemoryStream();
                this._image.GetStream().CopyTo(memoryStream);
                byte[] pictureArray = memoryStream.ToArray();

                (Boolean test, string message) = await RestService.Rest.PatchUser(this.NewFirstName, this.NewLastName, pictureArray);
                await App.Current.MainPage.DisplayAlert("Mise à jour utilisateur", message, "OK");
            }
            else if (!this.MyUser.FirstName.Equals(this.NewFirstName) || !this.MyUser.LastName.Equals(this.NewLastName))
            { // Si l'identité d'user a été modifié
                (Boolean test, string message) = await RestService.Rest.PatchUser(this.NewFirstName, this.NewLastName, null);
                await App.Current.MainPage.DisplayAlert("Mise à jour utilisateur", message, "OK");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Mise à jour utilisateur", "Aucune modification détectée.", "OK");
            }
        }

        private void UpdatePicture()
        {
            if (this._image == null)
            {
                this.NewPicture = ImageSource.FromFile("no_pic.jpg");
            }
            else
            {
                this.NewPicture = ImageSource.FromStream(() =>
                {
                    var stream = this._image.GetStream();
                    return stream;
                });
            }
            this.PicNeedPatch = true;
        }

        private async void PhotoCommand()
        {
            await CrossMedia.Current.Initialize();

            if (CrossMedia.Current.IsCameraAvailable || CrossMedia.Current.IsTakePhotoSupported)
            {
                var mediaOptions = new StoreCameraMediaOptions
                {
                    Name = DateTime.Now.ToShortTimeString() + ".jpg",
                    PhotoSize = PhotoSize.MaxWidthHeight,
                    MaxWidthHeight = 4096,
                    CompressionQuality = 75,
                    AllowCropping = true
                };
                
                var file = await CrossMedia.Current.TakePhotoAsync(mediaOptions);

                if (file != null)
                {
                    this._image = file;
                    UpdatePicture();
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("No Camera", " No camera available.", "OK");
            }
        }

        private async void ImageCommand()
        {
            await CrossMedia.Current.Initialize();

            if (CrossMedia.Current.IsPickPhotoSupported)
            {
                var mediaOptions = new PickMediaOptions
                {
                    PhotoSize = PhotoSize.MaxWidthHeight,
                    MaxWidthHeight = 4096,
                    CompressionQuality = 75
                };

                var file = await CrossMedia.Current.PickPhotoAsync(mediaOptions);

                if (file != null)
                {
                    this._image = file;
                    UpdatePicture();
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Photo picking unsupported", "Terrible erreur", "Continue");
            }
        }

    }
}
