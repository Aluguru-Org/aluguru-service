using Mubbi.Marketplace.Shared.Communication;
using Mubbi.Marketplace.Shared.Messages.Notifications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Catalog.Domain
{
    public class StockService : IStockService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMediatorHandler _mediatorHandler;
        
        public StockService(IProductRepository productRepository,
                            IMediatorHandler mediatorHandler)
        {
            _productRepository = productRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> DebitStock(Guid productId, int amount)
        {
            if (!await DebitItemStock(productId, amount)) return false;

            return await _productRepository.UnitOfWork.Commit();
        }

        public async Task<bool> ReplenishStock(Guid productId, int amount)
        {
            if (!await ReplenishItemStock(productId, amount)) return false;

            return await _productRepository.UnitOfWork.Commit();
        }

        private async Task<bool> DebitItemStock(Guid productId, int amount)
        {
            var product = await _productRepository.GetById(productId);

            if (product == null) return false;

            if (!product.HasStockFor(amount))
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("Stock", $"Product {product.Name} out of stock"));
                return false;
            }

            product.DebitStock(amount);

            _productRepository.UpdateProduct(product);

            return true;
        }

        private async Task<bool> ReplenishItemStock(Guid productId, int amount)
        {
            var product = await _productRepository.GetById(productId);

            if (product == null) return false;

            product.ReplenishStock(amount);

            _productRepository.UpdateProduct(product);

            return true;
        }

        public void Dispose()
        {
            _productRepository?.Dispose();
        }
    }
}
