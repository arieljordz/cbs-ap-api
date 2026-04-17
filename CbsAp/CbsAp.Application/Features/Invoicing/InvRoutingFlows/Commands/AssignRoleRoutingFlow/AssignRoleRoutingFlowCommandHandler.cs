using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Invoicing.InvRoutingFlows.Commands.UpdateRoutingFlow;
using CbsAp.Application.Shared.ResultPatten;
using Microsoft.Extensions.Logging;

namespace CbsAp.Application.Features.Invoicing.InvRoutingFlows.Commands.AssignRoleRoutingFlow
{
    public class AssignRoleRoutingFlowCommandHandler : ICommandHandler<AssignRoleRoutingFlowCommand, ResponseResult<bool>>
    {
        private readonly IUnitofWork _unitOfWork;
        private readonly IInvRoutingFlowRepository _invRoutingFlowRepository;
        private readonly ILogger<AssignRoleRoutingFlowCommandHandler> _logger;

        public AssignRoleRoutingFlowCommandHandler(IUnitofWork unitofWork, IInvRoutingFlowRepository invRoutingFlowRepository, ILogger<AssignRoleRoutingFlowCommandHandler> logger)
        {
            _unitOfWork = unitofWork;
            _invRoutingFlowRepository = invRoutingFlowRepository;
            _logger = logger;
        }

        public async Task<ResponseResult<bool>> Handle(AssignRoleRoutingFlowCommand request, CancellationToken cancellationToken)
        {
            var dto = request.RoleRoutingFlowDTO;

            try
            {
                var success = await _invRoutingFlowRepository.AssignRoleRoutingFlowAsync(
                    dto.InvoiceID,
                    dto.RoleID,
                    (int?)dto.Level,
                    request.assignedBy,
                    cancellationToken
                );

                if (!success)
                    return ResponseResult<bool>.BadRequest("Failed to assign role. It may already exist or invoice is invalid.");

                var saveResult = await _unitOfWork.SaveChanges(
                    request.assignedBy,
                    request.assignedBy,
                    cancellationToken
                );

                return saveResult
                    ? ResponseResult<bool>.Success(true)
                    : ResponseResult<bool>.BadRequest("Failed to save role assignment.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error assigning role for InvoiceID: {InvoiceID}",
                    dto.InvoiceID);

                return ResponseResult<bool>.BadRequest("Error assigning role.");
            }
        }

    }
}