using System.Text.Json.Serialization;

namespace Aluguru.Marketplace.Crosscutting.Viacep.Dtos
{
    public class CepDTO
    {
        [JsonPropertyName("cep")]
        public string CEP { get; set; }
        [JsonPropertyName("logradouro")]
        public string Logradouro { get; set; }
        [JsonPropertyName("complemento")]
        public string Complemento { get; set; }
        [JsonPropertyName("bairro")]
        public string Bairro { get; set; }
        [JsonPropertyName("localidade")]
        public string Localidade { get; set; }
        [JsonPropertyName("uf")]
        public string UF { get; set; }
        [JsonPropertyName("ibge")]
        public string IBGE { get; set; }
        [JsonPropertyName("gia")]
        public string GIA { get; set; }
        [JsonPropertyName("ddd")]
        public string DDD { get; set; }
        [JsonPropertyName("siafi")]
        public string Siafi { get; set; }
        [JsonPropertyName("erro")]
        public bool Erro { get; set; }
    }
}
