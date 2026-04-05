using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Invoicing.InvRoutingFlow;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Invoicing.InvRoutingFlows.Queries.Lookups
{
    public record GetRoutingFlowLookUpNewQuery(long? InvRoutingFlowID,
            string? InvRoutingFlowName,
            string? SupplierName,
            bool? IsActive,
            int pageNumber,
            int pageSize,
            string? sortField,
            int? sortOrder)
        : IQuery<ResponseResult<PaginatedList<RoutingFlowLockupDto>>>
    {
    }
}
