using AutoMapper;
using MediatR;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Rent.Data.Repositories;
using Aluguru.Marketplace.Rent.Domain;
using Aluguru.Marketplace.Rent.Dtos;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Rent.Usecases.UpdateOrderItemAmount
{
    public class UpdateOrderItemAmountHandler : IRequestHandler<UpdateOrderItemAmountCommand, UpdateOrderItemAmountCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _mediatorHandler;

        public UpdateOrderItemAmountHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediatorHandler mediatorHandler)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<UpdateOrderItemAmountCommandResponse> Handle(UpdateOrderItemAmountCommand command, CancellationToken cancellationToken)
        {
            var orderQueryRepository = _unitOfWork.QueryRepository<Order>();

            var order = await orderQueryRepository.GetOrderAsync(command.OrderId, false);

            if (order == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"Order not found"));
                return default;
            }

            if (order.UserId != command.UserId)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"Order can only be edited by order owner"));
                return default;
            }            
            
            order.UpdateItemAmount(command.OrderItemId, command.Amount);

            var orderRepository = _unitOfWork.Repository<Order>();

            order = orderRepository.Update(order);

            return new UpdateOrderItemAmountCommandResponse()
            {
                Order = _mapper.Map<OrderDTO>(order)
            };
        }
    }
}