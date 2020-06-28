using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mubbi.Marketplace.Register.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Data.Mappings
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasKey(x => x.Username);

            builder.OwnsOne(x => x.Name, onb =>
            {
                onb.Property(o => o.FirstName)
                    .HasColumnName("FirstName")
                    .HasColumnType("varchar(250)");
                onb.Property(o => o.LastName)
                    .HasColumnName("LastName")
                    .HasColumnType("varchar(250)");
                onb.Property(o => o.FullName)
                    .HasColumnName("FullName")
                    .HasColumnType("varchar(1000)");
            });

            builder.OwnsOne(x => x.Email);
            builder.OwnsOne(x => x.Address);
            builder.OwnsOne(x => x.Document);

            builder.ToTable("User");
        }
    }
}
