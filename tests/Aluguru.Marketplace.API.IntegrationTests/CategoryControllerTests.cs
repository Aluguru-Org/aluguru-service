using Aluguru.Marketplace.API.IntegrationTests.Config;
using Aluguru.Marketplace.API.IntegrationTests.Extensions;
using Aluguru.Marketplace.API.Models;
using Aluguru.Marketplace.Catalog.Usecases.CreateCategory;
using Aluguru.Marketplace.Catalog.Dtos;
using System.Net;
using Xunit;

namespace Aluguru.Marketplace.API.IntegrationTests
{
    [TestCaseOrderer("Aluguru.Marketplace.API.IntegrationTests.Extensions.PriorityOrderer", "Aluguru.Marketplace.API.IntegrationTests")]
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class CategoryControllerTests
    {
        private readonly IntegrationTestsFixture<StartupTests> _fixture;
        public CategoryControllerTests(IntegrationTestsFixture<StartupTests> testsFixture)
        {
            _fixture = testsFixture;
        }

        [Fact(DisplayName = "Create category with error"), TestPriority(1)]
        [Trait("Catalog", "Api Integration - Create category")]
        public void CreateCategory_WhenInvalidCategory_ShouldFail()
        {
            // Arrange
            var viewModel = new CreateCategoryDTO();

            // Act
            _fixture.Admin.LogIn().Wait();
            var response = _fixture.Admin.Client.PostAsJsonAsync("/api/v1/category", viewModel).Result;

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact(DisplayName = "Create category with success"), TestPriority(1)]
        [Trait("Catalog", "Api Integration - Create Category")]
        public void CreateCategory_ShouldPass()
        {
            // Arrange
            var viewModel = new CreateCategoryDTO()
            {
                Name = _fixture.Category.Name,
                Uri = _fixture.Category.Uri
            };

            // Act
            _fixture.Admin.LogIn().Wait();
            var response = _fixture.Admin.Client.PostAsJsonAsync("/api/v1/category", viewModel).Result;
            var category = response.Deserialize<ApiResponse<CreateCategoryCommandResponse>>().Data.Category;

            _fixture.Category.Id = category.Id;
            _fixture.SubCategory.MainCategoryId = category.Id;

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact(DisplayName = "Create sub category with success"), TestPriority(2)]
        [Trait("Catalog", "Api Integration - Create Sub Category")]
        public void CreateCategory_WhenSubCategory_ShouldPass()
        {
            // Arrange
            var viewModel = new CreateCategoryDTO()
            {
                MainCategoryId = _fixture.SubCategory.MainCategoryId,
                Name = _fixture.SubCategory.Name,
                Uri = _fixture.SubCategory.Uri
            };

            // Act
            _fixture.Admin.LogIn().Wait();
            var response = _fixture.Admin.Client.PostAsJsonAsync("/api/v1/category", viewModel).Result;
            var category = response.Deserialize<ApiResponse<CreateCategoryCommandResponse>>().Data.Category;

            _fixture.SubCategory.Id = category.Id;            

            //Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact(DisplayName = "Update category with success"), TestPriority(3)]
        [Trait("Catalog", "Api Integration - Update Category")]
        public void UpdateCategory_ShouldPass()
        {
            // Arrange            
            var categoryId = _fixture.SubCategory.Id;
            var viewModel = new UpdateCategoryDTO()
            {
                Id = _fixture.SubCategory.Id,
                Name = "Games",
                Uri = "games"
            };

            // Act
            _fixture.Admin.LogIn().Wait();
            var response = _fixture.Admin.Client.PutAsJsonAsync($"api/v1/category/{categoryId}", viewModel).Result;

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact(DisplayName = "Get category with success"), TestPriority(3)]
        [Trait("Catalog", "Api Integration - Get Category")]
        public void GetAllCategory_ShouldPass()
        {
            // Arrange

            // Act
            var response = _fixture.Admin.Client.GetAsync("/api/v1/category").Result;

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "Delete category with success"), TestPriority(9999)]
        [Trait("Catalog", "Api Integration - Delete Category")]
        public void DeleteCategory_ShouldPass()
        {
            // Arrange
            var categoryId = _fixture.Category.Id;

            // Act
            _fixture.Admin.LogIn().Wait();
            var response = _fixture.Admin.Client.DeleteAsync($"/api/v1/category/{categoryId}").Result;

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "Delete category with success"), TestPriority(9999)]
        [Trait("Catalog", "Api Integration - Delete Sub Category")]
        public void DeleteSubCategory_ShouldPass()
        {
            // Arrange
            var categoryId = _fixture.SubCategory.Id;

            // Act
            _fixture.Admin.LogIn().Wait();
            var response = _fixture.Admin.Client.DeleteAsync($"/api/v1/category/{categoryId}").Result;

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
