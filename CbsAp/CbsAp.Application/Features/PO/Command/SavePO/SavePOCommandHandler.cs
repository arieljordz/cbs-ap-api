using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.Features.AutoMatching;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.GoodReceipts;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Entities.PO;
using CbsAp.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace CbsAp.Application.Features.PO.Command.SavePO
{
    public class SavePOCommandHandler : ICommandHandler<SavePOCommand, ResponseResult<bool>>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly ISender _mediator;

        public SavePOCommandHandler(IUnitofWork unitofWork,ISender mediator)
        {
            _unitofWork = unitofWork;
            _mediator = mediator;
        }

        public async Task<ResponseResult<bool>> Handle(SavePOCommand request, CancellationToken cancellationToken)
        {
            var invoiceID = request.SavePOMatchingDto.InvoiceID;
            var lineNo = 1;

            var dtoLineIds = request.SavePOMatchingDto.PoLines!.Select(p => p.PurchaseOrderLineID).ToList();
            var polinesRepo = _unitofWork.GetRepository<PurchaseOrderLine>();        
            var grLineRepo = _unitofWork.GetRepository<GoodsReceiptLine>();
            
            var originalLines = await polinesRepo.Query()
                .Where(o => dtoLineIds.Contains(o.PurchaseOrderLineID))                
                .ToListAsync();

            var poMatchingRepo = _unitofWork.GetRepository<PurchaseOrderMatchTracking>();
            
            var matchSums = await poMatchingRepo.Query()
                .Where(m => dtoLineIds.Contains(m.PurchaseOrderLineID))
                .GroupBy(m => m.PurchaseOrderLineID)
                 .Select(g => new { PurchaseOrderLineID = g.Key, TotalMatchedQty = g.Sum(m => m.Qty) })
                 .ToListAsync();

            var matchDict = matchSums.ToDictionary(x => x.PurchaseOrderLineID, x => x.TotalMatchedQty);

            // amount  = qty * unit price

            var existinngPOMatching = await poMatchingRepo
                .Query()
                .Include(m => m.InvAllocLine)
                .Include(m => m.PurchaseOrder)
                    .ThenInclude(m => m!.GoodsReceiptLines)
                .Include(m => m.PurchaseOrderLine)
                
                .Include(m => m!.Invoice!)
                .Where(i => i.InvoiceID == request.SavePOMatchingDto.InvoiceID)
                .ToListAsync(cancellationToken);

            var saveAllocationLine = request.SavePOMatchingDto.PoLines!
            .Select(l =>
            {
                var original = originalLines.FirstOrDefault(o => o.PurchaseOrderLineID == l.PurchaseOrderLineID);
                var originalQty = original?.Qty ?? 0;
                var updatedQty = l.Qty;
                var qtyDifference = originalQty - updatedQty;
                long goodsReceiptLineId = 0;

                matchDict.TryGetValue(l.PurchaseOrderLineID, out var totalMatchedQty);
                var remainingQty = totalMatchedQty == 0 ? qtyDifference : originalQty - (totalMatchedQty + updatedQty);

                var newTotalMatched = totalMatchedQty + updatedQty;

                var exist = existinngPOMatching
                   .FirstOrDefault(x => x.PurchaseOrderLineID == l.PurchaseOrderLineID);

                if (original != null)
                {
                    var goodsReceipt = original.PurchaseOrder?.GoodsReceiptLines!.Where(x => x.LineNo == original.LineNo).FirstOrDefault();
                    if (goodsReceipt != null)
                    {
                        goodsReceiptLineId = goodsReceipt.GoodsReceiptLineID;
                    }
                    original.InvoiceStatus = (int)(newTotalMatched == originalQty ? InvoicePOMatchingStatus.FullyMatched : InvoicePOMatchingStatus.PartiallyMatched);                    
                }

                if (exist != null)
                {
                    exist.MatchingStatus = newTotalMatched == originalQty ? POMatchingStatus.FullyMatched : exist.MatchingStatus;                    
                    
                }

                return new InvAllocLine
                {
                    InvoiceID = invoiceID,
                    PoNo = l.PoNo,
                    LineNo = lineNo++,
                    PoLineNo = l.LineNo.ToString(),
                    AccountID = l.AccountID,
                    LineDescription = l.Description,
                    Qty = l.Qty,
                    LineNetAmount = l.Amount!.Value,                    

                    PurchaseOrderMatchTrackings = new List<PurchaseOrderMatchTracking> { new PurchaseOrderMatchTracking {
                         PurchaseOrderLineID = l.PurchaseOrderLineID,
                         PurchaseOrderID = l.PurchaseOrderID,
                         InvoiceID = invoiceID,
                         Qty = l.Qty,
                         GoodsReceiptLineID = goodsReceiptLineId,
                         RemainingQty = remainingQty,
                         MatchingStatus = newTotalMatched== originalQty ?  POMatchingStatus.FullyMatched : l.Status,
                         NetAmount = l.Amount,
                         MatchingDate = DateTime.UtcNow,
                         CreatedBy = request.CreatedBy,
                         CreatedDate = DateTime.UtcNow                         
                    }
                }
                };
            })
            .ToList();           


            saveAllocationLine.SetAuditFieldsOnCreate(request.CreatedBy);
            await polinesRepo.UpdateRangeAsync(originalLines);
            await _unitofWork.GetRepository<InvAllocLine>().AddRangeAsync(saveAllocationLine);            
            
            var savePOMatching = await _unitofWork.SaveChanges(string.Empty,string.Empty,cancellationToken);

            if (!savePOMatching)
            {
                return ResponseResult<bool>.BadRequest("Error saving PO matching");
            }

            //Update Purchase order MatchStatus if fully matched or partially matched
            var purchaseOrderIds = originalLines.Select(x =>x.PurchaseOrderID).ToList();
            UpdatePOMatchStatusCommand command = new UpdatePOMatchStatusCommand(purchaseOrderIds);     
            await _mediator.Send(command);

            return ResponseResult<bool>.Created(MessageConstants.Message(MessageOperationType.Create, "Po Matching"));
        }

        
    }
    
}