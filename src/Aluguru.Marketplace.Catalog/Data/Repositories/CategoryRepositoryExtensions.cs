using Microsoft.EntityFrameworkCore;
using Aluguru.Marketplace.Catalog.Domain;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Catalog.Data.Repositories
{
    public static class CategoryRepositoryExtensions
    {
        public static async Task<Category> GetCategoryAsync(this IQueryRepository<Category> repository, Guid categoryId, bool disableTracking = true)
        {
            var category = await repository.GetByIdAsync(
                categoryId,
                null,
                disableTracking);

            return category;
        }

        public static async Task<Category> GetCategoryByNameAsync(this IQueryRepository<Category> repository, string name, bool disableTracking = true)
        {
            var category = await repository.FindOneAsync(
                x => x.Name == name,
                null,
                disableTracking);

            return category;
        }

        public static async Task<Category> GetCategoryByUriAsync(this IQueryRepository<Category> repository, string uri, bool disableTracking = true)
        {
            var category = await repository.FindOneAsync(
                x => x.Uri == uri,
                null,
                disableTracking);

            return category;
        }

        public static async Task<List<Category>> GetCategoriesAsync(this IQueryRepository<Category> repository, bool disableTracking = true)
        {
            var queryable = repository.Queryable();

            if (disableTracking) queryable = queryable.AsNoTracking();

            var categories = await queryable
                .Where(x => !x.MainCategoryId.HasValue)
                    .Include(x => x.SubCategories)
                .ToListAsync();

            return categories;
        }
    }
}
