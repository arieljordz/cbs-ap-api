using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.DTOs.AdvanceSearch
{
    public class AdvanceSearchDto
    {
        public long AdvanceSearchId { get; set; }
        public string UserId { get; set; }
        public string? JsonFilter { get; set; }
        public string? FormName { get; set; }
    }

    public class AdvanceSearchRequestForm
    {
        public long AdvanceSearchId { get; set; }
        public string? SupplierName { get; set; }
        public string? InvoiceNo { get; set; }
        public string? PONo { get; set; }

        public string? PaymentTerm { get; set; }
        public string? SupplierNo { get; set; }
        public string? SuppABN { get; set; }
        public string? SuppBankAccount { get; set; }

        public int? EntityProfileID { get; set; }
        public string? GrNo { get; set; }

        public string? StartInvoiceDate { get; set; }
        public string? EndInvoiceDate { get; set; }

        public string? StartDueDate { get; set; }
        public string? EndDueDate { get; set; }

        public int? DaystillDue { get; set; }

        public decimal? NetAmount { get; set; }
        public int? TaxCodeID { get; set; }
        public decimal? TaxAmount { get; set; }
        public string? Currency { get; set; }
        public decimal? TotalAmount { get; set; }

        public string? InvRoutingFlowName { get; set; }
        public string? NextRole { get; set; }
        public string? Keyword { get; set; }

        public string? MapID { get; set; }
        public string? StartScanDate { get; set; }
        public string? EndScanDate { get; set; }

        public string? InvoiceID { get; set; }

        // Angular sends these because of BasePaginationQuery
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }

        public string FormName { get; set; }
    }

}
