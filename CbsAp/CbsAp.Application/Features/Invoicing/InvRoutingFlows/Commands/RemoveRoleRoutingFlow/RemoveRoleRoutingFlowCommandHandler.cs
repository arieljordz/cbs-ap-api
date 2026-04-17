using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.Features.Invoicing.InvRoutingFlows.Commands.AssignRoleRoutingFlow;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Entities.PermissionManagement;
using CbsAp.Domain.Entities.RoleManagement;
using CbsAp.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CbsAp.Application.Features.Invoicing.InvRoutingFlows.Commands.RemoveRoleRoutingFlow
{
    public class RemoveRoleRoutingFlowCommandHandler : ICommandHandler<RemoveRoleRoutingFlowCommand, ResponseResult<bool>>
    {
        private readonly IUnitofWork _unitOfWork;
        private readonly IInvRoutingFlowRepository _invRoutingFlowRepository;
        private readonly ILogger<RemoveRoleRoutingFlowCommandHandler> _logger;

        public RemoveRoleRoutingFlowCommandHandler(IUnitofWork unitofWork, IInvRoutingFlowRepository invRoutingFlowRepository, ILogger<RemoveRoleRoutingFlowCommandHandler> logger)
        {
            _unitOfWork = unitofWork;
            _invRoutingFlowRepository = invRoutingFlowRepository;
            _logger = logger;
        }

        public async Task<ResponseResult<bool>> Handle(RemoveRoleRoutingFlowCommand request, CancellationToken cancellationToken)
        {
            var dto = request.RoleRoutingFlowDTO;

            try
            {
                var success = await _invRoutingFlowRepository.RemoveRoleRoutingFlowAsync(
                    dto.InvoiceID,
                    dto.RoleID,
                    (int?)dto.Level,
                    request.removedBy,
                    cancellationToken
                );

                if (!success)
                    return ResponseResult<bool>.BadRequest("Routing level not found.");

                var saveResult = await _unitOfWork.SaveChanges(
                    request.removedBy,
                    request.removedBy,
                    cancellationToken
                );

                return saveResult
                    ? ResponseResult<bool>.Success(true)
                    : ResponseResult<bool>.BadRequest("Failed to remove role.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error removing role for InvoiceID: {InvoiceID}",
                    dto.InvoiceID);

                return ResponseResult<bool>.BadRequest("Error removing role.");
            }
        }

    }
}