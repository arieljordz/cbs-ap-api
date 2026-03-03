using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.TaxCodesManagement.Queries.Common
{
    public record ExportTaxCodesQuery(
        string? EntityName,
        string? TaxCodeName,
        string? Code
        )
        : IQuery<ResponseResult<byte[]>>
    {
    }
}
