using Aluguru.Marketplace.API.IntegrationTests.Config;
using Aluguru.Marketplace.API.IntegrationTests.Extensions;
using Aluguru.Marketplace.API.Models;
using Aluguru.Marketplace.Register.Usecases.CreateUser;
using Aluguru.Marketplace.Register.Usecases.GetUserById;
using Aluguru.Marketplace.Register.Dtos;
using System.Net;
using Xunit;

namespace Aluguru.Marketplace.API.IntegrationTests
{

    [TestCaseOrderer("Aluguru.Marketplace.API.IntegrationTests.Extensions.PriorityOrderer", "Aluguru.Marketplace.API.IntegrationTests")]
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class UserControllerTests
    {
        private readonly IntegrationTestsFixture<StartupTests> _fixture;
        public UserControllerTests(IntegrationTestsFixture<StartupTests> testsFixture)
        {
            _fixture = testsFixture;
        }

        [Fact(DisplayName = "Register user with success"), TestPriority(1)]
        [Trait("Register", "Api Integration - Register user")]
        public void CreateUser_ShouldPass()
        {
            // Arrange
            var viewModel = new UserRegistrationDTO()
            {
                Email = _fixture.User.Email,
                Password = _fixture.User.Password,
                FullName = _fixture.User.Name,
                Role = "User"
            };

            // Act
            var response = _fixture.User.Client.PostAsJsonAsync($"/api/v1/user", viewModel).Result;

            var createUserResponse = response.Deserialize<ApiResponse<CreateUserCommandResponse>>().Data;

            _fixture.User.Id = createUserResponse.User.Id;
            _fixture.User.ActivationHash = createUserResponse.ActivationHash;

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact(DisplayName = "Register company with success"), TestPriority(1)]
        [Trait("Register", "Api Integration - Register company")]
        public void CreateCompany_ShouldPass()
        {
            // Arrange
            var viewModel = new UserRegistrationDTO()
            {
                Email = _fixture.Company.Email,
                Password = _fixture.Company.Password,
                FullName = _fixture.Company.Name,
                Role = "Company"
            };

            // Act
            var response = _fixture.Company.Client.PostAsJsonAsync($"/api/v1/user", viewModel).Result;

            var createUserResponse = response.Deserialize<ApiResponse<CreateUserCommandResponse>>().Data;

            _fixture.Company.Id = createUserResponse.User.Id;
            _fixture.Company.ActivationHash = createUserResponse.ActivationHash;

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact(DisplayName = "Activate user with success"), TestPriority(2)]
        [Trait("Register", "Api Integration - Activate user")]
        public void ActivateUser_ShouldPass()
        {
            // Arrange
            var id = _fixture.User.Id;
            var activationHash = _fixture.User.ActivationHash;

            // Act
            var response = _fixture.User.Client.PutAsync($"/api/v1/user/{id}/activate?activationHash={activationHash}", null).Result;

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact(DisplayName = "Activate company with success"), TestPriority(2)]
        [Trait("Register", "Api Integration - Activate company")]
        public void ActivateCompany_ShouldPass()
        {
            // Arrange
            var id = _fixture.Company.Id;
            var activationHash = _fixture.Company.ActivationHash;

            // Act
            var response = _fixture.Company.Client.PutAsync($"/api/v1/user/{id}/activate?activationHash={activationHash}", null).Result;

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact(DisplayName = "Get user by id with success"), TestPriority(4)]
        [Trait("Register", "API Integration - Get User by Id")]
        public void GetUserById_ShouldPass()
        {
            // Arrange
            var id = _fixture.Company.Id;

            // Act
            _fixture.Company.LogIn().Wait();
            var response = _fixture.Company.Client.GetAsync($"/api/v1/user/{id}").Result;
            var user = response.Deserialize<ApiResponse<GetUserByIdCommandResponse>>().Data.User;

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(id, user.Id);
        }

        [Fact(DisplayName = "Update user with success"), TestPriority(4)]
        [Trait("Register", "API Integration - Update User")]
        public void UpdateUser_ShouldPass()
        {
            // Arrange
            var userId = _fixture.User.Id;            

            var viewModel = new UpdateUserNameDTO()
            {
                FullName = _fixture.User.Name + "__TEST_UPDATE__"            
            };

            // Act
            _fixture.User.LogIn().Wait();
            var response = _fixture.User.Client.PutAsJsonAsync($"/api/v1/user/{userId}", viewModel).Result;

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact(DisplayName = "Delete user with success"), TestPriority(9999)]
        [Trait("Register", "API Integration - Delete User")]
        public void DeleteUser_ShouldPass()
        {
            // Arrange
            var userId = _fixture.User.Id;

            // Act
            _fixture.User.LogIn().Wait();
            var response = _fixture.User.Client.DeleteAsync($"/api/v1/user/{userId}").Result;

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
