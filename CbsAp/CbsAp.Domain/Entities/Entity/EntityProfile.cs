using CbsAp.Domain.Common;
using CbsAp.Domain.Entities.RoleManagement;

namespace CbsAp.Domain.Entities.Entity
{
    public class EntityProfile : BaseAuditableEntity
    {
        public long EntityProfileID { get; set; }
        public string EntityName { get; set; }
        public string EntityCode { get; set; }
        public string EmailAddress { get; set; }
        public string TaxID { get; set; }
        public string ERPFinanceSystem { get; set; }

        public int? DefaultInvoiceDueInDays { get; set; }
        public bool? InvAllowPresetAmount { get; set; }
        public bool? InvAllowPresetDimension { get; set; }
        public decimal? TaxDollarAmt { get; set; }
        public decimal? TaxPercentageAmt { get; set; }
        public int? InvDueDateCalculation { get; set; }

        public virtual ICollection<EntityMatchingConfig>? MatchingConfigs { get; set; }

        public virtual ICollection<RoleEntity> RoleEntities { get; set; }

        public bool AutomaticGoodsDelivered { get; set; }
    }
}