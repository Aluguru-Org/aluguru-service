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

namespace Aluguru.Marketplace.Rent.Usecases.CreateOrder
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, CreateOrderCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _mediatorHandler;

        public CreateOrderHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediatorHandler mediatorHandler)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<CreateOrderCommandResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var productQueryRepository = _unitOfWork.QueryRepository<Product>();

            var productIds = request.OrderItems.Select(x => x.ProductId).ToList();
            var products = await productQueryRepository.GetProductsAsync(productIds);

            if (products.Count == 0)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(request.MessageType, $"No product was found"));
                return new CreateOrderCommandResponse();
            }

            var orderRepository = _unitOfWork.Repository<Order>();

            var order = new Order(request.UserId);

            List<DomainNotification> errors = new List<DomainNotification>();

            foreach (var orderItem in request.OrderItems)
            {
                var product = products.FirstOrDefault(x => x.Id == orderItem.ProductId);

                if (product == null)
                {
                    await _mediatorHandler.PublishNotification(new DomainNotification(request.MessageType, $"The product {product.Id} was not found in catalog."));
                    continue;
                }

                var notifications = RentUtils.ValidateProduct(request.MessageType, orderItem, product);
                errors.AddRange(notifications);
                if (notifications.Count > 0) continue;

                decimal price = RentUtils.CalculateProductPrice(orderItem, product);

                var newOrderItem = new OrderItem(product.Id, product.Name, orderItem.RentStartDate, orderItem.Amount ?? 1, price);
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