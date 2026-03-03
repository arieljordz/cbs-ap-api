using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.RolesManagement;
using CbsAp.Application.Features.Roles.Queries.Common;
using CbsAp.Application.Shared.ResultPatten;
using Mapster;

namespace CbsAp.Application.Features.Roles.Queries.SearchHandler
{
    public class GetAllRolesQueryHandler : IQueryHandler<GetAllRolesQuery, ResponseResult<IEnumerable<SearchRoleDtO>>>
    {
        private readonly IRoleManagementRepository _roleManagementRepository;

        public GetAllRolesQueryHandler(IRoleManagementRepository roleManagementRepository)
        {
            _roleManagementRepository = roleManagementRepository;
        }

        public async Task<ResponseResult<IEnumerable<SearchRoleDtO>>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await _roleManagementRepository.GetAllRolesSearchAsync();

            var mapToSearchRolesDTO = roles?.Adapt<IEnumerable<SearchRoleDtO>>();

            mapToSearchRolesDTO = mapToSearchRolesDTO ?? Enumerable.Empty<SearchRoleDtO>();

            return
               ResponseResult<IEnumerable<SearchRoleDtO>>
              .OK(mapToSearchRolesDTO, string.Empty);
        }
    }
}