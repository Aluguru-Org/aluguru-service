using Aluguru.Marketplace.Communication.IntegrationEvents;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Notification.Usecases.SendAccountActivationEmail;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Notification.Handlers
{
    public class UserRegisteredHandler : INotificationHandler<UserRegisteredEvent>
    {
        private readonly IMediatorHandler _mediatorHandler;

        public UserRegisteredHandler(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        public async Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
        {
            var command = new SendAccountActivationEmailCommand(notification.UserId, notification.UserName, notification.Email, notification.ActivationHash);
            await _mediatorHandler.SendCommand<SendAccountActivationEmailCommand, bool>(command);
        }
    }
}
