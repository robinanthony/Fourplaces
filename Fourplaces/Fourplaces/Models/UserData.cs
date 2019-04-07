using Newtonsoft.Json;
using Storm.Mvvm;
using System.IO;
using Xamarin.Forms;

namespace Fourplaces.Models
{
    public class UserData : NotifierBase
    {
        private int? _idPicture;
        private ImageSource _imageSource;

        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

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
                byte[] stream = await RestService.Rest.LoadPicture(IdPicture);
                ImageSource = ImageSource.FromStream(() => new MemoryStream(stream));
            }
        }

        public ImageSource ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
        }

        public UserData(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public UserData(string email, string password, string firstName, string lastName)
        {
            Email = email;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
        }

        [JsonConstructor]
        public UserData(int id, string first_name, string last_name, string email, int? image_id)
        {
            Id = id;
            FirstName = first_name;
            LastName = last_name;
            Email = email;
            IdPicture = image_id;
        }
    }
}
