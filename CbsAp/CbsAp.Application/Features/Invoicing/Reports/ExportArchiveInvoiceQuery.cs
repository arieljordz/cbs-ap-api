using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Invoicing.Reports
{
    public record ExportArchiveInvoiceQuery(
            string? SupplierName,
            string? InvoiceNo,
            string? PONo) : IQuery<ResponseResult<byte[]>>
    {
    }
}