using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Entity;
using CbsAp.Application.Shared.ResultPatten;


namespace CbsAp.Application.Features.Entity.Queries.GetEntityByID
{
    public record GetEntityByIDQuery(long EntityID) : IQuery<ResponseResult<EntityDto>>
    {
    }
}
