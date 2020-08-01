using System;
using Xunit;

namespace Mubbi.Marketplace.Rent.Domain.Tests
{
    public class OrderItemTests
    {
        [Fact]
        public void CreateOrderItem_WhenEmptyProductId_ShouldThrowDomainException()
        {
            Assert.Throws<Exception>(() => new OrderItem(Guid.Empty, "test", 1, 100));
        }

        [Fact]
        public void CreateOrderItem_WhenEmptyProductName_ShouldThrowDomainException()
        {
            Assert.Throws<Exception>(() => new OrderItem(Guid.NewGuid(), "", 1, 100));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void CreateOrderItem_WhenAmountSmallerOrEqualThanZero_ShouldThrowDomainException(int amount)
        {
            Assert.Throws<Exception>(() => new OrderItem(Guid.NewGuid(), "test", amount, 100));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void CreateOrderItem_WhenProductPriceSmallerOrEqualThanZero_ShouldThrowDomainException(decimal price)
        {
            Assert.Throws<Exception>(() => new OrderItem(Guid.NewGuid(), "test", 1, price));
        }

        [Theory]
        [InlineData(100, 1, 100)]
        [InlineData(200, 2, 100)]
        [InlineData(400, 4, 100)]
        public void CalculatePrice(decimal result, int amount, decimal price)
        {
            var item = new OrderItem(Guid.NewGuid(), "test", amount, price);

            var calcPrice = item.CalculatePrice();

            Assert.Equal(result, calcPrice);
        }
    }
}
