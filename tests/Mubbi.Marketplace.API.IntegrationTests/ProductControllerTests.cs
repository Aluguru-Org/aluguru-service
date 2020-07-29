using Mubbi.Marketplace.API.IntegrationTests.Extensions;
using Mubbi.Marketplace.Catalog.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
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

        //[Fact]
        //public async Task CreateProduct_ShouldPass()
        //{
        //    var client = Server.Instance.CreateClient();

        //    var user = await client.LogInUser();

        //    var viewModel = new CreateProductViewModel()
        //    {

        //    };

        //    var response = await client.PostAsync("/api/v1/product", viewModel.ToStringContent());

        //    Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        //}
    }
}
