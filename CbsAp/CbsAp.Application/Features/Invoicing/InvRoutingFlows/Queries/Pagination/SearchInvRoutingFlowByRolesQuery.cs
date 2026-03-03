using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Invoicing.InvRoutingFlow;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Application.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.Invoicing.InvRoutingFlows.Queries.Pagination
{
    public record SearchInvRoutingFlowByRolesQuery(long? InvRoutingFlowID,
            int pageNumber,
            int pageSize,
            string? sortField,
            int? sortOrder) : IQuery<ResponseResult<PaginatedList<SearchInvRoutingFlowRolesDto>>>
    {
    }
}
