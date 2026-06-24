using Bogus.DataSets;
using CbsAp.Domain.Entities.GoodReceipts;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Entities.PO;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CbsAp.Application.DTOs.PO
{
    public class PurchaseHeaderLineDetailsDto
    {
        public long PurchaseOrderLineID { get; set; }

        public string? PurchaseNumber { get; set; }
        public long? LineNumber { get; set; }
        public string? Item { get; set; }
        public string? Description { get; set; }
        public decimal? POOrderQuantity { get; set; }
        public string? GoodsReceiptNo { get; set; }
        public decimal? GRReceiptedQuantity { get; set; }
        public DateTimeOffset? GRReceiptDate { get; set; }

        public string? GRReceiptDateDisplayString { get; set; }
        public decimal? VarianceQuantity { get; set; }
        public string? UnitType { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? POOrderAmount { get; set; }
        public decimal? POReceiptedAmount { get; set; }
        public decimal? VarianceAmount { get; set; }
        public string? LineCurrency { get; set; }
        public string? GoodReceiptedStatus { get; set; } 
        public string? InvoiceMatchStatus { get; set; }

        public string? InvoiceNo { get; set; }
        public long? InvoiceId { get; set; }

        public decimal InvoiceAmount { get; set; }

        public decimal GoodReceiveAmount { get; set; }

        public decimal OutstandingAmount { get; set; }

    }

    public class grlineDTO
    {

        public long GoodsReceiptLineID { get; set; }
        public virtual GoodReceipt? GoodsReceipt { get; set; }
        public virtual PurchaseOrder? PurchaseOrder { get; set; }
        public long GoodsReceiptID { get; set; }

        public long? LineNo { get; set; }

        public decimal Qty { get; set; }
        public decimal Amount { get; set; }

        public string? SupplierNo { get; set; }
        public string? PurchaseOrderNo { get; set; }
        public string? ReceiptNo { get; set; }

        public string? FreeField1 { get; set; }
        public string? FreeField2 { get; set; }
        public string? FreeField3 { get; set; }
        public int InvoiceStatus { get; set; }
    }

    public class ExportPoDetailSearchDto
    {
 

        public string? PurchaseNumber { get; set; }
        public long? LineNumber { get; set; }
        public string? Item { get; set; }
        public string? Description { get; set; }
        public decimal? POOrderQuantity { get; set; }
        public string? GoodsReceiptNo { get; set; }
        public decimal? GRReceiptedQuantity { get; set; }

        public string? GRReceiptDateDisplayString { get; set; }
        public decimal? VarianceQuantity { get; set; }
        public string? UnitType { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? POOrderAmount { get; set; }
        public decimal? POReceiptedAmount { get; set; }
        public decimal? VarianceAmount { get; set; }
        public string? LineCurrency { get; set; }
        public string? GoodReceiptedStatus { get; set; }
        public string? InvoiceMatchStatus { get; set; }

        public string? InvoiceNo { get; set; }


        public decimal InvoiceAmount { get; set; }
        public decimal GoodReceiveAmount { get; set; }

        public decimal OutstandingAmount { get; set; }
    }
}

