using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Aluguru.Marketplace.Register.Domain;

namespace Aluguru.Marketplace.Data.Mappings
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