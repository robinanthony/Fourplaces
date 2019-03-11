using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Storm.Mvvm;
using Newtonsoft.Json;
using System.IO;
using Xamarin.Forms.Maps;

namespace Fourplaces.Models
{
    public class Place : NotifierBase
    {
        private int _idPicture;
        private ImageSource _imageSource;

        [JsonProperty(PropertyName = "id")]
        public long Id { get; set; } // id

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; } // title

        [JsonProperty(PropertyName = "image_id")]
        public int IdPicture
        {
            get => _idPicture;
            set
            {
                _idPicture = value;
                updatePicture();
            }
        } // image id

        public async void updatePicture()
        {
            byte[] stream = await RestService.Rest.loadPicture(IdPicture);
            ImageSource = ImageSource.FromStream(() => new MemoryStream(stream));
        }

        public ImageSource ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
        }

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
    }
}
