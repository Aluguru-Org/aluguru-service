using Microsoft.EntityFrameworkCore;
using Mubbi.Marketplace.Catalog.Domain;
using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Infrastructure.Data;
using PampaDevs.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Catalog.Data.Repositories
{
    public static class CategoryRepositoryExtensions
    {
        public static async Task<List<Category>> GetCategories(this IQueryRepository<Category> repository, bool disableTracking = true)
        {
            var queryable = repository.Queryable();

            if (disableTracking) queryable = queryable.AsNoTracking();

            var categories = await queryable
                .Where(x => !x.MainCategoryId.HasValue)
                    .Include(x => x.SubCategories)
                .ToListAsync();

            return categories;
        }

        public static async Task<Category> GetCategoryByCode(this IQueryRepository<Category> repository, int categoryCode, bool disableTracking = true)
        {
            var category = await repository.FindOneAsync(x => x.Code == categoryCode, 
                productQueryable => productQueryable.Include(x => x.Products),
                !disableTracking);

            return category;
        }
    }
}
