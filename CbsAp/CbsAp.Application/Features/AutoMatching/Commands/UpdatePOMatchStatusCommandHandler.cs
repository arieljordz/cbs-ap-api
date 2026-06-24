using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.GoodReceipts;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Entities.PO;
using CbsAp.Domain.Enums;
using CBSAP.ValidationEngine.Core;
using CBSAP.ValidationEngine.MatchingRules;
using CBSAP.ValidationEngine.Rules;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Application.Features.AutoMatching
{
    public class UpdatePOMatchStatusCommandHandler(IUnitofWork unitOfWork) : ICommandHandler<UpdatePOMatchStatusCommand, ResponseResult<bool>>
    {
        public async Task<ResponseResult<bool>> Handle(UpdatePOMatchStatusCommand request, CancellationToken cancellationToken)
        {
            /// This will update the PO MatchStatus based on the PO Lines
            /// If there are partially matched in PO lines, PO MatchStatus will also be PartiallyMatched

            var poRepo = unitOfWork.GetRepository<PurchaseOrder>();
            List<PurchaseOrder> purchaseOrders = await poRepo.Query()
                .Include(x => x.PurchaseOrderLines)
                .Where(x => request.PurchaseOrderIDs.Contains(x.PurchaseOrderID)).ToListAsync();

            foreach (var po in purchaseOrders)
            {
                var lines = po.PurchaseOrderLines;
                var totalLines = lines?.Count ?? 0;

                if (totalLines == 0)
                {
                    po.MatchStatus = MatchingStatus.UnMatched;
                }

                else
                {
                    bool isFullyMatched = lines!.All(x => x.InvoiceStatus == (int)InvoicePOMatchingStatus.FullyMatched);
                    bool isAnyMatched = lines!.Any(x => x.InvoiceStatus == (int)InvoicePOMatchingStatus.FullyMatched ||
                                                       x.InvoiceStatus == (int)InvoicePOMatchingStatus.PartiallyMatched);

                    po.MatchStatus = isFullyMatched ? MatchingStatus.FullyMatched
                                   : isAnyMatched ? MatchingStatus.PartiallyMatched
                                   : MatchingStatus.UnMatched;
                }                
            }
            
            await poRepo.UpdateRangeAsync(purchaseOrders.AsEnumerable());
            await unitOfWork.SaveChanges(string.Empty, string.Empty, cancellationToken);

            return ResponseResult<bool>.OK("Updated");
        }
    }
}
