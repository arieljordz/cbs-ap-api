using CbsAp.Domain.Common;

namespace CbsAp.Application.Shared.Extensions
{
    public static class AuditableEntityExtensions
    {
        public static void SetAuditFieldsOnCreate(this BaseAuditableEntity entity, string createdBy)
        {
            entity.CreatedDate = DateTime.UtcNow;
            entity.CreatedBy = createdBy;
        }

        public static void SetAuditFieldsOnUpdate(this BaseAuditableEntity entity, string lastUpdatedBy)
        {
            entity.LastUpdatedDate = DateTime.UtcNow;
            entity.LastUpdatedBy = lastUpdatedBy;
        }

        public static IEnumerable<T> SetAuditFieldsOnCreate<T>(this IEnumerable<T> entities, string createdBy)
       where T : BaseAuditableEntity
        {
            entities.ToList().ForEach(
                entity =>
                {
                    entity.CreatedDate = DateTime.UtcNow;
                    entity.CreatedBy = createdBy;
                }
                );

            return entities;
        }
    }
}