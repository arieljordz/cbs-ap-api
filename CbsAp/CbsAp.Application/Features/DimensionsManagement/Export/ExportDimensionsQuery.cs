using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.DimensionsManagement.Export
{
    public record ExportDimensionsQuery(
        string? EntityName,
        string? Dimension,
        string? Name,
        bool? Active) : IQuery<ResponseResult<byte[]>>;
}
