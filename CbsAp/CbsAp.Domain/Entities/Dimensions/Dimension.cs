using CbsAp.Domain.Common;
using CbsAp.Domain.Entities.Entity;

namespace CbsAp.Domain.Entities.Dimensions
{
    public class Dimension : BaseAuditableEntity
    {
        public long DimensionID { get; set; }

        public string DimensionCode { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public string? FreeField1 { get; set; }

        public string? FreeField2 { get; set; }

        public string? FreeField3 { get; set; }

        public long EntityProfileID { get; set; }

        public virtual EntityProfile? EntityProfile { get; set; }
    }
}
