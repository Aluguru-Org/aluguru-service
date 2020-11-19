using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Aluguru.Marketplace.Register.Domain;

namespace Aluguru.Marketplace.Data.Mappings
{
    public class UserClaimMapping : BaseMapConfiguration<UserClaim>
    {
        public override void Configure(EntityTypeBuilder<UserClaim> builder)
        {
            base.Configure(builder);

            builder.HasOne(x => x.UserRole)
                .WithMany(x => x.UserClaims)
                .HasForeignKey(x => x.UserRoleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("UserClaim");
        }
    }
}
