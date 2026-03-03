using System.Text.Json.Serialization;

namespace CBSAP.ValidationEngine.Rules
{
    public class SupplierActiveDeleteRule : IValidationRule
    {
        public string? Field { get; set; }
        public string? ReferenceSourceKey { get; set; }
        public string? ReferenceSourceKeyField { get; set; }
        public string? ReferenceActiveField { get; set; }
        public string? ErrorMessage { get; set; }
        public string? ErrorCode { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EngineValidationSeverity Severity { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EngineInvoiceStatusType? NextStatus { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EngineInvoiceQueueType? TargetQueue { get; set; }

        public string Name => "SupplierIDActiveDeleteRule";

        public EngineValidationResult Validate(object context, IDictionary<string, object>? runtimeContext = null)
        {
            var entityType = context.GetType();
            var value = entityType.GetProperty(Field!)?.GetValue(context);

            if (value == null)
                return EngineValidationResult.Success();

            if (!runtimeContext.TryGetValue(ReferenceSourceKey!, out var referenceData)
                || referenceData is not IEnumerable<object> refList)
                throw new InvalidOperationException("Missing or invalid data source");

            var match = refList.FirstOrDefault(item =>
            {
                var itemType = item.GetType();
                var key = itemType.GetProperty(ReferenceSourceKeyField!)?.GetValue(item);
                if (!Equals(key, value))
                    return false;

                var isActive = itemType.GetProperty(ReferenceActiveField!)?.GetValue(item) as bool?;
                return isActive == true;
            });

            if (match == null)
            {
                return EngineValidationResult.Failure(ErrorMessage!, ErrorCode!, Severity, NextStatus, TargetQueue);
            }

            return EngineValidationResult.Success();
        }
    }
}