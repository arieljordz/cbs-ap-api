using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Entity;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.EntityProfileManagement.Queries.EntityProfileSearchActions
{
    public record EntityProfileNotInRoleQuery(long RoleID)
        : IQuery<ResponseResult<IQueryable<GetAllEntityDto>>>;
}