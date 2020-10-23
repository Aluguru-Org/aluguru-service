using Aluguru.Marketplace.Communication.IntegrationEvents;
using Aluguru.Marketplace.Crosscutting.Mailing;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Notification.Settings;
using Aluguru.Marketplace.Notification.Templates;
using MediatR;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Notification.Handlers
{
    public class UserRegisteredHandler : INotificationHandler<UserRegisteredEvent>
    {
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IMailingService _mailingService;
        private readonly NotificationSettings _settings;

        public UserRegisteredHandler(IMediatorHandler mediatorHandler, IMailingService malingService, IOptions<NotificationSettings> options)
        {
            _mediatorHandler = mediatorHandler;
            _mailingService = malingService;
            _settings = options.Value;
        }

        public async Task Handle(UserRegisteredEvent @event, CancellationToken cancellationToken)
        {            
            var activationLink = $"{_settings.ClientBackofficeDomain}/#/login/validacao?userId={@event.UserId}&activationHash={@event.ActivationHash}";
            var message = string.Format(EmailTemplates.RegisterUser, @event.UserName, activationLink);

            if (!await _mailingService.SendMessageHtml("Aluguru", "noreply@aluguru.com", @event.UserName, @event.Email, "Bem-vindo a Aluguru!", message))
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(@event.MessageType, $"Failed to send e-mail to { @event.Email }"));
            }
        }
    }
}
