using Microsoft.EntityFrameworkCore;
using Mubbi.Marketplace.Infrastructure.Bus.Communication;
using Mubbi.Marketplace.Infrastructure.Data;

namespace Mubbi.Marketplace.Catalog.Data
{
    public class CatalogContext : AppDbContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> options, IMediatorHandler mediatorHandler) : base(options, mediatorHandler) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogContext).Assembly);
        }
    }
}
