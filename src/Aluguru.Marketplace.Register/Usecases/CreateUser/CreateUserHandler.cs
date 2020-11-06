using AutoMapper;
using MediatR;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Infrastructure.Data;
using Aluguru.Marketplace.Register.ViewModels;
using Aluguru.Marketplace.Register.Domain;
using Aluguru.Marketplace.Register.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;
using Aluguru.Marketplace.Security;
using Aluguru.Marketplace.Communication.IntegrationEvents;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;

namespace Aluguru.Marketplace.Register.Usecases.CreateUser
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, CreateUserCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly ICryptography _cryptography;

        public CreateUserHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediatorHandler mediatorHandler, ICryptography cryptography)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediatorHandler = mediatorHandler;
            _cryptography = cryptography;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<User>();

            if (await queryRepository.GetUserByEmailAsync(command.Email) != null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"The E-mail {command.Email} is already registered"));
                return default;
            }

            var roleQueryRepository = _unitOfWork.QueryRepository<UserRole>();
            var userRepository = _unitOfWork.Repository<User>();

            var role = await roleQueryRepository.FindOneAsync(x => x.Name == command.Role);

            if (role == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"The role {command.Role} does not exist"));
                return default;
            }

            var user = new User(command.Email, _cryptography.Encrypt(command.Password), command.FullName, role.Id, _cryptography.CreateRandomHash());

            await _mediatorHandler.PublishEvent(new UserRegisteredEvent(user.Id, user.FullName, user.Email, user.ActivationHash));

            user = await userRepository.AddAsync(user);

            return new CreateUserCommandResponse()
            {
                ActivationHash = user.ActivationHash,
                User = _mapper.Map<UserViewModel>(user)
            };
        }
    }
}
