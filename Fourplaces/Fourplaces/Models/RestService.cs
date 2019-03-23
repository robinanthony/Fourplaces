using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace Fourplaces.Models
{
    class RestService
    {
        private HttpClient client;

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
            //this.client.MaxResponseContentBufferSize = 256000 * 2; // Mis en commentaire pour que le buffer puisse grandir autant que besoin.
        }
        
        private int GetDistanceBetweenPositions(Position source, Position dest)
        {
            int R = 6378;

            double SourceLat = GetRadian(source.Latitude);
            double SourceLong = GetRadian(source.Longitude);
            double DestLat = GetRadian(dest.Latitude);
            double DestLong = GetRadian(dest.Longitude);

            return (int)(R * (Math.PI / 2 - Math.Asin(Math.Sin(DestLat) * Math.Sin(SourceLat) + Math.Cos(DestLong - SourceLong) * Math.Cos(DestLat) * Math.Cos(SourceLat))));
        }

        private double GetRadian(double degree)
        {
            return Math.PI * degree / 180;
        }

        public async Task<ObservableCollection<Place>> LoadPlaces(Position MaLocation)
        {
            ObservableCollection<Place> _places = new ObservableCollection<Place>();

            string RestUrl = "https://td-api.julienmialon.com/places";
            var uri = new Uri(string.Format(RestUrl, string.Empty));

            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    RestResponse<ObservableCollection<Place>> restResponse = JsonConvert.DeserializeObject<RestResponse<ObservableCollection<Place>>>(json);

                    List<Place> needOrder = new List<Place>();

                    if ("true".Equals(restResponse.IsSuccess))
                    {
                        foreach (Place p in restResponse.Data)
                        {
                            p.Distance = GetDistanceBetweenPositions(p.Position, MaLocation);
                            needOrder.Add(p);
                        }
                        needOrder.Sort(Place.Comparaison);
                        _places = new ObservableCollection<Place>(needOrder);
                    }
                    else
                    { 
                        // TODO Qu'est-ce que je fais ici?
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return _places;
        }

        public async Task<byte[]> LoadPicture(int? idPicture)
        {
            byte[] stream = null;

            string RestUrl = "https://td-api.julienmialon.com/images/" + idPicture;
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

        public async Task LogIn(string email, string password)
        {
            User utilisateur = new User(email, password);

            string RestUrl = "https://td-api.julienmialon.com/auth/login";
            var uri = new Uri(string.Format(RestUrl, string.Empty));

            var stringContent = new StringContent(JsonConvert.SerializeObject(utilisateur), Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync(uri, stringContent);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    RestResponse<Token> restResponse = JsonConvert.DeserializeObject<RestResponse<Token>>(json);

                    if ("true".Equals(restResponse.IsSuccess))
                    {
                        Token.Ticket = restResponse.Data;
                    }
                    else
                    {
                        // Qu'est-ce que je fais ici?
                        Token.Destroy();
                    }
                }
                else
                {
                    Token.Destroy();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
        
        private Place maPlace;

        public async Task<Place> LoadPlace(long idPlace)
        {
            string RestUrl = "https://td-api.julienmialon.com/places/"+ idPlace;
            var uri = new Uri(string.Format(RestUrl, string.Empty));

            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    RestResponse<Place> restResponse = JsonConvert.DeserializeObject<RestResponse<Place>>(json);

                    if ("true".Equals(restResponse.IsSuccess))
                    {
                        maPlace = restResponse.Data;
                    }
                    else
                    {
                        // TODO Qu'est-ce que je fais ici?
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return maPlace;
        }

        public async Task<(Boolean, string)> SignIn(string email, string password, string firstName, string lastName)
        {
            User utilisateur = new User(email, password, firstName, lastName);

            string RestUrl = "https://td-api.julienmialon.com/auth/register";
            var uri = new Uri(string.Format(RestUrl, string.Empty));

            var stringContent = new StringContent(JsonConvert.SerializeObject(utilisateur), Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync(uri, stringContent);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    RestResponse<Token> restResponse = JsonConvert.DeserializeObject<RestResponse<Token>>(json);

                    if ("true".Equals(restResponse.IsSuccess))
                    {
                        Token.Ticket = restResponse.Data;
                        return (true, "Votre compte a été crée. Bienvenue sur l'application Fourplaces !");
                    }
                    else
                    {
                        return (false, "L'adresse email demandée est déjà utilisée. Veuillez en utiliser une autre ou vous connecter.");
                    }
                }
                else
                {
                    Token.Destroy();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return (false, "Erreur lors de la tentative de création du compte. Veuillez réessayer.");
        }
    }
}