using Aluguru.Marketplace.Rent.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aluguru.Marketplace.Data.Mappings
{
    public class OrderItemMapping : BaseMapConfiguration<OrderItem>
    {
        public override void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            base.Configure(builder);

            builder.ToTable("OrderItem");
        }
    }
}
