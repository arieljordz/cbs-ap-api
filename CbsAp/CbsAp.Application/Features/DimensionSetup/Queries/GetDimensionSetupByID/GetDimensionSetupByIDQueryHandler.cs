using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Services.DimensionSetup;
using CbsAp.Application.DTOs.DimensionSetup;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.DimensionSetup.Queries.GetDimensionSetupByID
{
    public class GetDimensionSetupByIDQueryHandler : IQueryHandler<GetDimensionSetupByIDQuery, ResponseResult<DimensionSetupDto>>
    {
        private readonly IDimensionSetupService _dimensionSetupService;

        public GetDimensionSetupByIDQueryHandler(IDimensionSetupService dimensionSetupService)
        {
            _dimensionSetupService = dimensionSetupService;
        }

        public async Task<ResponseResult<DimensionSetupDto>> Handle(GetDimensionSetupByIDQuery request, CancellationToken cancellationToken)
        {
            var DimensionSetup = await _dimensionSetupService.GetDimensionSetupByIdAsync(request.dimensionSetupId)!;
            return DimensionSetup == null ?
                ResponseResult<DimensionSetupDto>.BadRequest("DimensionSetup not found") :
                ResponseResult<DimensionSetupDto>.OK(DimensionSetup);
        }
    }
}