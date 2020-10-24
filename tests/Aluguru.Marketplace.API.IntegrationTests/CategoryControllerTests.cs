using Aluguru.Marketplace.API.IntegrationTests.Extensions;
using Aluguru.Marketplace.API.Models;
using Aluguru.Marketplace.Catalog.Usecases.CreateCategory;
using Aluguru.Marketplace.Catalog.Usecases.UpdateCategory;
using Aluguru.Marketplace.Catalog.ViewModels;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Aluguru.Marketplace.API.IntegrationTests
{
    public class CategoryControllerTests
    {
        [Fact]
        public void CreateCategory_WhenInvalidCategory_ShouldFail()
        {
            var client = Server.Instance.CreateClient();

            client.LogInUser();

            var viewModel = new CreateCategoryViewModel();
            var data = new StringContent(JsonConvert.SerializeObject(viewModel), Encoding.UTF8, "application/json");

            var response = client.PostAsync("/api/v1/category", data).Result;

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public void CreateCategory_ShouldPass()
        {
            var client = Server.Instance.CreateClient();

            client.LogInUser();

            var viewModel = new CreateCategoryViewModel()
            {
                Name = "Móveis"
            };
            var data = new StringContent(JsonConvert.SerializeObject(viewModel), Encoding.UTF8, "application/json");

            var response = client.PostAsync("/api/v1/category", data).Result;

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public void CreateCategory_WhenSubCategory_ShouldPass()
        {
            var client = Server.Instance.CreateClient();

            client.LogInUser();

            var viewModel = new CreateCategoryViewModel()
            {
                Name = "Games"
            };

            var response = client.PostAsync("/api/v1/category", viewModel.ToStringContent()).Result;
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var mainCategory = response.Deserialize<ApiResponse<CreateCategoryCommandResponse>>().Data.Category;
            Assert.Equal("Games", mainCategory.Name);

            viewModel = new CreateCategoryViewModel()
            {
                MainCategoryId = mainCategory.Id,
                Name = "Computador"
            };

            response = client.PostAsync("/api/v1/category", viewModel.ToStringContent()).Result;
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            
            var apiResponse = response.Deserialize<ApiResponse<CreateCategoryCommandResponse>>();
            Assert.Equal(mainCategory.Id, apiResponse.Data.Category.MainCategoryId);
            Assert.Equal("Computador", apiResponse.Data.Category.Name);
        }

        [Fact]
        public void UpdateCategory_ShouldPass()
        {
            var client = Server.Instance.CreateClient();

            client.LogInUser();

            var createViewModel = new CreateCategoryViewModel()
            {
                Name = "Games"
            };

            var response = client.PostAsync("/api/v1/category", createViewModel.ToStringContent()).Result;
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var category = response.Deserialize<ApiResponse<CreateCategoryCommandResponse>>().Data.Category;

            var updateViewModel = new UpdateCategoryViewModel()
            {
                Id = category.Id,
                Name = "Jogos"
            };

            response = client.PutAsync($"/api/v1/category/{category.Id}", updateViewModel.ToStringContent()).Result;
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var editedCategory = response.Deserialize<ApiResponse<UpdateCategoryCommandResponse>>().Data.Category;
            Assert.Equal(category.Id, editedCategory.Id);
            Assert.Equal("Jogos", editedCategory.Name);
        }

        [Fact]
        public void GetAllCategory_ShouldPass()
        {
            var client = Server.Instance.CreateClient();

            var getAllResponse = client.GetAsync("/api/v1/category").Result;
            Assert.Equal(HttpStatusCode.OK, getAllResponse.StatusCode);
        }

        [Fact]
        public void DeleteCategory_ShouldPass()
        {
            var client = Server.Instance.CreateClient();

            client.LogInUser();

            var createViewModel = new CreateCategoryViewModel()
            {
                Name = "Games"
            };

            var response = client.PostAsync("/api/v1/category", createViewModel.ToStringContent()).Result;
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var category = response.Deserialize<ApiResponse<CreateCategoryCommandResponse>>().Data.Category;

            var deleteResponse = client.DeleteAsync($"/api/v1/category/{category.Id}").Result;
            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
        }
    }
}
