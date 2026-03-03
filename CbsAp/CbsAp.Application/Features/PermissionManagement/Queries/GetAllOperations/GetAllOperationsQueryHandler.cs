using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.PermissionManagement.OperationDTO;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.PermissionManagement;
using CbsAp.Domain.Enums;
using Mapster;

namespace CbsAp.Application.Features.PermissionManagement.Queries.GetAllOperations
{
    public class GetAllOperationsQueryHandler :
        IQueryHandler<GetAllOperationQuery,
            ResponseResult<IQueryable<GroupPanelDTO>>>
    {
        private readonly IUnitofWork _unitofWork;

        public GetAllOperationsQueryHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<ResponseResult<IQueryable<GroupPanelDTO>>>
            Handle(GetAllOperationQuery request,
            CancellationToken cancellationToken)
        {
            var operationLists = await _unitofWork.GetRepository<Operation>()
                .GetAllAsync();
            var operationDtos = operationLists.ProjectToType<OperationsDTO>();

            var groupbyPanel =
                operationDtos.GroupBy(o => o.Panel)
                .Select(grp =>
                new GroupPanelDTO { Panel = grp.Key, Operations = grp.ToList() });

            return !operationDtos.Any() ?
                ResponseResult<IQueryable<GroupPanelDTO>>
                .NotFound(MessageConstants.Message("Operations", MessageOperationType.NotFound)) :

                ResponseResult<IQueryable<GroupPanelDTO>>
                .SuccessRetrieveRecords(groupbyPanel, "Retieve Operations Successfully");
        }
    }
}