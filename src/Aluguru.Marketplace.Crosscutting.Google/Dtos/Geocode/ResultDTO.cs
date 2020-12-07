using System.Text.Json.Serialization;

namespace Aluguru.Marketplace.Crosscutting.Google.Dtos.Geocode
{
    public class ResultDTO
    {
        [JsonPropertyName("address_components")]
        public AddressComponentDTO[] AddressComponents { get; set; }
        [JsonPropertyName("formatted_address")]
        public string FormattedAddress { get; set; }
        [JsonPropertyName("place_id")]
        public string PlaceId { get; set; }
        [JsonPropertyName("types")]
        public string[] Types { get; set; }
    }
}
