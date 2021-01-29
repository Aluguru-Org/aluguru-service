using Aluguru.Marketplace.API.IntegrationTests.Config;
using Aluguru.Marketplace.API.IntegrationTests.Extensions;
using Xunit;

namespace Aluguru.Marketplace.API.IntegrationTests
{
    [TestCaseOrderer("Aluguru.Marketplace.API.IntegrationTests.Extensions.PriorityOrderer", "Aluguru.Marketplace.API.IntegrationTests")]
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class AuthControllerTests
    {
        private readonly IntegrationTestsFixture<StartupTests> _fixture;
        public AuthControllerTests(IntegrationTestsFixture<StartupTests> testsFixture)
        {
            _fixture = testsFixture;
        }

        [Fact(DisplayName = "LogIn user with success"), TestPriority(1)]
        [Trait("Register", "Api Integration - LogIn user")]
        public void LogIn_WithExistingUser_ShouldReturnSuccess()
        {
            _fixture.Admin.LogIn().Wait();
        }

    }
}
