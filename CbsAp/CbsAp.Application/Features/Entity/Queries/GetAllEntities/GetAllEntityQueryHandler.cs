using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.Entity;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Entity;
using CbsAp.Domain.Enums;
using Mapster;

namespace CbsAp.Application.Features.EntityProfileManagement.Queries.GetAllEntities
{
    public class GetAllEntityQueryHandler :
        IQueryHandler<GetAllEntityQuery, ResponseResult<IQueryable<GetAllEntityDto>>>
    {
        private readonly IUnitofWork _unitofWork;

        public GetAllEntityQueryHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<ResponseResult<IQueryable<GetAllEntityDto>>> Handle(
            GetAllEntityQuery request,
            CancellationToken cancellationToken)
        {
            var entityList = await _unitofWork
               .GetRepository<EntityProfile>()
               .GetAllAsync();

            var getAllEntityDto = entityList
                .ProjectToType<GetAllEntityDto>();

            return !getAllEntityDto.Any() 
               ? ResponseResult<IQueryable<GetAllEntityDto>>
                    .NotFound(
                            MessageConstants.Message("Entity Profiles", MessageOperationType.NotFound)
                      )
               : ResponseResult<IQueryable<GetAllEntityDto>>
                .SuccessRetrieveRecords(getAllEntityDto, "Entity Profiles");
        }
    }
}