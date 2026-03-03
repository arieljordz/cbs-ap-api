using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.PO;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.PO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.Features.PO.Queries.ReCalculateRemainingQty
{
    public class ReCalculateRemainingQtyQueryHandler : IQueryHandler<ReCalculateRemainingQtyQuery, ResponseResult<List<SearchPoLinesDto>>>
    {
        private readonly IUnitofWork _unitofWork;
        public ReCalculateRemainingQtyQueryHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }
        public async Task<ResponseResult<List<SearchPoLinesDto>>> Handle(ReCalculateRemainingQtyQuery request, CancellationToken cancellationToken)
        {
            var dtoLineIds = request.SavePOMatchingDto.PoLines!.Select(p => p.PurchaseOrderLineID).ToList();


            var polinesRepo = _unitofWork.GetRepository<PurchaseOrderLine>();
            var poRepo = _unitofWork.GetRepository<PurchaseOrder>();

            var poMatchingRepo = _unitofWork.GetRepository<PurchaseOrderMatchTracking>();


            var originalLines = await polinesRepo.Query()
               .Where(o => dtoLineIds.Contains(o.PurchaseOrderLineID))
               .AsNoTracking()
               .ToListAsync();



            var firstLine = await polinesRepo.Query()
                    .FirstOrDefaultAsync(l => l.PurchaseOrderLineID == dtoLineIds.First());

            long orderLineValue = firstLine!.PurchaseOrderLineID;

            var allLines = await poMatchingRepo.Query()
                .Where(l => l.PurchaseOrderLineID == orderLineValue)
                .OrderBy(l => l.PurchaseOrderLineID)
                .ToListAsync(cancellationToken);


            var poLines = await polinesRepo.Query().
                FirstOrDefaultAsync(po => po.PurchaseOrderLineID == firstLine.PurchaseOrderLineID);

            var remaining = poLines!.Qty;





            foreach (var line in allLines)
            {
                var isCanceled = dtoLineIds.Contains(line.PurchaseOrderLineID) && line.PurchaseOrderMatchTrackingID != 0;

                line.RemainingQty = isCanceled ? remaining : remaining - line.Qty;
                remaining = line.RemainingQty.Value;
            }

            var result = allLines.Select(lines => new SearchPoLinesDto
            {
                PoLines = new List<PoLinesDto> { new PoLinesDto {
                 PurchaseOrderLineID = lines.PurchaseOrderLineID,
                    PurchaseOrderID = lines.PurchaseOrderID,
                    InvoiceID  = lines.InvoiceID,
                    InvAllocLineID = lines.InvAllocLineID,
                    PoNo = lines.PurchaseOrder!.PoNo,
                    LineNo  =lines.PurchaseOrderLine!.LineNo,
                    Description = lines.PurchaseOrderLine.Description,
                    AccountID = lines.PurchaseOrderLine.AccountID,
                    OriginalQty = lines.PurchaseOrderLine.Qty,

                    Qty = lines.Qty,
                    Amount =  lines.NetAmount,
                    NetAmount = lines.NetAmount,
                    TaxAmount =  lines.TaxAmount,
                    Status = lines.MatchingStatus,
                    BaseRemainingQty = remaining,
                } }
            }).ToList();


            return result == null ?
              ResponseResult<List<SearchPoLinesDto>>.NotFound("No PoLines") :
              ResponseResult<List<SearchPoLinesDto>>.SuccessRetrieveRecords(result);



        }
    }
}
