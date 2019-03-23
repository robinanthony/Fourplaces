using Plugin.Media;
using Storm.Mvvm;
using System;
using Xamarin.Forms;

namespace Fourplaces.ViewModels
{
    public class NewPlaceViewModel : ViewModelBase
    {
        private string _nomLieu;
        private string _descriptionLieu;

        private string _latitudeLieu;
        private string _longitudeLieu;

        private Plugin.Media.Abstractions.MediaFile _image;

        public Command TakeAPhoto { get; private set; }
        public Command TakeAnImage { get; private set; }
        public Command GetPosition { get; private set; }
        public Command AddPlace { get; private set; }
        
        public string TitleLabel { get; set; }

        public string Nom
        {
            get => _nomLieu;
            set => SetProperty(ref _nomLieu, value);
        }

        public string Description
        {
            get => _descriptionLieu;
            set => SetProperty(ref _descriptionLieu, value);
        }

        public string Latitude
        {
            get => _latitudeLieu;
            set => SetProperty(ref _latitudeLieu, value);
        }

        public string Longitude
        {
            get => _longitudeLieu;
            set => SetProperty(ref _longitudeLieu, value);
        }

        private async void PhotoCommand()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await App.Current.MainPage.DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Name = DateTime.Now.ToShortTimeString() + ".jpg"
            });

            if (file != null)
            {
                _image = file;
            }
        }

        private async void ImageCommand()
        {
            if (CrossMedia.Current.IsPickPhotoSupported)
            {
                var file = await CrossMedia.Current.PickPhotoAsync();

                if (file != null)
                {
                    _image = file;
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Photo picking unsupported", "Terrible erreur", "Continue");
            }
        }

        private async void PositionCommand()
        {
            await App.Current.MainPage.DisplayAlert("position command", "clicked !", "Ok");

        }

        private async void AddCommand()
        {
            await App.Current.MainPage.DisplayAlert("add command", "clicked !", "ok");

        }

        public NewPlaceViewModel()
        {
            this.TitleLabel = "Ajout d'un lieu";
            this.TakeAnImage = new Command(ImageCommand);
            this.TakeAPhoto = new Command(PhotoCommand);
            this.GetPosition = new Command(PositionCommand);
            this.AddPlace = new Command(AddCommand);
        }
    }
}
