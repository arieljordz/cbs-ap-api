using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Invoicing.Reports
{
    public record ExportExceptionInvoiceQuery(
            string? SupplierName,
            string? InvoiceNumber,
            string? PONumber,
            long RoleId,
        //Advance Search - Supplier Information
        string? PaymentTerm,
        string? SupplierNo,
        string? SuppABN,
        string? SuppBankAccount,
        //Advance Search - Invoice Detail
        int? EntityProfileID,
        string? GrNo,
        string? DateRangeInvoiceDate,
        DateTime? StartInvoiceDate,
        DateTime? EndInvoiceDate,
        string? DateRangeDueDate,
        DateTime? StartDueDate,
        DateTime? EndDueDate,
        int? DaystillDue,
        //Advance Search - Invoice Amounts
        decimal? NetAmount,
        int? TaxCodeID,
        decimal? TaxAmount,
        string? Currency,
        decimal? TotalAmount,
        //Advance Search - Routing flow
        string? InvRoutingFlowName,
        string? NextRole,
        string? Keyword,
        //Advance Search - Transaction Information
        string? MapID,
        string? DateRangeScanDate,
        DateTime? StartScanDate,
        DateTime? EndScanDate,
        string? InvoiceID
            ) : IQuery<ResponseResult<byte[]>>
    {
    }
}