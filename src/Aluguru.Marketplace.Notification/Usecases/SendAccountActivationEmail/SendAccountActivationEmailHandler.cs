using Aluguru.Marketplace.Crosscutting.Mailing;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Notification.Settings;
using Aluguru.Marketplace.Notification.Templates;
using MediatR;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Notification.Usecases.SendAccountActivationEmail
{
    public class SendAccountActivationEmailHandler : IRequestHandler<SendAccountActivationEmailCommand, bool>
    {
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IMailingService _mailingService;
        private readonly NotificationSettings _settings;

        public SendAccountActivationEmailHandler(IMediatorHandler mediatorHandler, IMailingService malingService, IOptions<NotificationSettings> options)
        {
            _mediatorHandler = mediatorHandler;
            _mailingService = malingService;
            _settings = options.Value;
        }

        public async Task<bool> Handle(SendAccountActivationEmailCommand request, CancellationToken cancellationToken)
        {
            var activationLink = $"{_settings.ClientDomain}/ativacao?userId={request.UserId}&activationHash={request.ActivationHash}";
            var message = string.Format(EmailTemplates.RegisterUser, request.UserName, activationLink);

            if (!await _mailingService.SendMessageHtml(_settings.Sender, _settings.SenderEmail, request.UserName, request.Email, "Bem-vindo a Aluguru!", message))
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(request.MessageType, $"Failed to send e-mail to { request.Email }"));
                return false;
            }

            return true;
        }
    }
}
