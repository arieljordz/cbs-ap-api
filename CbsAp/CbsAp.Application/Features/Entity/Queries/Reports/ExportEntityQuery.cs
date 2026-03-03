using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Entity.Queries.Reports
{
    public record ExportEntityQuery(string? EntityCode, string? EntityName)
        : IQuery<ResponseResult<byte[]>>
    {
    }
}