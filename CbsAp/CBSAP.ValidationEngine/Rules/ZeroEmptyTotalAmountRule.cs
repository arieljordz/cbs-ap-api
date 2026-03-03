using System.Text.Json.Serialization;

namespace CBSAP.ValidationEngine.Rules
{
    internal class ZeroEmptyTotalAmountRule : IValidationRule
    {
        public string Field { get; set; }

        public string ErrorMessage { get; set; }

        public string ErrorCode { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EngineValidationSeverity Severity { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EngineInvoiceStatusType? NextStatus { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EngineInvoiceQueueType? TargetQueue { get; set; }

        public string Name => "ZeroEmptyTotalAmountRule";

        public EngineValidationResult Validate(object context, IDictionary<string, object>? runtimeContext = null)
        {
            if (context == null || string.IsNullOrWhiteSpace(Field))
                return EngineValidationResult.Success();

            var type = context.GetType();
            var prop = type.GetProperty(Field);
            var value = prop?.GetValue(context);

            if (value == null)
            {
                return EngineValidationResult.Failure(ErrorMessage!, ErrorCode!, Severity, NextStatus, TargetQueue);
            }
            if (value is decimal decValue && decValue == 0)
            {
                return EngineValidationResult.Failure(ErrorMessage!, ErrorCode!, Severity, NextStatus, TargetQueue);
            }
            return EngineValidationResult.Success();
        }
    }
}