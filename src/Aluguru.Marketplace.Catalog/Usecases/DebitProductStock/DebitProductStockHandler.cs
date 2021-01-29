using Aluguru.Marketplace.Catalog.Data.Repositories;
using Aluguru.Marketplace.Catalog.Domain;
using Aluguru.Marketplace.Communication.Dtos;
using Aluguru.Marketplace.Communication.IntegrationEvents;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Catalog.Usecases.DebitProductStock
{
    public class DebitProductStockHandler : IRequestHandler<DebitProductStockCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediatorHandler _mediatorHandler;
        public DebitProductStockHandler(IUnitOfWork unitOfWork, IMediatorHandler mediatorHandler)
        {
            _unitOfWork = unitOfWork;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Handle(DebitProductStockCommand command, CancellationToken cancellationToken)
        {
            var productQueryRepository = _unitOfWork.QueryRepository<Product>();
            var productRepository = _unitOfWork.Repository<Product>();

            var productIds = command.Order.OrderItems.Select(x => x.ProductId).ToList();

            var products = await productQueryRepository.GetProductsAsync(productIds, false);

            List<OrderItemDTO> debitRejectedProducts = new List<OrderItemDTO>();

            foreach (var item in command.Order.OrderItems)
            {
                var product = products.FirstOrDefault(x => x.Id == item.ProductId);

                if (product == null)
                {
                    await _mediatorHandler.PublishNotification(new DomainNotification("Catalog", $"Product - {product.Name} does not exist"));
                    continue;
                }
                
                if (!product.HasDateAvaiabilityFor(item.RentStartDate))
                {
                    await _mediatorHandler.PublishNotification(new DomainNotification("Catalog", $"Product - {product.Name} cannot be rented on a blocked date ´[{item.RentStartDate.Date}]"));
                    debitRejectedProducts.Add(item);
                    continue;
                }

                if (product.HasStockFor(item.Amount))
                {
                    product.DebitStock(item.Amount);
                }
                else
                {
                    await _mediatorHandler.PublishNotification(new DomainNotification("Catalog", $"Product - {product.Name} is out of stock"));
                    debitRejectedProducts.Add(item);
                }
            }

            if (debitRejectedProducts.Count == 0)
            {
                for (int i = 0; i < products.Count; i++)
                {
                    productRepository.Update(products[i]);
                }

                await _mediatorHandler.PublishEvent(new OrderStockAcceptedEvent(command.Order));
                return true;
            }
            else
            {
                await _mediatorHandler.PublishEvent(new OrderStockRejectedEvent(command.Order.Id, debitRejectedProducts));
                return false;
            }
        }
    }
}
