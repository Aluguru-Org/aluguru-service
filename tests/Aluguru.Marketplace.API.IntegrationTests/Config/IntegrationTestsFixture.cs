using Bogus;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using Xunit;

namespace Aluguru.Marketplace.API.IntegrationTests.Config
{
    [CollectionDefinition(nameof(IntegrationApiTestsFixtureCollection))]
    public class IntegrationApiTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture<StartupTests>> { }

    public class IntegrationTestsFixture<TStartup> : IDisposable where TStartup : class
    {
        public readonly AluguruServiceFactory<TStartup> Factory;

        public TestUser User;
        public TestUser Company;
        public TestUser Admin;

        public TestCategory Category;
        public TestCategory SubCategory;

        public TestRentPeriod RentPeriodMonth;
        public TestRentPeriod RentPeriodWeek;

        public TestProduct ProductWithFixedPrice;
        public TestProduct ProductWithIndefinitePrice;

        public IntegrationTestsFixture()
        {
            var clientOptions = new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = true,
                BaseAddress = new Uri("http://localhost"),
                HandleCookies = true,
                MaxAutomaticRedirections = 7
            };

            Factory = new AluguruServiceFactory<TStartup>();

            User = GenerateUser(Factory.CreateClient(clientOptions));
            Company = GenerateUser(Factory.CreateClient(clientOptions));
            Admin = GenerateAdmin(Factory.CreateClient(clientOptions));

            Category = GenerateCategory();
            SubCategory = GenerateCategory();

            RentPeriodMonth = new TestRentPeriod("1 Month", 30);
            RentPeriodWeek = new TestRentPeriod("1 Week", 7);

            ProductWithFixedPrice = new TestProduct();
            ProductWithIndefinitePrice = new TestProduct();
        }

        private TestUser GenerateUser(HttpClient client)
        {
            var faker = new Faker("pt_BR");

            return new TestUser(
                faker.Name.FullName(), 
                faker.Internet.Email().ToLower(), 
                faker.Internet.Password(8, false, "", "@Aa1"), 
                client);
        }

        private TestCategory GenerateCategory()
        {
            var faker = new Faker("pt_BR");
            var categoryName = faker.Commerce.Categories(1)[0];

            return new TestCategory(categoryName, categoryName.Trim().ToLower().Replace(" ", "-"));
        }

        private TestUser GenerateAdmin(HttpClient client)
        {
            return new TestUser("admin", "admin@aluguru.com.br", "really", client);
        }

        public void Dispose()
        {
            User.Dispose();
            Company.Dispose();
            Factory.Dispose();
        }
    }
}
