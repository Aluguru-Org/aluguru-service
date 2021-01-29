using MediatR;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Infrastructure.Data;
using Aluguru.Marketplace.Rent.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Rent.Usecases.DeleteOrder
{
    public class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediatorHandler _mediatorHandler;

        public DeleteOrderHandler(IUnitOfWork unitOfWork, IMediatorHandler mediatorHandler)
        {
            _unitOfWork = unitOfWork;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<Order>();

            var order = await queryRepository.GetByIdAsync(request.OrderId);
            
            if (order == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(request.MessageType, $"The user order Id=[{request.OrderId}] was not found"));
                return false;
            }

            var userRepository = _unitOfWork.Repository<Order>();

            userRepository.Delete(order);

            return true;
        }
    }
}
