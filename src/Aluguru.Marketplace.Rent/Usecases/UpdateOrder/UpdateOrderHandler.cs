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

namespace Aluguru.Marketplace.Rent.Usecases.UpdateOrder
{
    public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand, UpdateOrderCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _mediatorHandler;

        public UpdateOrderHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediatorHandler mediatorHandler)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<UpdateOrderCommandResponse> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderQueryRepository = _unitOfWork.QueryRepository<Order>();

            var order = await orderQueryRepository.GetOrderAsync(request.OrderId, false);

            if (order == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(request.MessageType, $"Order not found"));
                return default;
            }

            var productQueryRepository = _unitOfWork.QueryRepository<Product>();

            var products = await productQueryRepository.GetProductsAsync(
                request.OrderItems.Select(x => x.ProductId).ToList()
            );

            List<DomainNotification> errors = new List<DomainNotification>();

            foreach(var orderItem in request.OrderItems)
            {
                var product = products.FirstOrDefault(x => x.Id == orderItem.ProductId);

                if (product == null)
                {
                    await _mediatorHandler.PublishNotification(new DomainNotification(request.MessageType, $"The product {product.Id} was not found in catalog."));
                    continue;
                }

                var notifications = ValidateProduct(request, orderItem, product);

                if (notifications.Count > 0) continue;

                decimal price = CalculateProductPrice(orderItem, product);

                var newOrderItem = new OrderItem(product.Id, product.Name, orderItem.RentStartDate, orderItem.Amount ?? 1, price);
                order.AddItem(newOrderItem);
            }

            if (errors.Count > 0)
            {
                foreach(var notification in errors)
                {
                    await _mediatorHandler.PublishNotification(notification);
                }
            }

            var orderRepository = _unitOfWork.Repository<Order>();

            order = orderRepository.Update(order);

            return new UpdateOrderCommandResponse()
            {
                Order = _mapper.Map<OrderDTO>(order)
            };
        }

        private decimal CalculateProductPrice(CreateOrderItemDTO orderItem, Product product)
        {
            decimal price = 0;

            switch (product.RentType)
            {
                case ERentType.Fixed:
                    price = orderItem.RentDays * product.Price.GetDailyRentPrice();
                    break;
                case ERentType.Indefinite:
                    price = product.Price.GetPeriodRentPrice(orderItem.SelectedRentPeriod.Value);
                    break;
            }

            return price;
        }

        private List<DomainNotification> ValidateProduct(UpdateOrderCommand request, CreateOrderItemDTO orderItem, Product product)
        {
            List<DomainNotification> notifications = new List<DomainNotification>();            

            if (product.CheckValidRentStartDate(orderItem.RentStartDate))
            {
                notifications.Add(new DomainNotification(request.MessageType, $"The product {product.Id} does not have a valid rent start date"));
            }

            switch(product.RentType)
            {
                case ERentType.Indefinite:
                    if (product.CheckValidRentDays(orderItem.RentDays))
                    {
                        notifications.Add(new DomainNotification(request.MessageType, $"The product {product.Id} have invalid rent days."));
                    }
                    break;
            }

            return notifications;
        }
    }
}