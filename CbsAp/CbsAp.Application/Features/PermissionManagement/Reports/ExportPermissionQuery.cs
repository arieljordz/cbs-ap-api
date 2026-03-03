using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.PermissionManagement.Reports
{
    public record ExportPermissionQuery(
        long? PermissionID,
        string? PermissionName,
        bool? IsActive) : IQuery<ResponseResult<byte[]>>
    {

    }
}
