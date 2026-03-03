using System.Text.Json.Serialization;

namespace CBSAP.ValidationEngine.Rules
{
    public class RejectDuplicatesRules : IValidationRule
    {
        public string? ReferenceSourceKey { get; set; }

        public List<string>? Properties { get; set; }

        public string? ErrorMessage { get; set; }

        public string? ErrorCode { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EngineValidationSeverity Severity { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EngineInvoiceStatusType? NextStatus { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EngineInvoiceQueueType? TargetQueue { get; set; }

        public string Name => "RejectInvoiceRule";

        public EngineValidationResult Validate(object context, IDictionary<string, object>? runtimeContext = null)
        {
            if (!runtimeContext.TryGetValue(ReferenceSourceKey!, out var source))
                throw new InvalidOperationException($"Missing data for '{ReferenceSourceKey}'");

            var list = source as IEnumerable<object>;
            if (list == null) throw new InvalidCastException("Data must be IEnumerable<object>");

            var valuesToCheck = Properties!
                .ToDictionary(p => p, p => context.GetType().GetProperty(p)?.GetValue(context));

            foreach (var item in list)
            {
                bool match = Properties!.All(p =>
                {
                    var prop = item.GetType().GetProperty(p);
                    if (prop == null) return false;

                    var value = prop.GetValue(item);
                    return value != null &&
                           valuesToCheck.TryGetValue(p, out var expected) &&
                           Equals(value, expected);
                });

                if (match)
                    return EngineValidationResult.Failure(ErrorMessage!, ErrorCode!, Severity, NextStatus, TargetQueue);
            }

            return EngineValidationResult.Success();
        }
    }
}