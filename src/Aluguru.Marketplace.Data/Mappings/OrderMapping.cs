using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Aluguru.Marketplace.Rent.Domain;

namespace Aluguru.Marketplace.Data.Mappings
{
    public class OrderMapping : BaseMapConfiguration<Order>
    {
        public override void Configure(EntityTypeBuilder<Order> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.OrderStatus)
                .IsRequired();

            builder.HasMany(order => order.OrderItems)
                .WithOne(orderItem => orderItem.Order)
                .HasForeignKey(orderItem => orderItem.OrderId);

            builder.ToTable("Order");
        }
    }
}
