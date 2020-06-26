using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Mubbi.Marketplace.Register.Domain.Models;
using Mubbi.Marketplace.Shared.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Register.Data
{
    public class RegisterContext : IdentityDbContext, IUnitOfWork
    {
        public RegisterContext(DbContextOptions<RegisterContext> options)
            : base(options)
        {

        }

        public async Task<bool> Commit()
        {
            return await base.SaveChangesAsync() > 0;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RegisterContext).Assembly);
        }
    }
}
