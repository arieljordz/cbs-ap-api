using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.KeywordManagement.Queries
{
    public record ExportKeywordsQuery(
         string? KeywordName,
        string? EntityName,
        string? InvoiceRoutingFlowName
        )
        : IQuery<ResponseResult<byte[]>>
    {
    }
}
