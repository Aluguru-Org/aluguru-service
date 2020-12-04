using System.Text.Json.Serialization;

namespace Aluguru.Marketplace.Crosscutting.Google.Dtos
{
    public class ElementDTO
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("duration")]
        public DataDTO Duration { get; set; }
        [JsonPropertyName("distance")]
        public DataDTO Distance { get; set; }
    }
}
