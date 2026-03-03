using CbsAp.Domain.Common.Interfaces;

namespace CbsAp.Domain.Common
{
    public abstract class BaseAuditableEntity : IBaseAudtitableEntity
    {
      
        public string? CreatedBy { get; set; }

        public DateTimeOffset? CreatedDate { get; set; }
        public string? LastUpdatedBy { get; set; }
        public DateTimeOffset? LastUpdatedDate { get; set; }
    }
}