using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using System;

namespace Aluguru.Marketplace.Communication.IntegrationEvents
{
    public class OrderPaidEvent : Event
    {
        public OrderPaidEvent(Guid orderId)
        {
            OrderId = orderId;
        }

        public Guid OrderId { get; set; }
    }
}
