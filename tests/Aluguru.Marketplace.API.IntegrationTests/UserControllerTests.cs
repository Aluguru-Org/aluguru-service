using Aluguru.Marketplace.API.IntegrationTests.Extensions;
using Aluguru.Marketplace.API.Models;
using Aluguru.Marketplace.Register.Usecases.CreateUser;
using Aluguru.Marketplace.Register.Usecases.GetUserById;
using Aluguru.Marketplace.Register.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Aluguru.Marketplace.API.IntegrationTests
{
    public class UserControllerTests
    {
        [Fact]
        public void GetUserById_ShouldPass()
        {
            var client = Server.Instance.CreateClient();

            client.LogInUser().Wait();

            var userId = "96d1fb97-47e9-4ad5-b07e-448f88defd9c";
            var response = client.GetAsync($"/api/v1/user/{userId}").Result;

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var user = response.Deserialize<ApiResponse<GetUserByIdCommandResponse>>().Data.User;

            Assert.Equal(Guid.Parse(userId), user.Id);
        }

        [Fact]
        public void CreateUser_ShouldPass()
        {
            var client = Server.Instance.CreateClient();

            var viewModel = new UserRegistrationViewModel()
            {
                Email = "someemail@test.com",
                Password = "someAwesomePa$$word1",
                FullName = "Aluguru Admin Account",
                Role = "User"
            };

            var response = client.PostAsync($"/api/v1/user", viewModel.ToStringContent()).Result;

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public void UpdateUser_ShouldPass()
        {
            var client = Server.Instance.CreateClient();

            client.LogInUser().Wait();

            var userId = "96d1fb97-47e9-4ad5-b07e-448f88defd9c";

            var viewModel = new UpdateUserViewModel()
            {
                UserId = Guid.Parse("96d1fb97-47e9-4ad5-b07e-448f88defd9c"),
                FullName = "Aluguru Admin Account",
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

            var response = client.PutAsync($"/api/v1/user/{userId}", viewModel.ToStringContent()).Result;

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public void DeleteUser_ShouldPass()
        {
            var client = Server.Instance.CreateClient();

            client.LogInUser().Wait();

            var viewModel = new UserRegistrationViewModel()
            {
                Email = "someemail@test.com",
                Password = "someAwesomePa$$word1",
                FullName = "Aluguru Admin Account",
                Role = "User"
            };

            var response = client.PostAsync($"/api/v1/user", viewModel.ToStringContent()).Result;

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var user = response.Deserialize<ApiResponse<CreateUserCommandResponse>>().Data.User;

            response = client.DeleteAsync($"/api/v1/user/{user.Id}").Result;

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
