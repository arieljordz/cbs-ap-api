using Bogus.DataSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.DTOs.PO
{
    // since oly retrieval will make it string only
    public class PurchaseOrderHeaderDto
    {
        public long? PurchaseOrderId { get; set; }
        public string? Entity { get; set; }
        public string? SupplierName { get; set; }
        public string? SupplierNo { get; set; }
        public string? PurchaseOrderNo { get; set; }
        public string? PurchaseDate { get; set; }
        public string? GoodsReceiptNo { get; set; }
        public string? GoodsReceiptDate { get; set; }

        public string? PurchaseOrderAmount { get; set; }
        public string? Currency { get; set; }
        public string? Keyword1 { get; set; }
        public string? Keyword2 { get; set; }
        public string? FreeField1 { get; set; }
        public string? FreeField2 { get; set; }
        public string? FreeField3 { get; set; }
        public string? Status { get; set; }
        public string? OrderNotes { get; set; }

        public string? MatchStatus { get; set; }

        public string SumGoodReceivedAmount { get; set; }
        public string SumInvoiceAmount { get; set; }
        public string OutstandingAmount { get; set; }

    }
}
