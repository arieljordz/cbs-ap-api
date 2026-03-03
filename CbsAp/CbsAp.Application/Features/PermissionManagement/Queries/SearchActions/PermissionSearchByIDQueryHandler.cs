using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.PermissionManagement;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Enums;

namespace CbsAp.Application.Features.PermissionManagement.Queries.SearchActions
{
    public class PermissionSearchByIDQueryHandler :
        IQueryHandler<SearchPermissionByIDQuery, ResponseResult<IQueryable<PermissionSearchByIdDTO>>>
    {
        private readonly IPermissionManagementRepository _permissionManagementRepository;

        public PermissionSearchByIDQueryHandler(IPermissionManagementRepository permissionManagementRepository)
        {
            _permissionManagementRepository = permissionManagementRepository;
        }

        public async Task<ResponseResult<IQueryable<PermissionSearchByIdDTO>>> Handle(SearchPermissionByIDQuery request, CancellationToken cancellationToken)
        {
            var results = await _permissionManagementRepository
                .GetSearchPermissionIDAsync(request.PermissionID);

            return !results.Any() ?
                ResponseResult<IQueryable<PermissionSearchByIdDTO>>
                    .NotFound(MessageConstants.Message("Permission Id not found", MessageOperationType.NotFound)) :
                ResponseResult<IQueryable<PermissionSearchByIdDTO>>
                .SuccessRetrieveRecords(results, "Permission found");
        }
    }
}