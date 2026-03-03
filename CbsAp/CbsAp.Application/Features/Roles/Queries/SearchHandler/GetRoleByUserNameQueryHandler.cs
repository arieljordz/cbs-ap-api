using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.RolesManagement;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Roles.Queries.SearchHandler
{
    public class GetRoleByUserNameQueryHandler : IQueryHandler<GetRoleByUserNameQuery, ResponseResult<IEnumerable<RoleDTO>>>
    {
        private readonly IRoleManagementRepository _roleManagementRepository;

        public GetRoleByUserNameQueryHandler(IRoleManagementRepository roleManagementRepository)
        {
            _roleManagementRepository = roleManagementRepository;
        }

        public async Task<ResponseResult<IEnumerable<RoleDTO>>> Handle(GetRoleByUserNameQuery request, CancellationToken cancellationToken)
        {
            var results = await _roleManagementRepository
               .GetActiveRoleByUserAsync(request.UserName) ?? Enumerable.Empty<RoleDTO>();

            return ResponseResult<IEnumerable<RoleDTO>>.OK(results);
        }
    }
}