using System.Text.Json.Serialization;

namespace CBSAP.ValidationEngine.Rules
{
    public class InvoiceDateNotnFutureRule : IValidationRule
    {
        public string? Field { get; set; }

        public string? ErrorMessage { get; set; }

        public string? ErrorCode { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EngineValidationSeverity Severity { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EngineInvoiceStatusType? NextStatus { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EngineInvoiceQueueType? TargetQueue { get; set; }

        public string Name => throw new NotImplementedException();

        public EngineValidationResult Validate(object context, IDictionary<string, object>? runtimeContext = null)
        {
            var entityType = context?.GetType() ?? throw new ArgumentNullException(nameof(context));

            var prop = entityType.GetProperty(Field);

            var value = prop!.GetValue(context);

            if (value is not DateTimeOffset dateValue)
                return EngineValidationResult.Success();

            if (dateValue.Date > DateTime.Today)
            {
                return EngineValidationResult.Failure(ErrorMessage!, ErrorCode!, Severity, NextStatus, TargetQueue);
            }

            return EngineValidationResult.Success();
        }
    }
}