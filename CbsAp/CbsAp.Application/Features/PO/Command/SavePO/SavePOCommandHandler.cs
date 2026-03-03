using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Entities.PO;
using CbsAp.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Application.Features.PO.Command.SavePO
{
    public class SavePOCommandHandler : ICommandHandler<SavePOCommand, ResponseResult<bool>>
    {
        private readonly IUnitofWork _unitofWork;

        public SavePOCommandHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<ResponseResult<bool>> Handle(SavePOCommand request, CancellationToken cancellationToken)
        {
            var invoiceID = request.SavePOMatchingDto.InvoiceID;
            var lineNo = 1;

            var dtoLineIds = request.SavePOMatchingDto.PoLines!.Select(p => p.PurchaseOrderLineID).ToList();

            var polinesRepo = _unitofWork.GetRepository<PurchaseOrderLine>();

            var originalLines = await polinesRepo.Query()
                .Where(o => dtoLineIds.Contains(o.PurchaseOrderLineID))
                .AsNoTracking()
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

                matchDict.TryGetValue(l.PurchaseOrderLineID, out var totalMatchedQty);
                var remainingQty = totalMatchedQty == 0 ? qtyDifference : originalQty - (totalMatchedQty + updatedQty);

                var newTotalMatched = totalMatchedQty + updatedQty;

                var exist = existinngPOMatching
                   .FirstOrDefault(x => x.PurchaseOrderLineID == l.PurchaseOrderLineID);

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
                         RemainingQty = remainingQty,
                         MatchingStatus = newTotalMatched== originalQty ?  POMatchingStatus.FullyMatched : l.Status,
                         NetAmount = l.Amount,
                         MatchingDate = DateTime.UtcNow,
                         CreatedBy = request.CreatedBy,
                         CreatedDate = DateTime.UtcNow,
                    }
                }
                };
            })
            .ToList();

            saveAllocationLine.SetAuditFieldsOnCreate(request.CreatedBy);

            await _unitofWork.GetRepository<InvAllocLine>().AddRangeAsync(saveAllocationLine);
            var savePOMatching = await _unitofWork.SaveChanges(cancellationToken);

            if (!savePOMatching)
            {
                return ResponseResult<bool>.BadRequest("Error saving PO matching");
            }
            return ResponseResult<bool>.Created(MessageConstants.Message(MessageOperationType.Create, "Po Matching"));
        }
    }
}