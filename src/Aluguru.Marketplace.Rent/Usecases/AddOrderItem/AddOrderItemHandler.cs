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
using Aluguru.Marketplace.Register.Domain;
using Aluguru.Marketplace.Crosscutting.Google;
using Aluguru.Marketplace.Register.Domain.Repositories;

namespace Aluguru.Marketplace.Rent.Usecases.AddOrderItem
{
    public class AddOrderItemHandler : IRequestHandler<AddOrderItemCommand, AddOrderItemCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IDistanceMatrixService _distanceMatrixService;

        public AddOrderItemHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediatorHandler mediatorHandler, IDistanceMatrixService distanceMatrixService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediatorHandler = mediatorHandler;
            _distanceMatrixService = distanceMatrixService;
        }

        public async Task<AddOrderItemCommandResponse> Handle(AddOrderItemCommand command, CancellationToken cancellationToken)
        {
            var userQueryRepository = _unitOfWork.QueryRepository<User>();
            var rentPeriodQueryRepository = _unitOfWork.QueryRepository<RentPeriod>();
            var orderQueryRepository = _unitOfWork.QueryRepository<Order>();

            var rentPeriods = await rentPeriodQueryRepository.GetRentPeriodsAsync();

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

            var user = await userQueryRepository.GetUserAsync(command.UserId);

            if (user == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"User not found"));
                return default;
            }

            if (user.Address == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"User not address found"));
                return default;
            }

            var productQueryRepository = _unitOfWork.QueryRepository<Product>();

            var product = await productQueryRepository.GetProductAsync(command.OrderItem.ProductId);

            if (product == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"The product {product.Id} was not found in catalog."));
                return default;
            }

            var owner = await userQueryRepository.GetUserAsync(product.UserId);

            if (owner == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"The product {product.Id} owner was not found in register."));
                return default; ;
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

            var distance = await _distanceMatrixService.Distance(owner.Address.ToString(), user.Address.ToString());

            var price = RentUtils.CalculateProductPrice(command.OrderItem, product);
            var freigthPrice = RentUtils.CalculateProductFreigthPrice(product, distance);
            var rentDays = RentUtils.GetRentDays(rentPeriods, command.OrderItem, product);


            var newOrderItem = new OrderItem(product.UserId, product.Id, product.Uri, product.Name, command.OrderItem.RentStartDate, rentDays, command.OrderItem.Amount ?? 1, price, freigthPrice);
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