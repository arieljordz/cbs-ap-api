using System.Text.Json.Serialization;

namespace CBSAP.ValidationEngine.Rules
{
    public class ComputedTotalAmountRule : IValidationRule
    {
        public string? TotalField { get; set; }

        public List<string> ComponentFields { get; set; } = new();

        public string? ContextPrefix { get; set; }

        public string? ErrorMessage { get; set; }

        public string? ErrorCode { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EngineValidationSeverity Severity { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EngineInvoiceStatusType? NextStatus { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EngineInvoiceQueueType? TargetQueue { get; set; }

        public string Name => "ComputedTotalAmountRule";

        public EngineValidationResult Validate(object context, IDictionary<string, object>? runtimeContext = null)
        {
            if (context == null || string.IsNullOrWhiteSpace(TotalField) || ComponentFields == null || ComponentFields.Count == 0)
                return EngineValidationResult.Success();

            var type = context.GetType();
            var totalVal = type.GetProperty(TotalField)?.GetValue(context);

            if (totalVal == null || !decimal.TryParse(totalVal.ToString(), out var totalAmount))
                return EngineValidationResult.Success();

            decimal sum = 0;

            foreach (var field in ComponentFields)
            {
                var propValue = type.GetProperty(field)?.GetValue(context);
                if (propValue != null && decimal.TryParse(propValue.ToString(), out var parsed))
                {
                    sum += parsed;
                }
                else if (!string.IsNullOrWhiteSpace(ContextPrefix) &&
                         runtimeContext != null &&
                         runtimeContext.TryGetValue($"{ContextPrefix}.{field}", out var fallbackVal) &&
                         fallbackVal != null &&
                         decimal.TryParse(fallbackVal.ToString(), out var fallbackParsed))
                {
                    sum += fallbackParsed;
                }
            }

            if (sum != totalAmount)
            {
                return EngineValidationResult.Failure(ErrorMessage!, ErrorCode!, Severity, NextStatus, TargetQueue);
            }

            return EngineValidationResult.Success();
        }
    }
}