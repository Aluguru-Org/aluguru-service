using Aluguru.Marketplace.Crosscutting.Iugu.Dtos;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Crosscutting.Iugu
{    
    public interface IIuguService
    {

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

        public async Task Charge(PaymentMethod paymentMethod)
        {
            //var directChargeDTO = new DirectChargeDTO();

            //switch(paymentMethod)
            //{
            //    case PaymentMethod.BOLETO:
            //        directChargeDTO.Method = "bank_slip";
            //        break;
            //    case PaymentMethod.CREDIT_CARD:
            //        directChargeDTO.Token = token;
            //        break;
            //}

            //directChargeDTO.Email = email;
            //directChargeDTO.Items = 

            //var content = new StringContent(JsonConvert.SerializeObject(directCharge), Encoding.UTF8, "application/json");
            //var response = await _httpClient.PostAsync($"{_settings.BaseUrl}/charge", content);
            //var responseData = await response.Content.ReadAsStringAsync();
        }
    }
}
