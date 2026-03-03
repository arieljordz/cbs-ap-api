using CbsAp.Domain.Common;
using CbsAp.Domain.Enums;

namespace CbsAp.Domain.Entities.Entity
{
    public class EntityMatchingConfig : BaseAuditableEntity
    {
        public long MatchingConfigID { get; set; }

        public long EntityProfileID { get; set; }

        public MatchingConfigType? ConfigType { get; set; }

        public string? MatchingLevel { get; set; }

        public string? InvoiceMatchBasis { get; set; }



        public decimal? DollarAmt { get; set; }
        public decimal? PercentageAmt { get; set; }

        public virtual EntityProfile EntityProfile { get; set; } = null!;
    }
}
