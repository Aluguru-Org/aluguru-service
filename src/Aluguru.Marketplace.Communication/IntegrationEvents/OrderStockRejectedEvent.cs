using Aluguru.Marketplace.Communication.Dtos;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using System;
using System.Collections.Generic;

namespace Aluguru.Marketplace.Communication.IntegrationEvents
{
    public class OrderStockRejectedEvent : Event
    {
        public OrderStockRejectedEvent(List<OrderItemDTO> orderItems)
        {
            OrderItems = orderItems;
        }

        public Guid OrderId { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }
    }
}
