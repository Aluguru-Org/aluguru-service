using AutoMapper;
using MediatR;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Register.Domain;
using Aluguru.Marketplace.Register.Domain.Repositories;
using Aluguru.Marketplace.Register.Services;
using System.Threading;
using System.Threading.Tasks;
using Aluguru.Marketplace.Security;

namespace Aluguru.Marketplace.Register.Usecases.LogInClient
{
    public class LogInUserClientHandler : IRequestHandler<LogInUserClientCommand, LogInUserClientCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly ITokenBuilderService _tokenBuilderService;
        private readonly ICryptography _cryptography;

        public LogInUserClientHandler(IUnitOfWork unitOfWork, IMediatorHandler mediatorHandler, ITokenBuilderService tokenBuilderService, ICryptography cryptography)
        {
            _unitOfWork = unitOfWork;
            _mediatorHandler = mediatorHandler;
            _tokenBuilderService = tokenBuilderService;
            _cryptography = cryptography;
        }

        public async Task<LogInUserClientCommandResponse> Handle(LogInUserClientCommand command, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<User>();

            var encryptedPassword = _cryptography.Encrypt(command.Password);

            var user = await queryRepository.GetUserByEmailAsync(command.Email);

            if (user == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"The E-mail {command.Email} does not exist"));
                return new LogInUserClientCommandResponse();
            }

            if (user.Password != encryptedPassword)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"Wrong password"));
                return new LogInUserClientCommandResponse();
            }

            if (!user.IsActive)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"The E-mail '{command.Email}' is not active."));
                return new LogInUserClientCommandResponse();
            }

            var token = _tokenBuilderService.BuildClientToken(user, options =>
            {
                options.WithUserClaims();
                options.WithJwtClaims();
            });

            return new LogInUserClientCommandResponse() { UserId = user.Id, Token = token };
        }
    }
}