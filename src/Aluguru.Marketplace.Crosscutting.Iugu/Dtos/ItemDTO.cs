using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Aluguru.Marketplace.Crosscutting.Iugu.Dtos
{
    public class ItemDTO
    {
        public string Description { get; set; }
        public string Quantity { get; set; }
        [JsonPropertyName("price_cents")]
        public string PriceCents { get; set; }
    }
}
