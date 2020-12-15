using Microsoft.AspNetCore.Authentication.JwtBearer;
using Aluguru.Marketplace.API.Models;
using Aluguru.Marketplace.Register.Usecases.LogInBackofficeClient;
using Aluguru.Marketplace.Register.Dtos;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.API.IntegrationTests
{
    public static class HttpClientExtension
    {
        public static Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient client, string requestUri, T value)
        {
            return client.PostAsync(requestUri, value.ToStringContent());
        }

        public static Task<HttpResponseMessage> PutAsJsonAsync<T>(this HttpClient client, string requestUri, T value)
        {
            return client.PutAsync(requestUri, value.ToStringContent());
        }

        private static StringContent ToStringContent(this object data, string mediaType = "application/json")
        {
            return new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, mediaType);
        }
    }
}
