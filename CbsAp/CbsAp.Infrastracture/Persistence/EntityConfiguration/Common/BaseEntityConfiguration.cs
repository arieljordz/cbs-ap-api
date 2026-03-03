using CbsAp.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.Common
{
    public class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseAuditableEntity
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            CommonConfiguration.ConfigureAuditFields(builder);
        }
    }
}