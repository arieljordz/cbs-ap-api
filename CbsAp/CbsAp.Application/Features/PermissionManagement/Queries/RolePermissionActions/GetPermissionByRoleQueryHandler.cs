using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.PermissionManagement;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.PermissionManagement.Queries.RolePermissionActions
{
    public class GetPermissionByRoleQueryHandler
        : IQueryHandler<GetPermissionByRoleQuery, ResponseResult<IQueryable<PermissionDetailDTO>>>
    {
        private readonly IPermissionManagementRepository _permissionManagementRepository;

        public GetPermissionByRoleQueryHandler(IPermissionManagementRepository permissionMgmtRepository)
        {
            _permissionManagementRepository = permissionMgmtRepository;
        }

        public async Task<ResponseResult<IQueryable<PermissionDetailDTO>>> Handle(GetPermissionByRoleQuery request, CancellationToken cancellationToken)
        {
            var results = await _permissionManagementRepository
                 .GetPermissionByRoleAsync(request.roleID);

            return ResponseResult<IQueryable<PermissionDetailDTO>>.OK(results);
        }
    }
}