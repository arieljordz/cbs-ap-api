using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Invoicing.Reports
{
    public class ExportMyInvoiceQuery : IQuery<ResponseResult<byte[]>>
    {
        public string? SupplierName { get; set; }
        public string? InvoiceNo { get; set; }
        public string? PONo { get; set; }
        public int RoleId { get; set; } = 0;

        
    }
}