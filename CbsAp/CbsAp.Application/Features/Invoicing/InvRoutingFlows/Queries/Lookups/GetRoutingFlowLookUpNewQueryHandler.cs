using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Abstractions.Services.Invoicing;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.Invoicing.Accounts;
using CbsAp.Application.DTOs.Invoicing.InvRoutingFlow;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Entities.Supplier;
using CbsAp.Domain.Enums;

namespace CbsAp.Application.Features.Invoicing.InvRoutingFlows.Queries.Lookups
{
    public class GetRoutingFlowLookUpNewQueryHandler
        : IQueryHandler<GetRoutingFlowLookUpNewQuery, ResponseResult<PaginatedList<RoutingFlowLockupDto>>>
    {
        private readonly IUnitofWork _unitofWork;

        public GetRoutingFlowLookUpNewQueryHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<ResponseResult<PaginatedList<RoutingFlowLockupDto>>> Handle(GetRoutingFlowLookUpNewQuery request, CancellationToken cancellationToken)
        {
            var routingFlowRepo = _unitofWork.GetRepository<InvRoutingFlow>();
            var supplierRepo = _unitofWork.GetRepository<SupplierInfo>();

            var routingFlow = await routingFlowRepo.GetAllAsync();
            var supplier = await supplierRepo.GetAllAsync();


            var filteredRoutingFlow = routingFlow
                .Where(rf =>
                    (request.InvRoutingFlowID == null || rf.InvRoutingFlowID == request.InvRoutingFlowID) &&
                    (string.IsNullOrWhiteSpace(request.InvRoutingFlowName) ||
                        rf.InvRoutingFlowName.Contains(request.InvRoutingFlowName)) &&
                    (request.IsActive == null || rf.IsActive == request.IsActive)
                )
                .ToList();

            var filteredSupplier = supplier
                .Where(s =>
                    (string.IsNullOrWhiteSpace(request.SupplierName) ||
                        s.SupplierName.Contains(request.SupplierName))
                )
                .ToList();


            var results = filteredRoutingFlow
                .GroupJoin(
                    filteredSupplier,
                    rf => rf.SupplierInfoID,
                    s => s.SupplierInfoID,
                    (rf, supplierGroup) => new { rf, supplierGroup }
                )
                .SelectMany(
                    x => x.supplierGroup.DefaultIfEmpty(),
                    (x, s) => new RoutingFlowLockupDto
                    {
                        InvRoutingFlowID = x.rf.InvRoutingFlowID,
                        InvRoutingFlowName = x.rf.InvRoutingFlowName,
                        SupplierName = s?.SupplierName,  
                        IsActive = x.rf.IsActive
                    }
                )
                .ToList();



            results = results.Where(x =>
                string.IsNullOrWhiteSpace(request.SupplierName) ||
                (x.SupplierName != null &&
                 x.SupplierName.IndexOf(request.SupplierName, StringComparison.OrdinalIgnoreCase) >= 0)
            ).ToList();



            var resultPagination = await results.OrderByDynamic(request.sortField, request.sortOrder)
                .ToPaginatedListAsync(request.pageNumber, request.pageSize, cancellationToken);


            return resultPagination == null ?

          ResponseResult<PaginatedList<RoutingFlowLockupDto>>
             .NotFound(MessageConstants.Message("Routing Flow Search", MessageOperationType.NotFound)) :
             ResponseResult<PaginatedList<RoutingFlowLockupDto>>
             .SuccessRetrieveRecords(resultPagination);
        }
    }
}
