using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.RolesManagement;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Roles.Queries.SearchHandler
{
    public class GetActiveRolesQueryHandler : IQueryHandler<GetActiveRolesQuery, ResponseResult<IEnumerable<RoleManagerDTO>>>
    {
        private readonly IRoleManagementRepository _roleManagementRepository;

        public GetActiveRolesQueryHandler(IRoleManagementRepository roleManagementRepository)
        {
            _roleManagementRepository = roleManagementRepository;
        }

        public async Task<ResponseResult<IEnumerable<RoleManagerDTO>>> Handle(GetActiveRolesQuery request, CancellationToken cancellationToken)
        {
            var results = await _roleManagementRepository
                .GetActiveRoleAsync(request.RoleName, request.FirstName, request.LastName) ?? Enumerable.Empty<RoleManagerDTO>();

            return ResponseResult<IEnumerable<RoleManagerDTO>>.OK(results);
        }
    }
}