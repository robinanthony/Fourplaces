using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Fourplaces.Models
{
    class RestService
    {
        HttpClient client;

        private static RestService _rest;

        public static RestService Rest
        {
            get
            {
                if (_rest == null)
                {
                    _rest = new RestService();
                }
                return _rest;
            }
        }

        private RestService()
        {
            this.client = new HttpClient();
            this.client.MaxResponseContentBufferSize = 256000;
        }

        public ObservableCollection<Place> Places { get; private set; }

        public async Task<ObservableCollection<Place>> LoadPlaces()
        {
            Places = new ObservableCollection<Place>();

            string RestUrl = "http://td-api.julienmialon.com/places";
            var uri = new Uri(string.Format(RestUrl, string.Empty));

            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    RestResponse<ObservableCollection<Place>> restResponse = JsonConvert.DeserializeObject<RestResponse<ObservableCollection<Place>>>(json);
                    Places = restResponse.Data;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return Places;
        }

        public async Task<byte[]> loadPicture(int? idPicture)
        {
            byte[] stream = null;

            string RestUrl = "http://td-api.julienmialon.com/images/" + idPicture;
            var uri = new Uri(string.Format(RestUrl, string.Empty));

            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    stream = await response.Content.ReadAsByteArrayAsync();
                }
                else
                {
                    Debug.WriteLine("Plantage dans le chargement d'une image");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return stream;
        }

        public async Task<User> LoadUser(string email, string password)
        {
            User utilisateur = new User(email, password);

            string RestUrl = "http://td-api.julienmialon.com/auth/login";
            var uri = new Uri(string.Format(RestUrl, string.Empty));

            var stringContent = new StringContent(utilisateur.ToJson(), Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync(uri, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    RestResponse<Token> restResponse = JsonConvert.DeserializeObject<RestResponse<Token>>(json);
                }
                else
                {
                    Token.Ticket.AccessToken = null;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return utilisateur;
        }

        public Place MaPlace { get; private set; }

        public async Task<Place> LoadPlace(long idPlace)
        {
            //MaPlace = new Place();

            string RestUrl = "http://td-api.julienmialon.com/places/"+ idPlace;
            var uri = new Uri(string.Format(RestUrl, string.Empty));

            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    RestResponse<Place> restResponse = JsonConvert.DeserializeObject<RestResponse<Place>>(json);
                    MaPlace = restResponse.Data;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return MaPlace;
        }
    }
}
;