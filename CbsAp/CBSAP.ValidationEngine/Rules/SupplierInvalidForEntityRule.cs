using System.Text.Json.Serialization;

namespace CBSAP.ValidationEngine.Rules
{
    public class SupplierInvalidForEntityRule : IValidationRule
    {
        public string? Field { get; set; }
        public string? ScopeField { get; set; }
        public string? ReferenceSourceKey { get; set; }
        public string? ReferenceSourceKeyField { get; set; }
        public string? ReferenceSourceScopeField { get; set; }
        public string? ErrorMessage { get; set; }
        public string? ErrorCode { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EngineValidationSeverity Severity { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EngineInvoiceStatusType? NextStatus { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EngineInvoiceQueueType? TargetQueue { get; set; }

        public string Name => "SupplierInValidForEntityRule";

        public EngineValidationResult Validate(object context, IDictionary<string, object>? runtimeContext = null)
        {
            var entityType = context?.GetType()
            ?? throw new ArgumentNullException(nameof(context));

            var fieldValue = entityType.GetProperty(Field!)?.GetValue(context);
            var scopeValue = entityType.GetProperty(ScopeField!)?.GetValue(context);

            if (fieldValue == null || scopeValue == null)
                return EngineValidationResult.Success();

            var referenceData = (IEnumerable<object>)runtimeContext![ReferenceSourceKey!];

            bool exists = referenceData.Any(item =>
            {
                var itemType = item.GetType();
                var referenceValue = itemType.GetProperty(ReferenceSourceKeyField!)?.GetValue(item);
                var referenceScopeValue = itemType.GetProperty(ReferenceSourceScopeField!)?.GetValue(item);

                return Equals(referenceValue, fieldValue) && Equals(referenceScopeValue, scopeValue);
            });

            return exists
                ? EngineValidationResult.Success()
                : EngineValidationResult.Failure(ErrorMessage!, ErrorCode!, Severity, NextStatus, TargetQueue);
        }
    }
}