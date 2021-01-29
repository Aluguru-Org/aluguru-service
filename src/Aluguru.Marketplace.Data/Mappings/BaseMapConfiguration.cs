using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Aluguru.Marketplace.Domain;

namespace Aluguru.Marketplace.Data.Mappings
{
    public abstract class BaseMapConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : Entity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
        }
    }
}
