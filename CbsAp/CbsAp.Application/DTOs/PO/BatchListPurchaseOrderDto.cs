using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.DTOs.PO
{
    public class BatchListPurchaseOrderDto
    {
        public int IndexId { get; set; }
        public long PurchaseOrderID { get; set; }
        public string? PoNo { get; set; }
        public string? EntityName { get; set; }
        public string? SupplierName { get; set; }
        public string? SupplierTaxID { get; set; }
        public string? Currency { get; set; }
        public decimal? NetAmount { get; set; }
        public bool? IsActive { get; set; }
    }
}
