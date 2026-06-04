using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.Entity;
using CbsAp.Application.Shared.ResultPatten;



namespace CbsAp.Application.Features.Entity.Queries.GetEntityByRoleID
{
    public class GetEntitiesByRoleQueryHandler
    : IQueryHandler<
    GetEntitiesByRoleQuery,
    ResponseResult<List<GetAllEntityDto>>
   >
    {
        private readonly IEntityRepository _entityRepository;



        public GetEntitiesByRoleQueryHandler(
        IEntityRepository entityRepository
        )
        {
            _entityRepository = entityRepository;
        }



        public async Task<ResponseResult<List<GetAllEntityDto>>> Handle(
        GetEntitiesByRoleQuery request,
        CancellationToken cancellationToken
        )
        {
            var result = await _entityRepository
            .GetEntitiesByRoleAsync(request.RoleID);



            return ResponseResult<List<GetAllEntityDto>>
            .OK(result);
        }
    }
}