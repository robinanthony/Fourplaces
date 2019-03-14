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
                return this._idPicture;
            }
            set
            {
                this._idPicture = value;
                UpdatePicture();
            }
        }

        private async void UpdatePicture()
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
            get => this._imageSource;
            set => SetProperty(ref _imageSource, value);
        }

        [JsonProperty(PropertyName = "comments")]
        public ObservableCollection<Commentaire> Commentaires { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "latitude")]
        public double Latitude { get; set; }

        [JsonProperty(PropertyName = "longitude")]
        public double Longitude { get; set; }

        public Position Position
        {
            get => new Position(Latitude, Longitude);
        }

        private int _distance;
        public int Distance {
            get
            {
                return this._distance;
            }
            set
            {
                SetProperty(ref this._distance, value);
                TexteDistance = "est à " + this._distance + " km de vous.";
            }
        }

        private string _texteDistance;
        public string TexteDistance
        {
            get => this._texteDistance;
            set => SetProperty(ref this._texteDistance, value);
        }

        public static int Comparaison(Place p1, Place p2)
        {
            if (p1.Distance == p2.Distance)
            {
                return 0;
            }
            if (p1.Distance > p2.Distance)
            {
                return 1;
            }
            return -1;
        }
    }
}
