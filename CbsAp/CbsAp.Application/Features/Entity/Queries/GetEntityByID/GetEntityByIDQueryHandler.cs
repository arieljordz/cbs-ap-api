using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Services.Entity;
using CbsAp.Application.DTOs.Entity;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Entity.Queries.GetEntityByID
{
    public class GetEntityByIDQueryHandler : IQueryHandler<GetEntityByIDQuery, ResponseResult<EntityDto>>
    {
        private readonly IEntityService _entityService;

        public GetEntityByIDQueryHandler(IEntityService entityService)
        {
            _entityService = entityService;
        }

        public async Task<ResponseResult<EntityDto>> Handle(GetEntityByIDQuery request, CancellationToken cancellationToken)
        {
            var entity = await _entityService.GetEntityByIdAsync(request.EntityID)!;
            return entity == null ?
                ResponseResult<EntityDto>.BadRequest("Entity not found") :
                ResponseResult<EntityDto>.OK(entity);
        }
    }
}