using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.PermissionManagement;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.PermissionManagement.Queries.RolePermissionActions
{
    public class GetPermissionNotInRoleQueryHandler
        : IQueryHandler<GetPermissionNotInRoleQuery, ResponseResult<IQueryable<PermissionDetailDTO>>>
    {
        private readonly IPermissionManagementRepository _permissionManagementRepository;

        public GetPermissionNotInRoleQueryHandler(IPermissionManagementRepository permissionManagementRepository)
        {
            _permissionManagementRepository = permissionManagementRepository;
        }

        public async Task<ResponseResult<IQueryable<PermissionDetailDTO>>> Handle(GetPermissionNotInRoleQuery request, CancellationToken cancellationToken)
        {
            var results = await _permissionManagementRepository
                .GetPermissionNotInRoleAsync(request.RoleID);

            return ResponseResult<IQueryable<PermissionDetailDTO>>.OK(results);
        }
    }
}