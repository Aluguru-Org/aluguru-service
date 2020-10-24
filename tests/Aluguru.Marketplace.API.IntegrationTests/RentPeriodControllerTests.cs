using Aluguru.Marketplace.API.IntegrationTests.Extensions;
using Aluguru.Marketplace.API.Models;
using Aluguru.Marketplace.Catalog.Usecases.CreateRentPeriod;
using Aluguru.Marketplace.Catalog.ViewModels;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Aluguru.Marketplace.API.IntegrationTests
{
    public class RentPeriodControllerTests
    {
        private const string RENT_PERIOD_ENDPOINT = "/api/v1/rent-period";

        [Fact]
        public void CreateRentPeriod_WhenInvalidRentPeriod_ShouldFail()
        {
            var client = Server.Instance.CreateClient();

            client.LogInUser();

            var viewModel = new CreateRentPeriodViewModel();
            var data = new StringContent(JsonConvert.SerializeObject(viewModel), Encoding.UTF8, "application/json");

            var response = client.PostAsync(RENT_PERIOD_ENDPOINT, data).Result;

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public void CreateRentPeriod_ShouldPass()
        {
            var client = Server.Instance.CreateClient();

            client.LogInUser();

            var viewModel = new CreateRentPeriodViewModel()
            {
                Name = "1 Month",
                Days = 30
            };
            var data = new StringContent(JsonConvert.SerializeObject(viewModel), Encoding.UTF8, "application/json");

            var response = client.PostAsync(RENT_PERIOD_ENDPOINT, data).Result;

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public void GetAllRentPeriod_ShouldPass()
        {
            var client = Server.Instance.CreateClient();

            client.LogInUser();

            var getAllResponse = client.GetAsync(RENT_PERIOD_ENDPOINT).Result;
            Assert.Equal(HttpStatusCode.OK, getAllResponse.StatusCode);
        }

        [Fact]
        public void DeleteRentPeriod_ShouldPass()
        {
            var client = Server.Instance.CreateClient();

            client.LogInUser();

            var createViewModel = new CreateRentPeriodViewModel()
            {
                Name = "3 Month",
                Days = 30
            };

            var response = client.PostAsync(RENT_PERIOD_ENDPOINT, createViewModel.ToStringContent()).Result;
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var rentPeriod = response.Deserialize<ApiResponse<CreateRentPeriodCommandResponse>>().Data.RentPeriod;

            var deleteResponse = client.DeleteAsync($"{RENT_PERIOD_ENDPOINT}/{rentPeriod.Id}").Result;
            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
        }
    }
}
