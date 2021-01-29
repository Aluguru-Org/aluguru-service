using Aluguru.Marketplace.Communication.Dtos;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;

namespace Aluguru.Marketplace.Communication.IntegrationEvents
{
    public class OrderStockAcceptedEvent : Event
    {
        public OrderStockAcceptedEvent(OrderDTO order)
        {
            Order = order;
        }

        public OrderDTO Order { get; set; }
    }
}
