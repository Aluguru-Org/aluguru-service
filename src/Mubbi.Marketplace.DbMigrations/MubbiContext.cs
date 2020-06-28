using Microsoft.EntityFrameworkCore;
using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Infrastructure.Bus.Communication;
using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Data
{
    public class MubbiContext : DbContext
    {
        private readonly IMediatorHandler _mediatorHandler = null;

        public MubbiContext(DbContextOptions<MubbiContext> options, IMediatorHandler mediatorHandler)
            : base(options) 
        {
            _mediatorHandler = mediatorHandler;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<Event>();

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MubbiContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            var result = base.SaveChanges();

            if (result > 0)
            {
                PublishEntityEvents();
            }

            return result;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await base.SaveChangesAsync(cancellationToken);

            if (result > 0)
            {
                PublishEntityEvents();
            }

            return result;
        }

        /// <summary>
        /// Source: https://github.com/ardalis/CleanArchitecture/blob/master/src/CleanArchitecture.Infrastructure/Data/AppDbContext.cs
        /// </summary>
        private void PublishEntityEvents()
        {
            var aggregators = ChangeTracker
                .Entries<AggregateRoot>()
                .Select(x => x.Entity)
                .Where(e => e.GetUncommittedEvents().Count > 0);

            foreach (var aggregator in aggregators)
            {
                var @events = aggregator.GetUncommittedEvents();

                foreach (var @event in @events)
                {
                    _mediatorHandler.PublishEvent(@event);
                }

                aggregator.ClearUncommittedEvents();
            }
        }
    }
}
