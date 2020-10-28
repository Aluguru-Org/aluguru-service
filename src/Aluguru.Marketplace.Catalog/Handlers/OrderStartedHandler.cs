using Aluguru.Marketplace.Catalog.Data.Repositories;
using Aluguru.Marketplace.Catalog.Domain;
using Aluguru.Marketplace.Communication.Dtos;
using Aluguru.Marketplace.Communication.IntegrationEvents;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Catalog.Handlers
{
    public class OrderStartedHandler : INotificationHandler<OrderStartedEvent>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediatorHandler _mediatorHandler;
        public OrderStartedHandler(IUnitOfWork unitOfWork, IMediatorHandler mediatorHandler)
        {
            _unitOfWork = unitOfWork;
            _mediatorHandler = mediatorHandler;
        }

        public async Task Handle(OrderStartedEvent notification, CancellationToken cancellationToken)
        {
            var productQueryRepository = _unitOfWork.QueryRepository<Product>();

            var productIds = notification.Order.OrderItems.Select(x => x.ProductId).ToList();

            var products = await productQueryRepository.GetProductsAsync(productIds, false);

            List<OrderItemDTO> noStockProducts = new List<OrderItemDTO>();

            foreach(var item in notification.Order.OrderItems)
            {
                var product = products.FirstOrDefault(x => x.Id == item.ProductId);

                if (product == null) continue;

                if (product.HasStockFor(item.Amount))
                {
                    product.DebitStock(item.Amount);
                }
                else
                {
                    await _mediatorHandler.PublishNotification(new DomainNotification("Stock", $"Product - {product.Name} is out of stock"));
                    noStockProducts.Add(item);
                }
            }

            if (noStockProducts.Count == 0)
            {
                await _mediatorHandler.PublishEvent(new OrderStockConfirmedEvent(notification.Order));
            }
            else
            {
                await _mediatorHandler.PublishEvent(new OrderStockRejectedEvent(noStockProducts));
            }
        }
    }
}
