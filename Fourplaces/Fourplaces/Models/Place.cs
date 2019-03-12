using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Storm.Mvvm;
using Newtonsoft.Json;
using System.IO;
using Xamarin.Forms.Maps;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Fourplaces.Models
{
    public class Place : NotifierBase
    {
        private int? _idPicture;
        private ImageSource _imageSource;

        [JsonProperty(PropertyName = "id")]
        public long Id { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "image_id", NullValueHandling = NullValueHandling.Include)]
        public int? IdPicture
        {
            get
            {
                return _idPicture;
            }
            set
            {
                _idPicture = value;
                Console.WriteLine(value);
                updatePicture();
            }
        }

        public async void updatePicture()
        {
            if (_idPicture == null)
            {
                ImageSource = ImageSource.FromFile("no_pic.jpg");
            }
            else
            {
                byte[] stream = await RestService.Rest.loadPicture(IdPicture);
                ImageSource = ImageSource.FromStream(() => new MemoryStream(stream));
                Console.WriteLine("Bien chargé !");
            }
        }

        public ImageSource ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
        }

        [JsonProperty(PropertyName = "comments")]
        public ObservableCollection<Commentaire> Commentaires { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; } // description

        [JsonProperty(PropertyName = "latitude")]
        public double Latitude { get; set; } // latitude

        [JsonProperty(PropertyName = "longitude")]
        public double Longitude { get; set; } // longitude

        public ICommand DeleteCommand { get; set; }

        public Place()
        {
            this.DeleteCommand = new Command(DeleteAction);
        }

        private void DeleteAction(object _)
        {
            RestService.Rest.Places.Remove(this);
        }

        public Position Position
        {
            get => new Position(Latitude, Longitude);
        }

        public int Distance { get; set; }

        public string TexteDistance
        {
            get
            {
                return "est à " + Distance + " km de vous.";
            }
        }
    }
}
