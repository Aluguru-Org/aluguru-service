using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mubbi.Marketplace.Catalog.Domain;

namespace Mubbi.Marketplace.Data.Mappings
{
    public class CategoryMapping : BaseMapConfiguration<Category>
    {
        public override void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasColumnType("varchar(250)");

            builder.HasMany(c => c.Products)
                   .WithOne(p => p.Category)
                   .HasForeignKey(p => p.CategoryId);

            builder.HasMany(c => c.SubCategories)
                .WithOne(c => c.MainCategory)
                .HasForeignKey(c => c.MainCategoryId);

            builder.ToTable("Category");
        }
    }
}
