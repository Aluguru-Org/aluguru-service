using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mubbi.Marketplace.Register.Domain;

namespace Mubbi.Marketplace.Data.Mappings
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
