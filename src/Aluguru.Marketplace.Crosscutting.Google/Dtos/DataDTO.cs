using System.Text.Json.Serialization;

namespace Aluguru.Marketplace.Crosscutting.Google.Dtos
{
    public class DataDTO
    {
        [JsonPropertyName("value")]
        public int Value { get; set; }
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}
