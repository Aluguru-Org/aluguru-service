using Microsoft.EntityFrameworkCore;
using Aluguru.Marketplace.Domain;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Infrastructure.Data
{
    public class Repository<TEntity> : Repository<DbContext, TEntity>, IRepository<TEntity>
          where TEntity : class, IAggregateRoot
    {
        public Repository(DbContext dbContext) : base(dbContext)
        {

        }
    }

    public class Repository<TDbContext, TEntity> : IRepository<TEntity>
        where TDbContext : DbContext
        where TEntity : class, IAggregateRoot
    {
        private readonly TDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;
        public Repository(TDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public TEntity Add(TEntity entity)
        {
            _dbSet.Add(entity);
            return entity;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public int Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
            return 1;
        }

        public TEntity Update(TEntity entity)
        {
            var entry = _dbContext.Entry(entity);
            entry.State = EntityState.Modified;
            return entry.Entity;
        }
    }
}
