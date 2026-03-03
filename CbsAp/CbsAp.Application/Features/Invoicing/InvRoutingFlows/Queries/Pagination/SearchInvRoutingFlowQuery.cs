using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Invoicing.InvRoutingFlow;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Invoicing.InvRoutingFlows.Queries.Pagination
{
    public record SearchInvRoutingFlowQuery(
            string? EntityName,
            string? InvRoutingFlowName,
            string? LinkSupplier,
            string? MatchReference,
            string? Roles,
            int pageNumber,
            int pageSize,
            string? sortField,
            int? sortOrder) : IQuery<ResponseResult<PaginatedList<SearchInvRoutingFlowDto>>>
    {
    }
}
