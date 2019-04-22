using Newtonsoft.Json;

namespace Fourplaces.Models
{
    public class RestResponse
    {
//==============================================================================
//================================= ATTRIBUTS ==================================
//==============================================================================
        [JsonProperty(PropertyName = "is_success")]
        public string IsSuccess { get; set; }

        [JsonProperty(PropertyName = "error_code")]
        public string ErrorCode { get; set; }

        [JsonProperty(PropertyName = "error_message")]
        public string ErrorMessage { get; set; }
    }

    public class RestResponse<T> : RestResponse
    {
//==============================================================================
//================================= ATTRIBUTS ==================================
//==============================================================================
        [JsonProperty(PropertyName = "data")]
        public T Data { get; set; }
    }
}
