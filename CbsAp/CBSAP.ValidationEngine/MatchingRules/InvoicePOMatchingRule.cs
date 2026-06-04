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
            var total = po.PurchaseOrderLines!
                .Where(x=>x.DeliveryStatus!=0)
                .Sum(x => (x.NetAmount));

            var isTotalMatch = invoice.NetAmount == total;
            var isMatch = invoice.PoNo == po.PoNo && invoice.EntityProfileID == po.EntityProfileID && invoice.SupplierInfoID == po.SupplierInfoID &&
                invoice.TaxAmount == po.TaxAmount && isTotalMatch;

            return isMatch;
            
        }
    }
    
}
