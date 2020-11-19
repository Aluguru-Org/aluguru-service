using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Aluguru.Marketplace.Crosscutting.Iugu.Dtos
{
    public class ItemDTO
    {
        public string Description { get; set; }
        public int Quantity { get; set; }
        [JsonPropertyName("price_cents")]
        public int PriceCents { get; set; }
    }
}
