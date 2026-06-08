using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Invoicing.InvActions.Queries.ExceptionsSearch
{


    public record ExceptionsSearchQuery : IQuery<ResponseResult<PaginatedList<ExceptionInvoiceSearchDto>>>
    {
        public string? SupplierName { get; init; }
        public string? InvoiceNo { get; init; }
        public string? PONo { get; init; }
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
        public string? SortField { get; init; }
        public int? SortOrder { get; init; }
        public int RoleId { get; set; }



        // Advance Search - Supplier Information
        public string? PaymentTerm { get; init; }
        public string? SupplierNo { get; init; }
        public string? SuppABN { get; init; }
        public string? SuppBankAccount { get; init; }



        // Advance Search - Invoice Detail
        public int? EntityProfileID { get; init; }
        public string? GrNo { get; init; }
        public string? DateRangeInvoiceDate { get; init; }
        public DateTime? StartInvoiceDate { get; init; }
        public DateTime? EndInvoiceDate { get; init; }
        public string? DateRangeDueDate { get; init; }
        public DateTime? StartDueDate { get; init; }
        public DateTime? EndDueDate { get; init; }
        public int? DaystillDue { get; init; }



        // Advance Search - Invoice Amounts
        public decimal? NetAmount { get; init; }
        public int? TaxCodeID { get; init; }
        public decimal? TaxAmount { get; init; }
        public string? Currency { get; init; }
        public decimal? TotalAmount { get; init; }



        // Advance Search - Routing Flow
        public string? InvRoutingFlowName { get; init; }
        public string? NextRole { get; init; }
        public string? Keyword { get; init; }



        // Advance Search - Transaction Information
        public string? MapID { get; init; }
        public string? DateRangeScanDate { get; init; }
        public DateTime? StartScanDate { get; init; }
        public DateTime? EndScanDate { get; init; }
        public string? InvoiceID { get; init; }

    } 
}