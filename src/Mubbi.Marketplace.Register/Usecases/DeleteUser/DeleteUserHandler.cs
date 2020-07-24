using MediatR;
using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Infrastructure.Bus.Communication;
using Mubbi.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Mubbi.Marketplace.Infrastructure.Data;
using Mubbi.Marketplace.Register.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Register.Usecases.DeleteUser
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediatorHandler _mediatorHandler;

        public DeleteUserHandler(IUnitOfWork unitOfWork, IMediatorHandler mediatorHandler)
        {
            _unitOfWork = unitOfWork;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<User>();

            var user = await queryRepository.GetByIdAsync(request.UserId);
            
            if (user == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(request.MessageType, $"The user with Id=[{request.UserId}] was not found"));
                return false;
            }

            var userRepository = _unitOfWork.Repository<User>();

            userRepository.Delete(user);

            return true;
        }
    }
}
