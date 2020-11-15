using Aluguru.Marketplace.Crosscutting.Iugu.Dtos;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Crosscutting.Iugu
{
    public interface IIuguService
    {
        Task<PaymentResponseDTO> Charge(PaymentMethod paymentMethod, string token, int? installments, string email, PayerDTO payer, List<ItemDTO> items);
    }

    public class IuguService : IIuguService
    {
        private readonly IuguSettings _settings;
        private readonly HttpClient _httpClient;

        public IuguService(IOptions<IuguSettings> options, IHttpClientFactory httpClientFactory)
        {
            _settings = options.Value;

            _httpClient = httpClientFactory.CreateClient();
            
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_settings.Token);
        }

        public async Task<PaymentResponseDTO> Charge(PaymentMethod paymentMethod, string token, int? installments, string email, PayerDTO payer, List<ItemDTO> items)
        {
            var directCharge = new DirectChargeDTO();

            switch (paymentMethod)
            {
                case PaymentMethod.BOLETO:
                    directCharge.Method = "bank_slip";                    
                    break;
                case PaymentMethod.CREDIT_CARD:
                    directCharge.Token = token;
                    if (installments.HasValue) directCharge.Months = installments.Value.ToString();
                    break;
            }

            directCharge.Email = email;
            directCharge.Items = items;
            directCharge.Payer = payer;

            var content = new StringContent(JsonSerializer.Serialize(directCharge), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_settings.BaseUrl}/charge", content);

            var paymentResponse = JsonSerializer.Deserialize<PaymentResponseDTO>(await response.Content.ReadAsStringAsync());

            return paymentResponse;
        }
    }
}
