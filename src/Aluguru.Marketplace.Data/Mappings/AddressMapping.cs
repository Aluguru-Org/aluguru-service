using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Aluguru.Marketplace.Register.Domain;

namespace Aluguru.Marketplace.Data.Mappings
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