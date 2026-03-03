using System.Text.Json.Serialization;

namespace CBSAP.ValidationEngine.Rules
{
    public class SupplierTaxCodeMatchRule : IValidationRule
    {
        public string? InvoiceField { get; set; }
        public string? SupplierKeyField { get; set; }
        public string? SupplierReferenceSourceKey { get; set; }
        public string? ReferenceMatchField { get; set; }
        public string? EntityContextKey { get; set; }

        public string? TaxCodeReferenceKey { get; set; }

        public string? InvoiceReferenceSourceKey { get; set; }
        public string? SupplierToEntityField { get; set; }
        public string? EntityProfileIDField { get; set; }
        public string? EntityTaxCodesField { get; set; }
        public string? TaxCodeIdField { get; set; }

        public string? ErrorMessage { get; set; }
        public string? ErrorCode { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EngineValidationSeverity Severity { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EngineInvoiceStatusType? NextStatus { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EngineInvoiceQueueType? TargetQueue { get; set; }

        string IValidationRule.Name => "SupplierTaxCodeMatchRule";

        public EngineValidationResult Validate(object context, IDictionary<string, object>? runtimeContext = null)
        {
            // Check

            if (context == null || runtimeContext == null || string.IsNullOrWhiteSpace(InvoiceReferenceSourceKey))
                throw new ArgumentNullException(nameof(context));

            var type = context.GetType();
            var invoiceTaxCodeID = type.GetProperty(InvoiceField!)?.GetValue(context);
            var invoiceSupplierinfoID = type.GetProperty(SupplierKeyField!)?.GetValue(context);

            if (!runtimeContext.TryGetValue(InvoiceReferenceSourceKey!, out var invoiceData))

                return EngineValidationResult.Success();

            var invoices = invoiceData as IEnumerable<object>;

            if (!runtimeContext.TryGetValue(SupplierReferenceSourceKey!, out var supplierData))

                return EngineValidationResult.Success();

            var suppliers = supplierData as IEnumerable<object>;
            if (suppliers == null) return EngineValidationResult.Success();

            var supplier = suppliers.Where(s =>
            {
                var id = s.GetType().GetProperty(SupplierKeyField!)?.GetValue(s);
                return id != null && id.Equals(invoiceSupplierinfoID);
            }
            );

            bool anySupplierHasTaxCode = supplier.Any(tc =>
            {
                var taxCodeValue = tc.GetType().GetProperty(TaxCodeIdField!)?.GetValue(tc);
                return taxCodeValue != null;
            });

            if (anySupplierHasTaxCode)
            {
                bool anyMatch = supplier.Any(tc =>
                {
                    var taxCodeValue = tc.GetType().GetProperty(TaxCodeIdField!)?.GetValue(tc);
                    return taxCodeValue != null && taxCodeValue.Equals(invoiceTaxCodeID);
                });

                if (!anyMatch)
                {
                    return EngineValidationResult.Failure(
                        ErrorMessage!,
                        ErrorCode!,
                        Severity,
                        NextStatus,
                        TargetQueue
                    );
                }
            }
            else
            {
                if (invoices != null)
                {
                    bool invoiceHasTaxCode = invoices.Any(inv =>
                    {
                        var invoiceTaxCode = inv.GetType().GetProperty(TaxCodeIdField!)?.GetValue(inv);
                        return invoiceTaxCode != null && invoiceTaxCode.Equals(invoiceTaxCodeID);
                    });

                    if (!invoiceHasTaxCode)
                    {
                        return EngineValidationResult.Failure(
                        ErrorMessage!,
                        ErrorCode!,
                        Severity,
                        NextStatus,
                        TargetQueue
                    );
                    }
                }

                return EngineValidationResult.Success();
            }

            return EngineValidationResult.Success();
        }
    }
}