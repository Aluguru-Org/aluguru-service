using MediatR;
using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Infrastructure.Bus.Communication;
using Mubbi.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Mubbi.Marketplace.Infrastructure.Data;
using Mubbi.Marketplace.Register.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Register.Usecases.DeleteUserRole
{
    public class DeleteUserRoleHandler : IRequestHandler<DeleteUserRoleCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediatorHandler _mediatorHandler;

        public DeleteUserRoleHandler(IUnitOfWork unitOfWork, IMediatorHandler mediatorHandler)
        {
            _unitOfWork = unitOfWork;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Handle(DeleteUserRoleCommand request, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<UserRole>();

            var userRole = await queryRepository.GetByIdAsync(request.UserRoleId);

            if (userRole == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(request.MessageType, $"The role with Id=[{request.UserRoleId}] was not found"));
                return false;
            }

            var userRepository = _unitOfWork.Repository<UserRole>();

            userRepository.Delete(userRole);

            return true;
        }
    }
}
