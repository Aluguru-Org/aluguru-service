using Aluguru.Marketplace.Crosscutting.Mailing;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Notification.Settings;
using Aluguru.Marketplace.Notification.Templates;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Notification.Usecases.SendOrderPaymentConfirmedEmail
{
    public class SendPaymentConfirmedEmailHandler : IRequestHandler<SendPaymentConfirmedEmailCommand, bool>
    {
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IMailingService _mailingService;
        private readonly NotificationSettings _settings;

        public SendPaymentConfirmedEmailHandler(IMediatorHandler mediatorHandler, IMailingService malingService, IOptions<NotificationSettings> options)
        {
            _mediatorHandler = mediatorHandler;
            _mailingService = malingService;
            _settings = options.Value;
        }


        public async Task<bool> Handle(SendPaymentConfirmedEmailCommand command, CancellationToken cancellationToken)
        {
            var message = string.Format(EmailTemplates.PaymentConfirmed, command.UserName, command.OrderId);

            if (!await _mailingService.SendMessageHtml(_settings.Sender, _settings.SenderEmail, command.UserName, command.UserEmail, "Obrigado por escolher a gente", message))
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"Failed to send e-mail to { command.UserEmail }"));
                return false;
            }

            return true;
        }
    }
}
