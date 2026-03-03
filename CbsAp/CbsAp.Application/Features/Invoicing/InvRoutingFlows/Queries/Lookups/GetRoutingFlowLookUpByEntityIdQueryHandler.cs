using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Abstractions.Services.Invoicing;
using CbsAp.Application.DTOs.Invoicing.InvRoutingFlow;
using CbsAp.Application.DTOs.Keyword;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Entities.Keywords;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;

namespace CbsAp.Application.Features.Invoicing.InvRoutingFlows.Queries.Lookups
{
    public class GetRoutingFlowLookUpQueryByEntityIdHandler
        : IQueryHandler<GetRoutingFlowLookUpByEntityIdQuery, ResponseResult<IEnumerable<InvRoutingFlowLookupDto>>>
    {
        private readonly IUnitofWork _unitOfWork;

        public GetRoutingFlowLookUpQueryByEntityIdHandler(IUnitofWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseResult<IEnumerable<InvRoutingFlowLookupDto>>> Handle(GetRoutingFlowLookUpByEntityIdQuery request, CancellationToken cancellationToken)
        {
            var respository = _unitOfWork.GetRepository<InvRoutingFlow>();

            var invFlowQuery = respository.Query()
                .AsNoTracking()
                .WhereIf(request.EntityID>0, invRoutingFlow => invRoutingFlow.EntityProfileID == request.EntityID);

            var invRoutingFLows = await invFlowQuery.Select(x => new InvRoutingFlowLookupDto
            {
                InvRoutingFlowID = x.InvRoutingFlowID,
                InvRoutingFlowName = x.InvRoutingFlowName ?? ""
            }).ToListAsync(cancellationToken);

            return ResponseResult<IEnumerable<InvRoutingFlowLookupDto>>.SuccessRetrieveRecords(invRoutingFLows);
        }
    }
}
