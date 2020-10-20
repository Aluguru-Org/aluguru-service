using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Aluguru.Marketplace.Register.Domain;

namespace Aluguru.Marketplace.Data.Mappings
{
    public class UserMapping : BaseMapConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);

            builder.ToTable("User");
        }
    }
}
