using Mubbi.Marketplace.Catalog.Repositories;
using Mubbi.Marketplace.Infrastructure.Bus.Communication;
using Mubbi.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Mubbi.Marketplace.Infrastructure.Data;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Catalog.Domain
{
    public class StockService : IStockService
    {
        private readonly IEfUnitOfWork _unitOfWork;
        private readonly IMediatorHandler _mediatorHandler;
        
        public StockService(IMediatorHandler mediatorHandler, IEfUnitOfWork unitOfWork)
        {
            _mediatorHandler = mediatorHandler;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> DebitStock(Guid productId, int amount)
        {
            if (!await DebitItemStock(productId, amount)) return false;

            return await _unitOfWork.SaveChangesAsync(new CancellationToken()) == 1;
        }

        public async Task<bool> ReplenishStock(Guid productId, int amount)
        {
            if (!await ReplenishItemStock(productId, amount)) return false;
            
            return await _unitOfWork.SaveChangesAsync(new CancellationToken()) == 1;
        }

        private async Task<bool> DebitItemStock(Guid productId, int amount)
        {
            var product = await _unitOfWork.QueryRepository<Product>().GetProductAsync(productId);

            if (product == null) return false;

            if (!product.HasStockFor(amount))
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("Stock", $"Product {product.Name} out of stock"));
                return false;
            }

            product.DebitStock(amount);

            _unitOfWork.Repository<Product>().Update(product);

            return true;
        }

        private async Task<bool> ReplenishItemStock(Guid productId, int amount)
        {
            var product = await _unitOfWork.QueryRepository<Product>().GetProductAsync(productId);

            if (product == null) return false;

            product.ReplenishStock(amount);

            _unitOfWork.Repository<Product>().Update(product);

            return true;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
