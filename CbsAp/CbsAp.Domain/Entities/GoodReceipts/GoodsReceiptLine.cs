using CbsAp.Domain.Common;
using CbsAp.Domain.Entities.Entity;
using CbsAp.Domain.Entities.PO;
using CbsAp.Domain.Entities.Supplier;

namespace CbsAp.Domain.Entities.GoodReceipts
{
    public class GoodsReceiptLine : BaseAuditableEntity
    {
        public long GoodsReceiptLineID { get; set; }
        public virtual GoodReceipt? GoodsReceipt { get; set; }
        public virtual PurchaseOrder? PurchaseOrder { get; set; }
        public long GoodsReceiptID { get; set; }

        public int LineNo { get; set; }

        public decimal Qty { get; set; }
        public decimal Amount { get; set; }

        public string? SupplierNo{ get; set; }
        public string? PurchaseOrderNo { get; set; }
        public string? ReceiptNo { get; set; }

        public string? FreeField1 { get; set; }
        public string? FreeField2 { get; set; }
        public string? FreeField3 { get; set; }
        public int InvoiceStatus { get; set; }
        
    }
}
