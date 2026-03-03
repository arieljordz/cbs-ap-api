using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Users.Queries.ExportUserQuery
{
    public record ExportUserQuery(
        string? FullName,
        string? UserId,
        bool? IsActive
        )
        : IQuery<ResponseResult<byte[]>>
    {
    }
}
