using Aluguru.Marketplace.API.IntegrationTests.Extensions;
using Aluguru.Marketplace.API.Models;
using Aluguru.Marketplace.Catalog.Usecases.CreateCategory;
using Aluguru.Marketplace.Catalog.Usecases.CreateProduct;
using Aluguru.Marketplace.Catalog.Usecases.CreateRentPeriod;
using Aluguru.Marketplace.Catalog.ViewModels;
using Aluguru.Marketplace.Rent.Usecases.GetOrder;
using Aluguru.Marketplace.Rent.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Aluguru.Marketplace.API.IntegrationTests
{
    public class OrderControllerTests
    {
        [Fact]
        public void GetOrderById_ShouldPass()
        {
            var client = Server.Instance.CreateClient();

            client.LogInUser();

            var mockGuid = Guid.NewGuid();
            var response = client.GetAsync($"/api/v1/order/{mockGuid}").Result;

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public void CreateOrder_ShouldPass()
        {
            var client = Server.Instance.CreateClient();

            client.LogInUser();

            var product = CreateProduct(client);

            var viewModel = new CreateOrderViewModel()
            {
                UserId = Guid.Parse("96d1fb97-47e9-4ad5-b07e-448f88defd9c"),
                OrderItems = new List<CreateOrderItemViewModel>
                {
                    new CreateOrderItemViewModel()
                    {
                        ProductId = product.Id,
                        Amount = 1,
                        ProductName = product.Name,
                        Responses = new List<CustomFieldResponseViewModel>()
                        {
                            new CustomFieldResponseViewModel()
                            {
                                CustomFieldId = product.CustomFields[0].Id,
                                FieldName = "teste",
                                FieldResponses = new string[] { "Favor sem tomate" }
                            }
                        },
                        SelectedRentPeriod = product.Price.PeriodRentPrices[0].RentPeriodId
                    }
                }
            };
            var response = client.PostAsync($"/api/v1/order", viewModel.ToStringContent()).Result;

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        private ProductViewModel CreateProduct(HttpClient client)
        {
            var rentPeriod = CreateRentPeriod(client);
            var category = CreateCategory(client);

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
                    new CreateCustomFieldViewModel() {  FieldType = "Text", FieldName = "Observação?", Active = true }
                }
            };

            var response = client.PostAsync("/api/v1/product", viewModel.ToStringContent()).Result ;

            return response.Deserialize<ApiResponse<CreateProductCommandResponse>>().Data.Product;
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
