using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
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
    public class RemoveRoleRoutingFlowCommandHandler : ICommandHandler<RemoveRoleRoutingFlowCommand, ResponseResult<string>>
    {
        private readonly IUnitofWork _unitOfWork;



        private readonly ILogger<RemoveRoleRoutingFlowCommandHandler> _logger;



        public RemoveRoleRoutingFlowCommandHandler(IUnitofWork unitofWork, ILogger<RemoveRoleRoutingFlowCommandHandler> logger)
        {
            _unitOfWork = unitofWork;
            _logger = logger;
        }



        public async Task<ResponseResult<string>> Handle(RemoveRoleRoutingFlowCommand request, CancellationToken cancellationToken)
        {
            var dto = request.RoleRoutingFlowDTO;



            try
            {
                var routingRepo = _unitOfWork.GetRepository<InvInfoRoutingLevel>();



                var routing = await routingRepo.Query()
                .FirstOrDefaultAsync(x =>
                x.InvoiceID == dto.InvoiceID &&
                x.RoleID == dto.RoleID &&
                x.Level == dto.Level,
                cancellationToken);



                if (routing == null)
                    return ResponseResult<string>.BadRequest("Routing level not found.");



                await routingRepo.DeleteAsync(routing);



                var levelsToUpdate = await routingRepo.Query()
                .Where(x =>
                x.InvoiceID == dto.InvoiceID &&
                x.Level > dto.Level)
                .ToListAsync(cancellationToken);



                foreach (var level in levelsToUpdate)
                {
                    level.Level -= 1;
                    level.SetAuditFieldsOnUpdate(request.removedBy);
                }



                var saveResult = await _unitOfWork.SaveChanges(
                request.removedBy,
                request.removedBy,
                cancellationToken
                );



                if (saveResult)
                    return ResponseResult<string>.Success("Role removed successfully.");



                return ResponseResult<string>.BadRequest("Failed to remove role.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                "Error removing role for InvoiceID: {InvoiceID}",
                dto.InvoiceID);



                return ResponseResult<string>.BadRequest("Error removing role.");
            }
        }



    }
}