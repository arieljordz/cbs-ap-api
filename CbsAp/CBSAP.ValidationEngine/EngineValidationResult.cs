namespace CBSAP.ValidationEngine
{
    public class EngineValidationResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorCode { get; set; }
        public EngineValidationSeverity Severity { get; set; }

        public EngineInvoiceStatusType? NextStatus { get; set; }
        public EngineInvoiceQueueType? TargetQueue { get; set; }

        public static EngineValidationResult Success() => new() { IsSuccess = true };
        public static EngineValidationResult Success(string message) => new() { IsSuccess = true,Severity= EngineValidationSeverity.Info,ErrorMessage=message };
        public Dictionary<string, object>? RelatedIds { get; set; } = new();

        //additional info to be displayed in UI
        public List<EngineValidationResult> EngineValidationInfo { get; set; } = new();

        public static EngineValidationResult Failure(
            string message, string code, EngineValidationSeverity severity,
            EngineInvoiceStatusType? status = null, EngineInvoiceQueueType? queue = null)
        {
            return new EngineValidationResult
            {
                IsSuccess = false,
                ErrorMessage = message,
                ErrorCode = code,
                Severity = severity,
                NextStatus = status,
                TargetQueue = queue
            };
        }

        public Dictionary<string, long?> RelatedRelationshipIds { get; set; } = new();
    }
}