using Newtonsoft.Json;
using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fourplaces.Models
{
    public class Commentaire : NotifierBase
    {
        [JsonProperty(PropertyName = "date")]
        public string Date { get; set; }

        [JsonProperty(PropertyName = "author")]
        public UserData Auteur { get; set; }

        [JsonProperty(PropertyName ="text")]
        public string Texte { get; set; }

        public string NomComplet
        {
            get => Auteur.FirstName + " " + Auteur.LastName;
        }
    }
}
