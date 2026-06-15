using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Entity;
using CbsAp.Application.Shared.ResultPatten;



namespace CbsAp.Application.Features.Entity.Queries.GetEntityByRoleID
{
    public record GetEntitiesByRoleQuery(long RoleID)
    : IQuery<ResponseResult<List<GetAllEntityDto>>>;
}