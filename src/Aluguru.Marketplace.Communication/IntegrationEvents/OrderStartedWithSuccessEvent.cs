using Aluguru.Marketplace.Communication.Dtos;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;

namespace Aluguru.Marketplace.Communication.IntegrationEvents
{
    public class OrderStartedWithSuccessEvent : Event
    {
        public OrderStartedWithSuccessEvent(OrderDTO order)
        {
            Order = order;
        }

        public OrderDTO Order { get; set; }
    }
}
