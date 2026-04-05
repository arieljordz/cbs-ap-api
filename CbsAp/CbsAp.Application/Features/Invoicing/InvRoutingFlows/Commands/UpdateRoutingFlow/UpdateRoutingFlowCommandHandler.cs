using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Features.Invoicing.InvRoutingFlows;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CbsAp.Application.Invoicing.InvRoutingFlows.Commands.UpdateRoutingFlow
{
    public class UpdateRoutingFlowCommandHandler : ICommandHandler<UpdateRoutingFlowCommand, ResponseResult<bool>>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IInvRoutingFlowRepository _invRoutingFlowRepository;
        private readonly ILogger<UpdateRoutingFlowCommandHandler> _logger;

        public UpdateRoutingFlowCommandHandler(IUnitofWork unitofWork, IInvRoutingFlowRepository invRoutingFlowRepository, ILogger<UpdateRoutingFlowCommandHandler> logger)
        {
            _unitofWork = unitofWork;
            _invRoutingFlowRepository = invRoutingFlowRepository;
            _logger = logger;
        }

        public async Task<ResponseResult<bool>> Handle(UpdateRoutingFlowCommand request, CancellationToken cancellationToken)
        {
            var invRoutingFlowID = request.InvRoutingFlowDto.InvRoutingFlowID;

            if (await _invRoutingFlowRepository.IsInvRoutingFlowExist(
                request.InvRoutingFlowDto.SupplierInfoID,
                request.InvRoutingFlowDto.InvRoutingFlowName!,
                request.InvRoutingFlowDto.InvRoutingFlowID))
            {
                _logger.LogError("Invoice Routing Flow with InvRoutingFlowName: {InvRoutingFlowName} and SupplierInfoID : {SupplierInfoID}:  ",
                       request.InvRoutingFlowDto.InvRoutingFlowName!,
                       request.InvRoutingFlowDto.SupplierInfoID);
                return ResponseResult<bool>.BadRequest("Invoice Routing Flow is already exist");
            }

            var routingFlow = await GetRoutingFlowWithLevelsAsync(invRoutingFlowID, cancellationToken);
            if (routingFlow == null)
            {
                _logger.LogError("Routing flow not found. ID: {RoutingFlowId}", invRoutingFlowID);
                return ResponseResult<bool>.NotFound("Invoice Routing Flow not found.");
            }

            if (await IsInvoiceRoutingFlowActive(invRoutingFlowID))
            {
                _logger.LogError("Invoice Routing Flow with : {InvRoutingFlowID} is active in the system", invRoutingFlowID);
                return ResponseResult<bool>.BadRequest("This routing flow is currently in use and cannot be edited.");
            }

            // -- Updating records --
            ApplyUpdates(routingFlow, request);
     
            var saveResult = await _unitofWork.SaveChanges(string.Empty,string.Empty,cancellationToken);
            if (!saveResult)
            {
                _logger.LogError("Failed to update routing flow. InvRoutingFlowID: {InvroutingFlowID}", invRoutingFlowID);
                return ResponseResult<bool>.BadRequest("Failed to update Invoice Routing Flow.");
            }

            return ResponseResult<bool>.OK("Invoice Routing Flow updated successfully.");
        }

        private async Task<InvRoutingFlow?> GetRoutingFlowWithLevelsAsync(long invRoutingFlowID, CancellationToken cancellationToken)
        {
            return await _unitofWork.GetRepository<InvRoutingFlow>()
                .Query()
                .Include(rf => rf.Levels)
                .SingleOrDefaultAsync(rf => rf.InvRoutingFlowID == invRoutingFlowID, cancellationToken);
        }

        private async Task<bool> IsInvoiceRoutingFlowActive(long invRoutingFlowID)
        {
            return await _unitofWork.GetRepository<Invoice>()
               .AnyAsync(e => e.InvRoutingFlowID == invRoutingFlowID);
        }

        private static void ApplyUpdates(InvRoutingFlow routingFlow, UpdateRoutingFlowCommand request)
        {
            InvRoutingFlowFactory.Update(routingFlow, request.InvRoutingFlowDto);
            routingFlow.SetAuditFieldsOnUpdate(request.UpdateBy);
        }
    }
}