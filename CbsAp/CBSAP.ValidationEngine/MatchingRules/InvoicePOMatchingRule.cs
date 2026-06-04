using CbsAp.Domain.Entities.GoodReceipts;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Entities.PO;
using CbsAp.Domain.Enums;
using CBSAP.ValidationEngine.Core;


namespace CBSAP.ValidationEngine.MatchingRules
{
    public class InvoicePOMatchingRule : IMatchingRule<Invoice, PurchaseOrder>
    {
        public bool IsMatch(Invoice invoice, PurchaseOrder po)
        {
            var fullyDeliveredTotalAmt = po.PurchaseOrderLines!
                .Where(x => x.DeliveryStatus == (int)POLineDeliveryStatus.FullyDelivered)
                .Sum(x => (x.NetAmount))??0;
            var fullyDeliveredTaxAmt = po.PurchaseOrderLines!
                .Where(x => x.DeliveryStatus == (int)POLineDeliveryStatus.FullyDelivered)
                .Sum(x => (x.TaxAmount)) ?? 0;

            decimal partialDeliveredTotalAmt = 0;
            decimal partialDeliveredTaxAmt = 0;
            var partialDeliveredPoLines= po.PurchaseOrderLines!.Where(x => x.DeliveryStatus == (int)POLineDeliveryStatus.PartiallyDelivered);
                        
            foreach (var poLine in partialDeliveredPoLines)
            {
                var grLines = po.GoodsReceiptLines!.Where(x => x.LineNo == poLine.LineNo!);            
                if (grLines.Any())
                {
                    if (poLine.Qty != 0)
                    {
                        var grQty = grLines.Sum(x => x.Qty);
                        var ratio = grQty / poLine.Qty;

                        partialDeliveredTotalAmt += (poLine.NetAmount ?? 0) * ratio;
                        partialDeliveredTaxAmt += (poLine.TaxAmount ?? 0) * ratio;
                    }
                }
            }

            var totalAmt = Math.Round(fullyDeliveredTotalAmt + partialDeliveredTotalAmt, 2, MidpointRounding.AwayFromZero); 
            var totalTaxAmt = Math.Round(fullyDeliveredTaxAmt + partialDeliveredTaxAmt, 2, MidpointRounding.AwayFromZero);

            var isMatch = invoice.PoNo == po.PoNo && invoice.EntityProfileID == po.EntityProfileID && invoice.SupplierInfoID == po.SupplierInfoID &&
                invoice.TaxAmount == totalTaxAmt && invoice.NetAmount == totalAmt;

            return isMatch;
            
        }
    }

    public class InvoicePOFullyMatchingRule : IMatchingRule<Invoice, PurchaseOrder>
    {
        public bool IsMatch(Invoice invoice, PurchaseOrder po)
        {
            var fullyDeliveredTotalAmt = po.PurchaseOrderLines!
                .Where(x => x.DeliveryStatus == (int)POLineDeliveryStatus.FullyDelivered)
                .Sum(x => (x.NetAmount)) ?? 0;
            var fullyDeliveredTaxAmt = po.PurchaseOrderLines!
                .Where(x => x.DeliveryStatus == (int)POLineDeliveryStatus.FullyDelivered)
                .Sum(x => (x.TaxAmount)) ?? 0;

            

            var totalAmt = Math.Round(fullyDeliveredTotalAmt , 2, MidpointRounding.AwayFromZero);
            var totalTaxAmt = Math.Round(fullyDeliveredTaxAmt , 2, MidpointRounding.AwayFromZero);

            var isMatch = invoice.PoNo == po.PoNo && invoice.EntityProfileID == po.EntityProfileID && invoice.SupplierInfoID == po.SupplierInfoID &&
                invoice.TaxAmount == totalTaxAmt && invoice.NetAmount == totalAmt;

            return isMatch;

        }
    }

    public class DeliveredGreaterThanInvoiceAmtRule : IMatchingRule<Invoice, PurchaseOrder>
    {
        public bool IsMatch(Invoice invoice, PurchaseOrder po)
        {
            var fullyDeliveredTotalAmt = po.PurchaseOrderLines!
                .Where(x => x.DeliveryStatus == (int)POLineDeliveryStatus.FullyDelivered)
                .Sum(x => (x.NetAmount)) ?? 0;
            var fullyDeliveredTaxAmt = po.PurchaseOrderLines!
                .Where(x => x.DeliveryStatus == (int)POLineDeliveryStatus.FullyDelivered)
                .Sum(x => (x.TaxAmount)) ?? 0;

            decimal partialDeliveredTotalAmt = 0;
            decimal partialDeliveredTaxAmt = 0;
            var partialDeliveredPoLines = po.PurchaseOrderLines!.Where(x => x.DeliveryStatus == (int)POLineDeliveryStatus.PartiallyDelivered);

            var grLookup = po.GoodsReceiptLines?
                .ToDictionary(x => x.LineNo, x => x.Qty) ?? new Dictionary<int, decimal>();

            foreach (var poLine in partialDeliveredPoLines)
            {
                if (grLookup.TryGetValue((int)poLine.LineNo!, out decimal grQty))
                {
                    if (poLine.Qty != 0)
                    {
                        var ratio = grQty / poLine.Qty;

                        partialDeliveredTotalAmt += (poLine.NetAmount ?? 0) * ratio;
                        partialDeliveredTaxAmt += (poLine.TaxAmount ?? 0) * ratio;
                    }
                }
            }

            var totalAmt = Math.Round(fullyDeliveredTotalAmt + partialDeliveredTotalAmt, 2, MidpointRounding.AwayFromZero);
            var totalTaxAmt = Math.Round(fullyDeliveredTaxAmt + partialDeliveredTaxAmt, 2, MidpointRounding.AwayFromZero);

            var isMatch = invoice.PoNo == po.PoNo && invoice.EntityProfileID == po.EntityProfileID && invoice.SupplierInfoID == po.SupplierInfoID &&
                invoice.TaxAmount == totalTaxAmt && totalAmt > invoice.NetAmount;

            return isMatch;

        }
    }

}
