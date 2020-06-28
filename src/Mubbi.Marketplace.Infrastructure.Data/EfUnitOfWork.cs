using Microsoft.EntityFrameworkCore;
using Mubbi.Marketplace.Domain;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Infrastructure.Data
{
    public interface IEfUnitOfWork : IUnitOfWork { }

    public interface IEfUnitOfWork<TDbContext> : IUnitOfWork where TDbContext : DbContext { }

    public class EfUnitOfWork<TDbContext> : IEfUnitOfWork<TDbContext> where TDbContext : DbContext
    {
        private readonly TDbContext _context;
        private ConcurrentDictionary<string, object> _repositories;

        public EfUnitOfWork(TDbContext context)
        {
            _context = context;
        }

        public IQueryRepository<TEntity> QueryRepository<TEntity>() where TEntity : class, IAggregateRoot
        {
            if (_repositories == null)
                _repositories = new ConcurrentDictionary<string, object>();

            var key = $"{typeof(TEntity)}-query";

            if (!_repositories.ContainsKey(key))
            {
                var cachedRepo = new QueryRepository<TEntity>(_context);
                _repositories[key] = cachedRepo;
            }

            return (IQueryRepository<TEntity>)_repositories[key];
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class, IAggregateRoot
        {
            if (_repositories == null) _repositories = new ConcurrentDictionary<string, object>();

            var key = $"{typeof(TEntity)}-command";
            if (!_repositories.ContainsKey(key))
                _repositories[key] = new Repository<DbContext, TEntity>(_context);

            return (IRepository<TEntity>)_repositories[key];
        }

        public virtual int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
