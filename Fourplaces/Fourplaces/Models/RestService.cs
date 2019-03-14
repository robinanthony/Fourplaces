﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;
using System.Linq;

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
            //this.client.MaxResponseContentBufferSize = 256000 * 2; // Mis en commentaire pour que le buffer puisse grandir autant que besoin.
        }

        public ObservableCollection<Place> Places { get; private set; }

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

                    List<Place> needOrder = new List<Place>();

                    foreach (Place p in restResponse.Data)
                    {
                        p.Distance = GetDistanceBetweenPositions(p.Position, MaLocation);
                        needOrder.Add(p);
                    }
                    needOrder.Sort(Place.Comparaison);
                    // TODO : Trier l'ObervableCollection<Place> par apport à la Distance.
                    Places = new ObservableCollection<Place>(needOrder);
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