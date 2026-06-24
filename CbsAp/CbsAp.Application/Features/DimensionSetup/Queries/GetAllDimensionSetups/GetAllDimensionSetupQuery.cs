using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.DimensionSetup;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.DimensionSetup.Queries.GetAllEntities
{
    public record GetAllDimensionSetupQuery : IQuery<ResponseResult<IQueryable<DimensionSetupListDto>>>
    {
    }
}