using Microsoft.EntityFrameworkCore;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using Aluguru.Marketplace.Infrastructure.Data;
using System.Linq;

namespace Aluguru.Marketplace.Data
{
    public class AluguruContext : AppDbContext
    {
        public AluguruContext(DbContextOptions<AluguruContext> options, IMediatorHandler mediatorHandler)
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

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AluguruContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }      
    }
}
