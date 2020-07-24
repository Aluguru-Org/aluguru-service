using AutoMapper;
using MediatR;
using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Infrastructure.Bus.Communication;
using Mubbi.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Mubbi.Marketplace.Infrastructure.Data;
using Mubbi.Marketplace.Register.ViewModels;
using Mubbi.Marketplace.Register.Domain;
using Mubbi.Marketplace.Register.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;
using Mubbi.Marketplace.Security;

namespace Mubbi.Marketplace.Register.Usecases.CreateUser
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, CreateUserCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _mediatorHandler;

        public CreateUserHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediatorHandler mediatorHandler)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<User>();

            if (await queryRepository.GetUserByEmailAsync(command.Email) != null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"The E-mail {command.Email} is already registered"));
                return new CreateUserCommandResponse();
            }

            var roleQueryRepository = _unitOfWork.QueryRepository<UserRole>();
            var userRepository = _unitOfWork.Repository<User>();

            var role = await roleQueryRepository.FindOneAsync(x => x.Name == command.Role);

            if (role == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"The role {command.Role} does not exist"));
                return new CreateUserCommandResponse();
            }

            var user = new User(command.Email, Cryptography.Encrypt(command.Password), command.FullName, role.Id);

            user = await userRepository.AddAsync(user);

            return new CreateUserCommandResponse()
            {
                User = _mapper.Map<UserViewModel>(user)
            };
        }
    }
}
