using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Invoicing.Reports
{
    public record ExportRejectedInvoiceQuery(
            string? SupplierName,
            string? InvoiceNo,
            string? PONo) : IQuery<ResponseResult<byte[]>>
    {
    }
}