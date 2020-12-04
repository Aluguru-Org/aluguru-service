using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Aluguru.Marketplace.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;

namespace Aluguru.Marketplace.Infrastructure.Data
{
    public static class QueryRepositoryExtensions
    {
        public static async Task<TEntity> GetByIdAsync<TEntity>(
          this IQueryRepository<TEntity> repo,
          Guid id,
          Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
          bool disableTracking = true)
          where TEntity : class, IAggregateRoot
        {
            var queryable = repo.Queryable();

            if (disableTracking) queryable = queryable.AsNoTracking();

            if (include != null) queryable = include.Invoke(queryable);

            return await queryable.SingleOrDefaultAsync(e => e.Id.Equals(id));
        }

        public static async Task<TEntity> FindOneAsync<TEntity>(
            this IQueryRepository<TEntity> repo,
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true)
            where TEntity : class, IAggregateRoot
        {
            var queryable = repo.Queryable();

            if (include != null) queryable = include.Invoke(queryable);

            if (disableTracking) queryable = queryable.AsNoTracking();

            return await queryable.FirstOrDefaultAsync(filter);
        }

        public static async Task<IReadOnlyList<TEntity>> ListAsync<TEntity>(
            this IQueryRepository<TEntity> repo,
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true)
            where TEntity : class, IAggregateRoot
        {
            var queryable = repo.Queryable();

            if (filter != null) queryable = queryable.Where(filter);
            if (include != null) queryable = include.Invoke(queryable);

            if (disableTracking) queryable = queryable.AsNoTracking();

            return await queryable.ToListAsync();
        }

        public static async Task<PaginatedItem<TResponse>> QueryAsync<TEntity, TResponse>(
            this IQueryRepository<TEntity> repo,
            IMapper mapper,
            PaginateCriteria paginateCriteria,
            Expression<Func<TEntity, TEntity>> selector,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true)
            where TEntity : class, IAggregateRoot
            where TResponse : IDto
        {
            return await GetDataAsync<TEntity, TResponse>(repo, mapper, paginateCriteria, selector, null, include, disableTracking);
        }

        public static async Task<PaginatedItem<TResponse>> FindAllAsync<TEntity, TResponse>(
            this IQueryRepository<TEntity> repo,
            IMapper mapper,
            PaginateCriteria paginateCriteria,
            Expression<Func<TEntity, TEntity>> selector,
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true)
            where TEntity : class, IAggregateRoot
            where TResponse : IDto
        {
            return await GetDataAsync<TEntity, TResponse>(repo, mapper, paginateCriteria, selector, filter, include, disableTracking);
        }

        private static async Task<PaginatedItem<TResponse>> GetDataAsync<TEntity, TResponse>(
            IQueryRepository<TEntity> repo,
            IMapper mapper,
            PaginateCriteria paginateCriteria,
            Expression<Func<TEntity, TEntity>> selector,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true)
            where TEntity : class, IAggregateRoot
            where TResponse : IDto
        {
            var queryable = repo.Queryable();

            if (disableTracking) queryable = queryable.AsNoTracking();

            if (include != null) queryable = include.Invoke(queryable);

            if (filter != null) queryable = queryable.Where(filter);

            if (!string.IsNullOrWhiteSpace(paginateCriteria.SortBy))
            {
                var isDesc = string.Equals(paginateCriteria.SortOrder, "desc", StringComparison.OrdinalIgnoreCase) ? true : false;
                queryable = queryable.OrderByPropertyName(paginateCriteria.SortBy, isDesc);
            }

            var results = await queryable
                .Skip(paginateCriteria.CurrentPage * paginateCriteria.PageSize)
                .Take(paginateCriteria.PageSize)
                .Select(selector)
                .ToListAsync();

            var dtos = mapper.Map<List<TResponse>>(results);

            var totalRecord = await queryable.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalRecord / paginateCriteria.PageSize);

            if (paginateCriteria.CurrentPage > totalPages)
            {
                paginateCriteria.SetCurrentPage(totalPages);
            }

            return new PaginatedItem<TResponse>(totalRecord, totalPages, dtos);
        }
    }
}
