using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.Invoicing.InvRoutingFlow;
using CbsAp.Application.Invoicing.InvRoutingFlows.Queries.Pagination;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Enums;

namespace CbsAp.Application.Features.Invoicing.InvRoutingFlows.Queries.Pagination
{
    public class SearchInvRoutingFlowByRolesQueryHandler
        : IQueryHandler<SearchInvRoutingFlowByRolesQuery, ResponseResult<PaginatedList<SearchInvRoutingFlowRolesDto>>>
    {
        private readonly IInvRoutingFlowRepository _invRoutingFlowRepository;

        public SearchInvRoutingFlowByRolesQueryHandler(IInvRoutingFlowRepository invRoutingFlowRepository)
        {
            _invRoutingFlowRepository = invRoutingFlowRepository;
        }

        public async Task<ResponseResult<PaginatedList<SearchInvRoutingFlowRolesDto>>> Handle(SearchInvRoutingFlowByRolesQuery request, CancellationToken cancellationToken)
        {
            var result = await _invRoutingFlowRepository.InvRoutingFlowSearchWithRolesPagination(
                request.InvRoutingFlowID,
                 request.pageNumber,
                 request.pageSize,
                 request.sortField,
                 request.sortOrder,
                 cancellationToken
                 );
            return result == null ?
                ResponseResult<PaginatedList<SearchInvRoutingFlowRolesDto>>
               .NotFound(MessageConstants.Message("Invoice Routing Flow", MessageOperationType.NotFound)) :
               ResponseResult<PaginatedList<SearchInvRoutingFlowRolesDto>>
               .SuccessRetrieveRecords(result);
        }
    }
}
