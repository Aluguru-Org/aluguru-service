using Microsoft.EntityFrameworkCore;
using Mubbi.Marketplace.Domain;
using System.Linq;

namespace Mubbi.Marketplace.Infrastructure.Data
{
    public class QueryRepository<TEntity> : QueryRepository<DbContext, TEntity>
              where TEntity : class, IAggregateRoot
    {
        public QueryRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }

    public class QueryRepository<TDbContext, TEntity> : IQueryRepository<TEntity>
        where TDbContext : DbContext
        where TEntity : class, IAggregateRoot
    {
        private readonly TDbContext _dbContext;

        public QueryRepository(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<TEntity> Queryable()
        {
            return _dbContext.Set<TEntity>();
        }
    }
}
