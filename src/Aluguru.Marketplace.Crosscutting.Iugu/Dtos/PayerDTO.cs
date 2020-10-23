using System.Text.Json.Serialization;

namespace Aluguru.Marketplace.Crosscutting.Iugu.Dtos
{
    public class PayerDTO
    {
        [JsonPropertyName("cpf_cnpj")]
        public string CpfCnpj { get; set; }
        public string Name { get; set; }
        [JsonPropertyName("phone_prefix")]
        public string PhonePrefix { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
