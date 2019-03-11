﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace Fourplaces.Models
{
    class RestResponse<T>
    {

        [JsonProperty(PropertyName = "data", DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue(null)]
        public T Data { get; set; }

        [JsonProperty(PropertyName = "is_success")]
        public string IsSuccess { get; set; }

        [JsonProperty(PropertyName = "error_code")]
        public string ErrorCode { get; set; }

        [JsonProperty(PropertyName = "error_message")]
        public string ErrorMessage { get; set; }
    }
}