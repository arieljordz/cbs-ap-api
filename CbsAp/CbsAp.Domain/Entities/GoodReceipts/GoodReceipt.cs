using CbsAp.Domain.Common;
using CbsAp.Domain.Entities.Entity;
using CbsAp.Domain.Entities.Supplier;

namespace CbsAp.Domain.Entities.GoodReceipts
{
    public class GoodReceipt : BaseAuditableEntity
    {
        public long GoodsReceiptID { get; set; }

        public string GoodsReceiptNumber { get; set; } = string.Empty;

        public string? DeliveryNote { get; set; }

        public bool Active { get; set; }

        public DateTimeOffset? DeliveryDate { get; set; }

        public long EntityProfileID { get; set; }

        public virtual EntityProfile? EntityProfile { get; set; }

        public long SupplierInfoID { get; set; }

        public virtual SupplierInfo? Supplier { get; set; }
    }
}
