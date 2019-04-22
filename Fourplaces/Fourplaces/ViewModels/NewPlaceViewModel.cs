using Fourplaces.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Storm.Mvvm;
using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Forms;

namespace Fourplaces.ViewModels
{
    public class NewPlaceViewModel : ViewModelBase
    {
//==============================================================================
//================================= ATTRIBUTS ==================================
//==============================================================================
        public ICommand TakeAPhoto { get; private set; }
        public ICommand TakeAnImage { get; private set; }
        public ICommand GetPosition { get; private set; }
        public ICommand AddPlace { get; private set; }

        private string _titleLabel;
        public string TitleLabel
        {
            get => this._titleLabel;
            set => SetProperty(ref this._titleLabel, value);
        }

        private string _nomLieu;
        public string Nom
        {
            get => this._nomLieu;
            set => SetProperty(ref this._nomLieu, value);
        }

        private string _descriptionLieu;
        public string Description
        {
            get => this._descriptionLieu;
            set => SetProperty(ref this._descriptionLieu, value);
        }

        private string _latitudeLieu;
        public string Latitude
        {
            get => this._latitudeLieu;
            set => SetProperty(ref this._latitudeLieu, value);
        }

        private string _longitudeLieu;
        public string Longitude
        {
            get => this._longitudeLieu;
            set => SetProperty(ref this._longitudeLieu, value);
        }

        private MediaFile _image;
        private ImageSource _imageSource;
        public ImageSource ImageSource
        {
            get => this._imageSource;
            set => SetProperty(ref this._imageSource, value);
        }

//==============================================================================
//============================== FCT PRINCIPALES ===============================
//==============================================================================
        public NewPlaceViewModel()
        {
            this.TitleLabel = "Ajout d'un lieu";
            this.TakeAnImage = new Command(this.ImageCommand);
            this.TakeAPhoto = new Command(this.PhotoCommand);
            this.GetPosition = new Command(this.PositionCommand);
            this.AddPlace = new Command(this.AddCommand);

            UpdatePicture();
        }

//==============================================================================
//============================== FCT SECONDAIRES ===============================
//==============================================================================
        private void UpdatePicture()
        {
            if (this._image == null)
            {
                this.ImageSource = ImageSource.FromFile("no_pic.jpg");
            }
            else
            {
                this.ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = this._image.GetStream();
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
                var mediaOptions = new PickMediaOptions {
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

        private async void PositionCommand()
        {
            var res = await MyGeolocator.GetLocation();
            this.Latitude = res.Latitude.ToString("G", CultureInfo.InvariantCulture);
            this.Longitude = res.Longitude.ToString("G", CultureInfo.InvariantCulture);
        }

        private async void AddCommand()
        {
            if (this._image != null)
            {
                if (!string.IsNullOrWhiteSpace(this.Nom) && !string.IsNullOrWhiteSpace(this.Description))
                {
                    if (!string.IsNullOrWhiteSpace(this.Latitude) && !string.IsNullOrWhiteSpace(this.Longitude))
                    {
                        string pattern = @"^[\-]?\d+(\.\d+)*$";

                        Regex rgx = new Regex(pattern);

                        if (rgx.IsMatch(this.Latitude) && rgx.IsMatch(this.Longitude))
                        {
                            MemoryStream memoryStream = new MemoryStream();
                            this._image.GetStream().CopyTo(memoryStream);
                            byte[] pictureArray = memoryStream.ToArray();

                            (Boolean test, string infos) = await RestService.Rest.AddPlace(this.Nom, this.Description, pictureArray, this.Latitude, this.Longitude);

                            if (test)
                            {
                                await App.Current.MainPage.DisplayAlert("Ajout d'une place", infos, "ok");
                                await NavigationService.PopAsync();
                            }
                            else
                            {
                                await App.Current.MainPage.DisplayAlert("Ajout d'une place", infos, "ok");
                            }
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("Ajout d'une place", "La lattitude et la longitude doivent être de la forme ^[\\-]?\\d+(\\.\\d+)*$ .", "ok");
                        }
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Ajout d'une place", "Vous devez préciser une position pour votre lieu.", "ok");
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Ajout d'une place", "Vous devez préciser un nom et une description.", "ok");
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Ajout d'une place", "Vous devez selectionner une image.", "ok");
            }
        }

    }
}
