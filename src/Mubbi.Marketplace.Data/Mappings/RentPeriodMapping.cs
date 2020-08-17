using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mubbi.Marketplace.Catalog.Domain;

namespace Mubbi.Marketplace.Data.Mappings
{
    public class RentPeriodMapping : BaseMapConfiguration<RentPeriod>
    {
        public override void Configure(EntityTypeBuilder<RentPeriod> builder)
        {
            base.Configure(builder);

            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasColumnType("varchar(250)");

            builder.ToTable("RentPeriod");
        }
    }
}
