using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fourplaces.Models
{
    class Token
    {
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = "refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty(PropertyName = "expires_in")]
        public string ExpiresIn { get; set; }

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
        }

        private Token()
        {
            
        }
    }
}