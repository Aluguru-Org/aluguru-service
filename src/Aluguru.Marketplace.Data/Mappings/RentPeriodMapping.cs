using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Aluguru.Marketplace.Catalog.Domain;

namespace Aluguru.Marketplace.Data.Mappings
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
