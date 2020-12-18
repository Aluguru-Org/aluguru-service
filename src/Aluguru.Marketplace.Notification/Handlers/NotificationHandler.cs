using Aluguru.Marketplace.Communication.IntegrationEvents;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Notification.Usecases.SendAccountActivationEmail;
using Aluguru.Marketplace.Notification.Usecases.SendOrderPaymentConfirmedEmail;
using Aluguru.Marketplace.Notification.Usecases.SendOrderStartedEmail;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Notification.Handlers
{
    public class NotificationHandler : 
        INotificationHandler<UserRegisteredEvent>,
        INotificationHandler<OrderStockAcceptedEvent>,
        INotificationHandler<OrderPaidEvent>
    {
        private readonly IMediatorHandler _mediatorHandler;

        public NotificationHandler(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        public async Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
        {
            var command = new SendAccountActivationEmailCommand(notification.UserId, notification.UserName, notification.Email, notification.ActivationHash);
            await _mediatorHandler.SendCommand<SendAccountActivationEmailCommand, bool>(command);
        }

        public async Task Handle(OrderStockAcceptedEvent notification, CancellationToken cancellationToken)
        {
            var command = new SendOrderStartedEmailCommand(notification.Order.Id, notification.Order.UserName, notification.Order.UserEmail);
            await _mediatorHandler.SendCommand<SendOrderStartedEmailCommand, bool>(command);
        }

        public async Task Handle(OrderPaidEvent notification, CancellationToken cancellationToken)
        {
            var command = new SendPaymentConfirmedEmailCommand(notification.OrderId, notification.UserName, notification.UserEmail);
            await _mediatorHandler.SendCommand<SendPaymentConfirmedEmailCommand, bool>(command);
        }
    }
}
