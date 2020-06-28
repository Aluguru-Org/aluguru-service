using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mubbi.Marketplace.Catalog.Domain;

namespace Mubbi.Marketplace.Data.Mappings
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

            builder.Property(p => p.ImageUrl)
                   .IsRequired()
                   .HasColumnType("varchar(250)");

            builder.ToTable("Product");
        }
    }
}
