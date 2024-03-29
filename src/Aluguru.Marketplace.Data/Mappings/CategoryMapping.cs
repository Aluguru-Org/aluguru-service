﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Aluguru.Marketplace.Catalog.Domain;
using System.Security;

namespace Aluguru.Marketplace.Data.Mappings
{
    public class CategoryMapping : BaseMapConfiguration<Category>
    {
        public override void Configure(EntityTypeBuilder<Category> builder)
        {
            base.Configure(builder);

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
