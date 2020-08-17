﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mubbi.Marketplace.Rent.Domain;

namespace Mubbi.Marketplace.Data.Mappings
{
    public class VoucherMapping : BaseMapConfiguration<Voucher>
    {
        public override void Configure(EntityTypeBuilder<Voucher> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Code)
                .IsRequired();

            builder.HasMany(x => x.Orders)
                .WithOne(x => x.Voucher)
                .HasForeignKey(x => x.VoucherId);

            builder.ToTable("Voucher");
        }
    }
}
