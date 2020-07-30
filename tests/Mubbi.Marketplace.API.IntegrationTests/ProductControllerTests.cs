using Mubbi.Marketplace.API.IntegrationTests.Extensions;
using Mubbi.Marketplace.API.Models;
using Mubbi.Marketplace.Catalog.Domain;
using Mubbi.Marketplace.Catalog.Usecases.CreateCategory;
using Mubbi.Marketplace.Catalog.Usecases.CreateProduct;
using Mubbi.Marketplace.Catalog.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Mubbi.Marketplace.API.IntegrationTests
{
    public class ProductControllerTests
    { 
        [Fact]
        public async Task CreateProduct_WhenInvalidProduct_ShouldFail()
        {
            var client = Server.Instance.CreateClient();

            var viewModel = new CreateProductViewModel();
            var response = await client.PostAsync("/api/v1/product", viewModel.ToStringContent());

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateProduct_ShouldPass()
        {
            var client = Server.Instance.CreateClient();

            var user = await client.LogInUser();

            var category = await CreateCategory(client);

            var viewModel = new CreateProductViewModel()
            {
                UserId = Guid.Parse("96d1fb97-47e9-4ad5-b07e-448f88defd9c"),
                CategoryId = category.Id,
                Name = "Test Product",
                Description = "Test description",
                RentType = "Indefinite",
                Price = 50000,
                IsActive = true,
                MinNoticeRentDays = 2,
                MinRentDays = 7,
                MaxRentDays = 30,
                StockQuantity = 1,
                ImageUrls = new List<string> { "image.png" },
                CustomFields = new List<CreateCustomFieldViewModel> 
                { 
                    new CreateCustomFieldViewModel() {  FieldType = "Text", ValueAsString = "Observação?", Active = true } 
                }
            };

            var response = await client.PostAsync("/api/v1/product", viewModel.ToStringContent());

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        private async Task<CategoryViewModel> CreateCategory(HttpClient client)
        {
            var viewModel = new CreateCategoryViewModel() { Name = "CreateProductTestCategory" };

            var response = await client.PostAsync("/api/v1/category", viewModel.ToStringContent());

            return response.Deserialize<ApiResponse<CreateCategoryCommandResponse>>().Data.Category;
        }
    }
}
