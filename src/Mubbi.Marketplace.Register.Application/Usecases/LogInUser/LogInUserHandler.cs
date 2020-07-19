using AutoMapper;
using MediatR;
using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Infrastructure.Bus.Communication;
using Mubbi.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Mubbi.Marketplace.Register.Domain;
using Mubbi.Marketplace.Register.Domain.Repositories;
using Mubbi.Marketplace.Register.Services;
using System.Threading;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Register.Usecases.LogInUser
{
    public class LogInUserHandler : IRequestHandler<LogInUserCommand, LogInUserCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly ITokenBuilderService _tokenBuilderService;

        public LogInUserHandler(IUnitOfWork unitOfWork, IMediatorHandler mediatorHandler, ITokenBuilderService tokenBuilderService)
        {
            _unitOfWork = unitOfWork;
            _mediatorHandler = mediatorHandler;
            _tokenBuilderService = tokenBuilderService;
        }

        public async Task<LogInUserCommandResponse> Handle(LogInUserCommand command, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<User>();

            var user = await queryRepository.GetUserByEmailAsync(command.Email);

            if (user == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"The E-mail {command.Email} does not exist"));
                return new LogInUserCommandResponse();
            }

            if (user.Password != command.Password)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"Wrong password"));
                return new LogInUserCommandResponse();
            }

            var token = _tokenBuilderService.BuildToken(user, options =>
            {
                options.WithUserClaims();
                options.WithJwtClaims();
            });

            return new LogInUserCommandResponse() { Token = token };
        }
    }
}