using Newtonsoft.Json;
using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace Fourplaces.Models
{
    public class User : NotifierBase
    {
        private int? _idPicture;
        private ImageSource _imageSource;

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "first_name")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "last_name")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

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

        public bool Equals (string email, string password)
        {
            return (email == Email && password == Password);
        }

        public string ToJson()
        {
            return "{\"email\": \""+Email+"\", \"password\":\""+Password+"\"}";
        }

        public User(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
