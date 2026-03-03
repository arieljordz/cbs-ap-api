using CbsAp.Domain.Enums;

namespace CbsAp.Application.DTOs.PO
{
    public class POMatchingDto
    {
        public long PurchaseOrderMatchTrackingID { get; set; }

        public long? InvAllocLineID { get; set; }

        public long? Account { get; set; }

        public decimal Qty { get; set; }
        public decimal? BaseRemainingQty { get; set; } 
        public decimal? RemainingQty { get; set; }

        public decimal? Amount { get; set; }

        public decimal? NetAmount { get; set; }

        public decimal? TaxAmount { get; set; }

        public DateTimeOffset MatchingDate { get; set; }

        public POMatchingStatus? MatchingStatus { get; set; }

        public bool isForEditPOMatching { get; set; }
    }
}