using Microsoft.AspNetCore.Authentication.JwtBearer;
using Aluguru.Marketplace.API.Models;
using Aluguru.Marketplace.Register.Usecases.LogInUser;
using Aluguru.Marketplace.Register.ViewModels;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.API.IntegrationTests
{
    public static class HttpClientExtension
    {
        public static async Task<ApiResponse<LogInUserCommandResponse>> LogInUser(this HttpClient client, string email = "admin@aluguru.com.br", string password = "really")
        {
            var response = await client.PostAsync("/api/v1/auth/login", CreateContent(email, password));
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<LogInUserCommandResponse>>(await response.Content.ReadAsStringAsync());

            if (apiResponse.Success)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, apiResponse.Data.Token);
            }

            return apiResponse;
        }

        public static void LogOut(this HttpClient client)
        {
            if (client == null) return;
            if (client.DefaultRequestHeaders == null) return;
            if (client.DefaultRequestHeaders.Authorization == null) return;

            client.DefaultRequestHeaders.Authorization = null;
        }

        private static StringContent CreateContent(string email, string password)
        {
            return new StringContent(JsonConvert.SerializeObject(new LoginUserViewModel() { Email = email, Password = password }), Encoding.UTF8, "application/json");
        }
    }
}
