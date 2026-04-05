using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.Features.Invoicing.InvRoutingFlows;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Enums;

namespace CbsAp.Application.Invoicing.InvRoutingFlows.Commands.CreateRoutingFlow
{
    public class CreateRoutingFlowCommandHandler : ICommandHandler<CreateRoutingFlowCommand, ResponseResult<bool>>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IInvRoutingFlowRepository _invRoutingFlowRepository;

        public CreateRoutingFlowCommandHandler(IUnitofWork unitofWork, IInvRoutingFlowRepository invRoutingFlowRepository
           )
        {
            _unitofWork = unitofWork;
            _invRoutingFlowRepository = invRoutingFlowRepository;
        }

        public async Task<ResponseResult<bool>> Handle(CreateRoutingFlowCommand request, CancellationToken cancellationToken)
        {
            var invRoutingFlow = InvRoutingFlowFactory.Create(request.InvRoutingFlowDto);
            invRoutingFlow.SetAuditFieldsOnCreate(request.createdBy);

            if (await _invRoutingFlowRepository.IsInvRoutingFlowExist(
                request.InvRoutingFlowDto.SupplierInfoID,
                request.InvRoutingFlowDto.InvRoutingFlowName!))
            {
                return ResponseResult<bool>.BadRequest("Invoice Routing Flow  is already exist");
            }
            if (!await CreateInvRoutingFlow(invRoutingFlow, cancellationToken))
            {
                return ResponseResult<bool>.BadRequest("Error adding new invoice routing flow");
            }
            return ResponseResult<bool>.Created(MessageConstants.Message(MessageOperationType.Create, "invoice routing flow"));
        }

        private async Task<bool> CreateInvRoutingFlow(InvRoutingFlow invRoutingFlow, CancellationToken cancellationToken)
        {
            await _unitofWork.GetRepository<InvRoutingFlow>()
                .AddAsync(invRoutingFlow);
            return await _unitofWork.SaveChanges(string.Empty, string.Empty, cancellationToken);
        }
    }
}