using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mubbi.Marketplace.Register.Domain.Models;

namespace Mubbi.Marketplace.Data.Mappings
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.OwnsOne(x => x.Email);
            builder.OwnsOne(x => x.Address);
            builder.OwnsOne(x => x.Document);

            builder.ToTable("User");
        }
    }
}
