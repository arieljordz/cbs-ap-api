using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.Invoicing.InvRoutingFlow;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Enums;

namespace CbsAp.Application.Invoicing.InvRoutingFlows.Queries.Pagination
{
    public class SearchInvRoutingFlowByUserQueryHandler
        : IQueryHandler<SearchInvRoutingFlowByUserQuery, ResponseResult<PaginatedList<SearchInvRoutingFlowUserDto>>>
    {
        private readonly IInvRoutingFlowRepository _invRoutingFlowRepository;

        public SearchInvRoutingFlowByUserQueryHandler(IInvRoutingFlowRepository invRoutingFlowRepository)
        {
            _invRoutingFlowRepository = invRoutingFlowRepository;
        }

        public async Task<ResponseResult<PaginatedList<SearchInvRoutingFlowUserDto>>> Handle(SearchInvRoutingFlowByUserQuery request, CancellationToken cancellationToken)
        {
            var result = await _invRoutingFlowRepository.InvRoutingFlowSearchWithUsersPagination(
                request.InvRoutingFlowID,
                 request.pageNumber,
                 request.pageSize,
                 request.sortField,
                 request.sortOrder,
                 cancellationToken
                 );
            return result == null ?
                ResponseResult<PaginatedList<SearchInvRoutingFlowUserDto>>
               .NotFound(MessageConstants.Message("Invoice Routing Flow", MessageOperationType.NotFound)) :
               ResponseResult<PaginatedList<SearchInvRoutingFlowUserDto>>
               .SuccessRetrieveRecords(result);
        }
    }
}
