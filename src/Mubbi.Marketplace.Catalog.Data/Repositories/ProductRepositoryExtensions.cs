using Microsoft.EntityFrameworkCore;
using Mubbi.Marketplace.Catalog.Domain;
using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Infrastructure.Data;
using PampaDevs.Utils;
using System;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Catalog.Data.Repositories
{
    public static class ProductRepositoryExtensions
    {
        public static async Task<Product> GetProductAsync(this IQueryRepository<Product> repository, Guid productId, bool disableTracking = true)
        {
            var product = await repository.GetByIdAsync<CatalogContext, Product>(
                productId,
                productQueryable => productQueryable.Include(x => x.Category),
                !disableTracking);

            Ensure.NotNull(product, $"Could not find the product [{product.ToString()}]");

            return product;
        }
    }
}
