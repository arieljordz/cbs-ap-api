using System.Text.Json.Serialization;

namespace CBSAP.ValidationEngine.Rules
{
    public class SupplierTaxCodeRateMatchRule : IValidationRule
    {
        public string? InvoiceSupplierKeyField { get; set; }

        public string? InvoiceNetAmountField { get; set; }

        public string? InvoiceTaxAmountField { get; set; }

        public string? SupplierReferenceKey { get; set; }

        public string? EntityContextKey { get; set; }

        public string? TaxCodeReferenceKey { get; set; }

        public string? SupplierToEntityField { get; set; }

        public string? SupplierMatchField { get; set; }

        public string? EntityTaxCodesField { get; set; }

        public string? TaxCodeMatchField { get; set; }

        public string? TaxRateField { get; set; }

        public string? ErrorMessage { get; set; }

        public string? ErrorCode { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EngineValidationSeverity Severity { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EngineInvoiceStatusType? NextStatus { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EngineInvoiceQueueType? TargetQueue { get; set; }

        public string Name => "SupplierTaxCodeRateMatchRule";

        public EngineValidationResult Validate(object context, IDictionary<string, object>? runtimeContext = null)
        {
            if (context == null || runtimeContext == null
                || string.IsNullOrWhiteSpace(SupplierReferenceKey))
                return EngineValidationResult.Success();

            var type = context.GetType();
            var taxCodeID = type.GetProperty(TaxCodeMatchField!)?.GetValue(context);
            var supplierInfoID = type.GetProperty(InvoiceSupplierKeyField!)?.GetValue(context)?.ToString();
            var netAmountObj = type.GetProperty(InvoiceNetAmountField!)?.GetValue(context);
            var taxAmountObj = type.GetProperty(InvoiceTaxAmountField!)?.GetValue(context);

            if (string.IsNullOrWhiteSpace(supplierInfoID) ||
                !decimal.TryParse(netAmountObj?.ToString(), out var netAmount) ||
                !decimal.TryParse(taxAmountObj?.ToString(), out var taxAmount))
                return EngineValidationResult.Success();

            if (!runtimeContext.TryGetValue(SupplierReferenceKey, out var supplierData))
                return EngineValidationResult.Success();

            var suppliers = supplierData as IEnumerable<object>;
            if (suppliers == null)
                return EngineValidationResult.Success();

            var supplier = suppliers.FirstOrDefault(s =>
            {
                var matchSupplier = s.GetType().GetProperty(InvoiceSupplierKeyField!)?.GetValue(s)?.ToString();
                return matchSupplier != null && matchSupplier.Equals(supplierInfoID);
            });

            if (supplier == null)
                return EngineValidationResult.Success();
            //remove checking of entity patrick
            //var entityID = supplier.GetType().GetProperty(SupplierToEntityField!)?.GetValue(supplier); ;

            //if (entityID == null)
            //    throw new ArgumentNullException(nameof(context));

            //if (!runtimeContext.TryGetValue(EntityContextKey!, out var entityData)
            //    || entityData is not IEnumerable<object> entities)
            //    throw new ArgumentNullException(nameof(context));
            //var entity = entities.FirstOrDefault(e =>
            //{
            //    var id = e.GetType().GetProperty(SupplierToEntityField!)?.GetValue(e);
            //    return id != null && id.Equals(entityID);
            //});

            //if (entity == null)
            //    throw new ArgumentNullException(nameof(context));

            if (!runtimeContext.TryGetValue(TaxCodeReferenceKey!, out var taxCodeData)
               || taxCodeData is not IEnumerable<object> taxCodes)
                throw new ArgumentNullException(nameof(context));

            var taxCode = taxCodes.FirstOrDefault(tc =>
            {
                var matchVal = tc.GetType().GetProperty(TaxCodeMatchField!)?.GetValue(tc);
                return matchVal != null && matchVal.Equals(taxCodeID);
            });

            if (taxCode == null)
                throw new ArgumentNullException(nameof(context));

            var rateObj = taxCode.GetType().GetProperty(TaxRateField!)?.GetValue(taxCode);
            if (!decimal.TryParse(rateObj?.ToString(), out var taxRate))
                throw new ArgumentNullException(nameof(context));
            var expectedTax = Math.Round(netAmount * (taxRate / 100), 2);
            if (expectedTax != taxAmount)
                return EngineValidationResult.Failure(
                        ErrorMessage!,
                        ErrorCode!,
                        Severity,
                        NextStatus,
                        TargetQueue
                    );

            return EngineValidationResult.Success();
        }
    }
}