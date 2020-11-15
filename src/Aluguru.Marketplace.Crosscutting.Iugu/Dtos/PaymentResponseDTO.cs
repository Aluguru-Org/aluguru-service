using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Aluguru.Marketplace.Crosscutting.Iugu.Dtos
{
    public class PaymentResponseDTO
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("errors")]
        public Dictionary<string, List<string>> Errors { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("pdf")]
        public string Pdf { get; set; }

        [JsonPropertyName("identification")]
        public string Identification { get; set; }

        [JsonPropertyName("invoice_id")]
        public string InvoiceId { get; set; }

        public string LR { get; set; }
    }
}
