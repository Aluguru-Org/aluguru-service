using Mubbi.Marketplace.API.Models;
using Mubbi.Marketplace.Register.Usecases.LogInUser;
using Mubbi.Marketplace.Register.ViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Mubbi.Marketplace.API.IntegrationTests
{
    public class AuthControllerTests
    {        
        [Fact]
        public async Task LogIn_WithExistingUser_ShouldReturnSuccess()
        {
            var client = Server.Instance.CreateClient();

            var content = new StringContent(JsonConvert.SerializeObject(new LoginUserViewModel() { Email = "contato@mubbi.com.br", Password = "really" }), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/v1/auth/login", content);
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<LogInUserCommandResponse>>(await response.Content.ReadAsStringAsync());

            Assert.True(apiResponse.Success);
            Assert.True(!string.IsNullOrEmpty(apiResponse.Data.Token));
        }


        [Fact]
        public async Task LogIn_WithNonRegisteredUser_ShouldNotReturnSuccess()
        {
            var client = Server.Instance.CreateClient();

            var content = new StringContent(JsonConvert.SerializeObject(new LoginUserViewModel() { Email = "test@test.com", Password = "test" }), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/v1/auth/login", content);
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<List<string>>>(await response.Content.ReadAsStringAsync());

            Assert.False(apiResponse.Success);
            Assert.True(apiResponse.Data[0] == "The E-mail test@test.com does not exist");
        }
    }
}
