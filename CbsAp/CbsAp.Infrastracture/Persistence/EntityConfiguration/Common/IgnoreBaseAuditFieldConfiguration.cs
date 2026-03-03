using CbsAp.Domain.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.Common
{
    public class IgnoreBaseAuditFieldConfiguration
    {
        public static void IgnoreConfigureAuditFields<T>(EntityTypeBuilder<T> builder) where T : BaseAuditableEntity
        {

            builder.Ignore(e => e.CreatedDate);

            builder.Ignore(e => e.CreatedBy);

            builder.Ignore(e => e.LastUpdatedDate);

            builder.Ignore(e => e.LastUpdatedBy);
        }
    }
}
