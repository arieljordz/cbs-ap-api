using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.DimensionSetup;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.DimensionSetup;
using CbsAp.Domain.Enums;
using Mapster;

namespace CbsAp.Application.Features.DimensionSetup.Queries.GetAllEntities
{
    public class GetAllDimensionSetupQueryHandler :
        IQueryHandler<GetAllDimensionSetupQuery, ResponseResult<IQueryable<DimensionSetupListDto>>>
    {
        private readonly IUnitofWork _unitofWork;

        public GetAllDimensionSetupQueryHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<ResponseResult<IQueryable<DimensionSetupListDto>>> Handle(
            GetAllDimensionSetupQuery request,
            CancellationToken cancellationToken)
        {
            var DimensionSetupList = await _unitofWork
               .GetRepository<CbsAp.Domain.Entities.DimensionSetup.DimensionSetup>()
               .GetAllAsync();

            var getAllDimensionSetupDto = DimensionSetupList
                .ProjectToType<DimensionSetupListDto>();

            getAllDimensionSetupDto = getAllDimensionSetupDto.OrderBy(x => x.DisplayOrder).ThenBy(x => x.LastUpdatedDate);

            return !getAllDimensionSetupDto.Any() 
               ? ResponseResult<IQueryable<DimensionSetupListDto>>
                    .NotFound(
                            MessageConstants.Message("DimensionSetup", MessageOperationType.NotFound)
                      )
               : ResponseResult<IQueryable<DimensionSetupListDto>>
                .SuccessRetrieveRecords(getAllDimensionSetupDto, "DimensionSetup");
        }
    }
}