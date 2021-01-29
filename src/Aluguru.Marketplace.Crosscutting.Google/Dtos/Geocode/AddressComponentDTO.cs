using System.Text.Json.Serialization;

namespace Aluguru.Marketplace.Crosscutting.Google.Dtos.Geocode
{
    public class AddressComponentDTO
    {
        [JsonPropertyName("long_name")]
        public string LongName { get; set; }
        [JsonPropertyName("short_name")]
        public string ShortName { get; set; }
        [JsonPropertyName("types")]
        public string[] Types { get; set; }
    }
}
