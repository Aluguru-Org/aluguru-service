using AutoMapper;
using MediatR;
using Aluguru.Marketplace.Catalog.Data.Repositories;
using Aluguru.Marketplace.Catalog.Domain;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Rent.Domain;
using Aluguru.Marketplace.Rent.Dtos;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Aluguru.Marketplace.Rent.Utils;
using Aluguru.Marketplace.Crosscutting.Google;
using Aluguru.Marketplace.Register.Domain;
using Aluguru.Marketplace.Register.Domain.Repositories;

namespace Aluguru.Marketplace.Rent.Usecases.CreateOrder
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, CreateOrderCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IDistanceMatrixService _distanceMatrixService;

        public CreateOrderHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediatorHandler mediatorHandler, IDistanceMatrixService distanceMatrixService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediatorHandler = mediatorHandler;
            _distanceMatrixService = distanceMatrixService;
        }

        public async Task<CreateOrderCommandResponse> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            var userQueryRepository = _unitOfWork.QueryRepository<User>();
            var rentPeriodQueryRepository = _unitOfWork.QueryRepository<RentPeriod>();
            var productQueryRepository = _unitOfWork.QueryRepository<Product>();

            var rentPeriods = await rentPeriodQueryRepository.GetRentPeriodsAsync();

            var productIds = command.OrderItems.Select(x => x.ProductId).ToList();
            var products = await productQueryRepository.GetProductsAsync(productIds);

            if (products.Count == 0)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"No product was found"));
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
                await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"User address not found"));
                return default;
            }

            var orderRepository = _unitOfWork.Repository<Order>();

            var order = new Order(command.UserId);

            List<DomainNotification> errors = new List<DomainNotification>();

            foreach (var orderItem in command.OrderItems)
            {
                var product = products.FirstOrDefault(x => x.Id == orderItem.ProductId);                

                if (product == null)
                {
                    await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"The product {product.Id} was not found in catalog."));
                    continue;
                }

                var owner = await userQueryRepository.GetUserAsync(product.UserId);

                if (owner == null)
                {
                    await _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, $"The product {product.Id} owner was not found in register."));
                    continue;
                }     

                var notifications = RentUtils.ValidateProduct(command.MessageType, orderItem, product);
                errors.AddRange(notifications);
                if (notifications.Count > 0) continue;

                var distance = await _distanceMatrixService.Distance(owner.Address.ToString(), user.Address.ToString());

                var price = RentUtils.CalculateProductPrice(orderItem, product);
                var freigthPrice = RentUtils.CalculateProductFreigthPrice(product, distance);
                var rentDays = RentUtils.GetRentDays(rentPeriods, orderItem, product);


                var newOrderItem = new OrderItem(product.UserId, product.Id, product.Uri, product.Name, orderItem.RentStartDate, rentDays, orderItem.Amount ?? 1, price, freigthPrice);
                order.AddItem(newOrderItem);
            }

            if (errors.Count > 0)
            {
                foreach (var notification in errors)
                {
                    await _mediatorHandler.PublishNotification(notification);
                }
                return default;
            }

            order = await orderRepository.AddAsync(order);

            return new CreateOrderCommandResponse()
            {
                Order = _mapper.Map<OrderDTO>(order)
            };
        }
    }
}