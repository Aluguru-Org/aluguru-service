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

            builder.ToTable("Address");
        }      
    }
}