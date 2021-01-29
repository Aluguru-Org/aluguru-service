using System.Text.Json.Serialization;

namespace Aluguru.Marketplace.Crosscutting.Google.Dtos.DistanceMatrix
{
    public class RowDTO
    {
        [JsonPropertyName("elements")]
        public ElementDTO[] Elements { get; set; }
    }
}
