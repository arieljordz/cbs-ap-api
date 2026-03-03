using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.Invoicing.InvRoutingFlow;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Enums;

namespace CbsAp.Application.Invoicing.InvRoutingFlows.Queries.Pagination
{
    public class SearchInvRoutingFlowQueryHandler
        : IQueryHandler<SearchInvRoutingFlowQuery, ResponseResult<PaginatedList<SearchInvRoutingFlowDto>>>
    {
        private readonly IInvRoutingFlowRepository _invRoutingFlowRepository;

        public SearchInvRoutingFlowQueryHandler(IInvRoutingFlowRepository invRoutingFlowRepository)
        {
            _invRoutingFlowRepository = invRoutingFlowRepository;
        }

        public async Task<ResponseResult<PaginatedList<SearchInvRoutingFlowDto>>> Handle(SearchInvRoutingFlowQuery request, CancellationToken cancellationToken)
        {
            var result = await _invRoutingFlowRepository.InvRoutingFlowSearchWithPagination(
                request.EntityName,
                request.InvRoutingFlowName,
                request.LinkSupplier,
                request.Roles,
                request.MatchReference,
                request.pageNumber,
                request.pageSize,
                request.sortField,
                request.sortOrder,
                cancellationToken
                );
            return result == null ?
                ResponseResult<PaginatedList<SearchInvRoutingFlowDto>>
               .NotFound(MessageConstants.Message("Invoice Routing Flow", MessageOperationType.NotFound)) :
               ResponseResult<PaginatedList<SearchInvRoutingFlowDto>>
               .SuccessRetrieveRecords(result);
        }
    }
}
