using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Enums;

namespace CbsAp.Application.Features.InvoiceInquiry.Queries.Reports
{
    public record ExportInvoiceInquiryQuery(
        int? SupplierInfoID,
        string? InvoiceNumber, 
        string? PONumber, 
        int? RoleID,
        List<InvoiceStatusType>? Status,
        DateTimeOffset? InvoiceDateFrom,
        DateTimeOffset? InvoiceDateTo,
        DateTimeOffset? InvoiceDueDateFrom,
        DateTimeOffset? InvoiceDueDateTo,
        DateTimeOffset? PaymentDateFrom,
        DateTimeOffset? PaymentDateTo,
        DateTimeOffset? ScanDateFrom, 
        DateTimeOffset? ScanDateTo)
        : IQuery<ResponseResult<byte[]>>
    {
    }
}