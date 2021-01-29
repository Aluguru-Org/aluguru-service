using Aluguru.Marketplace.Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aluguru.Marketplace.Data.Mappings
{
    public class InvalidDatesMapping : BaseMapConfiguration<InvalidDates>
    {
        public override void Configure(EntityTypeBuilder<InvalidDates> builder)
        {
            base.Configure(builder);

            builder
                .HasOne(x => x.Product)
                .WithOne(x => x.InvalidDates)
                .HasForeignKey<InvalidDates>(x => x.ProductId);

            builder.Property(x => x.Days)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                    v => JsonConvert.DeserializeObject<List<DayOfWeek>>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
                );

            builder.Property(x => x.Dates)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                    v => JsonConvert.DeserializeObject<List<DateTime>>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
                );

            builder.Property(x => x.Periods)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                    v => JsonConvert.DeserializeObject<List<Period>>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
                );


            builder.ToTable("InvalidDates");
        }
    }
}
