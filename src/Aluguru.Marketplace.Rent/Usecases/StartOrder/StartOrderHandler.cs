using Aluguru.Marketplace.Communication.IntegrationEvents;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Rent.Data.Repositories;
using Aluguru.Marketplace.Rent.Domain;
using Aluguru.Marketplace.Rent.ViewModels;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Rent.Usecases.StartOrder
{
    public class StartOrderHandler : IRequestHandler<StartOrderCommand, StartOrderCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _mediatorHandler;

        public StartOrderHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediatorHandler mediatorHandler)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<StartOrderCommandResponse> Handle(StartOrderCommand command, CancellationToken cancellationToken)
        {
            var orderQueryRepository = _unitOfWork.QueryRepository<Order>();
            var orderRepository = _unitOfWork.Repository<Order>();

            var order = await orderQueryRepository.GetOrderAsync(command.OrderId, false);

            if (order == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"Order {command.OrderId} does not exist"));
            }

            order.Initiate();

            var dto = new Communication.Dtos.OrderDTO(
                order.Id, 
                order.UserId, 
                order.TotalPrice, 
                new List<Communication.Dtos.OrderItemDTO>(
                    order.OrderItems.Select(x => new Communication.Dtos.OrderItemDTO(
                        x.ProductId, 
                        x.ProductName, 
                        x.Amount, 
                        x.ProductPrice))
                    )
            );

            order.AddEvent(new OrderStartedEvent(dto));

            order = orderRepository.Update(order);

            return new StartOrderCommandResponse
            {
                Order = _mapper.Map<OrderViewModel>(order)
            };
        }
    }
}