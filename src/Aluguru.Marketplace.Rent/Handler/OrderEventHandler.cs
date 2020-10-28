using Aluguru.Marketplace.Communication.IntegrationEvents;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Rent.Usecases.CancelOrderProcessing;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Rent.Handler
{
    public class OrderEventHandler : INotificationHandler<OrderStockRejectedEvent>
    {
        private readonly IMediatorHandler _mediatorHandler;

        public OrderEventHandler(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        public async Task Handle(OrderStockRejectedEvent notification, CancellationToken cancellationToken)
        {
            var command = new CancelOrderProcessingCommand(notification.OrderId, notification.OrderItems);
            await _mediatorHandler.SendCommand<CancelOrderProcessingCommand, bool>(command);
        }
    }
}
