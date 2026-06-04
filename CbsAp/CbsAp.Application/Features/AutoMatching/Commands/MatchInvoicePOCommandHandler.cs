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
using System.Reflection.Metadata;

namespace CbsAp.Application.Features.AutoMatching
{
    public class MatchInvoicePOCommandHandler : ICommandHandler<MatchInvoicePOCommand, ResponseResult<bool>>
    {
        private readonly IUnitofWork _unitOfWork;

        public MatchInvoicePOCommandHandler(IUnitofWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseResult<bool>> Handle(MatchInvoicePOCommand request, CancellationToken cancellationToken)
        {
            var poRepo = _unitOfWork.GetRepository<PurchaseOrder>();
            var invoiceRepo = _unitOfWork.GetRepository<Invoice>();
            var poLineRepo = _unitOfWork.GetRepository<PurchaseOrderLine>();
            var grLineRepo = _unitOfWork.GetRepository<GoodsReceiptLine>();
            var invoiceAllocLineRepo = _unitOfWork.GetRepository<InvAllocLine>();
            var poMatchTrackingRepo = _unitOfWork.GetRepository<PurchaseOrderMatchTracking>();

            var purchaseOrders = poRepo.Query()
                 .Where(po => po.PurchaseOrderLines!.Any(pol => pol.DeliveryStatus != (int)POLineDeliveryStatus.NotDelivered));

            var matchingEngine = new MatchingEngine<Invoice, PurchaseOrder>();
            matchingEngine.AddRule(new InvoicePOMatchingRule());

            List<PurchaseOrderLine> matchedPoLine = new List<PurchaseOrderLine>();
            List<Invoice> invoices = new List<Invoice>();

            foreach (var purchaseOrder in purchaseOrders)
            {
                //var poLines = purchaseOrder!.PurchaseOrderLines!.OrderBy(x => x.LineNo).ToList();
                
                var invoiceList = invoiceRepo.Query()
                    .AsNoTracking()
                    .Where(x => x.PoNo == purchaseOrder.PoNo && x.EntityProfileID == purchaseOrder.EntityProfileID
                        && x.SupplierInfoID == purchaseOrder.SupplierInfoID && x.TaxAmount == purchaseOrder.TaxAmount).AsEnumerable();


                invoices.AddRange(invoiceList);

            }

            //fully matched
            var matchedPOs = matchingEngine.ExecuteMatch(invoices, purchaseOrders.ToList());
            
            List<InvAllocLine> invoiceAllocLines = new List<InvAllocLine>();
            List<PurchaseOrderMatchTracking> poMatchTrackings = new List<PurchaseOrderMatchTracking>();
            List<GoodsReceiptLine> grLines = new List<GoodsReceiptLine>();

            foreach (var matched in matchedPOs)
            {
                var purchaseOrder = matched.Right;
                var invoice = matched.Left;
               
                var poLines = purchaseOrder.PurchaseOrderLines!.ToList();
                foreach (var line in poLines)
                {
                    line.InvoiceStatus = (int)InvoicePOMatchingStatus.FullyMatched;
                    line.SetAuditFieldsOnUpdate("System");


                    var grLine = grLineRepo.Query()
                        .FirstOrDefault(x => x.PurchaseOrderNo == line.PurchaseOrder!.PoNo && x.LineNo == line.LineNo);
                    
                    if (grLine != null)
                    {
                        grLine.InvoiceStatus = (int)InvoicePOMatchingStatus.FullyMatched;
                        grLine.SetAuditFieldsOnUpdate("System");
                        grLines.Add(grLine);
                    }

                    decimal netAmt = line.NetAmount ?? 0;
                    decimal taxAmt = line.TaxAmount ?? 0;
                    var matchingStatus = line.DeliveryStatus switch
                    {
                        1=>POMatchingStatus.FullyMatched,
                        2=> POMatchingStatus.PartialMatched,
                        _ => POMatchingStatus.Unmatched
                    };
                
                    //add invoice allocation line
                    var invAllocLine = new InvAllocLine()
                    {
                        InvoiceID = invoice.InvoiceID,
                        LineNo = line.LineNo,
                        PoLineNo = line.LineNo.ToString(),
                        LineDescription = line.Description,
                        Qty = line.Qty,
                        LineNetAmount = netAmt,
                        LineTaxAmount = taxAmt,
                        TaxCodeID = line.TaxCodeID,
                        PoNo = line.PurchaseOrder!.PoNo,
                        AccountID = line.AccountID,

                        PurchaseOrderMatchTrackings = new List<PurchaseOrderMatchTracking> { new PurchaseOrderMatchTracking {
                             PurchaseOrderLineID = line.PurchaseOrderLineID,
                             PurchaseOrderID = line.PurchaseOrderID,                             
                             InvoiceID = invoice.InvoiceID,
                             Qty = line.Qty,
                             RemainingQty = 0,
                             MatchingStatus =  matchingStatus,
                             NetAmount = netAmt,
                             MatchingDate = DateTime.UtcNow,
                             CreatedBy = "System",
                             CreatedDate = DateTime.UtcNow
                            }
                        }

                    };
                    invAllocLine.SetAuditFieldsOnCreate("System");
                    invoiceAllocLines.Add(invAllocLine);

                }

                matchedPoLine.AddRange(poLines);

                await poLineRepo.UpdateRangeAsync(matchedPoLine);
                await invoiceAllocLineRepo.AddRangeAsync(invoiceAllocLines);
                await grLineRepo.UpdateRangeAsync(grLines);
            }

            await _unitOfWork.SaveChanges("System", "AutoMatching", cancellationToken);


            return ResponseResult<bool>.OK("Matched");
        }
    }
}
