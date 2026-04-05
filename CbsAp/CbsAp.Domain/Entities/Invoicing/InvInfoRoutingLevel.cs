using CbsAp.Domain.Common;
using CbsAp.Domain.Entities.Keywords;
using CbsAp.Domain.Entities.RoleManagement;
using CbsAp.Domain.Entities.Supplier;
using System;


namespace CbsAp.Domain.Entities.Invoicing
{
    public class InvInfoRoutingLevel : BaseAuditableEntity
    {
        public long  InvInfoRoutingLevelID { get; set; }

        public long? InvRoutingFlowID { get; set; }
        public virtual InvRoutingFlow? InvRoutingFlow { get; set; }

        public long? SupplierInfoID { get; set; }
        public virtual SupplierInfo? SupplierInfo { get; set; }

        public long? KeywordID { get; set; }
        public virtual Keyword? Keyword { get; set; }
        public long? InvoiceID { get; set; }
        public virtual Invoice? Invoice { get; set; }
        public long RoleID { get; set; }
        public virtual Role? Role { get; set; }
        public int Level { get; set; }
        public int? InvFlowStatus { get; set; }

    }
}
