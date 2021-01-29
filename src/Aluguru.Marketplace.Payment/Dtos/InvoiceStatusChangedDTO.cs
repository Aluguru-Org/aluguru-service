using Aluguru.Marketplace.Domain;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Aluguru.Marketplace.Payment.Dtos
{
    public class InvoiceStatusChangedDTO : IDto
    {
        public string Event { get; set; }
        public Dictionary<string, string> Data { get; set; }      
    }
}
