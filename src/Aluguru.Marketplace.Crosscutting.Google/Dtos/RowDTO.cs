using System.Text.Json.Serialization;

namespace Aluguru.Marketplace.Crosscutting.Google.Dtos
{
    public class RowDTO
    {
        [JsonPropertyName("elements")]
        public ElementDTO[] Elements { get; set; }
    }
}
