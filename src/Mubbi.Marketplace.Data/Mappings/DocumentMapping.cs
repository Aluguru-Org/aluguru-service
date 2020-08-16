using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mubbi.Marketplace.Register.Domain;

namespace Mubbi.Marketplace.Data.Mappings
{
    public class DocumentMapping : BaseMapConfiguration<Document>
    {
        public override void Configure(EntityTypeBuilder<Document> builder)
        {
            base.Configure(builder);
            builder.HasOne(x => x.User).WithOne(x => x.Document).HasForeignKey<Document>(x => x.UserId);
            builder.ToTable("Document");
        }      
    }
}