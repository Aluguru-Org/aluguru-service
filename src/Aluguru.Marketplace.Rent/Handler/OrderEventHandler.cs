using Aluguru.Marketplace.Communication.IntegrationEvents;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Rent.Usecases.CancelOrderProcessing;
using Aluguru.Marketplace.Rent.Usecases.ConfirmOrderPayment;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Rent.Handler
{
    public class OrderEventHandler : 
        INotificationHandler<OrderStockRejectedEvent>,
        INotificationHandler<OrderPaidEvent>
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

        public async Task Handle(OrderPaidEvent notification, CancellationToken cancellationToken)
        {
            var command = new ConfirmOrderPaymentCommand(notification.OrderId);
            await _mediatorHandler.SendCommand<ConfirmOrderPaymentCommand, bool>(command);
        }
    }
}
