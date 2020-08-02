using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mubbi.Marketplace.Rent.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Data.Mappings
{
    public class VoucherMapping : BaseMapConfiguration<Voucher>
    {
        public override void Configure(EntityTypeBuilder<Voucher> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Code)
                .IsRequired();



            builder.ToTable("Voucher");
        }
    }
}
