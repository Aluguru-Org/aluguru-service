using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mubbi.Marketplace.Register.Domain;

namespace Mubbi.Marketplace.Data.Mappings
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.OwnsOne(x => x.Document);

            builder.HasOne(x => x.UserRole)
                .WithOne(x => x.User)
                .HasForeignKey<UserRole>(x => x.UserId);

            builder.HasMany(x => x.Addresses)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);


            builder.ToTable("User");
        }
    }
}
