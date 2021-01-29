using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Register.Domain;
using Aluguru.Marketplace.Register.Domain.Repositories;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Register.Usecases.ActivateUser
{
    public class ActivateUserHandler : IRequestHandler<ActivateUserCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediatorHandler _mediatorHandler;

        public ActivateUserHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediatorHandler mediatorHandler)
        {
            _unitOfWork = unitOfWork;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Handle(ActivateUserCommand command, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<User>();

            var user = await queryRepository.GetUserAsync(command.UserId, false);

            if (user == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"The User with Id [{command.UserId}] does not exist"));
                return false;
            }

            var repository = _unitOfWork.Repository<User>();

            if (user.IsActive)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"The User with Id [{command.UserId}] is already active"));
                return false;
            }

            if (!user.Activate(command.ActivationHash))
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"The User with Id [{command.UserId}] activation failed due to incompatible activation hash"));
                return false;
            }

            repository.Update(user);

            return true;
        }
    }
}
