using CbsAp.Domain.Entities.GoodReceipts;
using CbsAp.Domain.Entities.PO;
using CBSAP.ValidationEngine.Core;


namespace CBSAP.ValidationEngine.MatchingRules
{
    public class PoGrLineMatchingByQtyRule : IMatchingRule<PurchaseOrderLine, GoodsReceiptLine>
    {
        public bool IsMatch(PurchaseOrderLine poLine, GoodsReceiptLine grLine)
        {           

            return poLine.LineNo == grLine.LineNo && grLine.Qty == poLine.Qty  && poLine.PurchaseOrder!.PoNo == grLine.PurchaseOrderNo
                && poLine.PurchaseOrder!.SupplierNo == grLine.SupplierNo;
        }
    }

    public class PoGrLineMatchingByAmountRule : IMatchingRule<PurchaseOrderLine, GoodsReceiptLine>
    {
        public bool IsMatch(PurchaseOrderLine poLine, GoodsReceiptLine grLine)
        {

            return poLine.LineNo == grLine.LineNo && grLine.Amount == poLine.NetAmount && poLine.PurchaseOrder!.PoNo == grLine.PurchaseOrderNo
                && poLine.PurchaseOrder!.SupplierNo == grLine.SupplierNo;
        }
    }

    public class PoGrLinePartialMatchingByQtyRule : IMatchingRule<PurchaseOrderLine, GoodsReceiptLine>
    {
        public bool IsMatch(PurchaseOrderLine poLine, GoodsReceiptLine grLine)
        {

            return poLine.LineNo == grLine.LineNo && grLine.Qty < poLine.Qty && poLine.PurchaseOrder!.PoNo == grLine.PurchaseOrderNo
                && poLine.PurchaseOrder!.SupplierNo == grLine.SupplierNo;
        }
    }

    public class PoGrLinePartialMatchingByAmountRule : IMatchingRule<PurchaseOrderLine, GoodsReceiptLine>
    {
        public bool IsMatch(PurchaseOrderLine poLine, GoodsReceiptLine grLine)
        {

            return poLine.LineNo == grLine.LineNo && grLine.Amount < poLine.NetAmount && poLine.PurchaseOrder!.PoNo == grLine.PurchaseOrderNo
                && poLine.PurchaseOrder!.SupplierNo == grLine.SupplierNo;
        }
    }
}
