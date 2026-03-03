using CbsAp.Domain.Entities.Supplier;
using System.Text.Json.Serialization;

namespace CBSAP.ValidationEngine.Rules
{
    public class RequiredFieldRule : IValidationRule
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

        public string Name => "RequiredFieldRule";

        public EngineValidationResult Validate(object context, IDictionary<string, object>? runtimeContext = null)
        {
            var type = context.GetType();
            var prop = type.GetProperty(Field);
            var value = prop?.GetValue(context);

            var isMissing = value == null || (value is string s && string.IsNullOrWhiteSpace(s));

            // Extra: Ensure SupplierID exists in SupplierInfos from context
            if (Field == "SupplierID" && runtimeContext!.TryGetValue("SupplierInfos", out var suppliersObj))
            {
                var supplier = suppliersObj as IEnumerable<SupplierInfo>;

                if (value != null && !supplier!.Any(s => s.SupplierInfoID == (long)value!))
                {
                    return EngineValidationResult.Failure(
                          ErrorMessage,
                          ErrorCode,
                          Severity,
                          NextStatus,
                          TargetQueue);
                }
            }
            else
            {
                if (isMissing)
                {
                    return EngineValidationResult.Failure(
                        ErrorMessage,
                        ErrorCode,
                        Severity,
                        NextStatus,
                        TargetQueue
                    );
                }
            }

            return EngineValidationResult.Success();
        }
    }
}