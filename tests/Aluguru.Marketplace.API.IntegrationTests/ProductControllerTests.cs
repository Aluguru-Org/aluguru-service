using Aluguru.Marketplace.API.IntegrationTests.Extensions;
using Aluguru.Marketplace.API.Models;
using Aluguru.Marketplace.Catalog.Usecases.CreateCategory;
using Aluguru.Marketplace.Catalog.Usecases.CreateProduct;
using Aluguru.Marketplace.Catalog.Usecases.CreateRentPeriod;
using Aluguru.Marketplace.Catalog.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Aluguru.Marketplace.API.IntegrationTests
{
    public class ProductControllerTests
    { 
        [Fact]
        public void CreateProduct_WhenInvalidProduct_ShouldFail()
        {
            var client = Server.Instance.CreateClient();

            client.LogInUser();

            var viewModel = new CreateProductViewModel();
            var response = client.PostAsync("/api/v1/product", viewModel.ToStringContent()).Result;

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public void CreateProduct_ShouldPass()
        {
            var client = Server.Instance.CreateClient();

            var user = client.LogInUser();

            var response = RequestCreateProduct(client);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public void UpdateProduct_ShouldPass()
        {
            var client = Server.Instance.CreateClient();

            var user = client.LogInUser();

            var response = RequestCreateProduct(client);

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

            response = client.PutAsync($"/api/v1/product/{product.Id}", viewModel.ToStringContent()).Result;

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public void DeleteProduct_ShouldPass()
        {
            var client = Server.Instance.CreateClient();

            var response = RequestCreateProduct(client);

            var product = response.Deserialize<ApiResponse<CreateProductCommandResponse>>().Data.Product;

            response = client.DeleteAsync($"/api/v1/product/{product.Id}").Result;

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        private HttpResponseMessage RequestCreateProduct(HttpClient client)
        {
            var rentPeriod = CreateRentPeriod(client);
            var category = CreateCategory(client);

            var viewModel = new CreateProductViewModel()
            {
                UserId = Guid.Parse("96d1fb97-47e9-4ad5-b07e-448f88defd9c"),
                CategoryId = category.Id,
                Name = "Test Product",
                Uri = "test-product",
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
                CustomFields = new List<CreateCustomFieldViewModel>
                {
                    new CreateCustomFieldViewModel() {  FieldType = "Text", FieldName = "Observação?", Active = true }
                }
            };

            var response = client.PostAsync("/api/v1/product", viewModel.ToStringContent()).Result;

            return response;
        }

        private RentPeriodViewModel CreateRentPeriod(HttpClient client)
        {
            var viewModel = new CreateRentPeriodViewModel() { Name = "1 week", Days = 7 };

            var response = client.PostAsync("/api/v1/rent-period", viewModel.ToStringContent()).Result;

            return response.Deserialize<ApiResponse<CreateRentPeriodCommandResponse>>().Data.RentPeriod;
        }

        private CategoryViewModel CreateCategory(HttpClient client)
        {
            var viewModel = new CreateCategoryViewModel() { Name = "CreateProductTestCategory" };

            var response = client.PostAsync("/api/v1/category", viewModel.ToStringContent()).Result;

            return response.Deserialize<ApiResponse<CreateCategoryCommandResponse>>().Data.Category;
        }
    }
}
