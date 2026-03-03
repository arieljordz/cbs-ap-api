using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.RolesManagement;
using CbsAp.Application.Shared.ResultPatten;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Application.Features.Roles.Queries.SearchHandler
{
    public class GetRolePermissionsByRoleIdHandler : IQueryHandler<GetRolePermissionsByRoleId, ResponseResult<ICollection<RolePermissionDto>>>
    {
        private readonly IRoleManagementRepository _roleManagementRepository;

        public GetRolePermissionsByRoleIdHandler(IRoleManagementRepository roleManagementRepository)
        {
            _roleManagementRepository = roleManagementRepository;
        }

        public async Task<ResponseResult<ICollection<RolePermissionDto>>> Handle(GetRolePermissionsByRoleId request, CancellationToken cancellationToken)
        {
            var rolePermissions = await _roleManagementRepository.GetRoleAsQueryable()
                 .Where(r => r.RoleID == request.RoleID)
                 .SelectMany(r => r.RolePermissionGroups)
                 .Select(rpg => new RolePermissionDto
                 {
                     PermissionName = rpg.Permission.PermissionName,
                     PermissionID = rpg.Permission.PermissionID,
                 })
                 .ToListAsync(cancellationToken);
            return ResponseResult<ICollection<RolePermissionDto>>.SuccessRetrieveRecords(rolePermissions, "Role Permissions");
        }
    }
}
