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
            var product = await repository.GetByIdAsync(
                productId,
                product => product.Include(x => x.CustomFields),
                !disableTracking);

            return product;
        }

        public static async Task<Product> GetProductByCategoryAsync(this IQueryRepository<Product> repository, Guid categoryId, bool disableTracking = true)
        {
            var product = await repository.FindOneAsync(x => x.CategoryId == categoryId);

            return product;
        }
    }
}
