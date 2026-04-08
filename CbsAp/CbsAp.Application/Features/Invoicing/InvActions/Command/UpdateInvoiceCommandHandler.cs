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
                .FirstOrDefaultAsync(i => i.InvoiceID == dto.InvoiceID);
            if (invoice == null)
                return ResponseResult<bool>.BadRequest("Invoice is not found");
            //get prev routing flow.
            var previousRoutingFlowID = invoice.InvRoutingFlowID;
            var previousKeywordID = invoice.KeywordID;
            var previousSupplierID = invoice.SupplierInfoID;

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


            bool routingFlowChanged = dto.InvRoutingFlowID != previousRoutingFlowID;
            bool keywordChanged = dto.KeywordID != previousKeywordID;
            bool supplierChanged = dto.SupplierInfoID != previousSupplierID;

            var prevQueue = invoice.QueueType;
            /*
            Only routing flow is change keyword and supplier is unchange. 
            */
            if (routingFlowChanged)
            {
                if (invoice.QueueType != InvoiceQueueType.MyInvoices)
                {
                    invoice.InvRoutingFlowID = dto.InvRoutingFlowID;
                }
            }
            else if (dto.InvRoutingFlowName == string.Empty)
            {
                if (invoice.QueueType != InvoiceQueueType.MyInvoices)
                {
                    // Try keyword routing
                    var keywordRepo = _unitofWork.GetRepository<Keyword>();
                    var keywordRouting = keywordRepo.Query()
                        .FirstOrDefault(a => a.KeywordID == dto.KeywordID);

                    if (keywordRouting != null)
                    {
                        invoice.InvRoutingFlowID = keywordRouting.InvoiceRoutingFlowID;
                    }
                    else
                    {
                        // Try supplier routing
                        var supplierRepo = _unitofWork.GetRepository<SupplierInfo>();
                        var supplierRouting = supplierRepo.Query()
                            .FirstOrDefault(w => w.SupplierInfoID == dto.SupplierInfoID);

                        if (supplierRouting != null)
                        {
                            invoice.InvRoutingFlowID = supplierRouting.InvRoutingFlowID;
                        }
                    }
                }
            }
            /*
            There's a changes in keyword and supplier
            */


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
            dto.InvInfoRoutingLevels = _unitofWork.GetRepository<InvRoutingFlowLevels>()
                .Query()
                .Where(w => w.InvRoutingFlowID == invoice.InvRoutingFlowID)
                .Select(s => new InvInfoRoutingLevelDto
                {
                    InvRoutingFlowID = s.InvRoutingFlowID,
                    InvoiceID = invoice.InvoiceID,
                    SupplierInfoID = invoice.SupplierInfoID,
                    KeywordID = invoice.KeywordID,
                    RoleID = s.RoleID,
                    Level = s.Level


                }).ToList();

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

            //var routingLevelsToAdd = _unitofWork.GetRepository<InvRoutingFlowLevels>().Query()
            //    .Where(w => w.InvRoutingFlowID == invoice.InvRoutingFlowID || !exisitngInvRoutingFlowLevel.Any(a => a.InvRoutingFlowID == w.InvRoutingFlowID)).ToList();


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
            //add new level only if the routing flow id changes.
            if (prevQueue != InvoiceQueueType.MyInvoices)
            {
                RoutingLevelsAddItems(invoice, routingLevelsToAdd);
                RoutingLevelsDeleteItems(invInfoRoutingLevelToDelete);
            }


            invoice.SetAuditFieldsOnUpdate(request.UpdatedBy);

            var module = Enum.GetValues(typeof(InvoiceQueueType)).Cast<InvoiceQueueType>().FirstOrDefault(s => s == invoice.QueueType);
            var saved = await _unitofWork.SaveChanges(request.UpdatedBy, module.ToString(), cancellationToken);
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
            if (newItems.Count == 0) invoice.InvInfoRoutingLevels = new List<InvInfoRoutingLevel>();
            foreach (var item in newItems)
            {
                invoice.InvInfoRoutingLevels!.Add(new InvInfoRoutingLevel
                {
                    InvoiceID = invoice.InvoiceID,
                    InvRoutingFlowID = item.InvRoutingFlowID,
                    Level = item.Level,
                    RoleID = item.RoleID,
                    KeywordID = invoice.KeywordID,
                    SupplierInfoID = invoice.SupplierInfoID,
                    InvFlowStatus = invoice.QueueType == InvoiceQueueType.MyInvoices ? item.Level == 1 ? (int?)InvFlowStatus.Assigned : (int)InvFlowStatus.Pending
                    : (int?)InvFlowStatus.Pending
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