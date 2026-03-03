using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.PermissionManagement;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.PermissionManagement.Queries.SearchActions
{
    public record SearchPermissionByIDQuery(long PermissionID) :
        IQuery<ResponseResult<IQueryable<PermissionSearchByIdDTO>>>;
}