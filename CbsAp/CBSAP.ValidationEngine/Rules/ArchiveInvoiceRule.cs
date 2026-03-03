using System.Text.Json.Serialization;

namespace CBSAP.ValidationEngine.Rules
{
    public class ArchiveInvoiceRule : IValidationRule
    {
        public List<string>? Properties { get; set; }

        public string ReferenceSourceKey { get; set; }

        public string ErrorMessage { get; set; }

        public string ErrorCode { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EngineValidationSeverity Severity { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EngineInvoiceStatusType? NextStatus { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EngineInvoiceQueueType? TargetQueue { get; set; }

        public string Name => "ArchiveInvoiceRule";

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
                    Equals(item.GetType().GetProperty(p)?.GetValue(item), valuesToCheck[p])
                );

                if (match)
                    return EngineValidationResult.Failure(ErrorMessage!, ErrorCode!, Severity, NextStatus, TargetQueue);
            }

            return EngineValidationResult.Success();
        }
    }
}
