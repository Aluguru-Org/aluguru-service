using Microsoft.EntityFrameworkCore;
using Mubbi.Marketplace.Infrastructure.Bus.Communication;
using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using Mubbi.Marketplace.Infrastructure.Data;
using System.Linq;

namespace Mubbi.Marketplace.Data
{
    public class MubbiContext : AppDbContext
    {
        public MubbiContext(DbContextOptions<MubbiContext> options, IMediatorHandler mediatorHandler)
            : base(options, mediatorHandler) 
        {
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
    }
}
