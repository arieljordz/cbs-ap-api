using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.Entity;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.EntityProfileManagement.Queries.EntityProfileSearchActions
{
    public class EntityProfileNotInRoleQueryHandler :
        IQueryHandler<EntityProfileNotInRoleQuery,
            ResponseResult<IQueryable<GetAllEntityDto>>>

    {
        private readonly IEntityRepository _entityProfileRepository;

        public EntityProfileNotInRoleQueryHandler(IEntityRepository entityProfileRepository)
        {
            _entityProfileRepository = entityProfileRepository;
        }

        public async Task<ResponseResult<IQueryable<GetAllEntityDto>>> Handle(
            EntityProfileNotInRoleQuery request,
            CancellationToken cancellationToken)
        {
            var results =
                await _entityProfileRepository
                .GetEntityProfileNotInRoleAsync(request.RoleID);

            results = results ?? Enumerable.Empty<GetAllEntityDto>().AsQueryable();

            return ResponseResult<IQueryable<GetAllEntityDto>>
                   .OK(results);
        }
    }
}