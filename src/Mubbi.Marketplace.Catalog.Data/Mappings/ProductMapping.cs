using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mubbi.Marketplace.Catalog.Domain;

namespace Mubbi.Marketplace.Catalog.Data.Mappings
{
    public class ProductMapping : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasColumnType("varchar(250)");            

            builder.Property(p => p.Description)
                    .IsRequired()
                   .HasColumnType("varchar(1000)");

            builder.Property(p => p.Image)
                   .IsRequired()
                   .HasColumnType("varchar(250)");

            builder.OwnsOne(p => p.Dimensions, onb =>
            {
                onb.Property(d => d.Height)
                  .HasColumnName("Height")
                  .HasColumnType("int");

                onb.Property(d => d.Width)
                  .HasColumnName("Width")
                  .HasColumnType("int");

                onb.Property(d => d.Depth)
                  .HasColumnName("Depth")
                  .HasColumnType("int");
            });
        }
    }
}
