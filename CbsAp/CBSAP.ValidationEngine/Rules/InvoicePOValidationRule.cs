using CbsAp.Domain.Entities.Entity;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Entities.PO;
using CbsAp.Domain.Entities.Supplier;
using CbsAp.Domain.Entities.TaxCodes;
using CbsAp.Domain.Enums;
using System.Text.Json.Serialization;

namespace CBSAP.ValidationEngine.Rules
{
    public class InvoicePOValidationRule : IValidationRule
    {
        public string? PoNoField { get; set; }

        public string? ErrorMessage { get; set; }

        public string? ErrorCode { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EngineValidationSeverity Severity { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EngineInvoiceStatusType? NextStatus { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EngineInvoiceQueueType? TargetQueue { get; set; }

        public string Name => "InvoicePOValidationRule";

        public EngineValidationResult Validate(object context, IDictionary<string, object>? runtimeContext = null)
        {
            Invoice? invoice = context as Invoice;

            if (string.IsNullOrEmpty(invoice?.PoNo))
            {
                return EngineValidationResult.Failure(
                    "Purchase Order is missing.",
                    ErrorCode!,
                    Severity,
                    NextStatus,
                    TargetQueue);

            }

            if (!runtimeContext!.TryGetValue("PurchaseOrders", out var purchaseOrders))
            {
                return EngineValidationResult.Failure(
                    "Purchase Orders data is missing.",
                    ErrorCode!,
                    Severity,
                    NextStatus,
                    TargetQueue);
            }

                        
            var pos = purchaseOrders as IEnumerable<PurchaseOrder>;
            PurchaseOrder? purchaseOrder = null;
            if (pos != null)            
            {
                purchaseOrder = pos.FirstOrDefault(po => po.PoNo == invoice.PoNo);
                if (purchaseOrder == null)
                {
                    return EngineValidationResult.Failure(
                        "Purchase Order is not valid.",
                        ErrorCode!,
                        Severity,
                        NextStatus,
                        TargetQueue);
                }
            }


            if (invoice.SupplierInfoID != purchaseOrder!.SupplierInfoID && invoice.EntityProfileID != purchaseOrder.EntityProfileID)
            {
                return EngineValidationResult.Failure(
                   "Invoice Supplier and Purchase Order Supplier doesn't match.",
                   ErrorCode!,
                   Severity,
                   NextStatus,
                   TargetQueue);
            }


            //validate tax code
            if (invoice.SupplierInfo != null && invoice.TaxCode != null)
            {
                if (invoice.SupplierInfo.SupplierTaxID != invoice.TaxCode.Code)
                {
                    return EngineValidationResult.Failure(
                       "Invoice Supplier Tax ID and Tax Code doesn't match.",
                       ErrorCode!,
                       Severity,
                       NextStatus,
                       TargetQueue);
                }
            }

            runtimeContext!.TryGetValue("POMatchingConfig", out var poMatchingCofigObj);            
            var poMatchingConfig = poMatchingCofigObj as EntityMatchingConfig;

            //PO amount validation
            bool isPercentage = false;
            decimal tolerance = 0;
            if (poMatchingConfig != null)
            {
                var dollarAmt = poMatchingConfig.DollarAmt ?? 0;
                if (dollarAmt > 0)
                {
                    tolerance = dollarAmt;
                }
                else
                {
                    isPercentage = true;
                    tolerance = poMatchingConfig.PercentageAmt ?? 0;
                }
                
            }
            decimal poNetAmt = (purchaseOrder.NetAmount ?? 0);
            decimal difference = invoice.NetAmount - poNetAmt;
            
            if (difference > 0)
            {
                decimal allowedOver = isPercentage ? (poNetAmt * (tolerance / 100)) : tolerance;
                if (difference > allowedOver)
                {
                    return EngineValidationResult.Failure(
                       "Invoice Net amount is greater than PO Net Amount.",
                       ErrorCode!,
                       Severity,
                       NextStatus,
                       TargetQueue);
                }
            }
            
            if (difference < 0)
            {
                decimal allowedUnder = isPercentage ? (poNetAmt * (tolerance / 100)) : tolerance;
                if (Math.Abs(difference) > allowedUnder)
                {
                    return EngineValidationResult.Failure(
                       "Invoice Net amount is less than PO Net Amount.",
                       ErrorCode!,
                       Severity,
                       NextStatus,
                       TargetQueue);
                }
            }

            //Po Line validation
            runtimeContext!.TryGetValue("MatchedPurchaseOrders", out var matchedPurchaseOrders);
            var matchedPOs = matchedPurchaseOrders as List<PurchaseOrderMatchTracking>;
            if (matchedPOs != null)
            {
                foreach(var po in matchedPOs)
                {
                    var poLineNetAmt = (po.NetAmount ?? 0.0m);
                    decimal poAmtDiff = po.PurchaseOrderLine!.NetAmount??0.0m - poLineNetAmt;
                    if (poAmtDiff > 0)
                    {
                        decimal allowedOver = isPercentage ? (poNetAmt * (tolerance / 100)) : tolerance;
                        if (difference > allowedOver)
                        {
                            return EngineValidationResult.Failure(
                               "Purchase Order Line Amount is greater than Matched PO Amount.",
                               ErrorCode!,
                               Severity,
                               NextStatus,
                               TargetQueue);
                        }
                    }
                    if (poAmtDiff < 0)
                    {
                        decimal allowedUnder = isPercentage ? (poLineNetAmt * (tolerance / 100)) : tolerance;
                        if (Math.Abs(difference) > allowedUnder)
                        {
                            return EngineValidationResult.Failure(
                               "Purchase Order Line Amount is less than Matched PO Amount.",
                               ErrorCode!,
                               Severity,
                               NextStatus,
                               TargetQueue);
                        }
                    }
                }
            }


            return EngineValidationResult.Success();
        }
    }
}