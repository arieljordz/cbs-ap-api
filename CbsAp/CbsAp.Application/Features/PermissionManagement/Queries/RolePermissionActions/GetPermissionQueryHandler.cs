using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.PermissionManagement;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.PermissionManagement;
using CbsAp.Domain.Enums;
using Mapster;

namespace CbsAp.Application.Features.PermissionManagement.Queries.RolePermissionActions
{
    public class GetPermissionQueryHandler :
        IQueryHandler<GetPermissionQuery, ResponseResult<IQueryable<PermissionDetailDTO>>>
    {
        private readonly IUnitofWork _unitofWork;

        public GetPermissionQueryHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<ResponseResult<IQueryable<PermissionDetailDTO>>> Handle(
            GetPermissionQuery request,
            CancellationToken cancellationToken)
        {
            var permissionlists = await _unitofWork
                .GetRepository<Permission>()
                .ApplyPredicateAsync(p => p.IsActive);

            var permissionListDTO = permissionlists
                .ProjectToType<PermissionDetailDTO>();

            return !permissionListDTO.Any()
                ? ResponseResult<IQueryable<PermissionDetailDTO>>
                     .NotFound(
                        MessageConstants.Message("Permission", MessageOperationType.NotFound)
                    )
                : ResponseResult<IQueryable<PermissionDetailDTO>>
                    .SuccessRetrieveRecords(permissionListDTO, "Permission");
        }
    }
}