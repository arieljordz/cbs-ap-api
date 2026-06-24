using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.DimensionSetup;
using CbsAp.Application.Shared.ResultPatten;


namespace CbsAp.Application.Features.DimensionSetup.Queries.GetDimensionSetupByID
{
    public record GetDimensionSetupByIDQuery(long dimensionSetupId) : IQuery<ResponseResult<DimensionSetupDto>>
    {
    }
}
