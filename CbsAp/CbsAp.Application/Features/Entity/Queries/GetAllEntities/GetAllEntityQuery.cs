using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Entity;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.EntityProfileManagement.Queries.GetAllEntities
{
    public record GetAllEntityQuery : IQuery<ResponseResult<IQueryable<GetAllEntityDto>>>
    {
    }
}