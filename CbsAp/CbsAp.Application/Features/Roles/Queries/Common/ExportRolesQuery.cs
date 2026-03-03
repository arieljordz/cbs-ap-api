using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Roles.Queries.Common
{
    public record ExportRolesQuery(
        string? EntityName,
        string? RoleName,
        bool? IsActive
    ) : IQuery<ResponseResult<byte[]>>
    {
    }
}