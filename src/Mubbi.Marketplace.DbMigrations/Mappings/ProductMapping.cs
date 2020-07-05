using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mubbi.Marketplace.Catalog.Domain;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Mubbi.Marketplace.Data.Mappings
{
    public class ProductMapping : BaseMapConfiguration<Product>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.Name)
                   .IsRequired();            

            builder.Property(p => p.Description)
                    .IsRequired();

            builder.Property(p => p.ImageUrls)
                   .IsRequired()
                   .HasConversion(
                        v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                        v => JsonConvert.DeserializeObject<IReadOnlyCollection<string>>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
                    );

            builder.HasMany(x => x.CustomFields)
                .WithOne(x => x.Product)
                .HasForeignKey(x => x.ProductId);

            builder.ToTable("Product");
        }
    }
}
