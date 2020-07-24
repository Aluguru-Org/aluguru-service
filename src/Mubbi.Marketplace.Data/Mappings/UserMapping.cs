using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mubbi.Marketplace.Register.Domain;

namespace Mubbi.Marketplace.Data.Mappings
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(user => user.Id);
            builder.OwnsOne(user => user.Document);

            builder.HasOne(user => user.Address)
                .WithOne(address => address.User)
                .HasForeignKey<Address>(address => address.UserId);

            builder.ToTable("User");
        }
    }
}
