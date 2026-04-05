using System.Text.Json.Serialization;
using CbsAp.Domain.Entities.Invoicing;
using System.Collections.Generic;
using System.Linq;

namespace CBSAP.ValidationEngine.Rules
{
    public class MissingRoutingLevelRule : IValidationRule
    {
        public string? ReferenceSourceKey { get; set; }

        public string? ErrorMessage { get; set; }

        public string? ErrorCode { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EngineValidationSeverity Severity { get; set; } = EngineValidationSeverity.Critical;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EngineInvoiceStatusType? NextStatus { get; set; } = EngineInvoiceStatusType.Exception;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EngineInvoiceQueueType? TargetQueue { get; set; } = EngineInvoiceQueueType.ExceptionQueue;

        public string Name => "MissingRoutingLevelRule";

        public EngineValidationResult Validate(object context, IDictionary<string, object>? runtimeContext = null)
        {
            if (context is not Invoice invoice)
                return EngineValidationResult.Success(); // skip if not an invoice

            // Check if invoice has a routing flow but no levels
            bool hasFlow = invoice.InvRoutingFlowID.HasValue;
            bool hasLevels = invoice.InvInfoRoutingLevels != null && invoice.InvInfoRoutingLevels.Any();

            if  (!hasLevels)
            {
                return EngineValidationResult.Failure(
                    ErrorMessage,
                    ErrorCode,
                    Severity,
                    NextStatus,
                    TargetQueue
                );
            }

            return EngineValidationResult.Success();
        }
    }
}