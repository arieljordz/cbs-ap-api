using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.PermissionManagement;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Enums;

namespace CbsAp.Application.Features.PermissionManagement.Queries.SearchActions
{
    public class PermissionSearchQueryHandler :
        IQueryHandler<SearchPermissionParamQuery, ResponseResult<PaginatedList<PermissionSearchDTO>>>
    {
        private readonly IPermissionManagementRepository _permissionManagementRepository;

        public PermissionSearchQueryHandler(IPermissionManagementRepository permissionManagementRepository)
        {
            _permissionManagementRepository = permissionManagementRepository;
        }

        public async Task<ResponseResult<PaginatedList<PermissionSearchDTO>>> Handle(SearchPermissionParamQuery request,
            CancellationToken cancellationToken)
        {
            var results =
               await _permissionManagementRepository.GetSearchPermissionAsync(
               request?.PermissionID,
               request?.PermissionName,
               request?.IsActive,
               request.PageNumber,
               request.PageSize,
               request.SortField!,
               request.SortOrder!,
               cancellationToken);

            return results == null ?
                  ResponseResult<PaginatedList<PermissionSearchDTO>>
                  .NotFound(MessageConstants.Message("Permissions", MessageOperationType.NotFound)) :

                  ResponseResult<PaginatedList<PermissionSearchDTO>>
                  .SuccessRetrieveRecords(results);
        }
    }
}