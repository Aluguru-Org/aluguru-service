using Microsoft.EntityFrameworkCore;
using Mubbi.Marketplace.Catalog.Domain;
using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Infrastructure.Data;
using PampaDevs.Utils;
using System;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Catalog.Repositories
{
    public static class CategoryRepositoryExtensions
    {
        public static async Task<Category> GetCategoryByCode(this IQueryRepository<Category> repository, int categoryCode, bool disableTracking = true)
        {
            var category = await repository.FindOneAsync(x => x.Code == categoryCode, 
                productQueryable => productQueryable.Include(x => x.Products),
                !disableTracking);

            Ensure.NotNull(category, $"Could not find the category [{category}]");

            return category;
        }
    }
}
