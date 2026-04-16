using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.InvoiceInquiry.Queries.Reports
{
    public record ExportInvoiceInquiryQuery(
        string? SupplierName,
        string? InvoiceNumber, 
        string? PONumber, 
        int? Status, 
        DateTimeOffset? ScanDateFrom, 
        DateTimeOffset? ScanDateTo,
        DateTimeOffset? InvoiceDateFrom, 
        DateTimeOffset? InvoiceDateTo)
        : IQuery<ResponseResult<byte[]>>
    {
    }
}