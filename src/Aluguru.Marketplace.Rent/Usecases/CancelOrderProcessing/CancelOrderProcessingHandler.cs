using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Rent.Data.Repositories;
using Aluguru.Marketplace.Rent.Domain;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Rent.Usecases.CancelOrderProcessing
{
    public class CancelOrderProcessingHandler : IRequestHandler<CancelOrderProcessingCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediatorHandler _mediatorHandler;

        public CancelOrderProcessingHandler(IUnitOfWork unitOfWork, IMediatorHandler mediatorHandler)
        {
            _unitOfWork = unitOfWork;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Handle(CancelOrderProcessingCommand request, CancellationToken cancellationToken)
        {
            var orderQueryRepository = _unitOfWork.QueryRepository<Order>();
            var orderRepository = _unitOfWork.Repository<Order>();

            var order = await orderQueryRepository.GetOrderAsync(request.OrderId, false);

            if (order == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(request.MessageType, $"The order [{request.OrderId}] was not found"));
                return false;
            }

            order.CancelInitiation();

            orderRepository.Update(order);

            return true;
        }
    }
}
