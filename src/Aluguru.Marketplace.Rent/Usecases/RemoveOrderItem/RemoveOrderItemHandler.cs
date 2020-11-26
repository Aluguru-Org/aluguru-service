using Aluguru.Marketplace.Communication.Dtos;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Rent.Data.Repositories;
using Aluguru.Marketplace.Rent.Domain;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Rent.Usecases.RemoveOrderItem
{
    public class RemoveOrderItemHandler : IRequestHandler<RemoveOrderItemCommand, RemoveOrderItemCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IMapper _mapper;

        public RemoveOrderItemHandler(IUnitOfWork unitOfWork, IMediatorHandler mediatorHandler, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mediatorHandler = mediatorHandler;
            _mapper = mapper;
        }

        public async Task<RemoveOrderItemCommandResponse> Handle(RemoveOrderItemCommand command, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<Order>();
            var orderRepository = _unitOfWork.Repository<Order>();

            var order = await queryRepository.GetOrderAsync(command.OrderId, false);

            if (order == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"The user order Id=[{command.OrderId}] was not found"));
                return default;
            }

            if (order.UserId != command.UserId)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"Order can only be edited by order owner"));
                return default;
            }

            order.RemoveItem(command.ProductId);

            order = orderRepository.Update(order);

            return new RemoveOrderItemCommandResponse()
            {
                Order = _mapper.Map<OrderDTO>(order)
            };
        }
    }
}
