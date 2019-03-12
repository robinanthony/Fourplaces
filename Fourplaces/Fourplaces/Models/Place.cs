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
    public class Place : NotifierBase, IComparable
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

        public Position Position
        {
            get => new Position(Latitude, Longitude);
        }

        private int _distance;
        public int Distance {
            get
            {
                return _distance;
            }
            set
            {
                SetProperty(ref _distance, value);
                TexteDistance = "est à " + _distance + " km de vous.";
            }
        }

        private string _texteDistance;
        public string TexteDistance
        {
            get => _texteDistance;
            set => SetProperty(ref _texteDistance, value);
        }

        public int CompareTo(object o)
        {
            Place b = (Place)o;
            if (Distance > b.Distance)
            {
                return 1;
            }
            return 0;
        }
    }
}
