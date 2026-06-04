using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.DTOs.Invoicing.Invoice
{
    public class InvMyAdvanceSearchDto
    {
        public string? SupplierName { get; init; }
        public string? InvoiceNo { get; init; }
        public string? PONo { get; init; }
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
        public string? SortField { get; init; }
        public int? SortOrder { get; init; }
        public int RoleId { get; set; }

        //Advance Search - Supplier Information
        public string? PaymentTerm { get; set; }
        public string? SupplierNo { get; set; }
        public string? SuppABN { get; set; }
        public string? SuppBankAccount { get; set; }

        //Advance Search - Invoice Detail
        public int? EntityProfileID { get; set; }

        public string? GrNo { get; set; }

        public string? DateRangeInvoiceDate { get; set; }

        public DateTime? StartInvoiceDate { get; set; }

        public DateTime? EndInvoiceDate { get; set; }

        public string? DateRangeDueDate { get; set; }

        public DateTime? StartDueDate { get; set; }

        public DateTime? EndDueDate { get; set; }

        public int? DaystillDue { get; set; }

        //Advance Search - Invoice Amounts

        public decimal? NetAmount { get; set; }
        public int? TaxCodeID { get; set; }
        public decimal? TaxAmount { get; set; }
        public int? Currency { get; set; }
        public decimal? TotalAmount { get; set; }

        //Advance Search - Routing flow
        public string? InvRoutingFlowName { get; set; }

        public string? NextRole { get; set; }
        public string? Keyword { get; set; }

        //Advance Search - Transaction Information
        public string? MapID { get; set; }
        public string? DateRangeScanDate { get; set; }

        public DateTime? StartScanDate { get; set; }

        public DateTime? EndScanDate { get; set; }

        public string? InvoiceID { get; set; }
    }
}
