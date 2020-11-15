using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aluguru.Marketplace.Data.Mappings
{
    public class PaymentMapping : BaseMapConfiguration<Payment.Domain.Payment>
    {
        public override void Configure(EntityTypeBuilder<Payment.Domain.Payment> builder)
        {
            base.Configure(builder);

            builder.ToTable("Payment");
        }
    }
}
