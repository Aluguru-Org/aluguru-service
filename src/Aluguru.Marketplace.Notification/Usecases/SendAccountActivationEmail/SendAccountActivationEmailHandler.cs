using Aluguru.Marketplace.Crosscutting.Mailing;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Notification.Settings;
using Aluguru.Marketplace.Notification.Templates;
using Aluguru.Marketplace.Register.Domain;
using Aluguru.Marketplace.Register.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Notification.Usecases.SendAccountActivationEmail
{
    public class SendAccountActivationEmailHandler : IRequestHandler<SendAccountActivationEmailCommand, bool>
    {
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailingService _mailingService;
        private readonly NotificationSettings _settings;

        public SendAccountActivationEmailHandler(IMediatorHandler mediatorHandler, IMailingService malingService, IOptions<NotificationSettings> options, IUnitOfWork unitOfWork)
        {
            _mediatorHandler = mediatorHandler;
            _mailingService = malingService;
            _settings = options.Value;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(SendAccountActivationEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.QueryRepository<User>().GetUserAsync(request.UserId);

            if (user == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(request.MessageType, $"User Id [{request.UserId}] not found"));
                return false; 
            }

            if (user.IsActive)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(request.MessageType, $"User Id [{request.UserId}] is already active"));
                return false;
            }

            var activationLink = $"{_settings.ClientDomain}/ativacao?userId={request.UserId}&activationHash={user.ActivationHash}";
            var message = string.Format(EmailTemplates.RegisterUser, user.FullName, activationLink);

            if (!await _mailingService.SendMessageHtml(_settings.Sender, _settings.SenderEmail, user.FullName, user.Email, "Bem-vindo a Aluguru!", message))
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(request.MessageType, $"Failed to send e-mail to { user.Email }"));
                return false;
            }

            return true;
        }
    }
}
