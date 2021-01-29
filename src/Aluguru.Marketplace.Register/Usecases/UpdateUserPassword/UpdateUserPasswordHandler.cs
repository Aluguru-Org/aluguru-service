using MediatR;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Register.Domain;
using Aluguru.Marketplace.Register.Domain.Repositories;
using Aluguru.Marketplace.Security;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Register.Usecases.UpdateUserPassword
{
    public class UpdateUserPasswordHandler : IRequestHandler<UpdateUserPasswordCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly ICryptography _cryptography;

        public UpdateUserPasswordHandler(IUnitOfWork unitOfWork, IMediatorHandler mediatorHandler, ICryptography cryptography)
        {
            _unitOfWork = unitOfWork;
            _mediatorHandler = mediatorHandler;
            _cryptography = cryptography;
        }

        public async Task<bool> Handle(UpdateUserPasswordCommand command, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<User>();

            if (command.CurrentUserId != command.UserId)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, "You can only change your own password"));
                return false;
            }

            var user = await queryRepository.GetUserAsync(command.UserId);

            if (user == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"The User with Id [{command.UserId}] does not exist"));
                return false;
            }

            var encryptedNewPassword = _cryptography.Encrypt(command.Password);

            if (user.Password == encryptedNewPassword)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"The new password must be different from the previous one"));
                return false;
            }

            var repository = _unitOfWork.Repository<User>();

            user.UpdatePassword(encryptedNewPassword);

            repository.Update(user);

            return true;
        }
    }
}
