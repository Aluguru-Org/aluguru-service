using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mubbi.Marketplace.Domain;
using static PampaDevs.Utils.Helpers.DateTimeHelper;

namespace Mubbi.Marketplace.Data.Mappings
{
    public abstract class BaseMapConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : Entity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.DateCreated).HasDefaultValue(NewDateTime());
            builder.Property(x => x.DateUpdated).HasDefaultValue(NewDateTime());
        }
    }
}
