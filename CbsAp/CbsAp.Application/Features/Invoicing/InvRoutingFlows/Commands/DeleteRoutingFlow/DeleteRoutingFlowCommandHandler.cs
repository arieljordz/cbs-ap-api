using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace CbsAp.Application.Invoicing.InvRoutingFlows.Commands.DeleteRoutingFlow
{
    public class DeleteRoutingFlowCommandHandler : ICommandHandler<DeleteRoutingFlowCommand, ResponseResult<bool>>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly ILogger<DeleteRoutingFlowCommandHandler> _logger;

        public DeleteRoutingFlowCommandHandler(IUnitofWork unitofWork, ILogger<DeleteRoutingFlowCommandHandler> logger)
        {
            _unitofWork = unitofWork;
            _logger = logger;
        }

        public async Task<ResponseResult<bool>> Handle(DeleteRoutingFlowCommand request, CancellationToken cancellationToken)
        {
            if (!await IsInvroutingFlowExist(request.InvRoutingFlowID))
            {
                _logger.LogError("Invoice Routing Flow with : {InvRoutingFlowID} is not existed", request.InvRoutingFlowID);
                return ResponseResult<bool>.BadRequest("Invoice Routing Flow is not existed");
            }

            if (!await DeleteRoutingFlow(request.InvRoutingFlowID, cancellationToken))
            {
                _logger.LogError("Error in deleting Invoice Routing Flow : {InvRoutingFlowID}", request.InvRoutingFlowID);
                return ResponseResult<bool>.BadRequest("Error on Deleting Invoice Routing Flow ");
            }
            return ResponseResult<bool>.OK(MessageConstants.Message("Invoice Routing Flow ", MessageOperationType.Delete));
        }

        private async Task<bool> IsInvroutingFlowExist(long invRoutingFlowID)
        {
            return await _unitofWork.GetRepository<InvRoutingFlow>()
               .AnyAsync(e => e.InvRoutingFlowID == invRoutingFlowID);
        }

        private async Task<bool> DeleteRoutingFlow(long invRoutingFlowID, CancellationToken cancellationToken)
        {
            var invRoutingFlow = await _unitofWork.GetRepository<InvRoutingFlow>()
                .SingleOrDefaultAsync(i => i.InvRoutingFlowID == invRoutingFlowID);

            if (invRoutingFlow == null)
                return false;
            await _unitofWork.GetRepository<InvRoutingFlow>().DeleteAsync(invRoutingFlow);

            return await _unitofWork.SaveChanges(cancellationToken);
        }
    }
}