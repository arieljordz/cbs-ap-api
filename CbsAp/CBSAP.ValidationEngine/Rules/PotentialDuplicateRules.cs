using System.Text.Json.Serialization;

namespace CBSAP.ValidationEngine.Rules
{
    public class PotentialDuplicateRules : IValidationRule
    {
        public List<string>? Properties { get; set; }

        public List<List<string>> FieldCombinations { get; set; } = new();

        public string ReferenceSourceKey { get; set; }

        public string ErrorMessage { get; set; }

        public string ErrorCode { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EngineValidationSeverity Severity { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EngineInvoiceStatusType? NextStatus { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EngineInvoiceQueueType? TargetQueue { get; set; }

        public string Name => "PotentialDuplicateRules";

        public EngineValidationResult Validate(object context, IDictionary<string, object>? runtimeContext = null)
        {
            if (!runtimeContext.TryGetValue(ReferenceSourceKey, out var dataSourceObj) || dataSourceObj is not IEnumerable<object> records)
                throw new InvalidOperationException("Missing or invalid data source");

            var ctxProps = context.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(context));
           
            
            ctxProps.TryGetValue("QueueType", out var queueType);
            //only validate potential duplicate in Exception Queue
            if (queueType?.ToString() == "ExceptionQueue")
            {
                foreach (var record in records)
                {
                    var recordProps = record.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(record));

                    foreach (var combo in FieldCombinations)
                    {
                        bool potentialDuplicates = combo.All(field =>
                            ctxProps.TryGetValue(field, out var ctxVal) &&
                            recordProps.TryGetValue(field, out var recVal) &&
                            ctxVal != null &&
                            recVal != null &&
                            Equals(ctxVal, recVal));

                        if (potentialDuplicates)
                        {
                            return EngineValidationResult.Failure(ErrorMessage, ErrorCode, Severity, NextStatus, TargetQueue);
                        }
                    }
                }
            }

            return EngineValidationResult.Success();
        }
    }
}