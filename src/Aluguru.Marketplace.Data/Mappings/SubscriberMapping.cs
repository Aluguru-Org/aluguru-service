using Aluguru.Marketplace.Newsletter.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aluguru.Marketplace.Data.Mappings
{
    public class SubscriberMapping : BaseMapConfiguration<Subscriber>
    {
        public override void Configure(EntityTypeBuilder<Subscriber> builder)
        {
            base.Configure(builder);

            builder.ToTable("Subscriber");
        }
    }
}
