using Newtonsoft.Json;
using System;

namespace Fourplaces.Models
{
    public class Token
    {
//==============================================================================
//================================= ATTRIBUTS ==================================
//==============================================================================
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = "refresh_token")]
        public string RefreshToken { get; set; }

        private DateTime _expiresIn;

        [JsonProperty(PropertyName = "expires_in")]
        public string ExpiresIn {
            get
            {
                return _expiresIn.ToString();
            }
            set
            {
                double add = Convert.ToDouble(value);
                DateTime day = DateTime.Today.AddSeconds(add);
                _expiresIn = new DateTime(day.Year, day.Month, day.Day, day.Hour, day.Minute, day.Second);
            }
        }

        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }

        private static Token _token;
        
        public string Data { get; set; }

        public static Token Ticket
        {
            get
            {
                if (_token == null)
                {
                    _token = new Token();
                }
                return _token;
            }
            set =>  _token = value;
        }

//==============================================================================
//============================== FCT PRINCIPALES ===============================
//==============================================================================
        private Token()
        {   }

        public static void Destroy()
        {
            _token = null;
        }

        public static bool IsInit()
        {
            if (_token == null)
            {
                return false;
            }
            return true;
        }

//==============================================================================
//================================ FCT METIERS =================================
//==============================================================================
        public static void RefreshIfNecessary()
        {
            if (DateTime.Today.AddMinutes(10) >= Ticket._expiresIn)
            {
                Refresh();
                // TODO : Si jamais Refresh fail et que le token n'existe plus (_token == null), il faudrait déconnecter l'utilisateur ...
            }
        }

        public static async void Refresh()
        {
            await RestService.Rest.RefreshToken();
        }
    }
}