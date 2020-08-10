using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Mubbi.Marketplace.Rent.Domain.Tests
{
    public class OrderTests
    {
        [Fact]
        public void AddItem_WhenNew_ShouldAddNewItem()
        {
            var item = new OrderItem(Guid.NewGuid(), "test", 1, 100);
            var order = new Order(Guid.NewGuid());

            order.AddItem(item);

            Assert.Equal(1, order.OrderItems.Count);
            Assert.Equal(100, order.TotalPrice);
        }

        [Fact]
        public void RemoveItem_WhenExisting_ShouldRemoveItem()
        {
            var item = new OrderItem(Guid.NewGuid(), "test", 1, 100);
            var order = new Order(Guid.NewGuid());

            order.AddItem(item);

            order.RemoveItem(item);

            Assert.Equal(0, order.OrderItems.Count);
            Assert.Equal(0, order.TotalPrice);

        }

        [Fact]
        public void AddItem_WhenExisting_ShouldUpdateItem()
        {
            var item = new OrderItem(Guid.NewGuid(), "test", 1, 100);
            var order = new Order(Guid.NewGuid());

            order.AddItem(item);
            order.AddItem(item);

            Assert.Equal(1, order.OrderItems.Count);
            Assert.Equal(200, order.TotalPrice);
        }
    }
}
