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



namespace CbsAp.Application.Features.Invoicing.InvRoutingFlows.Commands.AssignRoleRoutingFlow
{
    public class AssignRoleRoutingFlowCommandHandler : ICommandHandler<AssignRoleRoutingFlowCommand, ResponseResult<string>>
    {
        private readonly IUnitofWork _unitOfWork;



        private readonly ILogger<AssignRoleRoutingFlowCommandHandler> _logger;



        public AssignRoleRoutingFlowCommandHandler(IUnitofWork unitofWork, ILogger<AssignRoleRoutingFlowCommandHandler> logger)
        {
            _unitOfWork = unitofWork;
            _logger = logger;
        }



        public async Task<ResponseResult<string>> Handle(AssignRoleRoutingFlowCommand request, CancellationToken cancellationToken)
        {
            var dto = request.RoleRoutingFlowDTO;



            try
            {
                var invoiceRepo = _unitOfWork.GetRepository<Invoice>();
                var routingRepo = _unitOfWork.GetRepository<InvInfoRoutingLevel>();



                var invoice = await invoiceRepo.Query()
                .FirstOrDefaultAsync(x => x.InvoiceID == dto.InvoiceID, cancellationToken);



                if (invoice == null)
                    return ResponseResult<string>.BadRequest("Invoice not found.");



                if (!invoice.InvRoutingFlowID.HasValue)
                    return ResponseResult<string>.BadRequest("Invoice has no routing flow.");



                var exists = await routingRepo.Query()
                .AnyAsync(x => x.InvoiceID == dto.InvoiceID && x.RoleID == dto.RoleID, cancellationToken);



                if (exists)
                    return ResponseResult<string>.BadRequest("Role already assigned to this invoice.");



                var maxLevel = await routingRepo.Query()
                .Where(x => x.InvoiceID == dto.InvoiceID)
                .MaxAsync(x => (int?)x.Level, cancellationToken) ?? 0;



                var insertLevel = dto.Level.HasValue && dto.Level.Value > 0
                ? Math.Min(dto.Level.Value, maxLevel + 1)
                : maxLevel + 1;



                if (insertLevel <= maxLevel)
                {
                    var levelsToShift = await routingRepo.Query()
                    .Where(x => x.InvoiceID == dto.InvoiceID && x.Level >= insertLevel)
                    .ToListAsync(cancellationToken);



                    foreach (var lvl in levelsToShift)
                    {
                        lvl.Level += 1;
                    }
                }



                var newRouting = new InvInfoRoutingLevel
                {
                    InvoiceID = dto.InvoiceID,
                    InvRoutingFlowID = invoice.InvRoutingFlowID.Value,
                    RoleID = dto.RoleID,
                    Level = (int)insertLevel,
                    KeywordID = invoice.KeywordID,
                    SupplierInfoID = invoice.SupplierInfoID,
                    InvFlowStatus = (int?)InvFlowStatus.Pending
                };



                newRouting.SetAuditFieldsOnCreate(request.assignedBy);



                await routingRepo.AddAsync(newRouting);



                invoice.SetAuditFieldsOnUpdate(request.assignedBy);



                var saveResult = await _unitOfWork.SaveChanges(
                request.assignedBy,
                request.assignedBy,
                cancellationToken
                );



                if (saveResult)
                    return ResponseResult<string>.Success("Role assigned successfully.");



                return ResponseResult<string>.BadRequest("Failed to save role assignment.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                "Error assigning role for InvoiceID: {InvoiceID}",
                dto.InvoiceID);



                return ResponseResult<string>.BadRequest("Error assigning role.");
            }
        }



    }
}