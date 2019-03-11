using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fourplaces.Models
{
    class User
    {
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "first_name")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "last_name")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

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
