using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mubbi.Marketplace.Register.Domain;

namespace Mubbi.Marketplace.Data.Mappings
{
    public class UserRoleMapping : BaseMapConfiguration<UserRole>
    {
        public override void Configure(EntityTypeBuilder<UserRole> builder)
        {
            base.Configure(builder);

            builder.HasMany(x => x.Users)
                .WithOne(x => x.UserRole)
                .HasForeignKey(x => x.UserRoleId);

            builder.HasMany(x => x.UserClaims)
                .WithOne(x => x.UserRole)
                .HasForeignKey(x => x.UserRoleId);

            builder.ToTable("UserRole");
        }
    }
}
