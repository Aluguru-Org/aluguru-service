using Microsoft.EntityFrameworkCore;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Aluguru.Marketplace.Infrastructure.Data
{
    public abstract class AppDbContext : DbContext
    {
        private readonly IMediatorHandler _mediatorHandler = null;

        protected AppDbContext(DbContextOptions options, IMediatorHandler mediatorHandler)
            : base(options)
        {
            _mediatorHandler = mediatorHandler;
        }

        public override int SaveChanges()
        {
            var result = base.SaveChanges();

            if (result > 0)
            {
                PublishEntityEvents().Wait();
            }

            return result;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await base.SaveChangesAsync(cancellationToken);

            if (result > 0)
            {
                await PublishEntityEvents();
            }

            return result;
        }

        /// <summary>
        /// Source: https://github.com/ardalis/CleanArchitecture/blob/master/src/CleanArchitecture.Infrastructure/Data/AppDbContext.cs
        /// </summary>
        private async Task PublishEntityEvents()
        {
            var aggregators = ChangeTracker
                .Entries<AggregateRoot>()
                .Select(x => x.Entity)
                .Where(e => e.GetUncommittedEvents().Count > 0);

            var domainEvents = aggregators
                .SelectMany(x => x.GetUncommittedEvents())
                .ToList();

            aggregators.ToList().ForEach(x => x.ClearUncommittedEvents());

            var tasks = domainEvents
                .Select(async (domainEvent) =>
                {
                    await _mediatorHandler.PublishEvent(domainEvent);
                });
            
            await Task.WhenAll(tasks);
        }
    }
}
