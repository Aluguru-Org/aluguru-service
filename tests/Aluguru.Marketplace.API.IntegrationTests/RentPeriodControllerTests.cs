using Aluguru.Marketplace.API.IntegrationTests.Config;
using Aluguru.Marketplace.API.IntegrationTests.Extensions;
using Aluguru.Marketplace.API.Models;
using Aluguru.Marketplace.Catalog.Usecases.CreateRentPeriod;
using Aluguru.Marketplace.Catalog.ViewModels;
using System.Net;
using Xunit;

namespace Aluguru.Marketplace.API.IntegrationTests
{
    [TestCaseOrderer("Aluguru.Marketplace.API.IntegrationTests.Extensions.PriorityOrderer", "Aluguru.Marketplace.API.IntegrationTests")]
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class RentPeriodControllerTests
    {
        private const string RENT_PERIOD_ENDPOINT = "/api/v1/rent-period";

        private readonly IntegrationTestsFixture<StartupTests> _fixture;
        public RentPeriodControllerTests(IntegrationTestsFixture<StartupTests> testsFixture)
        {
            _fixture = testsFixture;
        }

        [Fact(DisplayName = "Create rent period with success"), TestPriority(1)]
        [Trait("Catalog", "Api Integration - Create Rent-Period")]
        public void CreateRentPeriod_ShouldPass()
        {
            // Arrange
            var viewModel = new CreateRentPeriodViewModel()
            {
                Name = _fixture.RentPeriodMonth.Name,
                Days = _fixture.RentPeriodMonth.Days
            };

            // Act
            _fixture.Admin.LogIn().Wait();
            var response = _fixture.Admin.Client.PostAsJsonAsync(RENT_PERIOD_ENDPOINT, viewModel).Result;
            var rentPeriod = response.Deserialize<ApiResponse<CreateRentPeriodCommandResponse>>().Data.RentPeriod;

            _fixture.RentPeriodMonth.Id = rentPeriod.Id;

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact(DisplayName = "Create rent period 2 with success"), TestPriority(1)]
        [Trait("Catalog", "Api Integration - Create Rent-Period 2")]
        public void CreateRentPeriod2_ShouldPass()
        {
            // Arrange
            var viewModel = new CreateRentPeriodViewModel()
            {
                Name = _fixture.RentPeriodWeek.Name,
                Days = _fixture.RentPeriodWeek.Days
            };

            // Act
            _fixture.Admin.LogIn().Wait();
            var response = _fixture.Admin.Client.PostAsJsonAsync(RENT_PERIOD_ENDPOINT, viewModel).Result;
            var rentPeriod = response.Deserialize<ApiResponse<CreateRentPeriodCommandResponse>>().Data.RentPeriod;

            _fixture.RentPeriodWeek.Id = rentPeriod.Id;

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact(DisplayName = "Get all rent period with success"), TestPriority(2)]
        [Trait("Catalog", "Api Integration - Get Rent-Periods")]
        public void GetAllRentPeriod_ShouldPass()
        {
            // Arrange
            // Act
            _fixture.Admin.LogIn().Wait();
            var response = _fixture.Admin.Client.GetAsync(RENT_PERIOD_ENDPOINT).Result;

            // Assert            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "Delete rent period with success"), TestPriority(9999)]
        [Trait("Catalog", "Api Integration - Delete Rent-Period 2")]
        public void DeleteRentPeriod_Monthly_ShouldPass()
        {
            // Arrange
            var rentPeriodId = _fixture.RentPeriodMonth.Id;

            // Act
            _fixture.Admin.LogIn().Wait();
            var response = _fixture.Admin.Client.DeleteAsync($"{RENT_PERIOD_ENDPOINT}/{rentPeriodId}").Result;

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "Delete rent period with success"), TestPriority(9999)]
        [Trait("Catalog", "Api Integration - Delete Rent-Period 2")]
        public void DeleteRentPeriod_Weekly_ShouldPass()
        {
            // Arrange
            var rentPeriodId = _fixture.RentPeriodWeek.Id;

            // Act
            _fixture.Admin.LogIn().Wait();
            var response = _fixture.Admin.Client.DeleteAsync($"{RENT_PERIOD_ENDPOINT}/{rentPeriodId}").Result;

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
