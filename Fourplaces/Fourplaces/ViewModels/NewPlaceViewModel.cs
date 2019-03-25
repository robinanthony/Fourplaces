using Plugin.Media;
using Plugin.Media.Abstractions;
using Storm.Mvvm;
using System;
using System.IO;
using Xamarin.Forms;

namespace Fourplaces.ViewModels
{
    public class NewPlaceViewModel : ViewModelBase
    {
        private string _nomLieu;
        private string _descriptionLieu;

        private string _latitudeLieu;
        private string _longitudeLieu;

        private MediaFile _image;
        private ImageSource _imageSource;

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

        public ImageSource ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
        }

        private void UpdatePicture()
        {
            if (_image == null)
            {
                ImageSource = ImageSource.FromFile("no_pic.jpg");
            }
            else
            {
                ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = _image.GetStream();
                    _image.Dispose();
                    return stream;
                });
            }
        }

        private async void PhotoCommand()
        {
            await CrossMedia.Current.Initialize();

            if (CrossMedia.Current.IsCameraAvailable || CrossMedia.Current.IsTakePhotoSupported)
            {
                // Supply media options for saving our photo after it's taken.
                var mediaOptions = new StoreCameraMediaOptions
                {
                    Directory = "Receipts",
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
                var mediaOptions = new PickMediaOptions {
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

        private async void PositionCommand()
        {
            // TODO
            await App.Current.MainPage.DisplayAlert("position command", "clicked !", "Ok");

        }

        private async void AddCommand()
        {
            // TODO 
            await App.Current.MainPage.DisplayAlert("add command", "clicked !", "ok");

        }

        public NewPlaceViewModel()
        {
            this.TitleLabel = "Ajout d'un lieu";
            this.TakeAnImage = new Command(ImageCommand);
            this.TakeAPhoto = new Command(PhotoCommand);
            this.GetPosition = new Command(PositionCommand);
            this.AddPlace = new Command(AddCommand);

            UpdatePicture();
        }
    }
}
