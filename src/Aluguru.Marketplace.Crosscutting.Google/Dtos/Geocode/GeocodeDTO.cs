using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Aluguru.Marketplace.Crosscutting.Google.Dtos.Geocode
{
    public class GeocodeDTO
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("results")]
        public ResultDTO[] Results { get; set; }
    }
}
