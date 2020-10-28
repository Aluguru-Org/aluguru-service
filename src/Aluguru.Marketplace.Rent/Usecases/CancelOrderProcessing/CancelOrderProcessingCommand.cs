using Aluguru.Marketplace.Communication.Dtos;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using System;
using System.Collections.Generic;

namespace Aluguru.Marketplace.Rent.Usecases.CancelOrderProcessing
{
    public class CancelOrderProcessingCommand : Command<bool>
    {
        public CancelOrderProcessingCommand(Guid orderId, List<OrderItemDTO> orderItems)
        {
            OrderId = orderId;
            OrderItems = orderItems;
        }

        public Guid OrderId { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }
    }
}
