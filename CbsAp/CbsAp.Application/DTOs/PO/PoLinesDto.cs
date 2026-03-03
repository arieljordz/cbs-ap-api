using CbsAp.Domain.Enums;

namespace CbsAp.Application.DTOs.PO
{
    public class PoLinesDto : POMatchingDto
    {
        public long PurchaseOrderLineID { get; set; }

        public long PurchaseOrderID { get; set; }

        public long? InvoiceID { get; set; }

        public string? PoNo { get; set; }

        public long? LineNo { get; set; }

        public string? Description { get; set; }

        public long? AccountID { get; set; } 
        public string? AccountName { get; set; }

        public decimal? OriginalQty { get; set; }
        public decimal? BasedQty { get; set; }
        public decimal? TotalMatchedQty { get; set; }
        public decimal? Price { get; set; }

        public string? DeliveryNo { get; set; }
        public string? SupplierNo { get; set; }

        public POMatchingStatus? Status { get; set; }

        public bool? IsAvailableOrder { get; set; }
        public bool? I { get; set; }
    }

    public class SearchPoLinesDto
    {
        public List<PoLinesDto>? PoLines { get; set; } = new List<PoLinesDto>();
    }

}
