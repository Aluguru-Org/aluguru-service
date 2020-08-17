using Mubbi.Marketplace.API.IntegrationTests.Extensions;
using Mubbi.Marketplace.API.Models;
using Mubbi.Marketplace.Catalog.Usecases.CreateRentPeriod;
using Mubbi.Marketplace.Catalog.ViewModels;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Mubbi.Marketplace.API.IntegrationTests
{
    public class RentPeriodControllerTests
    {
        private const string RENT_PERIOD_ENDPOINT = "/api/v1/rent-period";

        [Fact]
        public async Task CreateRentPeriod_WhenInvalidRentPeriod_ShouldFail()
        {
            var client = Server.Instance.CreateClient();

            var viewModel = new CreateRentPeriodViewModel();
            var data = new StringContent(JsonConvert.SerializeObject(viewModel), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(RENT_PERIOD_ENDPOINT, data);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateRentPeriod_ShouldPass()
        {
            var client = Server.Instance.CreateClient();

            var viewModel = new CreateRentPeriodViewModel()
            {
                Name = "1 Month",
                Days = 30
            };
            var data = new StringContent(JsonConvert.SerializeObject(viewModel), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(RENT_PERIOD_ENDPOINT, data);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task GetAllRentPeriod_ShouldPass()
        {
            var client = Server.Instance.CreateClient();

            var getAllResponse = await client.GetAsync(RENT_PERIOD_ENDPOINT);
            Assert.Equal(HttpStatusCode.OK, getAllResponse.StatusCode);
        }

        [Fact]
        public async Task DeleteRentPeriod_ShouldPass()
        {
            var client = Server.Instance.CreateClient();

            var createViewModel = new CreateRentPeriodViewModel()
            {
                Name = "3 Month",
                Days = 30
            };

            var response = await client.PostAsync(RENT_PERIOD_ENDPOINT, createViewModel.ToStringContent());
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var rentPeriod = response.Deserialize<ApiResponse<CreateRentPeriodCommandResponse>>().Data.RentPeriod;

            var deleteResponse = await client.DeleteAsync($"{RENT_PERIOD_ENDPOINT}/{rentPeriod.Id}");
            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
        }
    }
}
