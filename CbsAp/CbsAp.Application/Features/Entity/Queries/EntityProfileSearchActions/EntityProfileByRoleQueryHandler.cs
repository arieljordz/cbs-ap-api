using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.Entity;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.EntityProfileManagement.Queries.EntityProfileSearchActions
{
    public class EntityProfileByRoleQueryHandler :
        IQueryHandler<EntityProfileByRoleQuery, ResponseResult<IQueryable<EntityRoleDto>>>
    {
        private readonly IEntityRepository _entityProfileRepository;

        public EntityProfileByRoleQueryHandler(IEntityRepository entityProfileRepository)
        {
            _entityProfileRepository = entityProfileRepository;
        }

        public async Task<ResponseResult<IQueryable<EntityRoleDto>>>
            Handle(EntityProfileByRoleQuery request
            , CancellationToken cancellationToken)
        {
            var results = await _entityProfileRepository
            .GetAvailableEntityByRoleAsync(request.RoleID)
            ?? Enumerable.Empty<EntityRoleDto>().AsQueryable();

            return ResponseResult<IQueryable<EntityRoleDto>>.OK(results);
        }
    }
}