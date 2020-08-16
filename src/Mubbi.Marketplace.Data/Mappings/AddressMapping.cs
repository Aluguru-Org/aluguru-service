using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mubbi.Marketplace.Register.Domain;

namespace Mubbi.Marketplace.Data.Mappings
{
    public class AddressMapping : BaseMapConfiguration<Address>
    {
        public override void Configure(EntityTypeBuilder<Address> builder)
        {
            base.Configure(builder);
            builder.HasOne(x => x.User).WithOne(x => x.Address).HasForeignKey<Address>(x => x.UserId);
            builder.ToTable("Address");
        }      
    }
}