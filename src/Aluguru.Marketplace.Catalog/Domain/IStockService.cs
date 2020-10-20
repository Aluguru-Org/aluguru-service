using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Catalog.Domain
{
    public interface IStockService : IDisposable
    {
        Task<bool> DebitStock(Guid productId, int amount);
        Task<bool> ReplenishStock(Guid productId, int amount);
    }
}
