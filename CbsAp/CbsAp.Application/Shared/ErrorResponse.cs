namespace CbsAp.Application.Shared
{
    public class ErrorResponse
    {
        public ErrorResponse(int errorCode, string? message, string? details = null, IReadOnlyDictionary<string, string[]>? errors = null)
        {
            ErrorCode = errorCode;
            Message = message;
            Details = details;
            Errors = errors!;
        }

        public int ErrorCode { get; set; }
        public string? Details { get; set; }
        public string? Message { get; set; }
        public IReadOnlyDictionary<string, string[]> Errors { get; set; }
    }
}