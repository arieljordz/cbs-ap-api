using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Entities.RoleManagement;
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
                .FirstOrDefaultAsync(i => i.InvoiceID == dto.InvoiceID);
            if (invoice == null)
                return ResponseResult<bool>.BadRequest("Invoice is not found");

            invoice.InvoiceID = dto.InvoiceID;
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

            // invoice allocation
            var existingInvAllocationLine = invoice.InvoiceAllocationLines!.ToList();

            var mapIncomingInvAllocationItems = dto.InvoiceAllocationLines
                .Select(dto => new InvAllocLine
                {
                    InvAllocLineID = dto.InvAllocLineID,
                    InvoiceID = dto.InvoiceID,
                    LineNo = dto.LineNo,
                    PoNo = dto.PoNo,
                    PoLineNo = dto.PoLineNo,
                    Qty = dto.Qty,
                    LineDescription = dto.LineDescription,
                    Note = dto.Note,
                    LineNetAmount = dto.LineNetAmount,
                    LineTaxAmount = dto.LineTaxAmount,
                    LineAmount = dto.LineAmount,
                    TaxCodeID = dto.TaxCodeID,
                    AccountID = dto.Account,
                });

            var itemstoAdd = mapIncomingInvAllocationItems
                .Where(i => i.InvAllocLineID == 0 ||
                !existingInvAllocationLine.Any(e => e.InvAllocLineID == i.InvAllocLineID))
                .ToList();

            var itemsToUpdate = mapIncomingInvAllocationItems
                .Where(i => i.InvAllocLineID != 0 &&
                existingInvAllocationLine.Any(e => e.InvAllocLineID == i.InvAllocLineID))
                .ToList();

            var incominginvAllocItemsIds = mapIncomingInvAllocationItems
                .Where(i => i.InvAllocLineID != 0)
                .Select(i => i.InvAllocLineID)
                .ToHashSet();

            var itemsToDelete = existingInvAllocationLine
                .Where(i => !incominginvAllocItemsIds.Contains(i.InvAllocLineID))
                .ToList();


            // routing flow
            var exisitngInvRoutingFlowLevel = invoice.InvInfoRoutingLevels!.ToList();

            var mapIncomingInvRoutingFlowLevels = dto.InvInfoRoutingLevels
               .Select(dto => new InvInfoRoutingLevel
               {
                   InvoiceID = dto.InvoiceID,
                   InvInfoRoutingLevelID = dto.InvInfoRoutingLevelID,
                   InvRoutingFlowID = dto.InvRoutingFlowID,
                   Level = dto.Level,
                   RoleID = dto.RoleID,
               });

            var routingLevelsToAdd = mapIncomingInvRoutingFlowLevels
               .Where(i => i.InvInfoRoutingLevelID == 0 ||
               !exisitngInvRoutingFlowLevel.Any(e => i.InvInfoRoutingLevelID == i.InvInfoRoutingLevelID!))
               .ToList();


            var routingLevelsToUpdate = mapIncomingInvRoutingFlowLevels
               .Where(i => i.InvInfoRoutingLevelID != 0 &&
               exisitngInvRoutingFlowLevel.Any(e => e.InvInfoRoutingLevelID == i.InvInfoRoutingLevelID))
               .ToList();



            var incomingInvInfoRoutingLevelIds = mapIncomingInvRoutingFlowLevels
                .Where(i => i.InvInfoRoutingLevelID != 0)
                .Select(i => i.InvInfoRoutingLevelID)
                .ToHashSet();


            var invInfoRoutingLevelToDelete = exisitngInvRoutingFlowLevel
               .Where(i => !incomingInvInfoRoutingLevelIds.Contains(i.InvInfoRoutingLevelID))
               .ToList();

            // invoice allocation
            //disable the implementation; change the saving and adding of line items "inline"
          
            //UpdateItems(existingInvAllocationLine, itemsToUpdate, request.UpdatedBy);
            //AddItems(invoice, itemstoAdd, request.UpdatedBy);
            //DeleteItems(itemsToDelete);

            RoutingLevelsUpdateItems(exisitngInvRoutingFlowLevel, routingLevelsToUpdate);
            RoutingLevelsAddItems(invoice, routingLevelsToAdd);
            RoutingLevelsDeleteItems(invInfoRoutingLevelToDelete);

            invoice.SetAuditFieldsOnUpdate(request.UpdatedBy);

            var saved = await _unitofWork.SaveChanges(cancellationToken);
            if (!saved)
            {
                return ResponseResult<bool>.BadRequest("Failed to update Invoice ");
            }

            return ResponseResult<bool>.OK("Invoice updated successfully.");
        }

        private static void UpdateItems(List<InvAllocLine> existingAllocLines,
            List<InvAllocLine> updatedLines,
            string updatedBy)
        {
            foreach (var updated in updatedLines)
            {
                var existing = existingAllocLines
                    .First(e => e.InvAllocLineID == updated.InvAllocLineID);

                existing.InvoiceID = updated.InvoiceID;
                existing.LineNo = updated.LineNo;
                existing.PoNo = updated.PoNo;
                existing.PoLineNo = updated.PoLineNo;
                existing.LineDescription = updated.LineDescription;
                existing.Qty = updated.Qty;
                existing.LineNetAmount = updated.LineNetAmount;
                existing.LineTaxAmount = updated.LineTaxAmount;
                existing.LineAmount = updated.LineAmount;
                existing.Note = updated.Note;
                existing.TaxCodeID = updated.TaxCodeID;
                existing.AccountID = updated.AccountID;
                existing.SetAuditFieldsOnUpdate(updatedBy);
            }
        }

        private static void RoutingLevelsUpdateItems(List<InvInfoRoutingLevel> existingRoutingLevel,
        List<InvInfoRoutingLevel> updatedRoutingLevel
       )
        {
            foreach (var updated in updatedRoutingLevel)
            {
                var existing = existingRoutingLevel
                    .First(e => e.InvInfoRoutingLevelID == updated.InvInfoRoutingLevelID);

                existing.InvoiceID = updated.InvoiceID;
                existing.InvInfoRoutingLevelID = updated.InvInfoRoutingLevelID;
                existing.InvRoutingFlowID = updated.InvRoutingFlowID == 0 ? null : updated.InvRoutingFlowID;
                existing.Level = updated.Level;
                existing.RoleID = updated.RoleID;

            }
        }

        private static void AddItems(Invoice invoice, List<InvAllocLine> newItems, string createdBy)
        {
            foreach (var item in newItems)
            {
                invoice.InvoiceAllocationLines!.Add(new InvAllocLine
                {
                    InvoiceID = item.InvoiceID,
                    LineNo = item.LineNo,
                    PoNo = item.PoNo,
                    PoLineNo = item.PoLineNo,
                    LineDescription = item.LineDescription,
                    Qty = item.Qty,
                    LineNetAmount = item.LineNetAmount,
                    LineTaxAmount = item.LineTaxAmount,
                    LineAmount = item.LineAmount,
                    Note = item.Note,
                    TaxCodeID = item.TaxCodeID,
                    AccountID = item.AccountID,
                });
                invoice.InvoiceAllocationLines!.SetAuditFieldsOnCreate(createdBy);
            }
        }

        private static void RoutingLevelsAddItems(Invoice invoice, List<InvInfoRoutingLevel> newItems)
        {
            foreach (var item in newItems)
            {
                invoice.InvInfoRoutingLevels!.Add(new InvInfoRoutingLevel
                {
                    InvoiceID = item.InvoiceID,
                    //  InvRoutingFlowID = item.InvRoutingFlowID,
                    Level = item.Level,
                    RoleID = item.RoleID,

                });

            }
        }

        private void DeleteItems(List<InvAllocLine> todeletelines)
        {
            _unitofWork.GetRepository<InvAllocLine>().RemoveRangeAsync(todeletelines);
        }

        private void RoutingLevelsDeleteItems(List<InvInfoRoutingLevel> todeleteLevels)
        {
            _unitofWork.GetRepository<InvInfoRoutingLevel>().RemoveRangeAsync(todeleteLevels);
        }
    }
}