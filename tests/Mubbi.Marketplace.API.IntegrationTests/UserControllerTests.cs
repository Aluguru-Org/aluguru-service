using Mubbi.Marketplace.API.IntegrationTests.Extensions;
using Mubbi.Marketplace.API.Models;
using Mubbi.Marketplace.Register.Usecases.GetUserById;
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
    }
}
