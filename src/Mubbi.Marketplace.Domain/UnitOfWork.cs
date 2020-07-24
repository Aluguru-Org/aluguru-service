using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Domain
{
    public interface IUnitOfWork : IRepositoryFactory, IDisposable
    {
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }

    public interface IRepositoryFactory
    {
        IQueryRepository<TEntity> QueryRepository<TEntity>() where TEntity : class, IAggregateRoot;
        IRepository<TEntity> Repository<TEntity>() where TEntity : class, IAggregateRoot;
    }

    public interface IRepository<TEntity> where TEntity : IAggregateRoot
    {
        TEntity Add(TEntity entity);
        TEntity Update(TEntity entity);
        int Delete(TEntity entity);
        Task<TEntity> AddAsync(TEntity entity);
    }

    public interface IQueryRepository<TEntity> where TEntity : IAggregateRoot
    {
        IQueryable<TEntity> Queryable();
    }
}
