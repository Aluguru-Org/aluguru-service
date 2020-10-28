using Aluguru.Marketplace.Communication.Dtos;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;

namespace Aluguru.Marketplace.Communication.IntegrationEvents
{
    public class OrderStockConfirmedEvent : Event
    {
        public OrderStockConfirmedEvent(OrderDTO orderDTO)
        {
            OrderDTO = orderDTO;
        }

        public OrderDTO OrderDTO { get; set; }
    }
}
