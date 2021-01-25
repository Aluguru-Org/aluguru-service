using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Aluguru.Marketplace.Catalog.Domain;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;

namespace Aluguru.Marketplace.Data.Mappings
{
    public class ProductMapping : BaseMapConfiguration<Product>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.UserId)
                   .IsRequired();

            builder.Property(p => p.Name)
                   .IsRequired();            

            builder.Property(p => p.Description)
                    .IsRequired();

            builder.Property(p => p.BlockedDates)
                   .IsRequired()
                   .HasConversion(
                        v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                        v => JsonConvert.DeserializeObject<List<DateTime>>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
                    );

            builder.Property(p => p.ImageUrls)
                   .IsRequired()
                   .HasConversion(
                        v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                        v => JsonConvert.DeserializeObject<List<string>>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
                    );

            builder.OwnsOne(p => p.Price, onb =>
            {
                onb.Property(prc => prc.SellPrice)
                    .HasColumnName("SellPrice")
                    .HasColumnType("decimal");

                onb.Property(prc => prc.DailyRentPrice)
                    .HasColumnName("DailyRentPrice")
                    .HasColumnType("decimal");

                onb.Property(prc => prc.PeriodRentPrices)
                    .HasColumnName("PeriodRentPrices")
                    .HasConversion(
                        v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                        v => JsonConvert.DeserializeObject<List<PeriodPrice>>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
                    );
            });

            builder.HasMany(x => x.CustomFields)
                .WithOne(x => x.Product)
                .HasForeignKey(x => x.ProductId);

            builder.ToTable("Product");
        }
    }
}
