using Aluguru.Marketplace.API.Models;
using Aluguru.Marketplace.Register.Usecases.LogInUser;
using Aluguru.Marketplace.Register.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.API.IntegrationTests.Config
{
    public class TestUser : IDisposable
    {
        public TestUser(string name, string email, string password, HttpClient client)
        {
            Name = name;
            Email = email;
            Password = password;
            Client = client;
        }
        public string Name { get; }
        public string Email { get; }
        public string Password { get; }
        public HttpClient Client { get; }
        public Guid Id { get; set; }
        public string Token { get; set; }
        public string ActivationHash { get; set; }

        public async Task<bool> LogIn()
        {
            var viewModel = new LoginUserViewModel() { Email = Email, Password = Password };
            var response = await Client.PostAsJsonAsync("/api/v1/auth/login", viewModel);

            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<LogInUserCommandResponse>>(response.Content.ReadAsStringAsync().Result);

            if (apiResponse.Success)                
            {
                Token = apiResponse.Data.Token;
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, Token);
                return true;
            }
            return false;
        }

        public void LogOut()
        {
            Token = string.Empty;

            if (Client == null) return;
            if (Client.DefaultRequestHeaders == null) return;

            Client.DefaultRequestHeaders.Authorization = null;
        }

        public void Dispose()
        {
            Client.Dispose();
        }
    }
}
