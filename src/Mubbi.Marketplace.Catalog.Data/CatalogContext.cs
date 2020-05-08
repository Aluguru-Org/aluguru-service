using Microsoft.EntityFrameworkCore;
using Mubbi.Marketplace.Catalog.Domain.Models;
using Mubbi.Marketplace.Shared.Data;
using System;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Catalog.Data
{
    public class CatalogContext : DbContext, IUnitOfWork
    {
        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        public async Task<bool> Commit()
        {
            return await base.SaveChangesAsync() > 0;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogContext).Assembly);
        }
    }
}
