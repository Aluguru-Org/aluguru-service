using Mubbi.Marketplace.Domain;
using System;
using Xunit;

namespace Mubbi.Marketplace.Catalog.Domain.Tests
{
    public class ProductTests
    {
        [Fact]
        public void CreateProduct_WhenEmptyCategoryId_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() => new Product(Guid.Empty, "name", "description", "image.png", 50000, true, 1, ERentType.Daily, new TimeSpan(2, 0, 0), new TimeSpan(30, 0, 0, 0)));
        }

        [Fact]
        public void CreateProduct_WhenEmptyName_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() => new Product(Guid.NewGuid(), "", "description", "image.png", 50000, true, 1, ERentType.Daily, new TimeSpan(2, 0, 0), new TimeSpan(30, 0, 0, 0)));
        }

        [Fact]
        public void CreateProduct_WhenEmptyDescription_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() => new Product(Guid.NewGuid(), "name", "", "image.png", 50000, true, 1, ERentType.Daily, new TimeSpan(2, 0, 0), new TimeSpan(30, 0, 0, 0)));
        }

        [Fact]
        public void CreateProduct_WhenEmptyImage_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() => new Product(Guid.NewGuid(), "name", "description", "", 50000, true, 1, ERentType.Daily, new TimeSpan(2, 0, 0), new TimeSpan(30, 0, 0, 0)));
        }

        [Fact]
        public void CreateProduct_WhenPriceIsZero_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() => new Product(Guid.NewGuid(), "name", "description", "image.png", 0, true, 1, ERentType.Daily, new TimeSpan(2, 0, 0), new TimeSpan(30, 0, 0, 0)));
        }

        [Fact]
        public void CreateProduct_WhenStockQuantityIsNegative_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() => new Product(Guid.NewGuid(), "name", "description", "image.png", 50000, true, -1, ERentType.Daily, new TimeSpan(2, 0, 0), new TimeSpan(30, 0, 0, 0)));
        }

        [Fact]
        public void CreateProduct_WhenMinLocationTimeIsGreaterThanMaxLocationTime_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() => new Product(Guid.NewGuid(), "name", "description", "image.png", 50000, true, -1, ERentType.Daily, new TimeSpan(30, 0, 0), TimeSpan.Zero));
        }

        [Fact]
        public void CreateProduct_WhenDimensionsIsNull_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() => new Product(Guid.NewGuid(), "name", "description", "image.png", 50000, true, -1, ERentType.Daily, new TimeSpan(2, 0, 0), new TimeSpan(30, 0, 0, 0)));
        }

        [Fact]
        public void Activate_ShouldActivateProduct()
        {
            var product = new Product(Guid.NewGuid(), "name", "description", "image.png", 50000, true, 1, ERentType.Daily, new TimeSpan(2, 0, 0), new TimeSpan(30, 0, 0, 0));

            product.Active();

            Assert.True(product.IsActive);
        }

        [Fact]
        public void Deactivate_ShouldDeactivateProduct()
        {
            var product = new Product(Guid.NewGuid(), "name", "description", "image.png", 50000, true, 1, ERentType.Daily, new TimeSpan(2, 0, 0), new TimeSpan(30, 0, 0, 0));

            product.Deactivate();

            Assert.False(product.IsActive);
        }

        [Fact]
        public void UpdateCategory_SholdChangeCategory()
        {
            var product = new Product(Guid.NewGuid(), "name", "description", "image.png", 50000, true, 1, ERentType.Daily, new TimeSpan(2, 0, 0), new TimeSpan(30, 0, 0, 0));

            var category = new Category("test", 1000);
            product.UpdateCategory(category);

            Assert.Equal(category.Id, product.CategoryId);
        }

        [Fact]
        public void UpdateCategory_WhenCategoryIsNull_SholdThrowDomainException()
        {
            var product = new Product(Guid.NewGuid(), "name", "description", "image.png", 50000, true, 1, ERentType.Daily, new TimeSpan(2, 0, 0), new TimeSpan(30, 0, 0, 0));

            Assert.Throws<ArgumentNullException>(() => product.UpdateCategory(null));
        }

        [Fact]
        public void UpdateDescription_SholdChangeDescription()
        {
            var product = new Product(Guid.NewGuid(), "name", "description", "image.png", 50000, true, 1, ERentType.Daily, new TimeSpan(2, 0, 0), new TimeSpan(30, 0, 0, 0));

            product.UpdateDescription("new description");

            Assert.Equal("new description", product.Description);
        }

        [Fact]
        public void UpdateDescription_WhenEmptyDescription_ShouldThrowDomainException()
        {
            var product = new Product(Guid.NewGuid(), "name", "description", "image.png", 50000, true, 1, ERentType.Daily, new TimeSpan(2, 0, 0), new TimeSpan(30, 0, 0, 0));

            Assert.Throws<ArgumentException>(() => product.UpdateDescription(""));
        }

        [Fact]
        public void ReplenishStock_ShouldAddValueToStock()
        {
            var product = new Product(Guid.NewGuid(), "name", "description", "image.png", 50000, true, 1, ERentType.Daily, new TimeSpan(2, 0, 0), new TimeSpan(30, 0, 0, 0));

            product.ReplenishStock(2);

            Assert.Equal(3, product.StockQuantity);
        }

        [Fact]
        public void ReplenishStock_WhenNegativeStock_ShouldThrowDomainException()
        {
            var product = new Product(Guid.NewGuid(), "name", "description", "image.png", 50000, true, 1, ERentType.Daily, new TimeSpan(2, 0, 0), new TimeSpan(30, 0, 0, 0));

            Assert.Throws<ArgumentException>(() => product.ReplenishStock(-2));
        }

        [Fact]
        public void DebitStock_ShouldDebitValueFromStock()
        {
            var product = new Product(Guid.NewGuid(), "name", "description", "image.png", 50000, true, 1, ERentType.Daily, new TimeSpan(2, 0, 0), new TimeSpan(30, 0, 0, 0));

            product.DebitStock(1);

            Assert.Equal(0, product.StockQuantity);
        }

        [Fact]
        public void DebitStock_WhenInsufficientStock_ShouldThrowDomainException()
        {
            var product = new Product(Guid.NewGuid(), "name", "description", "image.png", 50000, true, 1, ERentType.Daily, new TimeSpan(2, 0, 0), new TimeSpan(30, 0, 0, 0));

            Assert.Throws<DomainException>(() => product.DebitStock(10));
        }
    }

}
