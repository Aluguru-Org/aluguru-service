using Moq;
using Mubbi.Marketplace.Catalog.Application.Services;
using Mubbi.Marketplace.Shared.DomainObjects;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mubbi.Marketplace.Catalog.Application.Tests
{
    public class ProductAppServiceTests
    {
        public ProductAppServiceTests()
        {            
        }

        [Fact]
        public void GetById_WhenExistingGuid_ShouldReturnProductViewModel()
        {
            IProductAppService service = new ProductAppService();

            var product = service.GetById(Guid.NewGuid()).Result;

            Assert.NotNull(product);
        }

        [Fact]
        public void GetById_WhenNonExistingGuid_ShouldReturnNull()
        {
            IProductAppService service = new ProductAppService();

            var product = service.GetById(Guid.NewGuid()).Result;

            Assert.Null(product);
        }

        [Fact]
        public void GetById_WhenEmptyGuid_ShouldThrowDomainException()
        {
            IProductAppService service = new ProductAppService();

            Assert.ThrowsAsync<DomainException>(async () => await service.GetById(Guid.Empty));
        }

        [Fact]
        public void GetByCategory_WhenExistingCode_ShouldReturnProductsList()
        {
            IProductAppService service = new ProductAppService();

            var products = service.GetByCategory(1000).Result;

            Assert.NotNull(products);
            Assert.Equal(3, products.Count());
        }

        [Fact]
        public void GetByCategory_WhenExistingCode_ShouldReturnEmptyProductsList()
        {
            IProductAppService service = new ProductAppService();

            var products = service.GetByCategory(2000).Result;

            Assert.NotNull(products);
            Assert.Empty(products);
        }

        [Fact]
        public void GetAllProducts_ShouldReturnAllProducts()
        {
            IProductAppService service = new ProductAppService();

            var products = service.GetByCategory(1000).Result;

            Assert.NotNull(products);
            Assert.Equal(10, products.Count());
        }

        [Fact]
        public void GetAllCategories_ShouldReturnAllCategories()
        {
            IProductAppService service = new ProductAppService();

            var categories = service.GetAllCategories().Result;

            Assert.NotNull(categories);
            Assert.Equal(4, categories.Count());
        }
    }
}
