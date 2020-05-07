using Microsoft.EntityFrameworkCore;
using Mubbi.Marketplace.Domain.Models;

namespace Mubbi.Marketplace.Infra.Data.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<User> Clients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().OwnsOne(x => x.Name);
            modelBuilder.Entity<User>().OwnsOne(x => x.Document);
            modelBuilder.Entity<User>().OwnsOne(x => x.Address);
        }
    }
}
