using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mubbi.Marketplace.Register.Domain;

namespace Mubbi.Marketplace.Data.Mappings
{
    public class ContactMapping : BaseMapConfiguration<Contact>
    {
        public override void Configure(EntityTypeBuilder<Contact> builder)
        {
            base.Configure(builder);
            builder.HasOne(x => x.User).WithOne(x => x.Contact).HasForeignKey<Contact>(x => x.UserId);
            builder.ToTable("Contact");
        }
    }
}
