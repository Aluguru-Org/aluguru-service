using Mubbi.Marketplace.API.IntegrationTests.Extensions;
using Mubbi.Marketplace.API.Models;
using Mubbi.Marketplace.Catalog.Usecases.CreateCategory;
using Mubbi.Marketplace.Catalog.Usecases.UpdateCategory;
using Mubbi.Marketplace.Catalog.ViewModels;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Mubbi.Marketplace.API.IntegrationTests
{
    public class CategoryControllerTests
    {
        [Fact]
        public async Task CreateCategory_WhenInvalidCategory_ShouldFail()
        {
            var client = Server.Instance.CreateClient();

            var viewModel = new CreateCategoryViewModel();
            var data = new StringContent(JsonConvert.SerializeObject(viewModel), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/v1/category", data);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateCategory_ShouldPass()
        {
            var client = Server.Instance.CreateClient();

            var viewModel = new CreateCategoryViewModel()
            {
                Name = "Móveis"
            };
            var data = new StringContent(JsonConvert.SerializeObject(viewModel), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/v1/category", data);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task CreateCategory_WhenSubCategory_ShouldPass()
        {
            var client = Server.Instance.CreateClient();

            var viewModel = new CreateCategoryViewModel()
            {
                Name = "Games"
            };

            var response = await client.PostAsync("/api/v1/category", viewModel.ToStringContent());
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var mainCategory = response.Deserialize<ApiResponse<CreateCategoryCommandResponse>>().Data.Category;
            Assert.Equal("Games", mainCategory.Name);

            viewModel = new CreateCategoryViewModel()
            {
                MainCategoryId = mainCategory.Id,
                Name = "Computador"
            };

            response = await client.PostAsync("/api/v1/category", viewModel.ToStringContent());
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            
            var apiResponse = response.Deserialize<ApiResponse<CreateCategoryCommandResponse>>();
            Assert.Equal(mainCategory.Id, apiResponse.Data.Category.MainCategoryId);
            Assert.Equal("Computador", apiResponse.Data.Category.Name);
        }

        [Fact]
        public async Task UpdateCategory_ShouldPass()
        {
            var client = Server.Instance.CreateClient();

            var createViewModel = new CreateCategoryViewModel()
            {
                Name = "Games"
            };

            var response = await client.PostAsync("/api/v1/category", createViewModel.ToStringContent());
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var category = response.Deserialize<ApiResponse<CreateCategoryCommandResponse>>().Data.Category;

            var updateViewModel = new UpdateCategoryViewModel()
            {
                Id = category.Id,
                Name = "Jogos"
            };

            response = await client.PutAsync($"/api/v1/category/{category.Id}", updateViewModel.ToStringContent());
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var editedCategory = response.Deserialize<ApiResponse<UpdateCategoryCommandResponse>>().Data.Category;
            Assert.Equal(category.Id, editedCategory.Id);
            Assert.Equal("Jogos", editedCategory.Name);
        }

        [Fact]
        public async Task GetAllCategory_ShouldPass()
        {
            var client = Server.Instance.CreateClient();

            var getAllResponse = await client.GetAsync("/api/v1/category");
            Assert.Equal(HttpStatusCode.OK, getAllResponse.StatusCode);
        }

        [Fact]
        public async Task DeleteCategory_ShouldPass()
        {
            var client = Server.Instance.CreateClient();

            var createViewModel = new CreateCategoryViewModel()
            {
                Name = "Games"
            };

            var response = await client.PostAsync("/api/v1/category", createViewModel.ToStringContent());
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var category = response.Deserialize<ApiResponse<CreateCategoryCommandResponse>>().Data.Category;

            var deleteResponse = await client.DeleteAsync($"/api/v1/category/{category.Id}");
            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
        }
    }
}
