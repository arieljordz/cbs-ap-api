using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.Invoicing.InvInfoRoutingLevel;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.DTOs.Invoicing.InvRoutingFlow;
using CbsAp.Application.Features.Invoicing.InvActions.Helpers;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Entities.Keywords;
using CbsAp.Domain.Entities.RoleManagement;
using CbsAp.Domain.Entities.Supplier;
using CbsAp.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Application.Features.Invoicing.InvActions.Command
{
    public class UpdateInvoiceCommandHandler : ICommandHandler<UpdateInvoiceCommand, ResponseResult<bool>>
    {
        private readonly IUnitofWork _unitofWork;

        public UpdateInvoiceCommandHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<ResponseResult<bool>> Handle(UpdateInvoiceCommand request, CancellationToken cancellationToken)
        {
            var dto = request.invoiceDto;

            var result = _unitofWork.GetRepository<Invoice>();
            var invoice = await result
                .Query()
                .Include(i => i.InvoiceAllocationLines)
                .Include(i => i.InvInfoRoutingLevels)
                .FirstOrDefaultAsync(i => i.InvoiceID == dto.InvoiceID, cancellationToken);

            if (invoice == null)
                return ResponseResult<bool>.BadRequest("Invoice is not found");

            // Previous values
            var previousRoutingFlowID = invoice.InvRoutingFlowID;
            var previousKeywordID = invoice.KeywordID;
            var previousSupplierID = invoice.SupplierInfoID;

            bool routingFlowChanged = dto.InvRoutingFlowID != previousRoutingFlowID;
            bool keywordChanged = dto.KeywordID != previousKeywordID;
            bool supplierChanged = dto.SupplierInfoID != previousSupplierID;

            var prevQueue = invoice.QueueType;

            // Update basic fields
            invoice.InvoiceNo = dto.InvoiceNo;
            invoice.InvoiceDate = dto.InvoiceDate;
            invoice.MapID = dto.MapID;
            invoice.ScanDate = dto.ScanDate;
            invoice.EntityProfileID = dto.EntityProfileID;
            invoice.SupplierInfoID = dto.SupplierInfoID;
            invoice.KeywordID = dto.KeywordID;
            invoice.DueDate = dto.DueDate;
            invoice.PoNo = dto.PoNo;
            invoice.GrNo = dto.GrNo;
            invoice.Currency = dto.Currency;
            invoice.NetAmount = dto.NetAmount;
            invoice.TaxAmount = dto.TaxAmount;
            invoice.TotalAmount = dto.TotalAmount;
            invoice.TaxCodeID = dto.TaxCodeID;
            invoice.PaymentTerm = dto.PaymentTerm;
            invoice.Note = dto.Note;

            // Routing Flow logic
            RoutingFlowHelper.ApplyRoutingFlowLogic(_unitofWork, invoice, dto);

            // Invoice Allocation
            AllocationHelper.UpdateInvoiceAllocations(_unitofWork, invoice, dto);

            // Routing Flow Levels
            RoutingFlowHelper.UpdateRoutingFlowLevels(_unitofWork, invoice, dto, prevQueue, routingFlowChanged);

            // Save changes
            invoice.SetAuditFieldsOnUpdate(request.UpdatedBy);

            var flowExists = await _unitofWork.GetRepository<InvRoutingFlow>()
                .Query()
                .AnyAsync(f => f.InvRoutingFlowID == invoice.InvRoutingFlowID, cancellationToken);

            if (!flowExists)
                return ResponseResult<bool>.BadRequest("Invalid routing flow selected");

            var module = invoice.QueueType?.ToString();
            var saved = await _unitofWork.SaveChanges(request.UpdatedBy, module, cancellationToken);

            if (!saved)
                return ResponseResult<bool>.BadRequest("Failed to update Invoice");

            return ResponseResult<bool>.OK("Invoice updated successfully.");

        }
    }

}