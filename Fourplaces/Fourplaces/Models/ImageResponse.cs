using Newtonsoft.Json;

namespace Fourplaces.Models
{
    public class ImageResponse
    {
//==============================================================================
//================================= ATTRIBUTS ==================================
//==============================================================================
        [JsonProperty(PropertyName = "id")]
        public int IdNewImage { get; set; }
    }
}
