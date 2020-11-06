using Aluguru.Marketplace.API.IntegrationTests.Config;
using Aluguru.Marketplace.API.IntegrationTests.Extensions;
using Aluguru.Marketplace.API.Models;
using Aluguru.Marketplace.Catalog.Domain;
using Aluguru.Marketplace.Catalog.Usecases.CreateProduct;
using Aluguru.Marketplace.Catalog.ViewModels;
using Bogus;
using System;
using System.Collections.Generic;
using System.Net;
using Xunit;

namespace Aluguru.Marketplace.API.IntegrationTests
{
    [TestCaseOrderer("Aluguru.Marketplace.API.IntegrationTests.Extensions.PriorityOrderer", "Aluguru.Marketplace.API.IntegrationTests")]
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class ProductControllerTests
    {
        private readonly IntegrationTestsFixture<StartupTests> _fixture;

        public ProductControllerTests(IntegrationTestsFixture<StartupTests> testsFixture)
        {
            _fixture = testsFixture;
        }

        [Fact(DisplayName = "Create product with fixed price  with success"), TestPriority(5)]
        [Trait("Catalog", "Api Integration - Create Product Fixed Price")]
        public void CreateProduct_WithFixedPrice_ShouldPass()
        {
            // Arrange
            var viewModel = CreateProduct(CreatePrice(ERentType.Fixed.ToString()));

            // Act
            _fixture.Company.LogIn().Wait();
            var response = _fixture.Company.Client.PostAsJsonAsync("/api/v1/product", viewModel).Result;
            var product = response.Deserialize<ApiResponse<CreateProductCommandResponse>>().Data.Product;

            _fixture.ProductWithFixedPrice.ViewModel = product;
            
            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact(DisplayName = "Create product with indefinite price  with success"), TestPriority(5)]
        [Trait("Catalog", "Api Integration - Create Product Indefinite Price")]
        public void CreateProduct_WithIndefinitePrice_ShouldPass()
        {
            // Arrange
            var viewModel = CreateProduct(CreatePrice(ERentType.Indefinite.ToString()));

            // Act
            _fixture.Company.LogIn().Wait();
            var response = _fixture.Company.Client.PostAsJsonAsync("/api/v1/product", viewModel).Result;
            var product = response.Deserialize<ApiResponse<CreateProductCommandResponse>>().Data.Product;

            _fixture.ProductWithIndefinitePrice.ViewModel = product;

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact(DisplayName = "Update product with fixed price with success"), TestPriority(6)]
        [Trait("Catalog", "Api Integration - Update Product Fixed Price")]
        public void UpdateProduct_ShouldPass()
        {
            // Arrange
            var faker = new Faker("pt_BR");
            var product = _fixture.ProductWithFixedPrice.ViewModel;

            var updateProductViewModel = new UpdateProductViewModel()
            {
                Id = product.Id,
                CategoryId = product.CategoryId,
                SubCategoryId = product.SubCategoryId,
                Name = faker.Commerce.ProductName(),
                Description = faker.Commerce.ProductDescription(),
                RentType = ERentType.Fixed.ToString(),
                Price = product.Price,
                IsActive = true,
                MinNoticeRentDays = 2,
                MinRentDays = 7,
                MaxRentDays = 30,
                StockQuantity = 1,
                CustomFields = new List<UpdateCustomFieldViewModel>
                {
                    new UpdateCustomFieldViewModel() {  FieldType = "Text", ValueAsString = "Observação?", Active = true }
                }
            };

            // Act
            _fixture.Company.LogIn().Wait();
            var response = _fixture.Company.Client.PutAsJsonAsync($"/api/v1/product/{updateProductViewModel.Id}", updateProductViewModel).Result;

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact(DisplayName = "Delete product with fixed price with success"), TestPriority(9999)]
        [Trait("Catalog", "Api Integration - Delete Product Fixed Price")]
        public void DeleteProduct_WithFixedPrice_ShouldPass()
        {
            // Arrange
            var productId = _fixture.ProductWithFixedPrice.ViewModel.Id;

            // Act
            _fixture.Company.LogIn().Wait();
            var response = _fixture.Company.Client.DeleteAsync($"/api/v1/product/{productId}").Result;

            // Assert            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        private CreateProductViewModel CreateProduct(PriceViewModel price)
        {
            var faker = new Faker("pt_BR");

            return new CreateProductViewModel()
            {
                UserId = _fixture.Company.Id,
                CategoryId = _fixture.Category.Id,
                SubCategoryId = _fixture.SubCategory.Id,
                Name = faker.Commerce.Product(),
                Description = faker.Commerce.ProductDescription(),
                RentType = "Indefinite",
                Price = price,
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
        }

        private PriceViewModel CreatePrice(string rentType)
        {
            decimal sellPrice = (decimal)new Random().NextDouble() * new Random().Next(100, 100000); 

            switch(rentType)
            {
                default:
                case "Fixed":
                    return new PriceViewModel()
                    {
                        SellPrice = sellPrice,
                        DailyRentPrice = sellPrice / 10
                    };
                case "Indefinite":
                    return new PriceViewModel()
                    {
                        SellPrice = sellPrice,
                        PeriodRentPrices = new List<PeriodPriceViewModel>()
                    {
                        new PeriodPriceViewModel()
                        {
                            RentPeriodId = _fixture.RentPeriodMonth.Id,
                            Price = sellPrice/10
                        }
                    }
                    };
            }
        }
    }
}
