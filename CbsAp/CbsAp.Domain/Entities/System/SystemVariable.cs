
using CbsAp.Domain.Common;

namespace CbsAp.Domain.Entities.System
{
    public class SystemVariable :BaseAuditableEntity
    {
        public long SystemVariableID { get; set; }

        public required string Name { get; set; }

        public string? Value { get; set; }

        public required string Description { get; set; }
    }
}
