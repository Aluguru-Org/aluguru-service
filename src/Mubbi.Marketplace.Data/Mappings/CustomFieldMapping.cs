using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mubbi.Marketplace.Catalog.Domain;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Mubbi.Marketplace.Data.Mappings
{
    public class CustomFieldMapping : BaseMapConfiguration<CustomField>
    {
        public override void Configure(EntityTypeBuilder<CustomField> builder)
        {
            base.Configure(builder);

            builder.HasOne(x => x.Product)
                .WithMany(x => x.CustomFields);                

            builder.Property(e => e.ValueAsOptions).HasConversion(
                v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                v => JsonConvert.DeserializeObject<List<string>>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
        }
    }
}
