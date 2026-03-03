using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.Invoicing.InvRoutingFlow;
using CbsAp.Application.Invoicing.InvRoutingFlows.Queries.GetInvRoutingFlowByID;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Invoicing.InvRoutingFlows.Queries.GetInvRoutingFlowByID
{
    public class GetInvRoutingFlowByIDQueryHandler
        : IQueryHandler<GetInvRoutingFlowByIDQuery, ResponseResult<InvRoutingFlowDto>>
    {
        private readonly IInvRoutingFlowRepository _invRoutingFlowRepository;

        public GetInvRoutingFlowByIDQueryHandler(IInvRoutingFlowRepository invRoutingFlowRepository)
        {
            _invRoutingFlowRepository = invRoutingFlowRepository;
        }

        public async Task<ResponseResult<InvRoutingFlowDto>> Handle(GetInvRoutingFlowByIDQuery request, CancellationToken cancellationToken)
        {
            var result = await _invRoutingFlowRepository.GetInvRoutingFlowByIdAsync(request.InvRoutingFlowID, cancellationToken);

            var dto = InvRoutingFlowFactory.GetInvRoutingFlowDto(result!);

            return dto == null ?
                 ResponseResult<InvRoutingFlowDto>.BadRequest("Invoice Routing Flow not found") :
                ResponseResult<InvRoutingFlowDto>.OK(dto);
        }
    }
}