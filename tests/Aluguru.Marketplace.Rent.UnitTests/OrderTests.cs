using Aluguru.Marketplace.Rent.Domain;
using System;
using Xunit;

namespace Aluguru.Marketplace.Rent.Tests
{
    public class OrderTests
    {
        [Fact]
        public void AddItem_WhenNew_ShouldAddNewItem()
        {
            var item = new OrderItem(Guid.NewGuid(), Guid.NewGuid(), "test", "test", DateTime.Now, 1, 1, 100, 100);
            var order = new Order(Guid.NewGuid());

            order.AddItem(item);

            Assert.Equal(1, order.OrderItems.Count);
            Assert.Equal(200, order.TotalPrice);
        }

        [Fact]
        public void ItemExists_WhenItemExists_ShouldReturnTrue()
        {
            var item = new OrderItem(Guid.NewGuid(), Guid.NewGuid(), "test", "test", DateTime.Now, 1, 1, 100, 100);
            var order = new Order(Guid.NewGuid());

            order.AddItem(item);

            Assert.True(order.ItemExists(item));
        }

        [Fact]
        public void ItemExists_WhenItemDoesNotExists_ShouldReturnFalse()
        {
            var item = new OrderItem(Guid.NewGuid(), Guid.NewGuid(), "test", "test", DateTime.Now, 1, 1, 100, 100);
            var order = new Order(Guid.NewGuid());

            Assert.False(order.ItemExists(item));
        }

        [Fact]
        public void ApplyVoucher_WhenValueDiscount_ShouldApplyDiscount()
        {
            var itemA = new OrderItem(Guid.NewGuid(), Guid.NewGuid(), "test", "test", DateTime.Now, 1, 1, 100, 100);
            var itemB = new OrderItem(Guid.NewGuid(), Guid.NewGuid(), "test", "test", DateTime.Now, 1, 1, 300, 100);

            var order = new Order(Guid.NewGuid());
            var voucher = new Voucher("some-code", EVoucherType.Value, 150, 1, DateTime.Now.AddDays(7));

            order.AddItem(itemA);
            order.AddItem(itemB);

            Assert.Equal(600, order.TotalPrice);

            order.ApplyVoucher(voucher);

            Assert.Equal(450, order.TotalPrice);
            Assert.True(order.VoucherUsed);
        }

        [Fact]
        public void ApplyVoucher_WhenPercentDiscount_ShouldApplyDiscount()
        {
            var itemA = new OrderItem(Guid.NewGuid(), Guid.NewGuid(), "test", "test", DateTime.Now, 1, 1, 100, 100);
            var itemB = new OrderItem(Guid.NewGuid(), Guid.NewGuid(), "test", "test", DateTime.Now, 1, 1, 300, 100);
            var order = new Order(Guid.NewGuid());
            var voucher = new Voucher("some-code", EVoucherType.Percent, 50, 1, DateTime.Now.AddDays(7));

            order.AddItem(itemA);
            order.AddItem(itemB);

            Assert.Equal(600, order.TotalPrice);

            order.ApplyVoucher(voucher);

            Assert.Equal(300, order.TotalPrice);
            Assert.True(order.VoucherUsed);
        }
    }
}
