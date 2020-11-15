using Aluguru.Marketplace.Domain;
using System.Text.Json.Serialization;

namespace Aluguru.Marketplace.Payment.Dtos
{
    public class InvoiceStatusChangedDTO : IDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("account_id")]
        public string AccountId { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }        
    }
}
