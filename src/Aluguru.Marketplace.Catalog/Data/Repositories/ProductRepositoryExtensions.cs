using Microsoft.EntityFrameworkCore;
using Aluguru.Marketplace.Catalog.Domain;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Catalog.Data.Repositories
{
    public static class ProductRepositoryExtensions
    {
        public static async Task<Product> GetProductAsync(this IQueryRepository<Product> repository, Guid productId, bool disableTracking = true)
        {
            var product = await repository.GetByIdAsync(
                productId,
                product => product.Include(x => x.CustomFields),
                disableTracking);

            return product;
        }

        public static async Task<IReadOnlyList<Product>> GetProductsAsync(this IQueryRepository<Product> repository, List<Guid> productIds, bool disableTracking = true)
        {
            var products = await repository.ListAsync(
                x => productIds.Contains(x.Id),
                product => product.Include(x => x.CustomFields),
                disableTracking);

            return products;
        }

        public static async Task<Product> GetProductByCategoryAsync(this IQueryRepository<Product> repository, Guid categoryId, bool disableTracking = true)
        {
            var product = await repository.FindOneAsync(x => x.CategoryId == categoryId, null, disableTracking);

            return product;
        }
    }
}
