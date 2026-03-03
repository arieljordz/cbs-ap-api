using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Supplier.Queries.Reports
{
    public record SupplierExportQuery(string? EntityName,
            string? SupplierID,
            string? SupplierName,
            bool? IsActive)
        : IQuery<ResponseResult<byte[]>>
    {
    }
}
