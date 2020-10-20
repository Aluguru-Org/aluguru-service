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
using Mubbi.Marketplace.Crosscutting.Mailing;
using Mubbi.Marketplace.Register.Templates;

namespace Mubbi.Marketplace.Register.Usecases.CreateUser
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, CreateUserCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IMailingService _mailingService;

        public CreateUserHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediatorHandler mediatorHandler, IMailingService mailingService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediatorHandler = mediatorHandler;
            _mailingService = mailingService;
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

            var user = new User(command.Email, Cryptography.Encrypt(command.Password), command.FullName, role.Id);

            var message = string.Format(EmailTemplates.RegisterUser, user.Email,"www.aluguru.com/login/validacao");
            if (!await _mailingService.SendMessageHtml("Aluguru", "noreply@aluguru.com", user.FullName, user.Email, "Bem vindo a Aluguru!", message))
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"Failed to send e-mail to {user.Email}"));
                return default;
            }            

            user = await userRepository.AddAsync(user);

            return new CreateUserCommandResponse()
            {
                User = _mapper.Map<UserViewModel>(user)
            };
        }
    }
}
