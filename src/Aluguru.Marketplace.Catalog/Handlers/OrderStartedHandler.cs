using Aluguru.Marketplace.Catalog.Usecases.DebitProductStock;
using Aluguru.Marketplace.Communication.IntegrationEvents;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Catalog.Handlers
{
    public class OrderStartedHandler : INotificationHandler<OrderStartedEvent>
    {
        private readonly IMediatorHandler _mediatorHandler;
        public OrderStartedHandler(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        public async Task Handle(OrderStartedEvent notification, CancellationToken cancellationToken)
        {
            await _mediatorHandler.SendCommand<DebitProductStockCommand, bool>(new DebitProductStockCommand(notification.Order));
        }
    }
}
