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
            Assert.Throws<Exception>(() => new OrderItem(Guid.Empty, "test", DateTime.Now, 1, 100));
        }

        [Fact]
        public void CreateOrderItem_WhenEmptyProductName_ShouldThrowDomainException()
        {
            Assert.Throws<Exception>(() => new OrderItem(Guid.NewGuid(), "", DateTime.Now, 1, 100));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void CreateOrderItem_WhenAmountSmallerOrEqualThanZero_ShouldThrowDomainException(int amount)
        {
            Assert.Throws<Exception>(() => new OrderItem(Guid.NewGuid(), "test", DateTime.Now, amount, 100));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void CreateOrderItem_WhenProductPriceSmallerOrEqualThanZero_ShouldThrowDomainException(decimal price)
        {
            Assert.Throws<Exception>(() => new OrderItem(Guid.NewGuid(), "test", DateTime.Now, 1, price));
        }

        [Theory]
        [InlineData(100, 1, 100)]
        [InlineData(200, 2, 100)]
        [InlineData(400, 4, 100)]
        public void CalculatePrice(decimal result, int amount, decimal price)
        {
            var item = new OrderItem(Guid.NewGuid(), "test", DateTime.Now, amount, price);

            var calcPrice = item.CalculatePrice();

            Assert.Equal(result, calcPrice);
        }
    }
}
