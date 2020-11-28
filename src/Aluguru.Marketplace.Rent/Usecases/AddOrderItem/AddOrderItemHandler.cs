using AutoMapper;
using MediatR;
using Aluguru.Marketplace.Catalog.Data.Repositories;
using Aluguru.Marketplace.Catalog.Domain;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Rent.Data.Repositories;
using Aluguru.Marketplace.Rent.Domain;
using Aluguru.Marketplace.Rent.Dtos;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Aluguru.Marketplace.Rent.Utils;

namespace Aluguru.Marketplace.Rent.Usecases.AddOrderItem
{
    public class AddOrderItemHandler : IRequestHandler<AddOrderItemCommand, AddOrderItemCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _mediatorHandler;

        public AddOrderItemHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediatorHandler mediatorHandler)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<AddOrderItemCommandResponse> Handle(AddOrderItemCommand command, CancellationToken cancellationToken)
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

            var productQueryRepository = _unitOfWork.QueryRepository<Product>();

            var product = await productQueryRepository.GetProductAsync(command.OrderItem.ProductId);

            if (product == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"The product {product.Id} was not found in catalog."));
                return default;
            }

            var errors = RentUtils.ValidateProduct(command.MessageType, command.OrderItem, product);

            if (errors.Count > 0)
            {
                foreach (var notification in errors)
                {
                    await _mediatorHandler.PublishNotification(notification);
                }
                return default;
            }

            decimal price = RentUtils.CalculateProductPrice(command.OrderItem, product);

            var newOrderItem = new OrderItem(product.Id, product.Name, command.OrderItem.RentStartDate, command.OrderItem.Amount ?? 1, price);
            order.AddItem(newOrderItem);

            var orderRepository = _unitOfWork.Repository<Order>();

            order = orderRepository.Update(order);

            return new AddOrderItemCommandResponse()
            {
                Order = _mapper.Map<OrderDTO>(order)
            };
        }
    }
}