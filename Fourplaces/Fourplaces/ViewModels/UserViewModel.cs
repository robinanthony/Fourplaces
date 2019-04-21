using Fourplaces.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;
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

        private Boolean PatchDataNeeded;
        private MediaFile _image;

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

        private string _newFirstName;
        public string NewFirstName
        {
            get
            {
                return this._newFirstName;
            }
            set
            {
                SetProperty(ref this._newFirstName, value);
                PatchDataNeeded = true;
            }
        }

        private string _newLastName;
        public string NewLastName
        {
            get
            {
                return this._newLastName;
            }
            set
            {
                SetProperty(ref this._newLastName, value);
                PatchDataNeeded = true;
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

                NewFirstName = MyUser.FirstName;
                NewLastName = MyUser.LastName;

                PatchDataNeeded = false;
            }
            else
            {
                // deconnexion ?
            }
        }

        public UserViewModel()
        {
            this.PatchPassword = new Command(PacthPasswordClicked);
            this.PatchMe = new Command(PatchUserData);
            this.TakeAPhoto = new Command(PhotoCommand);
            this.TakeAnImage = new Command(ImageCommand);
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

        private async void PatchUserData()
        {
            // TODO
        }

        private void UpdatePicture()
        {
            if (_image == null)
            {
                MyUser.ImageSource = ImageSource.FromFile("no_pic.jpg");
            }
            else
            {
                MyUser.ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = _image.GetStream();
                    //_image.Dispose();
                    return stream;
                });
            }
            PatchDataNeeded = true;
        }

        private async void PhotoCommand()
        {
            await CrossMedia.Current.Initialize();

            if (CrossMedia.Current.IsCameraAvailable || CrossMedia.Current.IsTakePhotoSupported)
            {
                // Supply media options for saving our photo after it's taken.
                var mediaOptions = new StoreCameraMediaOptions
                {
                    //Directory = "Receipts",
                    Name = DateTime.Now.ToShortTimeString() + ".jpg",
                    PhotoSize = PhotoSize.MaxWidthHeight,
                    MaxWidthHeight = 4096,
                    CompressionQuality = 75,
                    AllowCropping = true
                };

                // Take a photo of the business receipt.
                var file = await CrossMedia.Current.TakePhotoAsync(mediaOptions);

                if (file != null)
                {
                    _image = file;
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
                    _image = file;
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
