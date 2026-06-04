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
    public class MatchPOCommandHandler : ICommandHandler<MatchPOCommand, ResponseResult<bool>>
    {
        private readonly IUnitofWork _unitOfWork;

        public MatchPOCommandHandler(IUnitofWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseResult<bool>> Handle(MatchPOCommand request, CancellationToken cancellationToken)
        {
            var invoiceRepo = _unitOfWork.GetRepository<Invoice>();
            var poRepo = _unitOfWork.GetRepository<PurchaseOrder>();
            var poLineRepo = _unitOfWork.GetRepository<PurchaseOrderLine>();
            var grLineRepo = _unitOfWork.GetRepository<GoodsReceiptLine>();

            var purchaseOrders = poRepo.Query()
                 .Where(po => po.PurchaseOrderLines!.Any(pol => pol.DeliveryStatus == 0));

            var matchingEngine = new MatchingEngine<PurchaseOrderLine, GoodsReceiptLine>();            
            
            List<PurchaseOrderLine> matchedPoLine = new List<PurchaseOrderLine>();
            List<Invoice> invoices = new List<Invoice>();

            foreach (var purchaseOrder in purchaseOrders) 
            {
                var poLines = purchaseOrder!.PurchaseOrderLines!.OrderBy(x => x.LineNo).ToList();
                
                if (purchaseOrder.EntityProfile!.AutomaticGoodsDelivered)
                {
                    {
                        poLines.ForEach((line) => {
                            line.DeliveryStatus = (int)POLineDeliveryStatus.FullyDelivered;
                            line.SetAuditFieldsOnUpdate("System");
                        });
                    }
                    matchedPoLine.AddRange(poLines.AsEnumerable());
                    continue;
                }
                
                matchingEngine.ClearRule();
                if (purchaseOrder.PurchaseOrderMatchType == (int)PurchaseOrderMatchType.Quantity)
                {
                    matchingEngine.AddRule(new PoGrLineMatchingByQtyRule());
                }
                else
                {
                    matchingEngine.AddRule(new PoGrLineMatchingByAmountRule());
                }
                                
                var grLines = grLineRepo.Query().AsNoTracking()
                    .Include(x=>x.GoodsReceipt)
                    .Where(x => x.PurchaseOrderNo == purchaseOrder.PoNo).OrderBy(x => x.LineNo).ToList();

                var fullyMatchedLines = matchingEngine.ExecuteMatch(poLines, grLines);
                if (fullyMatchedLines.Any())
                {
                    var grLine = fullyMatchedLines[0].Right;
                    var invoice = invoiceRepo.Query().FirstOrDefault(x => x.PoNo == purchaseOrder.PoNo);
                    if (invoice != null)
                    {
                        invoice.GrNo = grLine.GoodsReceipt?.GoodsReceiptNumber;
                        invoice.SetAuditFieldsOnUpdate("System");
                        invoices.Add(invoice);
                    }
                }
                foreach (var item in fullyMatchedLines)
                {
                    var line = item.Left;
                    
                    line.DeliveryStatus = (int)POLineDeliveryStatus.FullyDelivered;
                    line.SetAuditFieldsOnUpdate("System");
                    matchedPoLine.Add(line);
                }

                matchingEngine.ClearRule();
                if (purchaseOrder.PurchaseOrderMatchType == (int)PurchaseOrderMatchType.Quantity)
                {
                    matchingEngine.AddRule(new PoGrLinePartialMatchingByQtyRule());
                }
                else
                {
                    matchingEngine.AddRule(new PoGrLinePartialMatchingByAmountRule());
                }
                var partiallyMatchedLines = matchingEngine.ExecuteMatch(poLines, grLines);
                if (partiallyMatchedLines.Any())
                {
                    var grLine = partiallyMatchedLines[0].Right;
                    var invoice = invoiceRepo.Query().FirstOrDefault(x => x.PoNo == purchaseOrder.PoNo);
                    if (invoice != null)
                    {
                        invoice.GrNo = grLine.GoodsReceipt?.GoodsReceiptNumber;
                        invoice.SetAuditFieldsOnUpdate("System");
                        invoices.Add(invoice);
                    }
                }
                foreach (var item in partiallyMatchedLines)
                {
                    var line = item.Left;
                    line.DeliveryStatus = (int)POLineDeliveryStatus.PartiallyDelivered;
                    line.SetAuditFieldsOnUpdate("System");
                    matchedPoLine.Add(line);
                }
            }
                        
            await poLineRepo.UpdateRangeAsync(matchedPoLine);
            await invoiceRepo.UpdateRangeAsync(invoices);
            await _unitOfWork.SaveChanges("System", "POAutoMatching",cancellationToken);


            return ResponseResult<bool>.OK("Matched");
        }
    }
}
