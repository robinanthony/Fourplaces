using Newtonsoft.Json;

namespace Fourplaces.Models
{
    public class ImageResponse
    {
        [JsonProperty(PropertyName = "id")]
        public int IdNewImage { get; set; }
    }
}
