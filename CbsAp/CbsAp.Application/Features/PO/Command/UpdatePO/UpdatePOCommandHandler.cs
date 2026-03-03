using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Entities.PO;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Application.Features.PO.Command.UpdatePO
{
    public class UpdatePOCommandHandler : ICommandHandler<UpdatePOCommand, ResponseResult<bool>>
    {
        private readonly IUnitofWork _unitofWork;

        public UpdatePOCommandHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<ResponseResult<bool>> Handle(UpdatePOCommand request, CancellationToken cancellationToken)
        {
            // reverse saving
            var poMatchTrackingRepo = _unitofWork.GetRepository<PurchaseOrderMatchTracking>();

            var invAllocRepo = _unitofWork.GetRepository<InvAllocLine>();

            var existinngPOMatching = await poMatchTrackingRepo
                .Query()
                .Include(m => m.InvAllocLine)
                .Include(m => m.PurchaseOrder)
                .Include(m => m.PurchaseOrderLine)
                .Include(m => m!.Invoice!)
                .Where(i => i.InvoiceID == request.SavePOMatchingDto.InvoiceID)
                .ToListAsync(cancellationToken);

            var existingLineNumbers = existinngPOMatching
                .Where(i => i.InvAllocLine != null)
                .Select(a => a.InvAllocLine!.LineNo!.Value)
                .Distinct()
                .ToList();

            var nextLineNumber = Enumerable
                    .Range(1, existingLineNumbers.Count + 1)
                    .Select(x => (long)x)
                    .Except(existingLineNumbers)
                    .First();

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

            foreach (var dto in request.SavePOMatchingDto.PoLines!)
            {
                var exist = existinngPOMatching
                    .FirstOrDefault(x => x.PurchaseOrderMatchTrackingID == dto.PurchaseOrderMatchTrackingID);

                var original = originalLines.FirstOrDefault(o => o.PurchaseOrderLineID == dto.PurchaseOrderLineID);
                var originalQty = original?.Qty ?? 0;
                var updatedQty = dto.Qty;
                var qtyDifference = originalQty - updatedQty;

                matchDict.TryGetValue(dto.PurchaseOrderLineID, out var totalMatchedQty);

                var remainingQty = totalMatchedQty == 0 ? qtyDifference : (totalMatchedQty + updatedQty) - originalQty;

                if (exist != null)
                {
                    remainingQty = Math.Max(0, remainingQty);

                
                    remainingQty = Math.Max(0, remainingQty);

                    exist.PurchaseOrderLineID = dto.PurchaseOrderLineID;
                    exist.PurchaseOrderID = dto.PurchaseOrderID;
                    exist.InvoiceID = dto.InvoiceID;
                    exist.InvAllocLineID = dto.InvAllocLineID;
                    exist.MatchingStatus = dto.Status;
                    exist.NetAmount = dto.NetAmount;
                    exist.Qty = dto.Qty;
                    exist.RemainingQty = remainingQty;
                    exist.MatchingDate = DateTime.UtcNow;
                    exist.NetAmount = dto.NetAmount;
                    exist.LastUpdatedDate = DateTime.UtcNow;
                    exist.LastUpdatedBy = request.UpdatedBy;

                    exist.InvAllocLine!.InvoiceID = dto.InvoiceID;
                    exist.InvAllocLine!.PoNo = dto.PoNo;
                    exist.InvAllocLine!.PoLineNo = dto.LineNo.ToString();
                    exist.InvAllocLine!.AccountID = dto.AccountID;
                    exist.InvAllocLine!.LineDescription = dto.Description;
                    exist.InvAllocLine!.Qty = dto.Qty;
                    exist.InvAllocLine!.LineNetAmount = dto.Amount!.Value;
                }
                else
                {
                    var newAllocationline = new InvAllocLine
                    {
                        InvoiceID = request.SavePOMatchingDto.InvoiceID,
                        PoNo = dto.PoNo,
                        PoLineNo = dto.LineNo.ToString(),
                        LineNo = nextLineNumber,
                        AccountID = dto.AccountID,
                        LineDescription = dto.Description,
                        Qty = dto.Qty,
                        LineNetAmount = dto.Amount!.Value,
                    };
                    newAllocationline.SetAuditFieldsOnCreate(request.UpdatedBy);
                    var newPOMatching = new PurchaseOrderMatchTracking
                    {
                        PurchaseOrderLineID = dto.PurchaseOrderLineID,
                        PurchaseOrderID = dto.PurchaseOrderID,
                        InvoiceID = request.SavePOMatchingDto.InvoiceID,
                        Qty = dto.Qty,
                        RemainingQty = remainingQty,
                        MatchingStatus = dto.Status,
                        NetAmount = dto.Amount,
                        MatchingDate = DateTime.UtcNow,
                        CreatedBy = request.UpdatedBy,
                        CreatedDate = DateTime.UtcNow,
                        InvAllocLine = newAllocationline
                    };
                    await poMatchTrackingRepo.AddAsync(newPOMatching);
                }
            }

            var idsToRemove = request.SavePOMatchingDto
                .PoLines.Where(x => x.InvAllocLineID.HasValue)
                .Select(x => x.InvAllocLineID).ToHashSet();

            var toRemove = existinngPOMatching
               .Where(x => !idsToRemove.Contains(x.InvAllocLineID))
               .ToList();

            var removeIdsAllocation = toRemove.Select(x => x.InvAllocLineID).Distinct().ToList();

            var allocationToRemove =
               await invAllocRepo.
                Query()
                .Where(a =>
                removeIdsAllocation.Contains(a.InvAllocLineID)).ToListAsync(cancellationToken);

            await poMatchTrackingRepo.RemoveRangeAsync(toRemove);

            await invAllocRepo.RemoveRangeAsync(allocationToRemove);
            var savePOMatching = await _unitofWork.SaveChanges(cancellationToken);

            if (!savePOMatching)
            {
                return ResponseResult<bool>.BadRequest("Error saving PO matching");
            }
            return ResponseResult<bool>.OK("PO matching successfully.");
        }
    }
}