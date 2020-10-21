using Aluguru.Marketplace.Communication.IntegrationEvents;
using Aluguru.Marketplace.Crosscutting.Mailing;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Notification.Templates;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Notification.Handlers
{
    public class UserRegisteredHandler : INotificationHandler<UserRegisteredEvent>
    {
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IMailingService _mailingService;

        public UserRegisteredHandler(IMediatorHandler mediatorHandler, IMailingService malingService)
        {
            _mediatorHandler = mediatorHandler;
            _mailingService = malingService;
        }

        public async Task Handle(UserRegisteredEvent @event, CancellationToken cancellationToken)
        {
            var message = string.Format(EmailTemplates.RegisterUser, @event.Email, $"www.aluguru.com/login/validacao?activationHash={@event.ActivationHash}");

            if (!await _mailingService.SendMessageHtml("Aluguru", "noreply@aluguru.com", @event.UserName, @event.Email, "Bem vindo a Aluguru!", message))
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(@event.MessageType, $"Failed to send e-mail to { @event.Email }"));
            }
        }
    }
}
