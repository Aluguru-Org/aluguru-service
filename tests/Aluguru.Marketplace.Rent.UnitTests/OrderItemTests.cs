using Aluguru.Marketplace.Rent.Domain;
using System;
using Xunit;

namespace Aluguru.Marketplace.Rent.Tests
{
    public class OrderItemTests
    {
        [Fact]
        public void CreateOrderItem_WhenEmptyProductId_ShouldThrowDomainException()
        {
            Assert.Throws<Exception>(() => new OrderItem(Guid.Empty, Guid.NewGuid(), "test", "test-uri", DateTime.Now, 7, 1, 10000, 500));
        }

        [Fact]
        public void CreateOrderItem_WhenEmptyCompanyId_ShouldThrowDomainException()
        {
            Assert.Throws<Exception>(() => new OrderItem(Guid.NewGuid(), Guid.Empty, "test", "test-uri", DateTime.Now, 7, 1, 10000, 500));
        }

        [Fact]
        public void CreateOrderItem_WhenEmptyProductName_ShouldThrowDomainException()
        {
            Assert.Throws<Exception>(() => new OrderItem(Guid.NewGuid(), Guid.NewGuid(), "", "test-uri", DateTime.Now, 7, 1, 10000, 500));
        }

        [Fact]
        public void CreateOrderItem_WhenEmptyProductUri_ShouldThrowDomainException()
        {
            Assert.Throws<Exception>(() => new OrderItem(Guid.NewGuid(), Guid.NewGuid(), "test", "", DateTime.Now, 7, 1, 10000, 500));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void CreateOrderItem_WhenRentDaysSmallerOrEqualThanZero_ShouldThrowDomainException(int rentDays)
        {
            Assert.Throws<Exception>(() => new OrderItem(Guid.NewGuid(), Guid.NewGuid(), "test", "test-uri", DateTime.Now, rentDays, 1, 10000, 500));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void CreateOrderItem_WhenAmountSmallerOrEqualThanZero_ShouldThrowDomainException(int amount)
        {
            Assert.Throws<Exception>(() => new OrderItem(Guid.NewGuid(), Guid.NewGuid(), "test", "test-uri", DateTime.Now, 7, amount, 10000, 500));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void CreateOrderItem_WhenProductPriceSmallerOrEqualThanZero_ShouldThrowDomainException(decimal price)
        {
            Assert.Throws<Exception>(() => new OrderItem(Guid.NewGuid(), Guid.NewGuid(), "test", "test-uri", DateTime.Now, 7, 1, price, 500));
        }

        [Fact]
        public void CreateOrderItem_WhenProductFreigthPriceSmallerOrEqualThanZero_ShouldThrowDomainException()
        {
            Assert.Throws<Exception>(() => new OrderItem(Guid.NewGuid(), Guid.NewGuid(), "test", "test-uri", DateTime.Now, 7, 1, 10000, -1));
        }

        [Theory]
        [InlineData(200, 1, 100, 100)]
        [InlineData(400, 2, 100, 100)]
        [InlineData(800, 4, 100, 100)]
        public void CalculatePrice(decimal result, int amount, decimal price, decimal freigthPrice)
        {
            var item = new OrderItem(Guid.NewGuid(), Guid.NewGuid(), "test", "test-uri", DateTime.Now, 7, amount, price, freigthPrice);

            var calcPrice = item.CalculatePrice();

            Assert.Equal(result, calcPrice);
        }
    }
}
