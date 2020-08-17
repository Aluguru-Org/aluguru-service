using Mubbi.Marketplace.API.IntegrationTests.Extensions;
using Mubbi.Marketplace.API.Models;
using Mubbi.Marketplace.Catalog.Usecases.CreateCategory;
using Mubbi.Marketplace.Catalog.Usecases.CreateProduct;
using Mubbi.Marketplace.Catalog.Usecases.CreateRentPeriod;
using Mubbi.Marketplace.Catalog.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
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

        [Fact(Skip = "True")]
        public async Task CreateProduct_ShouldPass()
        {
            var client = Server.Instance.CreateClient();

            var user = await client.LogInUser();

            var response = await RequestCreateProduct(client);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact(Skip = "True")]
        public async Task UpdateProduct_ShouldPass()
        {
            var client = Server.Instance.CreateClient();

            var user = await client.LogInUser();

            var response = await RequestCreateProduct(client);

            var product = response.Deserialize<ApiResponse<CreateProductCommandResponse>>().Data.Product;

            var viewModel = new UpdateProductViewModel()
            {                
                Id = product.Id,
                CategoryId = product.CategoryId,
                Name = "Test Update Product",
                Description = "Test description",
                RentType = "Indefinite",
                Price = product.Price,
                IsActive = false,
                MinNoticeRentDays = 2,
                MinRentDays = 7,
                MaxRentDays = 30,
                StockQuantity = 1,
                ImageUrls = new List<string> { "image.png" },
                CustomFields = new List<UpdateCustomFieldViewModel>
                {
                    new UpdateCustomFieldViewModel() {  FieldType = "Text", ValueAsString = "Observação?", Active = true }
                }
            };

            response = await client.PutAsync($"/api/v1/product/{product.Id}", viewModel.ToStringContent());

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact(Skip = "True")]
        public async Task DeleteProduct_ShouldPass()
        {
            var client = Server.Instance.CreateClient();

            var response = await RequestCreateProduct(client);

            var product = response.Deserialize<ApiResponse<CreateProductCommandResponse>>().Data.Product;

            response = await client.DeleteAsync($"/api/v1/product/{product.Id}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        private async Task<HttpResponseMessage> RequestCreateProduct(HttpClient client)
        {
            var rentPeriod = await CreateRentPeriod(client);
            var category = await CreateCategory(client);

            var viewModel = new CreateProductViewModel()
            {
                UserId = Guid.Parse("96d1fb97-47e9-4ad5-b07e-448f88defd9c"),
                CategoryId = category.Id,
                Name = "Test Product",
                Description = "Test description",
                RentType = "Indefinite",
                Price = new PriceViewModel()
                {
                    SellPrice = 500000,
                    PeriodRentPrices = new List<PeriodPriceViewModel>()
                    {
                        new PeriodPriceViewModel()
                        {
                            RentPeriodId = rentPeriod.Id,
                            Price = 50000
                        }
                    }
                },
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

            return response;
        }

        private async Task<RentPeriodViewModel> CreateRentPeriod(HttpClient client)
        {
            var viewModel = new CreateRentPeriodViewModel() { Name = "1 week", Days = 7 };

            var response = await client.PostAsync("/api/v1/rent-period", viewModel.ToStringContent());

            return response.Deserialize<ApiResponse<CreateRentPeriodCommandResponse>>().Data.RentPeriod;
        }

        private async Task<CategoryViewModel> CreateCategory(HttpClient client)
        {
            var viewModel = new CreateCategoryViewModel() { Name = "CreateProductTestCategory" };

            var response = await client.PostAsync("/api/v1/category", viewModel.ToStringContent());

            return response.Deserialize<ApiResponse<CreateCategoryCommandResponse>>().Data.Category;
        }
    }
}
