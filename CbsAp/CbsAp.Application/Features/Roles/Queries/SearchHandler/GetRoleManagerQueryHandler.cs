using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.RolesManagement;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Roles.Queries.SearchHandler
{
    public class GetRoleManagerQueryHandler :
        IQueryHandler<GetRoleManagerQuery, ResponseResult<IEnumerable<RoleManagerDTO>>>
    {
        private readonly IRoleManagementRepository _roleManagementRepository;

        public GetRoleManagerQueryHandler(IRoleManagementRepository roleManagementRepository)
        {
            _roleManagementRepository = roleManagementRepository;
        }

        public async Task<ResponseResult<IEnumerable<RoleManagerDTO>>> Handle(
            GetRoleManagerQuery request,
            CancellationToken cancellationToken)
        {
            var results = await _roleManagementRepository
                .GetActiveRoleManagerAsync(request.Name, request.UserID) ?? Enumerable.Empty<RoleManagerDTO>();

            return ResponseResult<IEnumerable<RoleManagerDTO>>.OK(results);
        }
    }
}