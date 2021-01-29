using Aluguru.Marketplace.Crosscutting.Viacep.Dtos;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Crosscutting.Viacep
{
    public interface ICepService
    {
        Task<CepResponse> GetAddress(string cep);
    }

    public class CepService : ICepService
    {
        public readonly ViacepSettings _settings;

        public CepService(IOptions<ViacepSettings> options)
        {
            _settings = options.Value;
        }

        public async Task<CepResponse> GetAddress(string cep)
        {
            using var client = new HttpClient();

            var url = string.Format(_settings.ViacepUrl, cep);
            var response = await client.GetAsync(new UriBuilder(url).ToString());

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to retrieve cep from ViaCep API");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var cepResponse = JsonSerializer.Deserialize<CepDTO>(responseContent);

            if (cepResponse.Erro)
            {
                return new CepResponse(false);
            }

            return new CepResponse(true, cepResponse.Logradouro, cepResponse.Bairro, cepResponse.Localidade, cepResponse.UF, cepResponse.CEP);
        }
    }
}
