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
                product => product.Include(x => x.CustomFields).Include(x => x.InvalidDates),
                disableTracking);

            return product;
        }

        public static async Task<Product> GetProductAsync(this IQueryRepository<Product> repository, string productUri, bool disableTracking = true)
        {
            var product = await repository.FindOneAsync(
                x => x.Uri.Trim() == productUri.Trim(),
                product => product.Include(x => x.CustomFields).Include(x => x.InvalidDates),
                disableTracking);

            return product;
        }

        public static async Task<IReadOnlyList<Product>> GetProductsAsync(this IQueryRepository<Product> repository, List<Guid> productIds, bool disableTracking = true)
        {
            var products = await repository.ListAsync(
                NewMethod(productIds),
                product => product.Include(x => x.CustomFields).Include(x => x.InvalidDates),
                disableTracking);

            return products;

            static System.Linq.Expressions.Expression<Func<Product, bool>> NewMethod(List<Guid> productIds)
            {
                return x => productIds.Contains(x.Id);
            }
        }

        public static async Task<Product> GetProductByCategoryAsync(this IQueryRepository<Product> repository, Guid categoryId, bool disableTracking = true)
        {
            var product = await repository.FindOneAsync(x => x.CategoryId == categoryId, null, disableTracking);

            return product;
        }
    }
}
