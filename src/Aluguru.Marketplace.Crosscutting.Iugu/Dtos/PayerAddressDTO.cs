using System.Text.Json.Serialization;

namespace Aluguru.Marketplace.Crosscutting.Iugu.Dtos
{
    public class PayerAddressDTO
    {
        public string Street { get; set; }

        public string Number { get; set; }

        public string District { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        [JsonPropertyName("zip_code")]
        public string ZipCode { get; set; }

        public string Complement { get; set; }
    }
}
