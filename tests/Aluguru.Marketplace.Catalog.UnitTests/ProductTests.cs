using Aluguru.Marketplace.Catalog.Domain;
using Aluguru.Marketplace.Domain;
using System;
using System.Collections.Generic;
using Xunit;

namespace Aluguru.Marketplace.Catalog.UnitTests
{
    public class ProductTests
    {
        [Fact]
        public void CreateProduct_WhenEmptyUserId_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() => new Product(Guid.Empty, Guid.NewGuid(), null, "name", "name-uri", "description", ERentType.Indefinite, new Price(500, 500000, 50000, null), true, 1, 2, 30, 1, new InvalidDates(new List<DayOfWeek>(), new List<DateTime>(), new List<Period>()), new List<CustomField>() { new CustomField(EFieldType.Text, "fake") }));
        }

        [Fact]
        public void CreateProduct_WhenEmptyCategoryId_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() => new Product(Guid.NewGuid(), Guid.Empty, null, "name", "name-uri", "description", ERentType.Indefinite, new Price(500, 500000, 50000, null), true, 1, 2, 30, 1, new InvalidDates(new List<DayOfWeek>(), new List<DateTime>(), new List<Period>()), new List<CustomField>() { new CustomField(EFieldType.Text, "fake") }));
        }

        [Fact]
        public void CreateProduct_WhenEmptyName_ShouldThrowDomainException()
        {            
            Assert.Throws<DomainException>(() => new Product(Guid.NewGuid(), Guid.NewGuid(), null, "", "name-uri", "description", ERentType.Indefinite, new Price(500, 500000, 50000, null), true, 1, 2, 30, 1, new InvalidDates(new List<DayOfWeek>(), new List<DateTime>(), new List<Period>()), new List<CustomField>() { new CustomField(EFieldType.Text, "fake") }));
        }

        [Fact]
        public void CreateProduct_WhenEmptyUri_ShouldThrowDomainException()
        {            
            Assert.Throws<DomainException>(() => new Product(Guid.NewGuid(), Guid.NewGuid(), null, "name", "", "description", ERentType.Indefinite, new Price(500, 500000, 50000, null), true, 1, 2, 30, 1, new InvalidDates(new List<DayOfWeek>(), new List<DateTime>(), new List<Period>()), new List<CustomField>() { new CustomField(EFieldType.Text, "fake") }));
        }

        [Fact]
        public void CreateProduct_WhenEmptyDescription_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() => new Product(Guid.NewGuid(), Guid.NewGuid(), null, "name", "name-uri", "", ERentType.Indefinite, new Price(500, 500000, 50000, null), true, 1, 2, 30, 1, new InvalidDates(new List<DayOfWeek>(), new List<DateTime>(), new List<Period>()), new List<CustomField>() { new CustomField(EFieldType.Text, "fake") }));
        }

        [Fact]
        public void CreateProduct_WhenPriceIsZero_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() => new Product(Guid.NewGuid(), Guid.NewGuid(), null, "name", "name-uri", "description", ERentType.Indefinite, null, true, 1, 2, 30, 1, new InvalidDates(new List<DayOfWeek>(), new List<DateTime>(), new List<Period>()), new List<CustomField>() { new CustomField(EFieldType.Text, "fake") }));
        }

        [Fact]
        public void CreateProduct_WhenStockQuantityIsNegative_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() => new Product(Guid.NewGuid(), Guid.NewGuid(), null, "name", "name-uri", "description", ERentType.Indefinite, new Price(500, 500000, 50000, null), true, -1, 2, 30, 1, new InvalidDates(new List<DayOfWeek>(), new List<DateTime>(), new List<Period>()), new List<CustomField>() { new CustomField(EFieldType.Text, "fake") }));
        }

        [Fact]
        public void CreateProduct_WhenMinLocationTimeIsGreaterThanMaxLocationTime_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() => new Product(Guid.NewGuid(), Guid.NewGuid(), null, "name", "name-uri", "description", ERentType.Indefinite, new Price(500, 500000, 50000, null), true, -1, 30, 15, 1, new InvalidDates(new List<DayOfWeek>(), new List<DateTime>(), new List<Period>()), new List<CustomField>() { new CustomField(EFieldType.Text, "fake") }));
        }

        [Fact]
        public void CreateProduct_WhenMinNoticeRentDaysIsLessThanOne_ShouldThrowDomainException()
        {
            Assert.Throws<DomainException>(() => new Product(Guid.NewGuid(), Guid.NewGuid(), null, "name", "name-uri", "description", ERentType.Indefinite, new Price(500, 500000, 50000, null), true, -1, 2, 30, 0, new InvalidDates(new List<DayOfWeek>(), new List<DateTime>(), new List<Period>()), new List<CustomField>() { new CustomField(EFieldType.Text, "fake") }));
        }

        [Fact]
        public void Activate_ShouldActivateProduct()
        {
            var product = CreateProduct();

            product.Active();

            Assert.True(product.IsActive);
        }

        [Fact]
        public void Deactivate_ShouldDeactivateProduct()
        {
            var product = CreateProduct();

            product.Deactivate();

            Assert.False(product.IsActive);
        }

        [Fact]
        public void UpdateCategory_SholdChangeCategory()
        {
            var product = CreateProduct();

            var category = new Category("test", "test", null);
            product.UpdateCategory(category);

            Assert.Equal(category.Id, product.CategoryId);
        }

        [Fact]
        public void UpdateCategory_WhenCategoryIsNull_SholdThrowDomainException()
        {
            var product = CreateProduct();

            Assert.Throws<ArgumentNullException>(() => product.UpdateCategory(null));
        }

        [Fact]
        public void UpdateDescription_SholdChangeDescription()
        {
            var product = CreateProduct();

            product.UpdateDescription("new description");

            Assert.Equal("new description", product.Description);
        }

        [Fact]
        public void UpdateDescription_WhenEmptyDescription_ShouldThrowDomainException()
        {
            var product = CreateProduct();

            Assert.Throws<ArgumentException>(() => product.UpdateDescription(""));
        }

        [Fact]
        public void ReplenishStock_ShouldAddValueToStock()
        {
            var product = CreateProduct();

            product.ReplenishStock(2);

            Assert.Equal(3, product.StockQuantity);
        }

        [Fact]
        public void ReplenishStock_WhenNegativeStock_ShouldThrowDomainException()
        {
            var product = CreateProduct();

            Assert.Throws<ArgumentException>(() => product.ReplenishStock(-2));
        }

        [Fact]
        public void DebitStock_ShouldDebitValueFromStock()
        {
            var product = CreateProduct();

            product.DebitStock(1);

            Assert.Equal(0, product.StockQuantity);
        }

        [Fact]
        public void DebitStock_WhenInsufficientStock_ShouldThrowDomainException()
        {
            var product = CreateProduct();

            Assert.Throws<DomainException>(() => product.DebitStock(10));
        }

        private Product CreateProduct()
        {
            return new Product(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "TestProduct", "test-product", "description", ERentType.Indefinite, new Price(500, 500000, 50000, null), true, 1, 2, 30, 1, new InvalidDates(new List<DayOfWeek>(), new List<DateTime>(), new List<Period>()), new List<CustomField> { new CustomField(EFieldType.Text, "Size") });
        }
    }

}
