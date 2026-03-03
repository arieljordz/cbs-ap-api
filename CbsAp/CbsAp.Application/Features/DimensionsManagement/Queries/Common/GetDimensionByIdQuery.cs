using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.DimensionsManagement;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.DimensionsManagement.Queries.Common
{
    public record GetDimensionByIdQuery(long DimensionID) : IQuery<ResponseResult<DimensionDTO>>;
}
