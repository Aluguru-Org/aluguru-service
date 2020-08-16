using Mubbi.Marketplace.API.IntegrationTests.Extensions;
using Mubbi.Marketplace.API.Models;
using Mubbi.Marketplace.Register.Usecases.GetUserById;
using Mubbi.Marketplace.Register.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Mubbi.Marketplace.API.IntegrationTests
{
    public class UserControllerTests
    {
        [Fact]
        public async Task GetUserById_ShouldPass()
        {
            var client = Server.Instance.CreateClient();

            var userId = "96d1fb97-47e9-4ad5-b07e-448f88defd9c";
            var response = await client.GetAsync($"/api/v1/user/{userId}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var user = response.Deserialize<ApiResponse<GetUserByIdCommandResponse>>().Data.User;

            Assert.Equal(Guid.Parse(userId), user.Id);
        }

        [Fact]
        public async Task UpdateUser_ShouldPass()
        {
            var client = Server.Instance.CreateClient();

            var userId = "96d1fb97-47e9-4ad5-b07e-448f88defd9c";

            var viewModel = new UpdateUserViewModel()
            {
                UserId = Guid.Parse("96d1fb97-47e9-4ad5-b07e-448f88defd9c"),
                FullName = "Mubbi Admin Account",
                Document = new DocumentViewModel()
                {
                    Number = "02482668026",
                    DocumentType = "CPF"
                },
                Address = new AddressViewModel()
                {
                    Street = "General Lima e Silva",
                    Number = "480",
                    Neighborhood = "Centro Histórico",
                    City = "Porto Alegre",
                    State = "Rio Grande do Sul",
                    Country = "Brasil",
                    ZipCode = "90050-100"
                }
            };

            var response = await client.PutAsync($"/api/v1/user/{userId}", viewModel.ToStringContent());

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
