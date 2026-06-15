using Bogus.DataSets;
using CbsAp.Domain.Entities.Invoicing;
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
        public decimal? VarianceQuantity { get; set; }
        public string? UnitType { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? POOrderAmount { get; set; }
        public decimal? POReceiptedAmount { get; set; }
        public decimal? VarianceAmount { get; set; }
        public string? LineCurrency { get; set; }
        public string? GoodReceiptedStatus { get; set; } 
        public string? InvoiceMatchStatus { get; set; }

    }
}
